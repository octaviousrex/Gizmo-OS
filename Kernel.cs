using System;
using System.Collections.Generic;
using System.Text;
using Sys = Cosmos.System;
using System.Drawing;
using Cosmos.System.Graphics;

//DrawRectangle(Pen pen, int x_start, int y_start,int width, int height) draws a rectangle specified by a coordinate pair, a width, and a height with the specified pen

namespace gizmoguitest
{
    public class Kernel : Sys.Kernel
    {
        Sys.FileSystem.CosmosVFS fs;
        string current_directory = "0:\\";

        protected override void BeforeRun()
        {
            fs = new Sys.FileSystem.CosmosVFS();
            Sys.FileSystem.VFS.VFSManager.RegisterVFS(fs);

        E:
            Console.WriteLine("Gizmo OS booted successfully. Enter password: ");
            string password = "1234";
            string input = Console.ReadLine();
            if (input == password)
            {
                Console.Clear();
                Run();
            }
            else
            {
                Console.WriteLine("WRONG PASSWORD. TRY AGAIN!");
                goto E;
            }
        }
        //Defines the screen x and y size. Make these ints that are not in a method.
        private static int screenX = 800;
        private static int screenY = 640;

        //Defines the pixel buffer. This will store all of the pixels you are setting until the screen is drawn
        private static Color[] pixelBuffer = new Color[(screenX * screenY) + screenX];
        private static Color[] pixelBufferOld = new Color[(screenX * screenY) + screenX];

        //Canvas Variable
        private static Canvas canvas = FullScreenCanvas.GetFullScreenCanvas();

        //Int Method that initializes the canvas and mouse
        public static void init()
        {
            canvas.Mode = new Mode(screenX, screenY, ColorDepth.ColorDepth32);
            Cosmos.System.MouseManager.ScreenWidth = Convert.ToUInt32(screenX);
            Cosmos.System.MouseManager.ScreenHeight = Convert.ToUInt32(screenY);
        }

        //make the first method. This one will set a pixel inside the buffer to a specific color.
        //We will use this instead of the canvas.setPixel() method.
        public static void setPixel(int x, int y, Color c)
        {
            if (x > screenX || y > screenY) return;
            pixelBuffer[(x * y) + x] = c;
        }

        //draw the screen. This just loops through all pixels on the screen,
        //checks if they are different from the last version,
        // and then changes them if they are.
        public static void drawScreen()
        {
            Pen pen = new Pen(Color.Orange);
            for (int y = 0, h = screenY; y < h; y++)
            {
                for (int x = 0, w = screenX; x < w; x++)
                {
                    if (!(pixelBuffer[(y * x) + x] == pixelBufferOld[(y * y) + x]))
                    {
                        pen.Color = pixelBuffer[(y * screenX) + x];
                        canvas.DrawPoint(pen, x, y);
                    }
                }
            }
            for (int i = 0, len = pixelBuffer.Length; i < len; i++)
            {
                pixelBuffer[i] = pixelBufferOld[i];
            }
        }

        //cg=hange all the pixels colors
        public static void clearScreen(Color c)
        {
            for (int i = 0, len = pixelBuffer.Length; i < len; i++)
            {
                pixelBuffer[i] = c;
            }
        }

        public static void update()
        {
            clearScreen(Color.Blue);
            setPixel(1, 1, Color.Black);
            setPixel(1, 2, Color.Black);
            setPixel(2, 1, Color.Black);
            setPixel(2, 2, Color.Black);
            setPixel(Convert.ToInt32(Cosmos.System.MouseManager.X), Convert.ToInt32(Cosmos.System.MouseManager.Y), Color.White);
            drawScreen();
        }

        protected override void Run()
        {
            try
            {
                canvas.Mode = new Mode(800, 600, ColorDepth.ColorDepth32);

                /* A red Point */
                Pen pen = new Pen(Color.Red);
                canvas.DrawPoint(pen, 69, 69);

                pen.Color = Color.RoyalBlue;
                canvas.DrawFilledRectangle(pen, 0, 0, 800, 600);

                Console.WriteLine("OS Booted");

                Console.ReadKey();

                Stop();
            }
            catch (Exception e)
            {
                mDebugger.Send("Exception occurred: " + e.Message);
                mDebugger.Send(e.Message);
                Stop();
            }
        }
    }
}