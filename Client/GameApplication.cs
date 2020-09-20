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

namespace Client
{
    class GameApplication
    {
        // Singleton instance
        private static readonly GameApplication _instance = new GameApplication();
        public static Random rnd = new Random(1000);
        private bool FullScreen { get; set; }
        private bool PrevFullScreen { get; set; }
        
        RenderWindow window;

        private TextureHolder Textures { get; } = new TextureHolder();
        private SoundHolder Sounds { get; } = new SoundHolder();
        private FontHolder Fonts { get; } = new FontHolder();
        private View MainView { get; set; }
        private View ZoomedView { get; set; }

        Position position = new Position();

        Sprite bgSprite;
        Sprite bulletSprite;
        Sprite ak47Sprite;
        Sprite playerBar;
        Sprite playerBarMask;
        Sprite crate;
        Sprite medkitSprite;
        Sprite movementSyringeSprite;
        Sprite reloadSyringeSprite;
        Sprite healingSyringeSprite;
        Sprite deflectionSyringeSprite;
        Sprite bush;
        static Texture bulletTexture = new Texture("Assets/bullet.png");
        static Sprite bullet = new Sprite(bulletTexture);
        Player mainPlayer = new Player();
        Weapon wep = new Weapon("AK-47", 10, 20, 2000, 200, 1, 20, true, bullet);

        AimCursor cursor = new AimCursor();
        List<Projectile> bulletList = new List<Projectile>();
        List<Pickupable> pickupableList = new List<Pickupable>();
        float attackCooldown;
        float attackSpeed = 150;
        bool facingRight = true;

        float zoomView = 1.0f;
        float previousZoom = 1.0f;

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
            window = CreateRenderWindow(Styles.Close);
            Vector2f winSize = window.GetView().Size;


            // Load resources
            LoadTextures();
            LoadSounds();
            LoadFonts();
            CreateSprites();
            wep.SetProjectileSprite(bulletSprite);
            // View
            MainView = window.DefaultView;
            ZoomedView = new View(MainView);
            window.SetView(ZoomedView);


            // Set initial posision for text
            mainPlayer.Position = new Vector2f(window.Size.X / 2f, window.Size.Y / 2f);

            // Configure sprite
            mainPlayer.Origin = SpriteUtils.GetSpriteCenter(mainPlayer);
            playerBar.Origin = SpriteUtils.GetSpriteCenter(playerBar);
            playerBarMask.Origin = SpriteUtils.GetSpriteCenter(playerBarMask);
            ak47Sprite.Origin = new Vector2f(SpriteUtils.GetSpriteCenter(ak47Sprite).X, 0.0f);

            playerBar.Scale = new Vector2f(1.5f, 1.5f);
            playerBarMask.Scale = new Vector2f(1.5f, 1.5f);

            Clock clock = new Clock();
            while (window.IsOpen)
            {
                ToogleScreen();
                Time deltaTime = clock.Restart();
                window.Clear();
                window.DispatchEvents();
               
                this.ProccesKeyboardInput(deltaTime);
                var mPos = window.MapPixelToCoords(Mouse.GetPosition(window));
                var middlePoint = VectorUtils.GetMiddlePoint(mainPlayer.Position, mPos);

                double dx = mPos.X - mainPlayer.Position.X;
                double dy = mPos.Y - mainPlayer.Position.Y;

                float rotation = VectorUtils.GetAngleBetweenVectors(mainPlayer.Position, mPos);

                Vector2f playerBarPos = new Vector2f(mainPlayer.Position.X, mainPlayer.Position.Y - 40);

                ak47Sprite.Position = mainPlayer.Position;
                playerBar.Position = playerBarPos;
                playerBarMask.Position = playerBarPos;
                crate.Position = new Vector2f(1000, 400);
                bush.Position = new Vector2f(500, 400);
                ak47Sprite.Rotation = rotation;
                ak47Sprite.Scale = rotation < -90 || rotation > 90 ? new Vector2f(1.0f, -1.0f) : new Vector2f(1.0f, 1.0f);
                playerBarMask.Scale = new Vector2f(mainPlayer.GetHealth(), 1.5f);


                //Draw order is important
                window.Draw(bgSprite);
                window.Draw(mainPlayer);
                window.Draw(ak47Sprite);
                window.Draw(playerBarMask);
                window.Draw(playerBar);
                window.Draw(crate);
                window.Draw(bush);
                attackCooldown -= deltaTime.AsMilliseconds();
                UpdatePickupables();
                DrawPickupables();
                UpdateBullets(deltaTime);
                DrawProjectiles();

                cursor.Update(mPos);
                window.Draw(cursor);

                ZoomedView.Center = middlePoint;
                ZoomedView.Zoom(zoomView);
                zoomView = 1.0f;
                window.SetView(ZoomedView);

                window.Display();

            }
        }

        private void ToogleScreen()
        {
            if(FullScreen != PrevFullScreen)
            {
                PrevFullScreen = FullScreen;
                var windowStyle = FullScreen ? Styles.Fullscreen : Styles.Close;
                window.Close();
                window = CreateRenderWindow(windowStyle);
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
                    if (CollisionTester.BoundingBoxTest(bullet.ProjectileSprite, mainPlayer))
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
            for (int i = 0; i < pickupableList.Count; i++)
            {
                Pickupable pickup = pickupableList[i];
                if (CollisionTester.BoundingBoxTest(mainPlayer, pickup))
                {
                    pickup.Pickup(mainPlayer);
                    pickupableList.RemoveAt(i);
                }
            }
        }
        private void DrawProjectiles()
        {
            for (int i = 0; i < bulletList.Count; i++)
            {
                window.Draw(bulletList[i]);
            }
        }
        private void DrawPickupables()
        {
            for (int i = 0; i < pickupableList.Count; i++)
            {
                window.Draw(pickupableList[i]);
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

        public void BindWindowEvents(RenderWindow window)
        {
            window.Closed += (obj, e) => { window.Close(); };
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
                if (mainPlayer.CheckMovementCollision(0, -moveDistance, crate))
                {
                    Console.WriteLine("Player collided with a crate");
                }
                else
                {
                    movementY -= moveDistance;
                }

            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.S))
            {
                if (mainPlayer.CheckMovementCollision(0, moveDistance, crate))
                {
                    Console.WriteLine("Player collided with a crate");
                }
                else
                {
                    movementY += moveDistance;
                }
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.D))
            {
                if (!facingRight)
                {
                    mainPlayer.Scale = new Vector2f(1, 1);
                    facingRight = true;
                }
                if (mainPlayer.CheckMovementCollision(moveDistance, 0, crate))
                {
                    Console.WriteLine("Player collided with a crate");
                }
                else
                {
                    movementX += moveDistance;
                }

            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.A))
            {
                if (facingRight)
                {
                    mainPlayer.Scale = new Vector2f(-1, 1);
                    facingRight = false;
                }

                if (mainPlayer.CheckMovementCollision(-moveDistance, 0, crate))
                {
                    Console.WriteLine("Player collided with a crate");
                }
                else
                {
                    movementX -= moveDistance;
                }

            }
            mainPlayer.Translate(movementX, movementY);

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
                mainPlayer.ApplyDamage(100);
            }

            if (Mouse.IsButtonPressed(Mouse.Button.Left))
            {
                if (attackCooldown <= 0)
                {
                    attackCooldown = attackSpeed;
                    ShootBullet();
                    //bulletList.AddRange(wep.Shoot(10, new Vector2(cursor.Position.X,
                    //    cursor.Position.Y), mainPlayer.Position));

                }

            }
        }
        private void SpawnRandomSyringe()
        {
            int num = rnd.Next(4);
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
            syringe.Position = new Vector2f(rnd.Next(1000), rnd.Next(1000));
            pickupableList.Add(syringe);

        }
        private void SpawnMedkit()
        {
            Medkit medkit = new Medkit();
            medkit.Texture = medkitSprite.Texture;
            medkit.Position = new Vector2f(rnd.Next(1000), rnd.Next(1000));
            pickupableList.Add(medkit);
        }
        private void ShootBullet()
        {
            Sprite myBullet = new Sprite(Textures.Get(TextureIdentifier.Bullet));
            Vector2 target = new Vector2(
                cursor.Position.X - mainPlayer.Position.X,
                cursor.Position.Y - mainPlayer.Position.Y
            );
            target = Vector2.Normalize(target);
            Projectile bullet = new Projectile(target.X * 1000, target.Y * 1000, myBullet);
            bullet.InitializeSpriteParams(SpriteUtils.GetSpriteCenter(bulletSprite), mainPlayer.Position);
            bullet.ProjectileSprite.Rotation = VectorUtils.VectorToAngle(target.X, target.Y);

            bulletList.Add(bullet);
            Sound sound = Sounds.Get(SoundIdentifier.GenericGun);
            sound.Play();
        }


        // ========================================================================
        // ======================== INITIALIZATION METHODS ========================
        // ========================================================================

        // Create sprites and some game objects
        // TODO: Make it better
        private void CreateSprites()
        {

            Console.WriteLine("Loading sprites...");
            mainPlayer.Texture = Textures.Get(TextureIdentifier.MainCharacter);
            IntRect rect = new IntRect(0, 0, 1280, 720);
            bgSprite = new Sprite(Textures.Get(TextureIdentifier.Background), rect);
            bulletSprite = new Sprite(Textures.Get(TextureIdentifier.Bullet));
            ak47Sprite = new Sprite(Textures.Get(TextureIdentifier.GunAk47));
            playerBar = new Sprite(Textures.Get(TextureIdentifier.PlayerBar));
            playerBarMask = new Sprite(Textures.Get(TextureIdentifier.PlayerBarMask));
            cursor.SetTexture(new Texture(Textures.Get(TextureIdentifier.AimCursor)));
            crate = new Sprite(Textures.Get(TextureIdentifier.Crate));
            medkitSprite = new Sprite(Textures.Get(TextureIdentifier.Medkit));
            movementSyringeSprite = new Sprite(Textures.Get(TextureIdentifier.MovementSyringe));
            reloadSyringeSprite = new Sprite(Textures.Get(TextureIdentifier.ReloadSyringe));
            healingSyringeSprite = new Sprite(Textures.Get(TextureIdentifier.HealingSyringe));
            deflectionSyringeSprite = new Sprite(Textures.Get(TextureIdentifier.DeflectionSyringe));
            bush = new Sprite(Textures.Get(TextureIdentifier.Bush));
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
