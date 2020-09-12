﻿using Client.Objects;
using Client.UI;
using Client.Utilities;
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
        TextureHolder textures;
        View MainView { get; set; }

        Position position = new Position();

        Sprite charSprite;
        Sprite bgSprite;
        Vector2f viewSize;
        Sprite bulletSprite;

        AimCursor cursor = new AimCursor();
        List<Projectile> bulletList = new List<Projectile>();
        float attackCooldown;
        float attackSpeed = 250;
        bool facingRight = true;

        float zoomView = 1.0f;
        float previousZoom = 1.0f;

        public GameApplication() { }


        public void Run()
        {
            // Create render window
            window = CreateRenderWindow(Styles.Close);

            Vector2f winSize = window.GetView().Size;
            viewSize = winSize;


            // Sprites
            textures = new TextureHolder();
            LoadTextures();
            CreateSprites(winSize);

            // View
            View MainView = window.GetView(); ;
            window.SetView(MainView);

            // Event hadlers
            BindWindowEvents(window);

            // Set initial posision for text
            position.X = window.Size.X / 2f;
            position.Y = window.Size.Y / 2f;

            // Configure text
            Font font = new Font("C:/Windows/Fonts/arial.ttf");
            Text text = new Text("000 000", font)
            {
                CharacterSize = 10
            };
            float textWidth = text.GetLocalBounds().Width;
            float textHeight = text.GetLocalBounds().Height;
            float xOffset = text.GetLocalBounds().Left;
            float yOffset = text.GetLocalBounds().Top;
            text.Origin = new Vector2f(textWidth / 2f + xOffset, textHeight / 2f + yOffset);
            text.Position = new Vector2f(position.X, position.Y);

            // Configure sprite
            charSprite.Origin = getCenterVector(charSprite);

            Clock clock = new Clock();
            while (window.IsOpen)
            {
                ToogleScreen();
                Time deltaTime = clock.Restart();
                window.DispatchEvents();
                window.Clear();
                this.ProccesKeyboardInput(deltaTime);
                var mPos = window.MapPixelToCoords(Mouse.GetPosition(window));
                var middlePoint = (mPos + position.toVec2f())/ 2.0f;

                charSprite.Position = position.toVec2f();
                Vector2f textPos = position.toVec2f();
                textPos.Y -= charSprite.Texture.Size.Y / 2;
                text.Position = textPos;
                text.DisplayedString = String.Format("{0} {1}", position.X, position.Y);

                // Draw order is important
                window.Draw(bgSprite);
                window.Draw(text);
                window.Draw(charSprite);

                attackCooldown -= deltaTime.AsMilliseconds();
                UpdateBullets(window, deltaTime);

                cursor.Update(mPos);
                window.Draw(cursor);

                MainView.Center = middlePoint;
                window.SetView(MainView);
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
                BindWindowEvents(window);
            }
        }

        private void UpdateBullets(RenderWindow window, Time deltaTime)
        {
            for (int i = 0; i < bulletList.Count; i++)
            {
                Projectile bullet = bulletList[i];
                bullet.TimeSinceCreation += deltaTime.AsMilliseconds();
                if (bullet.TimeSinceCreation > 300)
                {
                    bullet.ProjectileSprite.Dispose();
                    bulletList.RemoveAt(i);
                }
                else
                {
                    bulletList[i].Move(deltaTime.AsSeconds());
                    window.Draw(bulletList[i].ProjectileSprite);
                }
            }
        }

        public RenderWindow CreateRenderWindow(Styles windowStyle)
        {
            VideoMode videoMode = new VideoMode(1280, 720);
            RenderWindow window = new RenderWindow(videoMode, "ShootN'Dash v0.09", windowStyle);
            window.SetActive(true);
            window.SetMouseCursorVisible(false);
            window.SetFramerateLimit(120);

            return window;
        }

        public void BindWindowEvents(RenderWindow window)
        {
            window.Closed += (obj, e) => { window.Close(); };
            window.KeyPressed +=
                // Catch key event, event is better used for instant response
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
                    MainView = window.DefaultView;
                    MainView.Zoom(zoomView);
                    previousZoom = zoomView;


                    Console.WriteLine(e.Delta);
                    Console.WriteLine(zoomView);
                }
            };
        }
        /// <summary>
        /// Get center vector of sprite
        /// </summary>
        /// <param name="charSprite">Sprite to get center of</param>
        /// <returns>Center vector</returns>
        private static Vector2f getCenterVector(Sprite charSprite)
        {
            
            // Sprite size is native texture size multiplied by sprite's scale
            float xSize = charSprite.Texture.Size.X * charSprite.Scale.X;
            float ySize = charSprite.Texture.Size.Y * charSprite.Scale.Y;
            return new Vector2f(xSize / 2, ySize / 2);
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
            Sprite myBullet = new Sprite(textures.Get(TextureID.Bullet));
            Vector2 target = new Vector2(
                cursor.position.X - charSprite.Position.X,
                cursor.position.Y - charSprite.Position.Y
            );
            target = Vector2.Normalize(target);
            Projectile bullet = new Projectile(target.X * 1000, target.Y * 1000, myBullet);
            bullet.InitializeSpriteParams(getCenterVector(bulletSprite), position.toVec2f());
            bullet.ProjectileSprite.Rotation = MathF.Atan2(target.Y, target.X) * 180 / MathF.PI;

            bulletList.Add(bullet);
        }
        private void CreateSprites(Vector2f winSize)
        {
            
            Console.WriteLine("Loading sprites...");
            charSprite = new Sprite(textures.Get(TextureID.MainCharacter));
            IntRect rect = new IntRect(0, 0, 800, 600);
            bgSprite = new Sprite(textures.Get(TextureID.Background), rect);
            bulletSprite = new Sprite(textures.Get(TextureID.Bullet));
            cursor.SetTexture(new Texture(textures.Get(TextureID.AimCursor)));

        }

        public void LoadTextures()
        {
            textures.Load(TextureID.Background, "Assets/tileGrass.png", true);
            textures.Load(TextureID.AimCursor, "Assets/cursor.png");
            textures.Load(TextureID.MainCharacter, "Assets/char.png");
            textures.Load(TextureID.Bullet, "Assets/bullet.png");
            textures.Load(TextureID.GunAk47, "Assets/gunAk47.png");
        }

        static float SinceEpoch()
        {
            double since = (DateTime.Now).Second;
            float fsince = (float)since;
            return fsince;
        }
    }
}
