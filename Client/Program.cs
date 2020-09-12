using System;
using SFML.Window;
using SFML.Graphics;
using SFML.System;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Runtime.InteropServices;
using System.Net;
using SFML.Graphics.Glsl;

using System.Collections.Generic;
using Client.Objects;
using System.Threading.Tasks;
using System.Numerics;
namespace TestOpenTk2
{
    class Program
    {
        static void Main(string[] args)
        {   
            Console.WriteLine("Creating window...");
            ShootAndDashWindow window = new ShootAndDashWindow();

            window.Show();

            Console.WriteLine("Finished!");
        }
    }

    class ShootAndDashWindow
    {
        Position position = new Position();

        Texture charTexture;
        Sprite charSprite;
        Shader charShader;

        Texture bgTexture;
        Sprite bgSprite;

        Vector2f viewSize;
        Texture bulletTexture;
        Sprite bulletSprite;

        Cursor cursor = new Cursor();
        List<Projectile> bulletList = new List<Projectile>();
        float attackCooldown;
        float attackSpeed = 200;
        bool facingRight = true;
        public void Show()
        {
            // Create render window
            VideoMode mode = new VideoMode(800, 400);
            ContextSettings settings = new ContextSettings();
            settings.DepthBits = 32;
            RenderWindow window = new RenderWindow(mode, "ShootN'Dash v0.000001", Styles.None, settings);
            window.SetActive(true);
            window.SetMouseCursorVisible(false);
            window.SetFramerateLimit(120);
            Vector2f winSize = window.GetView().Size;
            viewSize = winSize;

            // Sprites
            createSprite(winSize);

            // View
            View view = new View(new Vector2f(winSize.X/2, winSize.Y/2), winSize);
            window.SetView(view);
            
            // Event hadlers
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
                };

            float zoomView = 1.0f;
            float previousZoom = 1.0f;
            window.MouseWheelScrolled += (sender, e) => {
                if (e.Wheel == Mouse.Wheel.VerticalWheel)
                {
                    zoomView -= -e.Delta / 10.0f;
                    zoomView = (zoomView < 0.3f || zoomView > 2.0f) ? previousZoom : zoomView;
                    view = window.DefaultView;
                    view.Zoom(zoomView);
                    previousZoom = zoomView;


                    Console.WriteLine(e.Delta);
                    Console.WriteLine(zoomView);
                }
            };

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
                Time deltaTime = clock.Restart();
                window.DispatchEvents();
                window.Clear();
                this.ProccesKeyboardInput(deltaTime);
                var mPos = window.MapPixelToCoords(Mouse.GetPosition(window));

                cursor.Update(mPos);
                charSprite.Position = position.toVec2f();
                Vector2f textPos = position.toVec2f();
                textPos.Y -= charSprite.Texture.Size.Y / 2;
                text.Position = textPos;
                text.DisplayedString = String.Format("{0} {1}", position.X, position.Y);

                //Console.WriteLine(1000 * 1000 / deltaTime.AsMicroseconds());
                // Draw order is important
                window.Draw(bgSprite);
                window.Draw(text);
                window.Draw(charSprite);

                attackCooldown -= deltaTime.AsMilliseconds();
                UpdateBullets(window, deltaTime);
                window.Draw(cursor);

                view.Center = position.toVec2f();
                window.SetView(view);
                window.Display();
            }
        }
        private void UpdateBullets( RenderWindow window, Time deltaTime)
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
                if(facingRight)
                {
                    charSprite.Scale = new Vector2f(-1, 1);
                    facingRight = false;
                }
                position.X -= movementSpeed * dt;
            }
            if(Mouse.IsButtonPressed(Mouse.Button.Left))
            {
                if(attackCooldown <= 0)
                {
                    attackCooldown = attackSpeed;
                    ShootBullet();
                }

            }
        }
        private void ShootBullet()
        {
            Sprite myBullet = new Sprite(bulletTexture);
            Vector2 target = new Vector2(
                cursor.position.X - charSprite.Position.X,
                cursor.position.Y - charSprite.Position.Y
            );
            target = Vector2.Normalize(target);
            Projectile bullet = new Projectile(target.X * 1000, target.Y * 1000, myBullet);
            bullet.InitializeSpriteParams(getCenterVector(bulletSprite), position.toVec2f() + new Vector2f(50, 0));
            bullet.ProjectileSprite.Rotation = MathF.Atan2(target.Y, target.X) * 180 / MathF.PI;
            
            bulletList.Add(bullet);
        }
        private void createSprite(Vector2f winSize)
        {
            Console.WriteLine("Loading sprites...");
            charTexture = new Texture("Assets/char.png");
            charSprite = new Sprite(charTexture);
            bgTexture = new Texture("Assets/groundTexture.png") { Repeated = true };
            IntRect rect = new IntRect(0, 0, 800, 600);
            bgSprite = new Sprite(bgTexture, rect);
            bulletTexture = new Texture("Assets/bullet.png");
            bulletSprite = new Sprite(bulletTexture);
            cursor.SetSprite(new Sprite(new Texture("Assets/cursor.png")));

        }

        static float SinceEpoch()
        {
            double since = (DateTime.Now).Second;
            float fsince = (float)since;
            return fsince;
        }
    }

    class Cursor : Drawable
    {
        private Sprite cursorSprite { get; set; }
        public Vector2f position { get; set; }

        public void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(cursorSprite);
        }

        public void Update(Vector2f pos)
        {
            this.position = pos;
            cursorSprite.Position = pos;
        }

        public void SetSprite(Sprite sprite)
        {
            sprite.Origin = Util.GetSpriteCenter(sprite);
            sprite.Scale = new Vector2f(2.0f, 2.0f);
            this.cursorSprite = sprite;
        }

    }

    static class Util
    {
        public static Vector2f GetSpriteCenter(Sprite sprite)
        {
            Vector2f size = GetSpriteSize(sprite);
            return new Vector2f(size.X / 2.0f, size.Y / 2.0f);
        }

        public static Vector2f GetSpriteSize(Sprite sprite)
        {
            float xSize = sprite.Scale.X * sprite.Texture.Size.X;
            float ySize = sprite.Scale.Y * sprite.Texture.Size.Y;

            return new Vector2f(xSize, ySize);
        }
    }



    class Position
    {
        public float X { get; set; }
        public float Y { get; set; }

        public Position() { }

        public Position(float X, float Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public Vector2f toVec2f()
        {
            return new Vector2f(this.X, this.Y);
        }
    }
}
