using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnderEngine
{
    public class HitBox
    {
        public Vector2 Position = Vector2.Zero;
        public Vector2 Size = new Vector2(1, 1);

        public bool Enabled = true;
        //public float Rotation;
        //public Vector2 RelativeOrigin = Vector2.Zero;

        public bool Intersects(Vector2 position, bool truncate = true)
        {
            return Enabled && (truncate
                ? (int) position.X >= (int) Position.X && (int) position.Y >= (int) Position.Y &&
                  (int) position.X < (int) Position.X + (int) Size.X &&
                  (int) position.Y < (int) Position.Y + (int) Size.Y
                : position.X >= Position.X &&
                  position.Y >= Position.Y &&
                  position.X < Position.X + Size.X &&
                  position.Y < Position.Y + Size.Y);
        }

        public Vector2 TopRight => Position + new Vector2(Size.X, 0);
        public Vector2 BottomLeft => Position + new Vector2(0, Size.Y);
        public Vector2 BottomRight => Position + Size;

        public bool HighResolutionIntersects(HitBox b)
        {
            return AllPositions().Intersect(b.AllPositions(), new VectorEqualityComparer()).Any();
        }

        public bool Intersects(HitBox b, bool truncate = true)
        {
            return Intersects(b.Position, truncate) || Intersects(b.TopRight, truncate) ||
                   Intersects(b.BottomLeft, truncate) || Intersects(b.BottomRight, truncate);
        }

        public IEnumerable<Vector2> AllPositions()
        {
            for (int i = 0; i < Size.X; i++)
            {
                for (int j = 0; j < Size.Y; j++)
                {
                    yield return Position + new Vector2(i, j);
                }
            }
        }

        public HitBox(Vector2 position, Vector2 size)
        {
            Position = position;
            Size = size;
        }

        public HitBox(float x, float y, float width, float height)
        {
            Position = new Vector2(x, y);
            Size = new Vector2(width, height);
        }

        public HitBox()
        {

        }

        public HitBox Copy => new HitBox(Position.Copy, Size.Copy);
    }
}
