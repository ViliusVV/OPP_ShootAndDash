﻿using Client.Config;
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
using Client.Objects.Pickupables.Strategy;
using Client.Objects.Pickupables.Decorator;
using System.Diagnostics;
using Common.Enums;

namespace Client
{
    class GameApplication : ICommand
    {
        // Singleton instance
        private static readonly GameApplication _instance = new GameApplication();

        // Settings
        private readonly int multiplayerSendRate = 30;


        // Screen 
        RenderWindow GameWindow { get; set; }
        private View MainView { get; set; }
        private View ZoomedView { get; set; }
        private bool FullScreen { get; set; }
        private bool PrevFullScreen { get; set; }
        public bool HasFocus { get; set; } = true;
        float zoomView = 1.0f;
        float previousZoom = 1.0f;

        private ResourceHolderFacade ResourceFacade = ResourceHolderFacade.GetInstance();

        MapBuilder builder = new MapBuilder();
        Director director;

        GameState GameState { get; set; } = GameState.GetInstance();
       


        Player MainPlayer { get; set; }

        Clock FrameClock { get; set; } = new Clock();

        CustomText scoreboardText;

        AimCursor AimCursor = new AimCursor();

        Weapon weaponProtoype;

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
            CreateSprites();

            builder = new MapBuilder();
            builder.LoadSprites();
            director = new Director(builder);
            director.Construct();
            GameState.TileMap = builder.GetResult();

            // Generate additional objects (destructibles, indestructibles, pickupables)
            SpawningManager(20, 15, 60);

            // View
            MainView = GameWindow.DefaultView;
            ZoomedView = new View(MainView);
            GameWindow.SetView(ZoomedView);


            // weapon prototype
            weaponProtoype = new Pistol();

            // Player init
            CreateMainPlayer();

            GameState.ConnectionManager = new ConnectionManager("http://localhost:5000/sd-server");


            scoreboardText = new CustomText(ResourceFacade.Fonts.Get(FontIdentifier.PixelatedSmall), 21);
            scoreboardText.DisplayedString = "Player01 - 15/2";

            Vector2f scoreboardTextPos = new Vector2f(0, 0);

            scoreboardText.Position = scoreboardTextPos;

            bool isPlayerSpawned = ObjectSpawnCollisionCheck(MainPlayer);
            if (isPlayerSpawned)
            {
                GameState.Players.Add(MainPlayer);
            }
           
            var mPos = GameWindow.MapPixelToCoords(Mouse.GetPosition(GameWindow));
            while (GameWindow.IsOpen)
            {
                GameWindow.Clear();
                GameWindow.DispatchEvents();

                if (this.HasFocus)
                {
                    this.ProccesKeyboardInput();
                    mPos = GameWindow.MapPixelToCoords(Mouse.GetPosition(GameWindow));
                }

                Time deltaTime = FrameClock.Restart();
                if (GameState.ConnectionManager.ActivityClock.ElapsedTime.AsSeconds() > (1f / multiplayerSendRate) 
                    && GameState.ConnectionManager.Connected)
                {
                    GameState.ConnectionManager.ActivityClock.Restart();
                    SendPos(GameState.ConnectionManager.Connection);
                }


                  

                var middlePoint = VectorUtils.GetMiddlePoint(MainPlayer.Position, mPos);

                MainPlayer.Heading = VectorUtils.GetAngleBetweenVectors(MainPlayer.Position, mPos);
                MainPlayer.LookingAtPoint = mPos;


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


        // ========================================================================
        // =============================== UPDATES ================================
        // ========================================================================

        private void RenderPlayers()
        {
            for (int i = 0; i < GameState.Players.Count; i++)
            {
                var player = GameState.Players[i];
                Vector2f playerBarPos = new Vector2f(player.Position.X, player.Position.Y - 40);
                player.PlayerBar.Position = playerBarPos;
                player.UpdateSpeed();
                player.TranslateFromSpeed();
                player.Update();

                GameWindow.Draw(player);
                GameWindow.Draw(player.PlayerBar);

                if (player.Weapon != null)
                {
                    GameWindow.Draw(player.Weapon);
                    DrawProjectiles(player);
                    if (player.Weapon.LaserSight != null) GameWindow.Draw(player.Weapon.LaserSprite);
                }


            }
        }


        private void UpdateBullets(Time deltaTime)
        {
            foreach (var player in GameState.Players)
            {
                foreach (var item in player.HoldingWeapon)
                {
                    if (item != null)
                    {
                        item.UpdateProjectiles(deltaTime.AsSeconds());
                        item.CheckCollisions(GameState.Collidables);
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

        public void UpdateLoop(Time deltaTime, Vector2f mPos)
        {
            UpdatePickupables();
            UpdateBullets(deltaTime);
            AimCursor.Update(mPos);
        }



        // ========================================================================
        // =========================== UPDATES (MULTIPLAYER) ======================
        // ========================================================================

        private void UpdatePlayers(ServerGameState stateDTO)
        {
            foreach (var dto in stateDTO.Players)
            {
                Player player = GameState.Players.Find(p => p.Name.Equals(dto.Name));
                if (player != null && !MainPlayer.Equals(player))
                {
                    player.RefreshData(dto);
                }

                if (player == null)
                {
                    Player tmpPlayer = new Player(dto);
                    GameState.Players.Add(tmpPlayer);
                }
            }
        }


        public void SendPos(HubConnection connection)
        {
            var tmpPlayer = MainPlayer.ToDTO();
            connection.SendAsync("UpdateGameStateServer", tmpPlayer);
        }


        public void CreatePlayer()
        {

            GameState.ConnectionManager.Connection.SendAsync("LoginPlayerServerEvent", MainPlayer.ToDTO()).Wait();
        }

        // ========================================================================
        // =============================== DRAWING ================================
        // ========================================================================

        private void DrawCollidables()
        {
            foreach (var item in GameState.Collidables)
            {
                GameWindow.Draw(item);
            }
        }


        private void DrawNonCollidables()
        {
            foreach (var item in GameState.NonCollidables)
            {
                GameWindow.Draw(item);
            }
        }


        private void DrawProjectiles(Player player)
        {
            foreach (var item in player.HoldingWeapon)
            {
                if (item != null)
                {
                    item.DrawProjectiles(GameWindow);
                }
            }
            //MainPlayer.Weapon.DrawProjectiles(GameWindow);
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
            GameWindow.Draw(GameState.TileMap);
            RenderPlayers();
            DrawCollidables();
            DrawNonCollidables();
            DrawPickupables();
            GameWindow.Draw(AimCursor);
        }

        // ========================================================================
        // ============================  EVENTS AND INPUTS ========================
        // ========================================================================

        public void BindEvents()
        {

            GameState.ConnectionManager.Connection.On<ServerGameState>("UpdateGameStateClient", (stateDto) =>
            {
                UpdatePlayers(stateDto);
            });

            GameState.ConnectionManager.Connection.On<ShootEventData>("ShootEventClient", (shootData) =>
            {
                Player player = GameState.Players.Find(p => p.Name.Equals(shootData.Shooter.Name));
                if(player != null)
                {
                    player.Weapon.Shoot(shootData.Target, shootData.Orgin, shootData.Rotation, player, false);
                }

                OurLogger.Log(shootData.ToString());

            });
        }


        public void BindWindowEvents(RenderWindow window)
        {
            window.Closed += (obj, e) => {
                GameState.ConnectionManager.Connection.StopAsync().Wait();
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

            window.GainedFocus += (sender, e) => {
                OurLogger.Log("Window gained focus");
                this.HasFocus = true;
            };
            window.LostFocus += (sender, e) => {
                OurLogger.Log("Window lost focus");
                this.HasFocus = false;
            };
        }


        private void ProccesKeyboardInput()
        {
            // Polling key presses is better than events if we
            // need to detect multiple key presses at same time
            if (Keyboard.IsKeyPressed(Keyboard.Key.X))
            {
                SpawnRandomSyringe();
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.Z))
            {
                MainPlayer.AddHealth(-1);
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.R))
            {
                MainPlayer.Weapon.Reload();
            }

            if (Mouse.IsButtonPressed(Mouse.Button.Left))
            {
                var target = AimCursor.Position - MainPlayer.Position;
                MainPlayer.Weapon.Shoot(target, MainPlayer);
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
                GameState.TileMap = builder.GetResult();
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.Q))
            {
                MainPlayer.Toggle();
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.G))
            {
                MainPlayer.DropWeapon();
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.Num1))
            {
                if (MainPlayer.Weapon.Name != MainPlayer.HoldingWeapon[0].Name)
                    MainPlayer.SetWeapon(MainPlayer.HoldingWeapon[0]);
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.Num2))
            {
                if (MainPlayer.HoldingWeapon[1] != null && MainPlayer.Weapon.Name != MainPlayer.HoldingWeapon[1].Name)
                    MainPlayer.SetWeapon(MainPlayer.HoldingWeapon[1]);
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.Num3))
            {
                if (MainPlayer.HoldingWeapon[2] != null && MainPlayer.Weapon.Name != MainPlayer.HoldingWeapon[2].Name)
                    MainPlayer.SetWeapon(MainPlayer.HoldingWeapon[2]);
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.M))
            {
                Toggle();
            }
        }


        // ========================================================================
        // ============================== SPAWNING ================================
        // ========================================================================

        private void SpawningManager(int destrCount, int indestrCount, int syringeCount)
        {
            for (int i = 0; i < destrCount; i++)
            {
                SpawnDestructible();
            }
            for (int i = 0; i < indestrCount; i++)
            {
                SpawnIndestructible();
            }
            for (int i = 0; i < syringeCount; i++)
            {
                SpawnRandomSyringe();
            }
        }


        private void SpawnDestructible()
        {
            AbstractFactory destrFactory = FactoryProducer.GetFactory("Destructible");
            List<Sprite> destructables = new List<Sprite>();
            Sprite healthCrateObj = destrFactory.GetDestructible("HealthCrate").SpawnObject();
            Sprite itemCrateObj = destrFactory.GetDestructible("ItemCrate").SpawnObject();
            destructables.Add(healthCrateObj);
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
                    if (indestructable != bushObj)
                    {
                        GameState.Collidables.Add(indestructable);
                    }
                    else
                        GameState.NonCollidables.Add(indestructable);
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
                objectSprite.Position = new Vector2f(GameState.Random.Next(GameState.TileMap.Length * 64), GameState.Random.Next(GameState.TileMap.Width * 64));
                foreach (Sprite collidable in GameState.Collidables.ToList())
                {
                    if (CollisionTester.BoundingBoxTest(collidable, objectSprite))
                    {
                        objectSpawned = false;
                        break;
                    }
                }
            }
            return objectSpawned;
        }


        private void SpawnRandomSyringe()
        {
            int num = GameState.Random.Next(5);
            PowerupFactory pickFactory = new PowerupFactory();
            Pickupable syringe;
            switch (num)
            {
                case 0:
                    syringe = pickFactory.GetPowerup("MovementSyringe");
                    break;
                case 1:
                    syringe = pickFactory.GetPowerup("ReloadSyringe");
                    break;
                case 2:
                    syringe = pickFactory.GetPowerup("HealingSyringe");
                    break;
                default:
                    syringe = pickFactory.GetPowerup("DeflectionSyringe");
                    break;
            }

            bool isSpawned = ObjectSpawnCollisionCheck(syringe);
            if (isSpawned)
            {
                GameState.Pickupables.Add(syringe);
            }


        }


        private void SpawnMedkit()
        {
            PowerupFactory pickFactory = new PowerupFactory();
            Pickupable medkit = pickFactory.GetPowerup("Medkit");
            bool isSpawned = ObjectSpawnCollisionCheck(medkit);
            if (isSpawned)
            {
                GameState.Pickupables.Add(medkit);
            }
        }



        // ========================================================================
        // ======================== INITIALIZATION METHODS ========================
        // ========================================================================

        private void CreateSprites()
        {
            OurLogger.Log("Loading sprites...");
            IntRect rect = new IntRect(0, 0, 1280, 720);
            AimCursor.SetTexture(new Texture(ResourceFacade.Textures.Get(TextureIdentifier.AimCursor)));
        }

        private void CreateMainPlayer()
        {
            MainPlayer = new Player(PlayerSkinType.TriggerHappyHipster);
            MainPlayer.IsMainPlayer = true;
            MainPlayer.Position = new Vector2f(GameWindow.Size.X / 2f, GameWindow.Size.Y / 2f);

            //new Weapon("AK-47", 50, 20, 2000, 50, 5000, 50);
            MainPlayer.HoldingWeapon[0] = (Weapon)weaponProtoype.Clone(); //for testing purposes
            MainPlayer.Weapon = MainPlayer.HoldingWeapon[0];
            MainPlayer.SetWeapon(MainPlayer.HoldingWeapon[0]);
            MainPlayer.PreviousWeapon = "";
        }


        // ========================================================================
        // ================================ META ==================================
        // ========================================================================

        public void ConntectToServer()
        {
            GameState.ConnectionManager.ConnectToHub();

            BindEvents();
            CreatePlayer();
        }


        public void Toggle()
        {
            if (ResourceFacade.CurrentVolume.GetVolume() > 0)
            {
                ResourceFacade.CurrentVolume.SetVolume(0);
            }
            else
            {
                ResourceFacade.CurrentVolume.SetVolume(50);
            }
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

        private void ToogleScreen()
        {
            if (FullScreen != PrevFullScreen)
            {
                PrevFullScreen = FullScreen;
                var windowStyle = FullScreen ? Styles.Fullscreen : Styles.Close;
                GameWindow.Close();
                GameWindow = CreateRenderWindow(windowStyle);
            }
        }
    }
}
