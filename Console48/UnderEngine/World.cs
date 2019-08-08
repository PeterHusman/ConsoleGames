﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;

namespace UnderEngine
{
    public class World
    {
        public ObjectBase[] Objects;

        public ConsoleColor[,] ScreenBuffer;

        public readonly int Width;

        public readonly int Height;

        public TimeSpan TimePerFrame;
        private Stopwatch stopwatch = new Stopwatch();
        private Stopwatch renderTimer = new Stopwatch();

        private HashSet<Vector2> pointsToRedraw = new HashSet<Vector2>(new VectorEqualityComparer());

        public World(TimeSpan timePerFrame, int width, int height)
        {
            TimePerFrame = timePerFrame;

            ScreenBuffer = new ConsoleColor[width, height];
            Width = width;
            Height = height;
            Console.BufferHeight = Console.WindowHeight = height / 2;
            Console.BufferWidth = Console.WindowWidth = width;
            Console.CursorVisible = false;
        }

        public World(float fps, int width, int height) : this(TimeSpan.FromSeconds(1 / fps), width, height)
        {

        }

        public void MarkDirty(Vector2 position)
        {
            pointsToRedraw.Add(position);
        }

        public void MarkDirty(IEnumerable<Vector2> positions)
        {
            pointsToRedraw.UnionWith(positions);
        }

        public void SetupObjects()
        {
            foreach (ObjectBase objectBase in Objects)
            {
                objectBase.World = this;
            }
        }

        public void UpdateAndDraw()
        {
            TimeSpan elapsed = stopwatch.Elapsed;
            stopwatch.Restart();
            //HashSet<Vector2> pointsToRedraw = new HashSet<Vector2>(new VectorEqualityComparer());
            //for (int i = 0; i < Objects.Length; i++)
            //{
            //    Objects[i].Update();
            //    pointsToRedraw.UnionWith(Objects[i].RedrawPositions);
            //}

            //Next, please fix this brokenness. Each object should know the World and the object should tell the World which points to redraw in that phase of the World's Update.

            foreach (ObjectBase obj in Objects)
            {
                obj.Update(elapsed);
                if (!((int)obj.HitBox.Position.X == (int)obj.OldHitBox.Position.X &&
                      (int)obj.HitBox.Position.Y == (int)obj.OldHitBox.Position.Y &&
                      (int)obj.HitBox.Size.X == (int)obj.OldHitBox.Size.X &&
                      (int)obj.HitBox.Size.Y == (int)obj.OldHitBox.Size.Y))
                {
                    MarkDirty(obj.HitBox.AllPositions());
                    MarkDirty(obj.OldHitBox.AllPositions());//.Select(a => new Vector2(a.X, (int)a.Y)));
                }

                obj.OldHitBox = obj.HitBox.Copy;
            }

            if (renderTimer.Elapsed >= TimePerFrame || !renderTimer.IsRunning)
            {
                HashSet<Vector2> actuallyPleaseDontDrawThese = new HashSet<Vector2>(new VectorEqualityComparer());
#if DebugPrint
                int numOfNewPixels = 0;
#endif
                foreach (Vector2 pos in pointsToRedraw)
                {
                    ConsoleColor color = CalcColor(pos);

                    

                    if (pos.X >= 0 && pos.Y >= 0 && pos.X < Width && pos.Y < Height && color != ScreenBuffer[(int)pos.X, (int)pos.Y] && !actuallyPleaseDontDrawThese.Contains(pos))
                    {
#if DebugPrint
                        numOfNewPixels++;
#endif
                        Console.SetCursorPosition(pos.X < 0 ? 0 : (int)pos.X, pos.Y < 0 ? 0 : ((int)pos.Y >> 1));

                        Vector2 matchingPoint = GetPairedPoint(pos);

                        ConsoleColor color2 = FindColor(matchingPoint);
                        actuallyPleaseDontDrawThese.Add(matchingPoint);

                        if (((int) pos.Y & 1) == 0)
                        {
                            Console.BackgroundColor = color2;
                            Console.ForegroundColor = color;
                        }
                        else
                        {
                            Console.BackgroundColor = color;
                            Console.ForegroundColor = color2;
                        }

                        Console.Write('▀');

                        ScreenBuffer[(int)pos.X, (int)pos.Y] = color;
                    }
                }

#if DebugPrint
                Console.BackgroundColor = ConsoleColor.Black;

                Console.SetCursorPosition(0, 0);
                Console.Write("        ");
                Console.SetCursorPosition(0, 0);
                Console.Write(numOfNewPixels);

#endif

                pointsToRedraw.Clear();

                renderTimer.Restart();
            }
        }

        private ConsoleColor FindColor(Vector2 pos)
        {
            int x = (int) pos.X;
            int y = (int) pos.Y;
            return ScreenBuffer[x, y] = pointsToRedraw.Contains(pos) ? CalcColor(pos) : ScreenBuffer[x, y];
        }

        private static Vector2 GetPairedPoint(Vector2 pos)
        {
            int y = (int)pos.Y;
            return new Vector2(pos.X, (y & (~1)) + 1 - (y & 1));
        }

        private ConsoleColor CalcColor(Vector2 pos)
        {
            ConsoleColor color = ConsoleColor.Black;

            foreach (ObjectBase obj in Objects)
            {
                if (obj.HitBox.Intersects(pos))
                {
                    color = obj.GetColor(pos);
                }
            }

            return color;
        }
    }

    public class VectorEqualityComparer : IEqualityComparer<Vector2>
    {
        public bool Equals(Vector2 x, Vector2 y)
        {
            return x == null && y == null || ((x != null && y != null) && (int)x.X == (int)y.X && (int)x.Y == (int)y.Y);
        }

        public int GetHashCode(Vector2 obj)
        {
            return (int)obj.X ^ (int)obj.Y;
        }
    }
}
