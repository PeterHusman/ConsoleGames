using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnderEngine
{
    public abstract class ObjectBase
    {
        public Vector2 Position
        {
            get => HitBox.Position;
            set => HitBox.Position = value;
        }
        public HitBox HitBox = new HitBox();

        public HitBox OldHitBox = new HitBox();

        //public Texture Texture = new Texture();

        public World World;

        public abstract ConsoleColor GetColor(Vector2 absolutePos);

        public abstract void Update(TimeSpan elapsedTime);

        public void MarkPixelDirty(Vector2 relPos)
        {
            World.MarkDirty(Position + relPos);
        }
    }
}
