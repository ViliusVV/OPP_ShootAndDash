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
using System.Text;
using Client.Collisions;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.InteropServices.ComTypes;
using Client.Models;
using Client.Objects.Pickupables;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.SignalR.Client;
using System.Runtime.CompilerServices;
using Common.DTO;
using Common.Utilities;
using System.Reflection.Metadata;
using Client.Managers;

namespace Client
{
    class GameApplication
    {
        // Singleton instance
        private static readonly GameApplication _instance = new GameApplication();

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
        private ConnectionManager ConnectionManager { get; set; }
        public static Random Rnd { get; set; } = new Random();


        Player MainPlayer { get; set; }

        Sprite bgSprite;
        Sprite bulletSprite;
        Sprite ak47Sprite;
        Sprite crate;
        Sprite medkitSprite;
        Sprite movementSyringeSprite;
        Sprite reloadSyringeSprite;
        Sprite healingSyringeSprite;
        Sprite deflectionSyringeSprite;
        Sprite bushSprite;
        Sprite scoreboardSprite;

        IntRect playerAnimation = new IntRect(36, 0, 36, 64);
        IntRect playerIdle = new IntRect(0, 0, 36, 64);
        Clock animationSpeed = new Clock();
        Clock reloadClock = new Clock();
        Clock reloadTimer = new Clock();
        bool isPlayerRunning = false;

        CustomText scoreboardText;
        Text txt = new Text();

        Weapon wep;

        AimCursor AimCursor = new AimCursor();
        List<Projectile> bulletList = new List<Projectile>();
        float attackCooldown;
        float attackSpeed = 150;
        bool facingRight = true;
        bool isReloading = false;

        GameState GameState = new GameState();


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

            wep = new Weapon("AK-47", 30, 29, 20, 2000, 200, 1, 20, true);
            wep.SetProjectileSprite(bulletSprite);

            // View
            MainView = GameWindow.DefaultView;
            ZoomedView = new View(MainView);
            GameWindow.SetView(ZoomedView);


            MainPlayer = new Player();
            MainPlayer.Position = new Vector2f(GameWindow.Size.X / 2f, GameWindow.Size.Y / 2f);

            MainPlayer.TextureRect = playerAnimation;
            // Configure sprite
            MainPlayer.Origin = SpriteUtils.GetSpriteCenter(MainPlayer);
            ak47Sprite.Origin = new Vector2f(SpriteUtils.GetSpriteCenter(ak47Sprite).X, 0.0f);


            // Connect to game hub server
            //ConnectionManager = new ConnectionManager("https://shoot-and-dash.azurewebsites.net/sd-server");
            ConnectionManager = new ConnectionManager("http://localhost:5000/sd-server");
            BindEvents();

            CreatePlayer();

            MainPlayer.Weapon = wep;
            GameState.Players.Add(MainPlayer);


            scoreboardText = new CustomText(Fonts.Get(FontIdentifier.PixelatedSmall), 21);
            scoreboardText.DisplayedString = "Testing";

            Clock clock = new Clock();
            Clock sendClock = new Clock();

            while (GameWindow.IsOpen)
            {
                if (true) { 

                Time deltaTime = clock.Restart();
                    if (sendClock.ElapsedTime.AsSeconds() > (1f / 60f))
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

                    Vector2f scoreboardPos = new Vector2f(0, 0);
                    Vector2f scoreboardTextPos = new Vector2f(0, 0);

                    ak47Sprite.Position = MainPlayer.Position;
                    scoreboardSprite.Position = scoreboardPos;
                    scoreboardText.Position = scoreboardTextPos;
                    crate.Position = new Vector2f(1000, 400);
                    bushSprite.Position = new Vector2f(500, 400);
                    ak47Sprite.Rotation = rotation;
                    ak47Sprite.Scale = rotation < -90 || rotation > 90 ? new Vector2f(1.0f, -1.0f) : new Vector2f(1.0f, 1.0f);


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
                    GameWindow.Draw(ak47Sprite);
                    GameWindow.Draw(crate);
                    GameWindow.Draw(bushSprite);
                    attackCooldown -= deltaTime.AsMilliseconds();
                    UpdatePickupables();
                    DrawPickupables();
                    UpdateBullets(deltaTime);
                    DrawProjectiles();
                    AimCursor.Update(mPos);
                    GameWindow.Draw(AimCursor);

                    GameWindow.SetView(MainView);
                    GameWindow.Draw(scoreboardSprite);
                    GameWindow.Draw(scoreboardText);

                    ZoomedView.Center = middlePoint;
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
            foreach (var player in GameState.Players)
            {
                GameWindow.Draw(player);
                Vector2f playerBarPos = new Vector2f(player.Position.X, player.Position.Y - 40);
                player.PlayerBar.Position = playerBarPos;
                player.UpdatePlayerBar();
                GameWindow.Draw(player.PlayerBar);
            }
        }


        public void SendPos(HubConnection connection)
        {
            var tmpPlayer = MainPlayer.ToDTO();
            connection.SendAsync("ReceivePos", tmpPlayer);
        }

        public void ReloadGun()
        {
            if (wep.Ammo < wep.MagazineSize)
            {
                if (reloadClock.ElapsedTime.AsMilliseconds() > wep.ReloadTime)
                {
                    reloadClock.Restart();
                    wep.AmmoConsume(1);
                    Sound sound = Sounds.Get(SoundIdentifier.Reload);
                    sound.Play();
                }
            }
            if (reloadTimer.ElapsedTime.AsMilliseconds() > wep.ReloadTime*600)
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
            window.KeyPressed +=
                // Catch key event. E
                // Event is better used for instant response
                // but can only trigger only one key at a time
                (sender, e) =>
                {
                    Window windowEvt = (Window)sender;
                    if (e.Code == Keyboard.Key.Escape)
                    {
                        windowEvt.Close();
                    }
                    if (e.Code == Keyboard.Key.Numpad0)
                    {
                        FullScreen = !FullScreen;
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
                    syringe.Texture = movementSyringeSprite.Texture;
                    break;
                case 1:
                    syringe = new ReloadSyringe();
                    syringe.Texture = reloadSyringeSprite.Texture;
                    break;
                case 2:
                    syringe = new HealingSyringe();
                    syringe.Texture = healingSyringeSprite.Texture;
                    break;
                default:
                    syringe = new DeflectionSyringe();
                    syringe.Texture = deflectionSyringeSprite.Texture;
                    break;
            }
            syringe.Position = new Vector2f(Rnd.Next(1000), Rnd.Next(1000));
            GameState.Pickupables.Add(syringe);

        }


        private void SpawnMedkit()
        {
            Medkit medkit = new Medkit();
            medkit.Texture = medkitSprite.Texture;
            medkit.Position = new Vector2f(Rnd.Next(1000), Rnd.Next(1000));
            GameState.Pickupables.Add(medkit);
        }


        private void ShootBullet()
        {
            if (wep.Ammo != 0 && isReloading != true)
            {
                Sprite myBullet = new Sprite(Textures.Get(TextureIdentifier.Bullet));
                Vector2 target = new Vector2(
                    AimCursor.Position.X - MainPlayer.Position.X,
                    AimCursor.Position.Y - MainPlayer.Position.Y
                );
                target = Vector2.Normalize(target);
                Projectile bullet = new Projectile(target.X * 1000, target.Y * 1000, myBullet);
                bullet.InitializeSpriteParams(SpriteUtils.GetSpriteCenter(bulletSprite), MainPlayer.Position);
                bullet.ProjectileSprite.Rotation = VectorUtils.VectorToAngle(target.X, target.Y);

                bulletList.Add(bullet);
                Sound sound = Sounds.Get(SoundIdentifier.GenericGun);
                sound.Play();
                wep.AmmoConsume(-1);
            }
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
            bulletSprite = new Sprite(Textures.Get(TextureIdentifier.Bullet));
            ak47Sprite = new Sprite(Textures.Get(TextureIdentifier.GunAk47));
            AimCursor.SetTexture(new Texture(Textures.Get(TextureIdentifier.AimCursor)));
            crate = new Sprite(Textures.Get(TextureIdentifier.Crate));
            medkitSprite = new Sprite(Textures.Get(TextureIdentifier.Medkit));
            movementSyringeSprite = new Sprite(Textures.Get(TextureIdentifier.MovementSyringe));
            reloadSyringeSprite = new Sprite(Textures.Get(TextureIdentifier.ReloadSyringe));
            healingSyringeSprite = new Sprite(Textures.Get(TextureIdentifier.HealingSyringe));
            deflectionSyringeSprite = new Sprite(Textures.Get(TextureIdentifier.DeflectionSyringe));
            bushSprite = new Sprite(Textures.Get(TextureIdentifier.Bush));
            scoreboardSprite = new Sprite(Textures.Get(TextureIdentifier.ScoreboardBox));
            //scoreboard.SetTexture(new Texture(Textures.Get(TextureIdentifier.ScoreboardBox)));
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
