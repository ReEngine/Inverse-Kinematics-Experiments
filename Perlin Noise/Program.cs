using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;



namespace Perlin_Noise
{
    class Program
    {

        public static int TestCounter = 0;

        public static uint _SWidth = VideoMode.DesktopMode.Width;
        public static uint _SHeight = VideoMode.DesktopMode.Height;

        const uint _ResMult = 10;
        public static uint _Width = _SWidth / _ResMult;
        public static uint _Height = _SHeight / _ResMult;

        public static uint mouseMult = _SWidth / _Width;

        public static Texture MainViewPort = new Texture(_Width, _Height);

        public static byte[] pixels = new byte[_Width * _Height * 4];
        public static Color[] cpixels = new Color[_Width * _Height];
        public static Color[,] field = new Color[_Width, _Height];
        public static Color[,] BG = new Color[_Width, _Height];
        public static int dotsAmount = 1000;                                                   // Amount of dots
        public static Vector2F[] dots = new Vector2F[dotsAmount + 1];
        public static Vector2F[] dotsVels = new Vector2F[dotsAmount + 1];

        public static Vector2F prevPos = new Vector2F(0, 0);

        public static int zOffset = 0;
        public static int xOffset = 0;
        public static int yOffset = 0;
        public static float fOffset = 0f;

        public static Vector2F windowMP = new Vector2F(_Width / 2, _Height / 2);

        public static int lineLength = 10;

        public static ulong CurrentTick = 0;
        public static ulong PrevTick = 0;

        public static int z = 0;

        public static Vector2F point;
        public static Vector2F pointVel = new Vector2F(0, 0);


        static float mouseDir = 0;

        //static void OnMouseScroll(object sender, EventArgs e)
        //{
        //    MouseWheelScrollEventArgs mouseWE = (MouseWheelScrollEventArgs)e;
        //    lineLength += (int)mouseWE.Delta;
        //}

        static void Main(string[] args)
        {
            for (int i = 0; i < dotsAmount; i++)
            {
                Random rnd = new Random();
                int x = rnd.Next(0, (int)_Width);
                int y = rnd.Next(0, (int)_Height);


                dots[i] = new Vector2F(x, y);
                dotsVels[i] = new Vector2F(0, 0);
                // dotsVels[i] = new Vector2F((float)rnd.Next(-20, 20) / 10, (float)rnd.Next(-20, 20) / 10);
            }

            point = new Vector2F(new Random().Next(0, (int)_Width), new Random().Next(0, (int)_Height));
            FastNoiseLite noise = new FastNoiseLite(1337);
            noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
            noise.SetFrequency(0.020f);
            noise.SetFractalType(FastNoiseLite.FractalType.FBm);
            noise.SetFractalOctaves(5);
            RenderWindow window = new RenderWindow(new SFML.Window.VideoMode(_SWidth, _SHeight), "Pixels");
            //window.MouseWheelScrolled += new EventHandler<MouseWheelScrollEventArgs>(OnMouseScroll);
            window.SetVerticalSyncEnabled(false);


            int amount = 5;                                                           //Amount of paws
            float da = (float)(Math.PI * 2 / amount);
            InverseKinematicsReacher[] paws = new InverseKinematicsReacher[amount];
            Vector2F[] positions = new Vector2F[amount];
            int counter = 0;
            for (float a = 0; a < Math.PI * 2; a += da)
            {
                float x = (float)((Math.Cos(a) * 10));
                float y = (float)((Math.Sin(a) * 10));
                paws[counter] = new InverseKinematicsReacher(x, y, 20, 4);
                paws[counter].Base = new Vector2F(x, y);
                //paws[counter].segments[0].Length /= 2;
                positions[counter] = new Vector2F(x, y);
                counter++;
            }

            foreach (InverseKinematicsReacher paw in paws)
            {
                paw.Target = dots[new Random().Next(0, dots.Length)];
            }


            window.SetMouseCursorVisible(true);
            Vector2i mousePosition;
            for (int x = 0; x < _Width; x++)
            {
                for (int y = 0; y < _Height; y++)
                {
                    field[x, y] = Color.White;
                }
            }
            while (window.IsOpen)
            {
                Update();

                //da = (float)(Math.PI * 2 / amount);
                //counter = 0;
                //for (float a = (float)(mouseDir + Math.PI); a < mouseDir + Math.PI * 2; a += da)
                //{
                //    float x = (float)((Math.Cos(a) * 10));
                //    float y = (float)((Math.Sin(a) * 10));
                //    //paws[counter].segments[0].Length /= 2;
                //    positions[counter] = new Vector2F(x, y);
                //    counter++;
                //}

                window.DispatchEvents();
                //window.Clear();
                window.DispatchEvents();

                Sprite mainviewport = new Sprite(MainViewPort)
                {
                    Scale = new Vector2f(_ResMult, _ResMult)
                };
                window.Draw(mainviewport);
                window.Display();
                mousePosition = Mouse.GetPosition(window);
                int mouseMultInt = Convert.ToInt32(mouseMult);

                //z = Convert.ToInt32(CurrentTick % Convert.ToDouble(_Z));
                //if (Keyboard.IsKeyPressed(Keyboard.Key.W))
                //    yOffset--;
                //if (Keyboard.IsKeyPressed(Keyboard.Key.S))
                //    yOffset++;
                //if (Keyboard.IsKeyPressed(Keyboard.Key.A))
                //    xOffset--;
                //if (Keyboard.IsKeyPressed(Keyboard.Key.D))
                //    xOffset++;
                //if (Keyboard.IsKeyPressed(Keyboard.Key.Q))
                //    zOffset++;
                //if (Keyboard.IsKeyPressed(Keyboard.Key.E))
                //    zOffset--;
                //if (Keyboard.IsKeyPressed(Keyboard.Key.LShift))
                //    fOffset += 0.0001f;
                //if (Keyboard.IsKeyPressed(Keyboard.Key.Space))
                //    fOffset -= 0.0001f;

                if (Keyboard.IsKeyPressed(Keyboard.Key.Escape))
                    window.Close();
                //if (Keyboard.IsKeyPressed(Keyboard.Key.F5))


                //    if (Keyboard.IsKeyPressed(Keyboard.Key.Up))
                //        z++;
                //if (Keyboard.IsKeyPressed(Keyboard.Key.Down))
                //    z--;


                for (int y = 0; y < _Height; y++)
                    for (int x = 0; x < _Width; x++)
                    {
                        field[x, y] = Color.White;
                        //BG[x, y] = Color.White;
                    }

                for (int i = 0; i < dots.Length - 1; i++)
                {

                    //dotsVels[i] = (dots[i] - paws[0].segments[paws[0].segments.Length - 1].End).Normalized * 0.5f;
                    //if ((int)(dotsVels[i] + dots[i]).X < _Width - 1)
                    //    if ((int)(dotsVels[i] + dots[i]).Y < _Height - 1)
                    //        if ((int)(dotsVels[i] + dots[i]).X >= 0)
                    //            if ((int)(dotsVels[i] + dots[i]).Y >= 0)
                    //                if (field[(int)(dots[i]+dotsVels[i]).X, (int)(dots[i] + dotsVels[i]).Y] == Color.White)

                    //                            dots[i] += dotsVels[i];

                    Random rnd = new Random();
                    field[(int)dots[i].X, (int)dots[i].Y] = Color.Blue;
                    //foreach (InverseKinematicsReacher paw in paws)
                    //{
                    //    int rndInt = rnd.Next(0, dots.Length-1);
                    //    if ((paw.segments[paw.segments.Length - 1].End - dots[i]).Length < 1)
                    //    {
                    //        dots[i] = new Vector2F(new Random().Next(0, (int)_Width), new Random().Next(0, (int)_Height));
                    //        paw.Target = dots[rndInt];
                    //    }
                    //    //paw.Target = dots[rndInt];

                    //}
                }


                if (Mouse.GetPosition().X / mouseMult < window.Size.X & Mouse.GetPosition().Y / mouseMult < window.Size.Y)
                    if (mousePosition.X >= 0 & mousePosition.Y >= 0)
                    {
                        field[(int)windowMP.X, (int)windowMP.Y] = Color.Black;
                        windowMP.X = mousePosition.X / mouseMultInt;
                        windowMP.Y = mousePosition.Y / mouseMultInt;
                        mouseDir = (prevPos - windowMP).Normalized.Heading();
                        if (CurrentTick == PrevTick - 5)
                            prevPos = windowMP;
                        // Vector2F mDir = windowMP - new Vector2F(_Width / 2, _Height / 2);
                        // Console.WriteLine(mDir.Heading());
                        //field[mousePosition.X / mouseMultInt, mousePosition.Y / mouseMultInt] = Color.White;
                    }


                if (CurrentTick - PrevTick > 10)
                {

                    if ((point + pointVel).X > _Width - 1)
                        pointVel.X *= -1;
                    if ((point + pointVel).X < 0)
                        pointVel.X *= -1;
                    if ((point + pointVel).Y > _Height - 1)
                        pointVel.Y *= -1;
                    if ((point + pointVel).Y < 0)
                        pointVel.Y *= -1;
                    point += pointVel;

                    PrevTick = CurrentTick;
                }
                //foreach (InverseKinematicsReacher paw in paws)
                //{
                //    if ((paw.segments[paw.segments.Length - 1].End - point).Length < 1)
                //        point = new Vector2F(new Random().Next(0, (int)_Width), new Random().Next(0, (int)_Height));
                //}
                field[(int)point.X, (int)point.Y] = Color.Red;
                dots[dots.Length - 1] = point;

                DrawLine((int)paws[0].Base.X, (int)paws[0].Base.Y, (int)paws[paws.Length - 1].Base.X, (int)paws[paws.Length - 1].Base.Y);
                for (int i = 1; i < paws.Length - 1; i++)
                {
                    DrawLine((int)paws[i].Base.X, (int)paws[i].Base.Y, (int)paws[i - 1].Base.X, (int)paws[i - 1].Base.Y);
                    DrawLine((int)paws[i].Base.X, (int)paws[i].Base.Y, (int)paws[i + 1].Base.X, (int)paws[i + 1].Base.Y);
                }

                for (int i = 0; i < paws.Length; i++)
                {
                    //paws[i].Target = null;
                    float minDist = 1000;
                    float minPosDist = paws[i].MaxLength;
                    foreach (Vector2F dot in dots)
                    {

                        if ((paws[i].Base - dot).Length < minDist & (paws[i].Target == null))
                        {
                            minDist = (paws[i].Base - dot).Length;
                            paws[i].Target = dot;
                        }
                        else if ((paws[i].Base - paws[i].Target).Length >= minPosDist * 0.8)
                        {
                            minDist = (paws[i].Base - dot).Length;
                            paws[i].Target = dot;

                        }
                    }
                    paws[i].Base = windowMP + positions[i];



                }





                foreach (InverseKinematicsReacher paw in paws)
                {

                    paw.Update();
                    paw.Show();
                }

                if (window.HasFocus())
                {
                    Update();
                    //Drawing function
                }
                CurrentTick++;
            }
        }

        public static void DrawLine(int x0, int y0, int x1, int y1)
        {
            DrawLine(x0, y0, x1, y1, Color.Black);
        }

        public static void DrawLine(int x0, int y0, int x1, int y1, Color color)
        {
            int dx = Math.Abs(x1 - x0);
            int sx = x0 < x1 ? 1 : -1;
            int dy = -Math.Abs(y1 - y0);
            int sy = y0 < y1 ? 1 : -1;
            int err = dx + dy;  /* error value e_xy */
            while (true)
            {/* loop */
                //if (y0 < 0) break;
                //if (x0 < 0) break;
                if (x0 >= 0 & y0 >= 0 & x0 < _Width & y0 < _Height)
                    field[x0, y0] = color;
                if (x0 == x1 & y0 == y1) break;
                int e2 = 2 * err;
                if (e2 >= dy) /* e_xy+e_x > 0 */
                {
                    err += dy;
                    x0 += sx;
                }
                if (e2 <= dx)
                {/* e_xy+e_y < 0 */
                    err += dx;
                    y0 += sy;
                }


            }
        }

        static void Update()
        {
            TestCounter++;
            if (TestCounter > _Width - 1) { TestCounter = 0; }

            for (uint x = 0; x < _Width; x++)
            {
                for (uint y = 0; y < _Height; y++)
                {

                    uint i = 4 * (x + (_Width * y));
                    cpixels[x + (_Width * y)] = field[x, y];
                    if (BG[x, y] == Color.Red)
                        cpixels[x + (_Width * y)] = BG[x, y];
                    //i *= 4;
                    pixels[i + 0] = cpixels[i / 4].R;
                    pixels[i + 1] = cpixels[i / 4].G;
                    pixels[i + 2] = cpixels[i / 4].B;
                    pixels[i + 3] = cpixels[i / 4].A;

                }
            }

            MainViewPort.Update(pixels);
        }
    }
}
