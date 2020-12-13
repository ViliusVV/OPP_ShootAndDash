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
using Client.Objects.Pickupables.Strategy;
using Client.Objects.Pickupables.Decorator;
using System.Diagnostics;
using Common.Enums;
using Client.Observer;
using Client.Objects.Template;
using Client.Objects.Memento;
using Client.UI.Visitor;
using Common.Utilities.Loggers;
using System.IO;
using Client.Objects.Pickupables.Mediator;

namespace Client
{
    class GameApplication : ICommand
    {
        // Singleton instance
        private static readonly GameApplication _instance = new GameApplication();

        // Settings
        private readonly int multiplayerSendRate = 30;
        private readonly int deathTimeout = 3;


        // Screen 
        public RenderWindow GameWindow { get; set; }
        public View MainView { get; set; }
        private View ZoomedView { get; set; }
        private bool FullScreen { get; set; }
        private bool PrevFullScreen { get; set; }
        public bool HasFocus { get; set; } = true;
        float zoomView = 1.0f;
        float previousZoom = 1.0f;

        public Object SFMLLock = new Object();

        private ResourceHolderFacade ResourceFacade = ResourceHolderFacade.GetInstance();

        MapBuilder builder = new MapBuilder();
        Director director;

        GameState GameState { get; set; } = GameState.GetInstance();
        PlayerEventManager PlayerEventManager { get; } = PlayerEventManager.GetInstance();

        public bool isMementoSet = false;

        public Player MainPlayer { get; set; }
        Clock FrameClock { get; set; } = new Clock();
        Clock RespawnTimer { get; set; } = new Clock();

        public AimCursor AimCursor = new AimCursor();
        GamePlayUI GameplayUI = new GamePlayUI();
        ButtonsClass container = new ButtonsClass();

        Weapon weaponProtoype;

        public ConcreteMediator mediator = new ConcreteMediator();

        public static AbstractLogger criticalLogger = new CriticalLogger(50);
        public static AbstractLogger importantLogger = new ImportantLogger(20);
        public static AbstractLogger fileLogger = new FileLogger(30, "logs.txt");
        public static AbstractLogger defaultLogger = new DefaultLogger(0);

        public GameApplication() {
        }

        public static GameApplication GetInstance()
        {

            return _instance;
        }

        // ========================================================================
        // =========================== MAIN ENTRY POINT ===========================
        // ========================================================================

        public void Run()
        {
            // set up loggers using chain of responsibility pattern
            Console.ForegroundColor = ConsoleColor.White;
            defaultLogger.SetNext(importantLogger);
            importantLogger.SetNext(fileLogger);
            fileLogger.SetNext(criticalLogger);
            criticalLogger.SetNext(null);
            defaultLogger.LogMessage(60, "Loggers setup!");

            // Create render window
            GameWindow = CreateRenderWindow(Styles.Close);
            Vector2f winSize = GameWindow.GetView().Size;

            // Load resources
            CreateSprites();

            GameState.InitRandom(5);
            builder = new MapBuilder();
            builder.LoadSprites();
            director = new Director(builder);
            director.Construct();
            GameState.TileMap = builder.GetResult();

            // Portal creation
            PortalProspect portal = new PortalProspect();
            Caretaker m1 = new Caretaker();
            Caretaker m2 = new Caretaker();

            portal.Pos = new Vector2f(3*64f, 45*64f);
            portal.Tex = TextureHolder.GetInstance().Get(TextureIdentifier.Portal);
            m1.Memento = portal.CreateMemento();
            portal.Pos = new Vector2f(60*64f, 3*64f);
            portal.Tex = TextureHolder.GetInstance().Get(TextureIdentifier.PortalRed);
            m2.Memento = portal.CreateMemento();

            GameState.NonCollidableRep.GetIterator().Add(portal);

            // Generate additional objects (destructibles, indestructibles, pickupables)
            SpawningManager(20, 15, 60, 20);

            // View
            MainView = GameWindow.DefaultView;
            ZoomedView = new View(MainView);
            GameWindow.SetView(ZoomedView);


            // weapon prototype
            weaponProtoype = new Pistol(mediator);


            // Player init
            CreateMainPlayer();

            mediator.GetPlayerText(MainPlayer.PlayerBar);

            ConnectionManager connectionManager = new ConnectionManager("http://underpoweredserver.tplinkdns.com:51230/sd-server");
            GameState.ConnectionManagerProxy = new Managers.Proxy.ConnectionManagerProxy(connectionManager);

            bool isPlayerSpawned = ForceSpawnObject(MainPlayer);

            if (isPlayerSpawned)
            {
                GameState.Players.Add(MainPlayer);
                GameState.PlayerRep.GetIterator().Add(MainPlayer);
                GameState.PlayerRep.GetIterator().Add(SpawnFakePlayer(4000, 2000));
                GameState.PlayerRep.GetIterator().Add(SpawnFakePlayer(4000, 2200));
                GameState.PlayerRep.GetIterator().Add(SpawnFakePlayer(4000, 2400));

            }

            PlayerEventManager.Subscribe(PlayerEventType.KilledPlayer, GameplayUI.KillNotifier);
            PlayerEventManager.Subscribe(PlayerEventType.KilledPlayer, GameplayUI.Scoreboard);
           
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
                if (GameState.ConnectionManagerProxy.ActivityClock.ElapsedTime.AsSeconds() > (1f / multiplayerSendRate) 
                    && GameState.ConnectionManagerProxy.IsConnected())
                {
                    GameState.ConnectionManagerProxy.ActivityClock.Restart();
                    SendPos(GameState.ConnectionManagerProxy.Connection);
                }

                var middlePoint = VectorUtils.GetMiddlePoint(MainPlayer.Position, mPos);

                MainPlayer.Heading = VectorUtils.GetAngleBetweenVectors(MainPlayer.Position, mPos);
                MainPlayer.LookingAtPoint = mPos;

                SpawnPortal(portal, m1, m2);
                UpdateLoop(deltaTime, mPos);

                lock (SFMLLock)
                {
                    HandleDeath();
                    DrawLoop();
                    GameWindow.SetView(MainView);
                    GameWindow.Draw(GameplayUI.Scoreboard);
                    GameWindow.Draw(GameplayUI.RespawnMesage);
                    GameWindow.Draw(GameplayUI.KillNotifier);
                    GameWindow.Draw(GameplayUI.InGameLog);
                    if (container.Show)
                    GameWindow.Draw(container.composite);
                    ZoomedView.Center = middlePoint;

                    ZoomedView.Zoom(zoomView);
                    zoomView = 1.0f;
                    GameWindow.SetView(ZoomedView);

                    GameWindow.Display();
                }
            }
        }


        // ========================================================================
        // =============================== UPDATES ================================
        // ========================================================================

        //private void RenderPlayers()
        //{
        //    for (int i = 0; i < GameState.Players.Count; i++)
        //    {
        //        var player = GameState.Players[i];
        //        if (!player.IsDead)
        //        {
        //            Vector2f playerBarPos = new Vector2f(player.Position.X, player.Position.Y - 40);
        //            player.PlayerBar.Position = playerBarPos;
        //            player.UpdateSpeed();
        //            player.TranslateFromSpeed();
        //            player.Update();

        //            UpdatePickupables(player);

        //            GameWindow.Draw(player);
        //            GameWindow.Draw(player.PlayerBar);

        //            if (player.Weapon != null)
        //            {
        //                GameWindow.Draw(player.Weapon);
        //                DrawProjectiles(player);
        //                if (player.Weapon.LaserSight != null) GameWindow.Draw(player.Weapon.LaserSprite);
        //            }
        //        }
        //    }
        //}
        //private void RenderPlayers()
        //{
        //    var iter = GameState.PlayerRep.GetIterator();
        //    while(iter.HasNext())
        //    {
        //        Player player = (Player)iter.Next();
        //        if(!player.IsDead)
        //        {
        //            Vector2f playerBarPos = new Vector2f(player.Position.X, player.Position.Y - 40);
        //            player.PlayerBar.Position = playerBarPos;
        //            player.UpdateSpeed();
        //            player.TranslateFromSpeed();
        //            player.Update();

        //            UpdatePickupables(player);

        //            GameWindow.Draw(player);
        //            GameWindow.Draw(player.PlayerBar);

        //            if (player.Weapon != null)
        //            {
        //                GameWindow.Draw(player.Weapon);
        //                DrawProjectiles(player);
        //                if (player.Weapon.LaserSight != null) GameWindow.Draw(player.Weapon.LaserSprite);
        //            }
        //        }
        //    }
        //}
        private void RenderPlayers()
        {
            var iter = GameState.PlayerRep.GetIterator(500, MainPlayer);

            while (iter.HasNext())
            {
                Player player = (Player)iter.Next();
                if (!player.IsDead)
                {
                    Vector2f playerBarPos = new Vector2f(player.Position.X, player.Position.Y - 40);
                    player.PlayerBar.Position = playerBarPos;
                    if(!container.Show)
                    player.UpdateSpeed();
                    player.TranslateFromSpeed();
                    player.Update();

                    UpdatePickupables(player);

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
        }


        //private void UpdateBullets(Time deltaTime)
        //{
        //    foreach (var player in GameState.Players)
        //    {
        //        foreach (var wep in player.HoldingWeapon)
        //        {
        //            if (wep != null)
        //            {
        //                wep.UpdateProjectiles(deltaTime.AsSeconds());
        //                wep.CheckCollisions(player);
        //            }
        //        }
        //    }
        //}
        private void UpdateBullets(Time deltaTime)
        {
            var iter = GameState.PlayerRep.GetIterator();
            while(iter.HasNext())
            {
                Player player = (Player)iter.Next();
                foreach (var wep in player.HoldingWeapon)
                {
                    if(wep != null)
                    {
                        wep.UpdateProjectiles(deltaTime.AsSeconds());
                        wep.CheckCollisions(player);
                    }
                }
            }
        }

        //private void UpdatePickupables(Player player)
        //{
        //    for (int i = 0; i < GameState.Pickupables.Count; i++)
        //    {
        //        Pickupable pickup = GameState.Pickupables[i];
        //        if (CollisionTester.BoundingBoxTest(player, pickup))
        //        {
        //            pickup.Pickup(player);
        //            GameState.Pickupables.RemoveAt(i);
        //        }
        //    }
        //}
        private void UpdatePickupables(Player player)
        {
            var iter = GameState.PickupableRep.GetIterator();
            while(iter.HasNext())
            {
                Pickupable pickup = (Pickupable)iter.Next();
                if(CollisionTester.BoundingBoxTest(player, pickup))
                {
                    pickup.Pickup(player);
                    iter.Remove();
                }
            }
        }
        public void UpdateLoop(Time deltaTime, Vector2f mPos)
        {
            UpdateBullets(deltaTime);
            AimCursor.Update(mPos);
        }

        public void HandleDeath()
        {

            if (MainPlayer.IsDead)
            {
                float elapsedDeath = RespawnTimer.ElapsedTime.AsSeconds();
                var text = GameplayUI.RespawnMesage;

                if (elapsedDeath > deathTimeout)
                {
                    ForceSpawnObject(MainPlayer);
                    MainPlayer.HoldingWeapon = new Weapon[3];
                    MainPlayer.HoldingWeapon[0] = new Pistol(mediator); //for testing purposes
                    MainPlayer.Weapon = MainPlayer.HoldingWeapon[0];
                    MainPlayer.SetWeapon(MainPlayer.HoldingWeapon[0]);
                    MainPlayer.PreviousWeapon = "";
                    MainPlayer.Health = 100;
                    text.DisplayedString = "";
                }
                else
                {
                    try
                    {
                        text.DisplayedString = "You're dead. Respawning in " + (deathTimeout - elapsedDeath).ToString("N2");
                        text.Origin = new Vector2f(text.GetLocalBounds().Left / 2f, text.GetLocalBounds().Top / 2f);
                        text.Position = new Vector2f(GameWindow.GetViewport(MainView).Height / 2f, GameWindow.GetViewport(MainView).Width / 2f);
                    }
                    catch { }
                }
            }
            else
            {
                RespawnTimer.Restart();
            }
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
            GameState.ConnectionManagerProxy.Connection.SendAsync("LoginPlayerServerEvent", MainPlayer.ToDTO()).Wait();
            //GameState.ConnectionManager.Connection.SendAsync("LoginPlayerServerEvent", MainPlayer.ToDTO()).Wait();
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


        //private void DrawNonCollidables()
        //{
        //    foreach (var item in GameState.NonCollidables)
        //    {
        //        GameWindow.Draw(item);
        //    }
        //}
        private void DrawNonCollidables()
        {
            var iter = GameState.NonCollidableRep.GetIterator();
            while(iter.HasNext())
            {
                GameWindow.Draw((Sprite)iter.Next());
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


        //private void DrawPickupables()
        //{
        //    for (int i = 0; i < GameState.Pickupables.Count; i++)
        //    {
        //        GameWindow.Draw(GameState.Pickupables[i]);
        //    }
        //}
        private void DrawPickupables()
        {
            var iter = GameState.PickupableRep.GetIterator(1000, MainPlayer);
            while(iter.HasNext())
            {
                GameWindow.Draw((Pickupable)iter.Next());
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
            GameState.ConnectionManagerProxy.Connection.On<ServerGameState>("UpdateGameStateClient", (stateDto) =>
            {
                UpdatePlayers(stateDto);
            });
            //GameState.ConnectionManager.Connection.On<ServerGameState>("UpdateGameStateClient", (stateDto) =>
            //{
            //    UpdatePlayers(stateDto);
            //});
            GameState.ConnectionManagerProxy.Connection.On<ShootEventData>("ShootEventClient", (shootData) =>
            {
                Player player = GameState.Players.Find(p => p.Name.Equals(shootData.Shooter.Name));
                if (player != null)
                {
                    player.Weapon.Shoot(shootData.Target, shootData.Orgin, shootData.Rotation, player, false);
                }
                defaultLogger.LogMessage(10, shootData.ToString());
                //OurLogger.Log(shootData.ToString());

            });
            //GameState.ConnectionManager.Connection.On<ShootEventData>("ShootEventClient", (shootData) =>
            //{
            //    Player player = GameState.Players.Find(p => p.Name.Equals(shootData.Shooter.Name));
            //    if(player != null)
            //    {
            //        player.Weapon.Shoot(shootData.Target, shootData.Orgin, shootData.Rotation, player, false);
            //    }
            //    defaultLogger.LogMessage(10, shootData.ToString());
            //    //OurLogger.Log(shootData.ToString());

            //});
            GameState.ConnectionManagerProxy.Connection.On<ServerPlayer, ServerPlayer>("UpdateScoresClient", (killerServ, victimServ) =>
            {
                Player killer = GameState.Players.Find(p => p.Name.Equals(killerServ.Name));
                Player victim = GameState.Players.Find(p => p.Name.Equals(victimServ.Name));

                //OurLogger.Log($"{victimServ.Name} got shot. Before {victim.Health} after {victimServ.Health} ");
                defaultLogger.LogMessage(15, $"{victimServ.Name} got shot. Before {victim.Health} after {victimServ.Health} ");

                victim.Health = victimServ.Health;

                if (victim.IsDead)
                {
                    //OurLogger.Log($"{killerServ.Name} killed ---> {victimServ.Name}");
                    defaultLogger.LogMessage(30, $"{killerServ.Name} killed ---> {victimServ.Name}");

                    killer.Kills = killerServ.Kills;
                    victim.Deaths = victimServ.Deaths;

                    var evtData = new PlayerEventData()
                    {
                        Shooter = killer,
                        Victim = victim
                    };
                    PlayerEventManager.Notify(PlayerEventType.KilledPlayer, evtData);
                }
                else
                {
                    defaultLogger.LogMessage(15, $"{victimServ.Name} got shot. Befor ");
                    //OurLogger.Log($"{victimServ.Name} got shot. Befor ");
                }
            });
            //GameState.ConnectionManager.Connection.On<ServerPlayer, ServerPlayer>("UpdateScoresClient", (killerServ, victimServ) =>
            //{
            //    Player killer = GameState.Players.Find(p => p.Name.Equals(killerServ.Name));
            //    Player victim = GameState.Players.Find(p => p.Name.Equals(victimServ.Name));

            //    //OurLogger.Log($"{victimServ.Name} got shot. Before {victim.Health} after {victimServ.Health} ");
            //    defaultLogger.LogMessage(15, $"{victimServ.Name} got shot. Before {victim.Health} after {victimServ.Health} ");

            //    victim.Health = victimServ.Health;

            //    if (victim.IsDead)
            //    {
            //        //OurLogger.Log($"{killerServ.Name} killed ---> {victimServ.Name}");
            //        defaultLogger.LogMessage(30, $"{killerServ.Name} killed ---> {victimServ.Name}");

            //       killer.Kills = killerServ.Kills;
            //        victim.Deaths = victimServ.Deaths;

            //        var evtData = new PlayerEventData()
            //        {
            //            Shooter = killer,
            //            Victim = victim
            //        };
            //        PlayerEventManager.Notify(PlayerEventType.KilledPlayer, evtData);
            //    }
            //    else
            //    {
            //        defaultLogger.LogMessage(15, $"{victimServ.Name} got shot. Befor ");
            //        //OurLogger.Log($"{victimServ.Name} got shot. Befor ");
            //    }
            //});
        }


        public void BindWindowEvents(RenderWindow window)
        {
            window.Closed += (obj, e) => {
                //GameState.ConnectionManager.Connection.StopAsync().Wait();
                GameState.ConnectionManagerProxy.Connection.StopAsync().Wait();
                window.Close();
            };

            // Catch key event. E
            // Event is better used for instant response
            // but can only trigger only one key at a time
            window.KeyPressed += (sender, e) => {
                Window windowEvt = (Window)sender;
                if (e.Code == Keyboard.Key.Escape || GameState.CloseWindow)
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
                if (e.Code == Keyboard.Key.Tilde)
                {
                    GameplayUI.InGameLog.ShowLog = !GameplayUI.InGameLog.ShowLog;
                    MainPlayer.StopControlls = GameplayUI.InGameLog.ShowLog;

                }

                if (e.Code == Keyboard.Key.Enter)
                {
                    if (GameplayUI.InGameLog.ShowLog)
                    {
                        GameplayUI.InGameLog.ConfirmLine();
                    }
                }

                if(e.Code == Keyboard.Key.Backspace)
                {
                    if (GameplayUI.InGameLog.ShowLog)
                    {
                        GameplayUI.InGameLog.ClearLine();
                    }
                }

            };

            window.TextEntered += (sender, e) =>
            {
                if (GameplayUI.InGameLog.ShowLog)
                {
                    Window windowEvt = (Window)sender;
                    TextEventArgs args = (TextEventArgs)e;
                    args.Unicode = args.Unicode.Trim('\b');
                    GameplayUI.InGameLog.AddText(e.Unicode);
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
                //OurLogger.Log("Window gained focus");
                defaultLogger.LogMessage(10, "Window gained focus");
                this.HasFocus = true;
            };
            window.LostFocus += (sender, e) => {
                defaultLogger.LogMessage(10, "Window lost focus");
                //OurLogger.Log("Window lost focus");
                this.HasFocus = false;
            };
        }


        private void ProccesKeyboardInput()
        {
            // Polling key presses is better than events if we
            // need to detect multiple key presses at same time
            if (!GameplayUI.InGameLog.ShowLog)
            {
                if (container.Show == false)
                {
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
                        //SpawnDestructible();
                        //SpawnIndestructible();
                        SpawnTraps();
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
                // Menu
                if (Keyboard.IsKeyPressed(Keyboard.Key.P))
                {
                    container.ChangeVisibility();
                }


                if (Keyboard.IsKeyPressed(Keyboard.Key.Space))
                {
                    container.selectButton();
                    container.chooseComposite();
                    if (container.composite.tempSelected <= 2)
                    {
                        container.composite.Accept(new CursorVisitor());
                    }
                    else if (container.composite.tempSelected <= 4)
                    {
                        container.composite.Accept(new ControlsVisitor());
                    }
                    else if (container.composite.tempSelected <= 6)
                    {
                        container.composite.Accept(new VolumeVisitor());
                    }
                    container.returnComposite();
                }
            }
        }

        // ========================================================================
        // ============================== SPAWNING ================================
        // ========================================================================

        private void SpawningManager(int destrCount, int indestrCount, int syringeCount, int trapCount)
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
            for (int i = 0; i < trapCount; i++)
            {
                SpawnTraps();
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
                bool isSpawned = ForceSpawnObject(destructable);
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
                bool isSpawned = ForceSpawnObject(indestructable);
                if (isSpawned)
                {
                    if (indestructable != bushObj)
                    {
                        GameState.Collidables.Add(indestructable);
                    }
                    else
                        GameState.NonCollidableRep.GetIterator().Add(indestructable);
                        //GameState.NonCollidables.Add(indestructable);
                }
            }
        }

        private bool ForceSpawnObject(Sprite objectSprite)
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

        private void SpawnPortal(PortalProspect portal, Caretaker m1, Caretaker m2)
        {
            //defaultLogger.LogMessage(3, CollisionTester.BoundingBoxTest(MainPlayer, portal).ToString());
            //defaultLogger.LogMessage(4, MainPlayer.Position.ToString());
            //defaultLogger.LogMessage(4, portal.Position.ToString());

            if (isMementoSet && (CollisionTester.BoundingBoxTest(MainPlayer, portal) || Keyboard.IsKeyPressed(Keyboard.Key.Num9)))
            {
                portal.RestoreMemento(m2.Memento);
                MainPlayer.Position = new Vector2f(16 * 64f, 16 * 64f);
                isMementoSet = false;
                OurLogger.Log("200; 200");
            }
            else if (!isMementoSet && (CollisionTester.BoundingBoxTest(MainPlayer, portal) || Keyboard.IsKeyPressed(Keyboard.Key.Num0)))
            {
                portal.RestoreMemento(m1.Memento);
                MainPlayer.Position = new Vector2f(16 * 64f, 16 * 64f);
                OurLogger.Log("400; 400");
                isMementoSet = true;
            }
        }

        private void SpawnTraps()
        {
            TrapSpawner trap;
            int num = GameState.Random.Next(4);
            switch (num)
            {
                case 0:
                    trap = new DamageTrapBuilder(); 
                    break;
                case 1:
                    trap = new FreezeTrapBuilder();
                    break;
                default:
                    trap = new RemoveAmmoTrapBuilder();
                    break;
            }

            bool isSpawned = ForceSpawnObject(trap);
            if (isSpawned)
            {
                trap.BuildTrap();
                GameState.PickupableRep.GetIterator().Add(trap);
            }
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

            bool isSpawned = ForceSpawnObject(syringe);
            if (isSpawned)
            {
                GameState.PickupableRep.GetIterator().Add(syringe);
                //GameState.Pickupables.Add(syringe);
            }


        }


        private void SpawnMedkit()
        {
            PowerupFactory pickFactory = new PowerupFactory();
            Pickupable medkit = pickFactory.GetPowerup("Medkit");
            bool isSpawned = ForceSpawnObject(medkit);
            if (isSpawned)
            {
                GameState.PickupableRep.GetIterator().Add(medkit);
                //GameState.Pickupables.Add(medkit);
            }
        }

        private Player SpawnFakePlayer(float x, float y)
        {
            Player FakePlayer = new Player();
            FakePlayer.Position = new Vector2f(x, y);

            //new Weapon("AK-47", 50, 20, 2000, 50, 5000, 50);
            FakePlayer.HoldingWeapon[0] = (Weapon)weaponProtoype.Clone(); //for testing purposes
            FakePlayer.Weapon = FakePlayer.HoldingWeapon[0];
            FakePlayer.SetWeapon(FakePlayer.HoldingWeapon[0]);
            FakePlayer.PreviousWeapon = "";
            return FakePlayer;
        }


        // ========================================================================
        // ======================== INITIALIZATION METHODS ========================
        // ========================================================================

        private void CreateSprites()
        {
            //OurLogger.Log("Loading sprites...");
            defaultLogger.LogMessage(30, "Loading sprites...");

            IntRect rect = new IntRect(0, 0, 1280, 720);
            AimCursor.SetTexture(new Texture(ResourceFacade.Textures.Get(TextureIdentifier.AimCursor)));
        }

        private void CreateMainPlayer()
        {
            MainPlayer = new Player();
            MainPlayer.IsMainPlayer = true;
            MainPlayer.Position = new Vector2f(GameWindow.Size.X / 2f, GameWindow.Size.Y / 2f);

            //new Weapon("AK-47", 50, 20, 2000, 50, 5000, 50);
            MainPlayer.HoldingWeapon[0] = (Weapon)weaponProtoype.Clone();
            MainPlayer.Weapon = MainPlayer.HoldingWeapon[0];
            MainPlayer.SetWeapon(MainPlayer.HoldingWeapon[0]);
            MainPlayer.PreviousWeapon = "";
        }


        // ========================================================================
        // ================================ META ==================================
        // ========================================================================

        public void ConntectToServer()
        {
            //if(GameState.ConnectionManager.ConnectToHub())
            //{
            //    BindEvents();
            //    CreatePlayer();
            //}
            if(GameState.ConnectionManagerProxy.ConnectToHub())
            {
                BindEvents();
                CreatePlayer();
            }
            else
            {
                MainPlayer.Weapon.Ammo = 0;
                MainPlayer.Weapon.Reload();
            }
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
