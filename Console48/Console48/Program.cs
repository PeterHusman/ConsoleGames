using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Console48
{
    enum Direction
    {
        Up = 1,
        Left = 0,
        Down = 3,
        Right = 2
    }

    class Program
    {
        static Random random = new Random();

        static int ToSpawn()
        {
            //spawns a 2 tile 90% of the time
            var value = random.Next(1, 11) <= 9 ? 1 : 2;
            return value;
        }

        static void Main(string[] args)
        {
            ConsoleColor[] colors = { ConsoleColor.Black, ConsoleColor.Blue, ConsoleColor.DarkRed, ConsoleColor.DarkGray, ConsoleColor.Yellow, ConsoleColor.DarkGreen, ConsoleColor.DarkBlue, ConsoleColor.DarkYellow, ConsoleColor.DarkMagenta, ConsoleColor.Magenta, ConsoleColor.Gray, ConsoleColor.Cyan, ConsoleColor.Red };

            int numberOfStartingTiles = 2;

            int squareSize = 4;

            int gridSize = 4;

            int[,] values = new int[gridSize,gridSize];
            int redX = -1;
            int redY = -1;


            void SpawnTile()
            {
                int val = ToSpawn();
                int x;
                int y;
                do
                {
                    x = random.Next(0, gridSize);
                    y = random.Next(0, gridSize);
                } while (values[x, y] != 0);

                redX = x;
                redY = y;
                values[x, y] = val;
            }

            for (int i = 0; i < numberOfStartingTiles; i++)
            {
                SpawnTile();
            }

            bool MoveAllTiles(Direction dir)
            {
                int dx = dir == Direction.Left ? -1 : (dir == Direction.Right ? 1 : 0);
                int dy = dir == Direction.Up ? -1 : (dir == Direction.Down ? 1 : 0);

                bool valid = false;

                switch (dir)
                {
                    case Direction.Up:
                        for (int i = 0; i < gridSize; i++)
                        {
                            for (int j = 0; j < gridSize; j++)
                            {
                                valid = MoveOneTile(i, j, dx, dy) || valid;
                            }
                        }
                        break;
                    case Direction.Left:
                        for (int i = 0; i < gridSize; i++)
                        {
                            for (int j = 0; j < gridSize; j++)
                            {
                                valid = MoveOneTile(i, j, dx, dy) || valid;
                            }
                        }
                        break;
                    case Direction.Down:
                        for (int i = gridSize - 1; i >= 0; i--)
                        {
                            for (int j = gridSize - 1; j >= 0; j--)
                            {
                                valid = MoveOneTile(i, j, dx, dy) || valid;
                            }
                        }
                        break;
                    case Direction.Right:
                        for (int i = gridSize - 1; i >= 0; i--)
                        {
                            for (int j = gridSize - 1; j >= 0; j--)
                            {
                                valid = MoveOneTile(i, j, dx, dy) || valid;
                            }
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(dir), dir, null);
                }

                return valid;
            }

            bool MoveOneTile(int x, int y, int dx, int dy)
            {
                if (values[x, y] == 0)
                {
                    return false;
                }

                int oldX = x;
                int oldY = y;
                bool valid = false;
                while (GetAtInd(x + dx, y + dy) == 0)
                {
                    x += dx;
                    y += dy;
                    valid = true;
                }

                int d = GetAtInd(x + dx, y + dy);
                if (d == values[oldX, oldY] && d > 0)
                {
                    values[x + dx, y + dy] += 1;
                    values[x + dx, y + dy] *= -1;
                    values[oldX, oldY] = 0;
                    return true;
                }

                if (!valid) return false;

                values[x, y] = values[oldX, oldY];
                values[oldX, oldY] = 0;
                return true;
            }

            int GetAtInd(int x, int y)
            {
                return x >= gridSize || x < 0 || y >= gridSize || y < 0 ? int.MinValue : values[x, y];
            }

            void RenderAndClearFlags()
            {
                Console.Clear();
#if OLDRENDER
                for (int j = 0; j < gridSize; j++)
                {
                    for (int i = 0; i < gridSize; i++)
                    {
                        values[i, j] = values[i, j] > 0 ? values[i, j] : -values[i, j];
                        string toWrite = values[i, j] == 0 ? "" : Math.Pow(2, values[i, j]).ToString();
                        if (i == redX && j == redY)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        Console.Write(toWrite);
                        int numOfSpaces = (squareSize * 2) - toWrite.Length;
                        for (int k = 0; k < numOfSpaces; k++)
                        {
                            Console.Write(" ");
                        }
                    }

                    for (int k = 0; k < squareSize; k++)
                    {
                        Console.WriteLine();
                    }
                }

                Console.BackgroundColor = ConsoleColor.Red;
                for (int i = 0; i < (gridSize - 1) * squareSize * 2 + 1; i++)
                {
                    Console.Write(" ");
                }

                Console.BackgroundColor = ConsoleColor.Black;
#else
                for (int j = 0; j < gridSize; j++)
                {
                    for (int k = 0; k < squareSize / 2 + (squareSize % 2); k++)
                    {

                        for (int i = 0; i < gridSize; i++)
                        {
                            Console.BackgroundColor =
                                values[i, j]*values[i, j] >= colors.Length*colors.Length ? ConsoleColor.Black : colors[Math.Abs(values[i, j])];

                            for (int l = 0; l < squareSize * 2; l++)
                            {
                                Console.Write(" ");
                            }
                        }
                        Console.WriteLine();
                    }
                    for (int i = 0; i < gridSize; i++)
                    {
                        values[i, j] = values[i, j] > 0 ? values[i, j] : -values[i, j];
                        string toWrite = values[i, j] == 0 ? "" : Math.Pow(2, values[i, j]).ToString();
                        int extra = toWrite.Length % 2;
                        Console.BackgroundColor =
                            values[i, j] >= colors.Length ? ConsoleColor.Black : colors[values[i, j]];
                        int numOfSpaces = squareSize - toWrite.Length / 2;
                        for (int k = 0; k < numOfSpaces; k++)
                        {
                            Console.Write(" ");
                        }

                        Console.BackgroundColor = ConsoleColor.Black;
                        if (i == redX && j == redY)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        Console.Write(toWrite);
                        Console.BackgroundColor =
                            values[i, j] >= colors.Length ? ConsoleColor.Black : colors[values[i, j]];
                        for (int k = 0; k < numOfSpaces - extra; k++)
                        {
                            Console.Write(" ");
                        }
                    }
                    Console.WriteLine();
                    for (int k = 0; k < squareSize / 2; k++)
                    {
                        
                        for (int i = 0; i < gridSize; i++)
                        {
                            Console.BackgroundColor =
                                values[i, j] >= colors.Length ? ConsoleColor.Black : colors[values[i, j]];

                            for (int l = 0; l < squareSize * 2; l++)
                            {
                                Console.Write(" ");
                            }
                        }
                        Console.WriteLine();
                    }
                    
                }

                Console.BackgroundColor = ConsoleColor.Red;
                for (int i = 0; i < gridSize * squareSize * 2 + 1; i++)
                {
                    Console.Write(" ");
                }

                Console.BackgroundColor = ConsoleColor.Black;
#endif
            }

            redX = -1;
            redY = -1;

            RenderAndClearFlags();

            while (true)
            {
                while (!Console.KeyAvailable)
                {
                }

                var key = Console.ReadKey(true);
                /*switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        break;
                    case ConsoleKey.DownArrow:
                        break;
                    case
                }*/

                var s = (int)key.Key - 37;
                if (s < 0 || s > 3)
                {
                    continue;
                }

                if (MoveAllTiles((Direction) s))
                {
                    SpawnTile();
                }
                else
                {
                    redX = -1;
                    redY = -1;
                }

                RenderAndClearFlags();
            }
        }
    }
}