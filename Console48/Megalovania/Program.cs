using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
using UnderEngine;

namespace Megalovania
{
    class Program
    {
        static void Main(string[] args)
        {
            int karma = 0;
            Console.BackgroundColor = ConsoleColor.Black;
            World world = new World(40, 200, 200);
            Soul s = new Soul(6) {Position = new Vector2((Console.WindowWidth >> 1) - 5, (Console.WindowHeight) - 5), Mode = SoulMode.Blue };
            int healthWidth = Console.WindowWidth - 40;
            ProgressBar healthBar = new ProgressBar(healthWidth, 10, ConsoleColor.Red, ConsoleColor.Yellow)
                {Position = new Vector2((Console.WindowWidth - healthWidth) >> 1, 0)};
            BoneAttack[] bones = { new BoneAttack(Vector2.UnitX * 50, new HitBox(1, 70, 3, 30)), new BoneAttack(Vector2.UnitX * 50, new HitBox(-100, 70, 3, 30)), new BoneAttack(Vector2.UnitX * 50, new HitBox(-150, 70, 3, 30)), new BoneAttack(Vector2.UnitX * 70, new HitBox(-200, 70, 3, 30)) };
            world.Objects = new ObjectBase[] {s, healthBar
            }.Union(bones).ToArray();
            int maxHealth = 100;
            int currHealth = 100;

            SoundPlayer megalovania = new SoundPlayer("meg.wav");
            megalovania.PlayLooping();
            /*var reader = new Mp3FileReader(@"C:\Users\TreeMusketeers\Music\Toby Fox - Megalovania.mp3");
            var bitsPerSample = reader.Mp3WaveFormat.BitsPerSample;
            byte[] bytes = new byte[reader.Length];
            int read = reader.Read(bytes, 0, bytes.Length);
            new WaveFileWriter("meg.wav", reader.Mp3WaveFormat).Write(bytes, 0, read);*/
            //short[] sampleBuffer = new short[read / 2];
            //Buffer.BlockCopy(bytes, 0, sampleBuffer, 0, read);

            //for (int i = 0; i < sampleBuffer.Length; i++)
            //{
            //    Console.Beep(sampleBuffer[i], 10);
            //}

            Stopwatch sw = new Stopwatch();
            Stopwatch sw2 = new Stopwatch();

            world.SetupObjects();

            sw.Start();
            sw2.Start();
            healthBar.Progress = 1;
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    s.HandleInput(Console.ReadKey(true));
                }

                if (sw2.ElapsedMilliseconds >= 200 && karma >= 1)
                {
                    sw2.Restart();
                    karma--;
                    currHealth--;
                    healthBar.Progress = (float)currHealth / maxHealth;
                }

                if (sw.ElapsedMilliseconds >= 20 && bones.Any(a => a.HitBox.HighResolutionIntersects(s.HitBox)))
                {
                    karma = (karma += 3) >= currHealth ? currHealth - 1 : karma;
                    sw.Restart();
                    currHealth--;
                    healthBar.Progress = (float)currHealth / maxHealth;
                    if (currHealth <= 0)
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Clear();
                        Console.SetCursorPosition(Console.WindowWidth / 2 - 1, Console.WindowHeight / 2);
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.Write("  ");
                        megalovania.Stop();
                        break;
                    }
                }

                world.UpdateAndDraw();
            }

            while (true)
            {
                //Console.ReadKey(true);
            }
        }
    }
}
