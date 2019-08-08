using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace UnderEngine
{
    public interface ITexture
    {

        ConsoleColor this[int x, int y] { get; }
    }

    public class Texture
    {
        public virtual ConsoleColor[,] Data { get; protected set; }

        public virtual int GetLength(int dimension)
        {
            return Data.GetLength(dimension);
        }

        public ConsoleColor this[int x, int y] => Data[x, y];

        public Texture(ConsoleColor[,] data)
        {
            Data = data;
        }

        public Texture(ConsoleColor color, int width, int height)
        {
            Data = new ConsoleColor[width,height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Data[x, y] = color;
                }
            }
        }


        public Texture(string fileName, int size)
        {
            Data = FromBitmap(new Bitmap(fileName), size);
        }

        public Texture()
        {

        }

        public static ConsoleColor[,] FromBitmap(Bitmap bmpSrc, int longestSide)
        {
            int sMax = longestSide;
            decimal percent = Math.Min(decimal.Divide(sMax, bmpSrc.Width), decimal.Divide(sMax, bmpSrc.Height));
            Size resSize = new Size((int)(bmpSrc.Width * percent), (int)(bmpSrc.Height * percent));

            Bitmap bmpMin = new Bitmap(bmpSrc, resSize);
            ConsoleColor[,] colors = new ConsoleColor[resSize.Width,resSize.Height];
            for (int i = 0; i < resSize.Height; i++)
            {
                for (int j = 0; j < resSize.Width; j++)
                {
                    var color = ToConsoleColor(bmpMin.GetPixel(j, i));
                    colors[j, i] = /*colors[j *  + 1, i] = */color;
                }
            }

            return colors;
        }

        public virtual void Update(TimeSpan elapsedTime)
        {

        }

        public static ConsoleColor ToConsoleColor(Color c)
        {
            int index = (c.R > 128 | c.G > 128 | c.B > 128) ? 8 : 0;
            index |= (c.R > 64) ? 4 : 0;
            index |= (c.G > 64) ? 2 : 0;
            index |= (c.B > 64) ? 1 : 0;
            return (ConsoleColor)index;
        }
    }

    public class AnimatedTexture : Texture
    {
        private TimeSpan accumulatedElapsedTime = new TimeSpan();
        public ConsoleColor[][,] Frames { get; private set; }
        public int FrameIndex = 0;
        public override ConsoleColor[,] Data => Frames[FrameIndex];
        public TimeSpan TimePerFrame { get; }

        public AnimatedTexture(ConsoleColor[][,] data, TimeSpan timePerFrame)
        {
            Frames = data;
            TimePerFrame = timePerFrame;
        }

        public AnimatedTexture(string[] fileNames, TimeSpan timePerFrame, int size)
        {
            int numOfFrames = fileNames.Length;

            Frames = new ConsoleColor[numOfFrames][,];
            for (int i = 0; i < numOfFrames; i++)
            {
                Frames[i] = FromBitmap(new Bitmap(fileNames[i]), size);
            }

            TimePerFrame = timePerFrame;
        }

        public override void Update(TimeSpan elapsedTime)
        {
            accumulatedElapsedTime += elapsedTime;
            if (accumulatedElapsedTime < TimePerFrame) return;
            FrameIndex = (FrameIndex + 1) % Frames.Length;
            accumulatedElapsedTime = new TimeSpan();
        }
    }
}
