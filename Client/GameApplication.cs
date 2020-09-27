using Client.Config;
using Client.Objects;
using Client.UI;
using Client.Utilities;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Numerics;
using Client.Collisions;
using Client.Models;
using Client.Objects.Pickupables;
using Microsoft.AspNetCore.SignalR.Client;
using Common.DTO;
using Common.Utilities;
using Client.Managers;

namespace Client
{
    class GameApplication
    {
        // Singleton instance
        private static readonly GameApplication _instance = new GameApplication();
        public static Random Rnd { get; set; } = new Random();


        // Screen 
        RenderWindow GameWindow { get; set; }
        private View MainView { get; set; }
        private View ZoomedView { get; set; }
        private bool FullScreen { get; set; }
        private bool PrevFullScreen { get; set; }
        float zoomView = 1.0f;
        float previousZoom = 1.0f;


        // Resources
        private TextureHolder Textures { get; set; } = TextureHolder.GetInstance();
        private SoundHolder Sounds { get; set; } = SoundHolder.GetInstance();
        private FontHolder Fonts { get; set; } = FontHolder.GetInstance();

        GameState GameState { get; set; } = new GameState();
        private ConnectionManager ConnectionManager { get; set; } = new ConnectionManager("https://shoot-and-dash.azurewebsites.net/sd-server");


        Player MainPlayer { get; set; }

        Sprite bgSprite;
        Sprite crate;
        Sprite bushSprite;

        IntRect playerAnimation = new IntRect(36, 0, 36, 64);
        IntRect playerIdle = new IntRect(0, 0, 36, 64);
        Clock animationSpeed = new Clock();
        Clock reloadClock = new Clock();
        Clock reloadTimer = new Clock();
        bool isPlayerRunning = false;

        CustomText scoreboardText;

        AimCursor AimCursor = new AimCursor();
        List<Projectile> bulletList = new List<Projectile>();
        float attackCooldown;
        float attackSpeed = 150;
        bool facingRight = true;
        bool isReloading = false;


        public MapGeneration map = new MapGeneration();
        public GameApplication() { }

        public static GameApplication GetInstance()
        {
            return _instance;
        }



        // ========================================================================
        // =========================== MAIN ENTRY POINT ===========================
        // ========================================================================

        public void Run()
        {
            // Create render window
            GameWindow = CreateRenderWindow(Styles.Close);
            Vector2f winSize = GameWindow.GetView().Size;

            // Load resources
            LoadTextures();
            LoadSounds();
            LoadFonts();
            CreateSprites();


            map.CreateMap();


            // View
            MainView = GameWindow.DefaultView;
            ZoomedView = new View(MainView);
            GameWindow.SetView(ZoomedView);


            MainPlayer = new Player();
            MainPlayer.Position = new Vector2f(GameWindow.Size.X / 2f, GameWindow.Size.Y / 2f);
            MainPlayer.TextureRect = playerAnimation;
            MainPlayer.Weapon = new Weapon("AK-47", 30, 29, 20, 2000, 200, 1, 20, true);
            // Configure sprite


            MainPlayer.Origin = SpriteUtils.GetSpriteCenter(MainPlayer);

            GameState.Players.Add(MainPlayer);


            scoreboardText = new CustomText(Fonts.Get(FontIdentifier.PixelatedSmall), 21);
            scoreboardText.DisplayedString = "Player01 - 15/2";

            Clock clock = new Clock();
            Clock sendClock = new Clock();

            while (GameWindow.IsOpen)
            {
                if (true) { 

                Time deltaTime = clock.Restart();
                    if (sendClock.ElapsedTime.AsSeconds() > (1f / 60f) && ConnectionManager.Connected)
                    {
                        sendClock.Restart();
                        SendPos(ConnectionManager.Connection);
                    }

                    GameWindow.Clear();
                    GameWindow.DispatchEvents();

                    this.ProccesKeyboardInput(deltaTime);
                    var mPos = GameWindow.MapPixelToCoords(Mouse.GetPosition(GameWindow));
                    var middlePoint = VectorUtils.GetMiddlePoint(MainPlayer.Position, mPos);

                    float rotation = VectorUtils.GetAngleBetweenVectors(MainPlayer.Position, mPos);

                    Vector2f scoreboardTextPos = new Vector2f(0, 0);

                    scoreboardText.Position = scoreboardTextPos;
                    crate.Position = new Vector2f(1000, 400);
                    bushSprite.Position = new Vector2f(500, 400);
                    MainPlayer.Weapon.Rotation = rotation;
                    MainPlayer.Weapon.Scale = rotation < -90 || rotation > 90 ? new Vector2f(1.0f, -1.0f) : new Vector2f(1.0f, 1.0f);


                    // Run player animation
                    if (animationSpeed.ElapsedTime.AsSeconds() > 0.05f && isPlayerRunning)
                    {
                        if (playerAnimation.Left == 144)
                        {
                            playerAnimation.Left = 36;
                        }
                        else
                            playerAnimation.Left += 36;
                        MainPlayer.TextureRect = playerAnimation;
                        animationSpeed.Restart();
                    }
                    else if (!isPlayerRunning)
                    {
                        MainPlayer.TextureRect = playerIdle;
                    }

                    //Draw order is important
                    //GameWindow.Draw(bgSprite);
                    if (isReloading == true)
                    {
                        ReloadGun();
                    }

                    GameWindow.Draw(map.map);
                    RenderPlayers();
                    GameWindow.Draw(crate);
                    GameWindow.Draw(bushSprite);
                    attackCooldown -= deltaTime.AsMilliseconds();
                    UpdatePickupables();
                    DrawPickupables();
                    UpdateBullets(deltaTime);
                    DrawProjectiles();

                    GameWindow.SetView(MainView);
                    GameWindow.Draw(scoreboardText);
                    ZoomedView.Center = middlePoint;
                    mPos = GameWindow.MapPixelToCoords(Mouse.GetPosition(GameWindow));
                    AimCursor.Update(mPos);
                    GameWindow.Draw(AimCursor);
                    ZoomedView.Zoom(zoomView);
                    zoomView = 1.0f;
                    GameWindow.SetView(ZoomedView);

                    GameWindow.Display();
                }

            }

        }

        private void CreatePlayers(List<PlayerDTO> playerDTOs)
        {
            foreach(var playerDto in playerDTOs)
            {
                if(GameState.Players.FindIndex(player => player.Name.Equals(playerDto.Name)) < 0)
                {
                    Player tmpPlayer = new Player(playerDto);
                    GameState.Players.Add(tmpPlayer);
                }
            }
        }

        private void UpdatePlayers(List<PlayerDTO> playerDTOs)
        {
            foreach(var dto in playerDTOs)
            {
                Player player = GameState.Players.Find(p => p.Name.Equals(dto.Name));
                if(player != null && !MainPlayer.Equals(player))
                {
                    player.Position = dto.Position;
                }
            }
        }

        private void RenderPlayers()
        {
            for (int i = 0; i < GameState.Players.Count; i++)
            {
                var player = GameState.Players[i];
                GameWindow.Draw(player);
                Vector2f playerBarPos = new Vector2f(player.Position.X, player.Position.Y - 40);
                player.PlayerBar.Position = playerBarPos;
                player.Update();
                GameWindow.Draw(player.PlayerBar);
                
                if(player.Weapon != null) GameWindow.Draw(player.Weapon);
            }
        }


        public void SendPos(HubConnection connection)
        {
            var tmpPlayer = MainPlayer.ToDTO();
            connection.SendAsync("ReceivePos", tmpPlayer);
        }

        public void ReloadGun()
        {
            if (MainPlayer.Weapon.Ammo < MainPlayer.Weapon.MagazineSize)
            {
                if (reloadClock.ElapsedTime.AsMilliseconds() > MainPlayer.Weapon.ReloadTime)
                {
                    reloadClock.Restart();
                    MainPlayer.Weapon.AmmoConsume(1);
                    Sound sound = Sounds.Get(SoundIdentifier.Reload);
                    sound.Play();
                }
            }
            if (reloadTimer.ElapsedTime.AsMilliseconds() > MainPlayer.Weapon.ReloadTime*600)
            {
                isReloading = false;
                reloadTimer.Restart();
            }
        }

        public void CreatePlayer()
        {

            ConnectionManager.Connection.SendAsync("SpawnPlayer", MainPlayer.ToDTO()).Wait();
        }

        private void ToogleScreen()
        {
            if(FullScreen != PrevFullScreen)
            {
                PrevFullScreen = FullScreen;
                var windowStyle = FullScreen ? Styles.Fullscreen : Styles.Close;
                GameWindow.Close();
                GameWindow = CreateRenderWindow(windowStyle);
            }
        }

        private void UpdateBullets(Time deltaTime)
        {
            for (int i = 0; i < bulletList.Count; i++)
            {
                Projectile bullet = bulletList[i];
                bullet.TimeSinceCreation += deltaTime.AsMilliseconds();
                if (bullet.TimeSinceCreation > 600)
                {
                    bullet.ProjectileSprite.Dispose();
                    bulletList.RemoveAt(i);
                }
                else
                {
                    bulletList[i].Move(deltaTime.AsSeconds());
                    if (CollisionTester.BoundingBoxTest(bullet.ProjectileSprite, MainPlayer))
                    {
                        Console.WriteLine("Bullet and character colliding");
                    }
                    if(CollisionTester.BoundingBoxTest(bullet.ProjectileSprite, crate))
                    {
                        Console.WriteLine("Crate was hit by a bullet");
                        bullet.ProjectileSprite.Dispose();
                        bulletList.RemoveAt(i);
                    }
                }

            }
        }

        private void UpdatePickupables()
        {
            for (int i = 0; i < GameState.Pickupables.Count; i++)
            {
                Pickupable pickup = GameState.Pickupables[i];
                if (CollisionTester.BoundingBoxTest(MainPlayer, pickup))
                {
                    pickup.Pickup(MainPlayer);
                    GameState.Pickupables.RemoveAt(i);
                }
            }
        }
        private void DrawProjectiles()
        {
            for (int i = 0; i < bulletList.Count; i++)
            {
                GameWindow.Draw(bulletList[i]);
            }
        }
        private void DrawPickupables()
        {
            for (int i = 0; i < GameState.Pickupables.Count; i++)
            {
                GameWindow.Draw(GameState.Pickupables[i]);
            }
        }

        public void GameLoop()
        {
        }
        public RenderWindow CreateRenderWindow(Styles windowStyle)
        {
            VideoMode videoMode = new VideoMode(1280, 720);
            RenderWindow window = new RenderWindow(videoMode, "ShootN'Dash v0.011", windowStyle);
            window.SetMouseCursorVisible(false);
            window.SetFramerateLimit(120);

            BindWindowEvents(window);

            return window;
        }

        public void BindWindowEvents(RenderWindow window)
        {
            window.Closed += (obj, e) => {
                ConnectionManager.Connection.StopAsync().Wait();
                window.Close(); 
            };

            // Catch key event. E
            // Event is better used for instant response
            // but can only trigger only one key at a time
            window.KeyPressed += (sender, e) => {
                Window windowEvt = (Window)sender;
                if (e.Code == Keyboard.Key.Escape)
                {
                    windowEvt.Close();
                }
                if (e.Code == Keyboard.Key.Numpad0)
                {
                    FullScreen = !FullScreen;
                }
                if (e.Code == Keyboard.Key.F1)
                {
                    ConntectToServer();
                }
            };

            window.MouseWheelScrolled += (sender, e) => {
                if (e.Wheel == Mouse.Wheel.VerticalWheel)
                {
                    zoomView += -e.Delta / 10.0f;
                    zoomView = (zoomView < 0.3f || zoomView > 2.0f) ? previousZoom : zoomView;
                    previousZoom = zoomView;
                    
                }
            };
        }

        private void AnimateCharacter()
        {

        }

        private void ProccesKeyboardInput(Time deltaTime)
        {
            float movementSpeed = 500;
            float dt = deltaTime.AsSeconds();
            float moveDistance = movementSpeed * dt;
            float movementX = 0;
            float movementY = 0;
            // Polling key presses is better than events if we
            // need to detect multiple key presses at same time
            if (Keyboard.IsKeyPressed(Keyboard.Key.W))
            {
                if (MainPlayer.CheckMovementCollision(0, -moveDistance, crate))
                {
                    Console.WriteLine("Player collided with a crate");
                }
                else
                {
                    isPlayerRunning = true;
                    movementY -= moveDistance;
                }

            }
            else
                isPlayerRunning = false;
            if (Keyboard.IsKeyPressed(Keyboard.Key.S))
            {
                if (MainPlayer.CheckMovementCollision(0, moveDistance, crate))
                {
                    Console.WriteLine("Player collided with a crate");
                }
                else
                {
                    isPlayerRunning = true;
                    movementY += moveDistance;
                }
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.D))
            {
                if (!facingRight)
                {
                    MainPlayer.Scale = new Vector2f(1, 1);
                    facingRight = true;
                }
                if (MainPlayer.CheckMovementCollision(moveDistance, 0, crate))
                {
                    Console.WriteLine("Player collided with a crate");
                }
                else
                {
                    isPlayerRunning = true;
                    movementX += moveDistance;
                }

            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.A))
            {
                if (facingRight)
                {
                    MainPlayer.Scale = new Vector2f(-1, 1);
                    facingRight = false;
                }

                if (MainPlayer.CheckMovementCollision(-moveDistance, 0, crate))
                {
                    Console.WriteLine("Player collided with a crate");
                }
                else
                {
                    isPlayerRunning = true;
                    movementX -= moveDistance;
                }

            }
            MainPlayer.Translate(movementX, movementY);

            if(Keyboard.IsKeyPressed(Keyboard.Key.M))
            {
                SpawnMedkit();
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.X))
            {
                SpawnRandomSyringe();
            }
            if(Keyboard.IsKeyPressed(Keyboard.Key.Z))
            {
                MainPlayer.ApplyDamage(-1);
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.R))
            {
                isReloading = true;
            }

            if (Mouse.IsButtonPressed(Mouse.Button.Left))
            {
                if (attackCooldown <= 0)
                {
                    attackCooldown = attackSpeed;
                    ShootBullet();
                    //bulletList.AddRange(wep.Shoot(10, new Vector2(AimCursor.Position.X,
                    //    AimCursor.Position.Y), MainPlayer.Position));

                }

            }
        }


        private void SpawnRandomSyringe()
        {
            int num = Rnd.Next(4);
            Pickupable syringe;
            switch (num)
            {
                case 0:
                    syringe = new MovementSyringe();
                    break;
                case 1:
                    syringe = new ReloadSyringe();
                    break;
                case 2:
                    syringe = new HealingSyringe();
                    break;
                default:
                    syringe = new DeflectionSyringe();
                    break;
            }
            syringe.Position = new Vector2f(Rnd.Next(1000), Rnd.Next(1000));
            GameState.Pickupables.Add(syringe);

        }


        private void SpawnMedkit()
        {
            Medkit medkit = new Medkit();
            medkit.Position = new Vector2f(Rnd.Next(1000), Rnd.Next(1000));
            GameState.Pickupables.Add(medkit);
        }


        private void ShootBullet()
        {
            if (MainPlayer.Weapon.Ammo != 0 && isReloading != true)
            {
                Sprite myBullet = new Sprite(Textures.Get(TextureIdentifier.Bullet));
                Vector2 target = new Vector2(
                    AimCursor.Position.X - MainPlayer.Position.X,
                    AimCursor.Position.Y - MainPlayer.Position.Y
                );
                target = Vector2.Normalize(target);
                Projectile bullet = new Projectile(target.X * 1000, target.Y * 1000, myBullet);
                bullet.InitializeSpriteParams(SpriteUtils.GetSpriteCenter(MainPlayer.Weapon.ProjectileSprite), MainPlayer.Position);
                bullet.ProjectileSprite.Rotation = VectorUtils.VectorToAngle(target.X, target.Y);

                bulletList.Add(bullet);
                Sound sound = Sounds.Get(SoundIdentifier.GenericGun);
                sound.Play();
                MainPlayer.Weapon.AmmoConsume(-1);
            }
        }

        public void ConntectToServer()
        {
            ConnectionManager.ConnectToHub();

            BindEvents();
            CreatePlayer();
        }

        public void BindEvents()
        {
            ConnectionManager.Connection.On<List<PlayerDTO>>("CreatePlayer", (list) =>
            {
                CreatePlayers(list);
            });

            ConnectionManager.Connection.On<List<PlayerDTO>>("UpdateState", (list) =>
            {
                UpdatePlayers(list);
            });
        }


        // ========================================================================
        // ======================== INITIALIZATION METHODS ========================
        // ========================================================================

        // Create sprites and some game objects
        // TODO: Make it better
        private void CreateSprites()
        {

            Console.WriteLine("Loading sprites...");
            IntRect rect = new IntRect(0, 0, 1280, 720);
            bgSprite = new Sprite(Textures.Get(TextureIdentifier.Background), rect);
            AimCursor.SetTexture(new Texture(Textures.Get(TextureIdentifier.AimCursor)));
            crate = new Sprite(Textures.Get(TextureIdentifier.Crate));
            bushSprite = new Sprite(Textures.Get(TextureIdentifier.Bush));
        }


        // Load all game textures
        public void LoadTextures()
        {
            Console.WriteLine("Loading textures...");

            // Iterate over all textures and load
            var allTextures = Enum.GetValues(typeof(TextureIdentifier));
            foreach(TextureIdentifier texture in allTextures)
            {
                Textures.Load(texture);
            }

            // Set special properties for some textures
            Textures.Get(TextureIdentifier.Background).Repeated = true;
        }


        // Load all music and sound efects
        public void LoadSounds()
        {
            Console.WriteLine("Loading sounds...");

            var allSounds = Enum.GetValues(typeof(SoundIdentifier));
            foreach (SoundIdentifier sound in allSounds)
            {
                Sounds.Load(sound);
            }
        }


        // Load all custom fonts
        public void LoadFonts()
        {
            Console.WriteLine("Loading fonts...");

            var allFonts = Enum.GetValues(typeof(FontIdentifier));
            foreach (FontIdentifier font in allFonts)
            {
                Fonts.Load(font);
            }
        }
    }
}
