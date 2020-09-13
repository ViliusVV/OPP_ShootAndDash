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

namespace Client
{
    class GameApplication
    {
        public static GameApplication Instance { get; } = new GameApplication();

        private bool FullScreen { get; set; }
        private bool PrevFullScreen { get; set; }
        
        RenderWindow window;

        private TextureHolder Textures { get; } = new TextureHolder();
        private SoundHolder Sounds { get; } = new SoundHolder();
        private FontHolder Fonts { get; } = new FontHolder();
        View MainView { get; set; }

        Position position = new Position();

        Sprite charSprite;
        Sprite bgSprite;
        Sprite bulletSprite;
        Sprite ak47Sprite;
        Sprite playerBar;
        Sprite playerBarMask;

        AimCursor cursor = new AimCursor();
        List<Projectile> bulletList = new List<Projectile>();
        float attackCooldown;
        float attackSpeed = 150;
        bool facingRight = true;

        float zoomView = 1.0f;
        float previousZoom = 1.0f;

        public GameApplication() { }



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

            // View
            View MainView = window.DefaultView; 
            window.SetView(MainView);


            // Set initial posision for text
            position.X = window.Size.X / 2f;
            position.Y = window.Size.Y / 2f;

            // Configure text
            Font font = Fonts.Get(FontIdentifier.PixelatedSmall);
            Text text = new Text("000 000", font)
            {
                CharacterSize = 14,
                OutlineThickness = 2.0f
                
            };
            float textWidth = text.GetLocalBounds().Width;
            float textHeight = text.GetLocalBounds().Height;
            float xOffset = text.GetLocalBounds().Left + 30;
            float yOffset = text.GetLocalBounds().Top + 30;
            text.Origin = new Vector2f(textWidth / 2f + xOffset, textHeight / 2f + yOffset);
            text.Position = new Vector2f(position.X, position.Y);

            // Configure sprite
            charSprite.Origin = SpriteUtils.GetSpriteCenter(charSprite);
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
                Console.WriteLine(deltaTime.AsMicroseconds());
                window.Clear();
                window.DispatchEvents();
               
                this.ProccesKeyboardInput(deltaTime);
                var mPos = window.MapPixelToCoords(Mouse.GetPosition(window));
                var middlePoint = (mPos + position.ToVec2f())/ 2.0f;
                MainView.Center = middlePoint;
                MainView.Zoom(zoomView);
                zoomView = 1.0f;
                window.SetView(MainView);
                double dx = mPos.X - position.X;
                double dy = mPos.Y - position.Y;

                float rotation = (float)((Math.Atan2(dy, dx)) * 180 / Math.PI);
                Console.WriteLine("Rotation: {0}", rotation);

                Vector2f playerBarPos = new Vector2f(position.X, position.Y - 40);

                charSprite.Position = position.ToVec2f();
                ak47Sprite.Position = position.ToVec2f();
                playerBar.Position = playerBarPos;
                playerBarMask.Position = playerBarPos;
                ak47Sprite.Rotation = rotation;
                ak47Sprite.Scale = rotation < -90 || rotation > 90 ? new Vector2f(1.0f, -1.0f) : new Vector2f(1.0f, 1.0f);
                Vector2f textPos = position.ToVec2f();
               
                textPos.Y -= charSprite.Texture.Size.Y / 2;
                text.Position = textPos;
                text.DisplayedString = String.Format("{0} {1}", position.X, position.Y);

                //Draw order is important
                window.Draw(bgSprite);
                window.Draw(text);
                window.Draw(charSprite);
                window.Draw(ak47Sprite);
                window.Draw(playerBarMask);
                window.Draw(playerBar);
                attackCooldown -= deltaTime.AsMilliseconds();
                UpdateBullets(deltaTime);

                cursor.Update(mPos);
                window.Draw(cursor);


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
                    window.Draw(bullet);
                }
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
                    


                    Console.WriteLine(e.Delta);
                    Console.WriteLine(zoomView);
                }
            };
        }

        private void ProccesKeyboardInput(Time deltaTime)
        {
            float movementSpeed = 500;
            float dt = deltaTime.AsSeconds();
            // Polling key presses is better than events if we
            // need to detect multiple key presses at same time
            if (Keyboard.IsKeyPressed(Keyboard.Key.W))
            {
                position.Y -= movementSpeed * dt;
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.S))
            {
                position.Y += movementSpeed * dt;
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.D))
            {
                if (!facingRight)
                {
                    charSprite.Scale = new Vector2f(1, 1);
                    facingRight = true;
                }
                position.X += movementSpeed * dt;
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.A))
            {
                if (facingRight)
                {
                    charSprite.Scale = new Vector2f(-1, 1);
                    facingRight = false;
                }
                position.X -= movementSpeed * dt;
            }
            if (Mouse.IsButtonPressed(Mouse.Button.Left))
            {
                if (attackCooldown <= 0)
                {
                    attackCooldown = attackSpeed;
                    ShootBullet();
                }

            }
        }
        private void ShootBullet()
        {
            Sprite myBullet = new Sprite(Textures.Get(TextureIdentifier.Bullet));
            Vector2 target = new Vector2(
                cursor.Position.X - charSprite.Position.X,
                cursor.Position.Y - charSprite.Position.Y
            );
            target = Vector2.Normalize(target);
            Projectile bullet = new Projectile(target.X * 1000, target.Y * 1000, myBullet);
            bullet.InitializeSpriteParams(SpriteUtils.GetSpriteCenter(bulletSprite), position.ToVec2f());
            bullet.ProjectileSprite.Rotation = MathF.Atan2(target.Y, target.X) * 180 / MathF.PI;

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
            charSprite = new Sprite(Textures.Get(TextureIdentifier.MainCharacter));
            IntRect rect = new IntRect(0, 0, 1280, 720);
            bgSprite = new Sprite(Textures.Get(TextureIdentifier.Background), rect);
            bulletSprite = new Sprite(Textures.Get(TextureIdentifier.Bullet));
            ak47Sprite = new Sprite(Textures.Get(TextureIdentifier.GunAk47));
            playerBar = new Sprite(Textures.Get(TextureIdentifier.PlayerBar));
            playerBarMask = new Sprite(Textures.Get(TextureIdentifier.PlayerBarMask));
            cursor.SetTexture(new Texture(Textures.Get(TextureIdentifier.AimCursor)));

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
