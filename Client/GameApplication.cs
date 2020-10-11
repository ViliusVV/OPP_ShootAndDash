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
using Common;
using Client.Objects.Destructables;
using Client.Objects.BuilderObjects;
using System.Linq;

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

        GameState GameState { get; set; } = GameState.GetInstance();
        private ConnectionManager ConnectionManager { get; set; } = new ConnectionManager("https://shoot-and-dash.azurewebsites.net/sd-server");


        Player MainPlayer { get; set; }

        Clock FrameClock { get; set; } = new Clock();

        //Sprite bgSprite;
        //Sprite crate;
        //Sprite crate2;
        //Sprite bushSprite;

        IntRect playerAnimation = new IntRect(36, 0, 36, 64);
        IntRect playerIdle = new IntRect(0, 0, 36, 64);
        Clock animationSpeed = new Clock();
        Clock reloadClock = new Clock();
        Clock reloadTimer = new Clock();

        CustomText scoreboardText;

        AimCursor AimCursor = new AimCursor();
        public MapBuilder builder = new MapBuilder();
        public Director director;
        public TileMap tileMap;
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

            builder.LoadTextures();
            director = new Director(builder);
            director.Construct();
            tileMap = builder.GetResult();

            // Generate additional objects (destructibles, indestructibles)
            SpawningManager(20, 15);

            // View
            MainView = GameWindow.DefaultView;
            ZoomedView = new View(MainView);
            GameWindow.SetView(ZoomedView);


            MainPlayer = new Player();
            MainPlayer.IsMainPlayer = true;
            MainPlayer.Position = new Vector2f(GameWindow.Size.X / 2f, GameWindow.Size.Y / 2f);
            MainPlayer.TextureRect = playerAnimation;
            MainPlayer.Weapon = new Weapon("AK-47", 30, 29, 20, 2000, 20, 1, 20, true);
            // Configure sprite


            MainPlayer.Origin = SpriteUtils.GetSpriteCenter(MainPlayer);

            GameState.Players.Add(MainPlayer);


            scoreboardText = new CustomText(Fonts.Get(FontIdentifier.PixelatedSmall), 21);
            scoreboardText.DisplayedString = "Player01 - 15/2";

            //crate.Position = new Vector2f(1000, 400);
            //crate2.Position = new Vector2f(1000, 500);
            //bushSprite.Position = new Vector2f(500, 400);


            //GameState.Collidables.Add(crate);
            //GameState.Collidables.Add(crate2);

            //GameState.Collidables.Add(bushSprite);

            while (GameWindow.IsOpen)
            {
                if (true) {

                    Time deltaTime = FrameClock.Restart();
                    if (ConnectionManager.ActivityClock.ElapsedTime.AsSeconds() > (1f / 60f) && ConnectionManager.Connected)
                    {
                        ConnectionManager.ActivityClock.Restart();
                        SendPos(ConnectionManager.Connection);
                    }

                    GameWindow.Clear();
                    GameWindow.DispatchEvents();

                    this.ProccesKeyboardInput();
                    var mPos = GameWindow.MapPixelToCoords(Mouse.GetPosition(GameWindow));
                    var middlePoint = VectorUtils.GetMiddlePoint(MainPlayer.Position, mPos);
                    middlePoint.X += 0.375f;

                    float rotation = VectorUtils.GetAngleBetweenVectors(MainPlayer.Position, mPos);

                    Vector2f scoreboardTextPos = new Vector2f(0, 0);

                    scoreboardText.Position = scoreboardTextPos;

                    MainPlayer.Weapon.Rotation = rotation;
                    MainPlayer.Weapon.Scale = rotation < -90 || rotation > 90 ? new Vector2f(1.0f, -1.0f) : new Vector2f(1.0f, 1.0f);


                    // Run player animation
                    if (animationSpeed.ElapsedTime.AsSeconds() > 0.05f && MainPlayer.Running)
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
                    else if (!MainPlayer.Running)
                    {
                        MainPlayer.TextureRect = playerIdle;
                    }

                    //Draw order is important
                    //GameWindow.Draw(bgSprite);
                    if (MainPlayer.Weapon.Reloading == true)
                    {
                        ReloadGun();
                    }

                    UpdateLoop(deltaTime, mPos);
                    DrawLoop();

                    GameWindow.SetView(MainView);
                    GameWindow.Draw(scoreboardText);

                    ZoomedView.Center = middlePoint;

                    ZoomedView.Zoom(zoomView);
                    zoomView = 1.0f;
                    GameWindow.SetView(ZoomedView);

                    GameWindow.Display();
                }

            }

        }

        private void DrawCollidables()
        {
            foreach (var item in GameState.Collidables)
            {
                GameWindow.Draw(item);
            }
        }

        private void CreatePlayers(GameStateDTO stateDto)
        {
            foreach(var playerDto in stateDto.Players)
            {
                if(GameState.Players.FindIndex(player => player.Name.Equals(playerDto.Name)) < 0)
                {
                    Player tmpPlayer = new Player(playerDto);
                    GameState.Players.Add(tmpPlayer);
                }
            }
        }

        private void UpdatePlayers(GameStateDTO stateDTO)
        {
            foreach(var dto in stateDTO.Players)
            {
                Player player = GameState.Players.Find(p => p.Name.Equals(dto.Name));
                if (player != null && !MainPlayer.Equals(player))
                {
                    player.RefreshData(dto);
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
                player.UpdateSpeed();
                player.TranslateFromSpeed();
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
                }
            }
            if (reloadTimer.ElapsedTime.AsMilliseconds() > MainPlayer.Weapon.ReloadTime*600)
            {
                MainPlayer.Weapon.Reloading = false;
                reloadTimer.Restart();
                Sound sound = Sounds.Get(SoundIdentifier.Reload);
                sound.Play();
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
            MainPlayer.Weapon.UpdateProjectiles(deltaTime.AsSeconds());
            MainPlayer.Weapon.CheckCollisions(GameState.Collidables);
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
            MainPlayer.Weapon.DrawProjectiles(GameWindow);
        }
        private void DrawPickupables()
        {
            for (int i = 0; i < GameState.Pickupables.Count; i++)
            {
                GameWindow.Draw(GameState.Pickupables[i]);
            }
        }
        public void DrawLoop()
        {
            GameWindow.Draw(tileMap);
            RenderPlayers();
            DrawCollidables();
            //GameWindow.Draw(bushSprite);
            DrawPickupables();
            DrawProjectiles();
            GameWindow.Draw(AimCursor);
        }
        public void UpdateLoop(Time deltaTime, Vector2f mPos)
        {
            UpdatePickupables();
            UpdateBullets(deltaTime);
            AimCursor.Update(mPos);
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


        private void ProccesKeyboardInput()
        {
            // Polling key presses is better than events if we
            // need to detect multiple key presses at same time
            
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
                if (MainPlayer.Weapon.Ammo != MainPlayer.Weapon.MagazineSize)
                {
                    MainPlayer.Weapon.Reloading = true;
                }
            }

            if (Mouse.IsButtonPressed(Mouse.Button.Left))
            {
                var target = AimCursor.Position - MainPlayer.Position;
                MainPlayer.Weapon.Shoot(target);
            }

            // Testing abstract factory
            if (Keyboard.IsKeyPressed(Keyboard.Key.O))
            {
                SpawnDestructible();
                SpawnIndestructible();
            }
            // testing builder
            if (Keyboard.IsKeyPressed(Keyboard.Key.H))
            {

                director.ConstructBase();
                tileMap = builder.GetResult();
            }

        }

        private void SpawningManager(int destrCount, int indestrCount)
        {
            for (int i = 0; i < destrCount; i++)
            {
                SpawnDestructible();
            }
            for (int i = 0; i < indestrCount; i++)
            {
                SpawnIndestructible();
            }
        }

        private void SpawnDestructible()
        {
            AbstractFactory destrFactory = FactoryProducer.GetFactory("Destructible");
            List<Sprite> destructables = new List<Sprite>();
            Sprite landMineObj = destrFactory.GetDestructible("LandMine").SpawnObject();
            Sprite itemCrateObj = destrFactory.GetDestructible("ItemCrate").SpawnObject();
            destructables.Add(landMineObj);
            destructables.Add(itemCrateObj);

            foreach (Sprite destructable in destructables)
            {
                bool isSpawned = ObjectSpawnCollisionCheck(destructable);
                if (isSpawned)
                {
                    GameState.Collidables.Add(destructable);
                }
            }
        }

        private void SpawnIndestructible()
        {
            AbstractFactory indestrFactory = FactoryProducer.GetFactory("Indestructible");
            List<Sprite> indestructables = new List<Sprite>();
            Sprite barbWireObj = indestrFactory.GetIndestructible("BarbWire").SpawnObject();
            Sprite wallObj = indestrFactory.GetIndestructible("Wall").SpawnObject();
            Sprite bushObj = indestrFactory.GetIndestructible("Bush").SpawnObject();
            indestructables.Add(barbWireObj);
            indestructables.Add(wallObj);
            indestructables.Add(bushObj);

            foreach (Sprite indestructable in indestructables)
            {
                bool isSpawned = ObjectSpawnCollisionCheck(indestructable);
                if (isSpawned)
                {
                    GameState.Collidables.Add(indestructable);
                }
            }
        }

        private bool ObjectSpawnCollisionCheck(Sprite objectSprite)
        {
            bool objectSpawned = false;
            while (!objectSpawned)
            {
                objectSpawned = true;
                //destrObj.Position = new Vector2f(64 * Rnd.Next(60), 64 * Rnd.Next(45));
                objectSprite.Position = new Vector2f(Rnd.Next(4000), Rnd.Next(3000));
                foreach (Sprite collidable in GameState.Collidables.ToList())
                {
                    if (CollisionTester.BoundingBoxTest(collidable, objectSprite))
                    {
                        objectSpawned = false;
                        Console.WriteLine("does not collide");
                        break;
                    }
                }
            }
            return objectSpawned;
        }

        //

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


        public void ConntectToServer()
        {
            ConnectionManager.ConnectToHub();

            BindEvents();
            CreatePlayer();
        }

        public void BindEvents()
        {
            ConnectionManager.Connection.On<GameStateDTO>("CreatePlayer", (stateDto) =>
            {
                CreatePlayers(stateDto);
            });

            ConnectionManager.Connection.On<GameStateDTO>("UpdateState", (stateDto) =>
            {
                UpdatePlayers(stateDto);
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
//            bgSprite = new Sprite(Textures.Get(TextureIdentifier.Background), rect);
            AimCursor.SetTexture(new Texture(Textures.Get(TextureIdentifier.AimCursor)));
            //crate = new Sprite(Textures.Get(TextureIdentifier.Crate));
            //crate2 = new Sprite(Textures.Get(TextureIdentifier.Crate));
            //bushSprite = new Sprite(Textures.Get(TextureIdentifier.Bush));
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
