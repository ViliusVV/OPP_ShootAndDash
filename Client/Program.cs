using System;
using SFML.Window;
using SFML.Graphics;
using SFML.System;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Runtime.InteropServices;
using System.Net;

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

        String glowShader = @"precision mediump float;

varying vec2 position;
uniform float time;

float random(float p) {
  return fract(sin(p)*10000.);
}

float noise(vec2 p) {
  return random(p.x + p.y*10000.);
}

vec2 sw(vec2 p) {return vec2( floor(p.x) , floor(p.y) );}
vec2 se(vec2 p) {return vec2( ceil(p.x)  , floor(p.y) );}
vec2 nw(vec2 p) {return vec2( floor(p.x) , ceil(p.y)  );}
vec2 ne(vec2 p) {return vec2( ceil(p.x)  , ceil(p.y)  );}

float smoothNoise(vec2 p) {
  vec2 inter = smoothstep(0., 1., fract(p));
  float s = mix(noise(sw(p)), noise(se(p)), inter.x);
  float n = mix(noise(nw(p)), noise(ne(p)), inter.x);
  return mix(s, n, inter.y);
  return noise(nw(p));
}

float movingNoise(vec2 p) {
  float total = 0.0;
  total += smoothNoise(p     - time);
  total += smoothNoise(p*2.  + time) / 2.;
  total += smoothNoise(p*4.  - time) / 4.;
  total += smoothNoise(p*8.  + time) / 8.;
  total += smoothNoise(p*16. - time) / 16.;
  total /= 1. + 1./2. + 1./4. + 1./8. + 1./16.;
  return total;
}

float nestedNoise(vec2 p) {
  float x = movingNoise(p);
  float y = movingNoise(p + 100.);
  return movingNoise(p + vec2(x, y));
}

void main() {
  vec2 p = position * 6.;
  float brightness = nestedNoise(p);
  gl_FragColor.rgb = vec3(brightness);
  gl_FragColor.a = 1.;
}";


        public void Show()
        {
            // Create render window
            VideoMode mode = new VideoMode(800, 400);
            ContextSettings settings = new ContextSettings();
            settings.DepthBits = 32;
            RenderWindow window = new RenderWindow(mode, "ShootN'Dash", Styles.None, settings);
            window.SetActive(true);
            Vector2f winSize = window.GetView().Size;

            // Sprites
            createSprite(winSize);

            // Setup shaders
            charShader = Shader.FromString(null, null, glowShader);
            //charShader.SetUniform("frag_ScreenResolution", new Vector2f(window.Size.X, window.Size.X));

            // Event hadlers
            window.Closed += (obj, e) => { window.Close(); };
            window.KeyPressed +=
                // Catch key event, event is better used for instant response
                // but can only trigger only one key at a time
                (sender, e) =>
                {
                    Window window = (Window)sender;
                    if (e.Code == Keyboard.Key.Escape)
                    {
                        window.Close();
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
                Time elapsed = clock.Restart();
                window.DispatchEvents();
                window.Clear();
                this.ProccesKeyboardInput();

                charSprite.Position = position.toVec2f();
                Vector2f textPos = position.toVec2f();
                textPos.Y -= charSprite.Texture.Size.Y / 2;
                text.Position = textPos;
                text.DisplayedString = String.Format("{0} {1}", position.X, position.Y);

                Console.WriteLine(1000 * 1000 / elapsed.AsMicroseconds());
                window.Draw(bgSprite);

                window.Draw(text);

                //charShader.SetUniform("time", (float)SinceEpoch());
                //RenderStates rs = new RenderStates()
                //{
                //    Shader = charShader,
                //    BlendMode = BlendMode.Multiply,
                //    Transform = Transform.Identity
                //};



                window.Draw(charSprite);

                window.Display();
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

        private void ProccesKeyboardInput()
        {
            // Polling key presses is better than events if we
            // need to detect multiple key presses at same time
            if (Keyboard.IsKeyPressed(Keyboard.Key.Up))
            {
                position.Y -= 5;
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.Down))
            {
                position.Y += 5;
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.Right))
            {
                position.X += 5;
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.Left))
            {
                position.X -= 5;
            }
        }

        private void createSprite(Vector2f winSize)
        {
            Console.WriteLine("Loading sprites...");
            charTexture = new Texture("Assets/char.png");
            charSprite = new Sprite(charTexture);
            bgTexture = new Texture("Assets/groundTexture.png") { Repeated = true };
            IntRect rect = new IntRect(0, 0, 800, 600);
            bgSprite = new Sprite(bgTexture, rect);
        }

        static float SinceEpoch()
        {
            double since = (DateTime.Now).Second;
            float fsince = (float)since;
            return fsince;
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
