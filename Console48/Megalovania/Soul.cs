using System;
using UnderEngine;

namespace Megalovania
{
    public enum SoulMode
    {
        Red,
        Blue
    }

    public class Soul : ObjectBase
    {
        private Vector2 oldPos;

        private bool canSwitchModes = false;

        private static Vector2 Gravity = new Vector2(0, 40f);

        private Texture Texture;
        private const int maxPos = 95;
        private const int minPos = 50;

        public ConsoleColor Color => Mode == SoulMode.Red ? ConsoleColor.Red : ConsoleColor.Blue;

        public Vector2 Velocity { get; set; } = Vector2.Zero;
        public SoulMode Mode = SoulMode.Red;

        public Soul(int size, bool canToggleModes = false)
        {
            canSwitchModes = canToggleModes;
            Texture = new Texture(/*@"C:\Users\TreeMusketeers\Pictures\Heart.png"*/@"C:\Users\Peter.Husman\Pictures\Soul.png", size);
            HitBox = new HitBox {Size = new Vector2(Texture.GetLength(0), Texture.GetLength(1))}; //Vector2.UnitX * 9 + Vector2.UnitY * 9 };
        }

        private ConsoleKeyInfo keyToProcess;

        public void HandleInput(ConsoleKeyInfo key)
        {
            keyToProcess = key;
        }

        public override ConsoleColor GetColor(Vector2 absolutePos)
        {
            Vector2 vec = absolutePos - Position;
            switch (Mode)
            {
                case SoulMode.Red:
                    return /*HitBox.Intersects(absolutePos) ?*/ Texture[(int)vec.X, (int)vec.Y];//: ConsoleColor.Black;
                case SoulMode.Blue:
                    return /*HitBox.Intersects(absolutePos) ?*/ Texture[(int)vec.X, (int)vec.Y] == ConsoleColor.Red ? ConsoleColor.Blue : ConsoleColor.Black;//: ConsoleColor.Black;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            //return /*HitBox.Intersects(absolutePos) ?*/ Texture[(int)vec.X, (int)vec.Y] ;//: ConsoleColor.Black;
        }

        private TimeSpan airTime = new TimeSpan();

        public override void Update(TimeSpan elapsedTime)
        {
            oldPos = Position.Copy;
            switch (keyToProcess.Key)
            {
                case ConsoleKey.LeftArrow:
                    Position.X -= 3;
                    break;
                case ConsoleKey.RightArrow:
                    Position.X += 3;
                    break;
                case ConsoleKey.UpArrow:
                    if (Mode == SoulMode.Red)
                    {
                        Position.Y -= 3;
                        break;
                    }

                    if (airTime.TotalSeconds <= 0.2 && Velocity.Y <= 0)
                    {
                        Velocity.Y -= 9;// * (float)elapsedTime.TotalSeconds;
                    }
                    break;
                case ConsoleKey.DownArrow:
                    if (Mode == SoulMode.Red)
                    {
                        Position.Y += 3;
                    }
                    break;
                case ConsoleKey.Spacebar when canSwitchModes:
                    Mode = Mode == SoulMode.Red ? SoulMode.Blue : SoulMode.Red;
                    break;
            }

            keyToProcess = new ConsoleKeyInfo('a', ConsoleKey.A, false, false, false);

            if (Mode == SoulMode.Blue)
            {
                airTime += elapsedTime;
                Position += Velocity * (float)elapsedTime.TotalSeconds;
                if (Position.Y >= maxPos)
                {
                    Velocity.Y = 0;
                    Position.Y = maxPos;
                    airTime = new TimeSpan();
                }
                else
                {
                    
                    Velocity += Gravity * (float)elapsedTime.TotalSeconds;
                }
            }

            if (Position.Y > maxPos)
            {
                Velocity.Y = 0;
                Position.Y = maxPos;
                airTime = new TimeSpan();
            }

            if (Position.Y < minPos)
            {
                Position.Y = minPos;
                Velocity.Y = 0;
            }

            if (Position.X > Console.WindowWidth - HitBox.Size.X)
            {
                Position.X = Console.WindowWidth - HitBox.Size.X;
            }

            if (Position.X < 0)
            {
                Position.X = 0;
            }

            /*if ((int)oldPos.X != (int)Position.X || (int)oldPos.Y != (int)Position.Y)
            {
                var newPositions = HitBox.AllPositions();
                var oldPositions = new[] { oldPos, oldPos + Vector2.UnitX };
                World.MarkDirty(newPositions);
                World.MarkDirty(oldPositions);
            }*/
        }
    }
}
