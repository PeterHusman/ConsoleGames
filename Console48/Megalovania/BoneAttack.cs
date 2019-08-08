using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnderEngine;

namespace Megalovania
{
    public class BoneAttack : BasePhysicsObject
    {
        public override ConsoleColor GetColor(Vector2 absolutePos)
        {
            return ConsoleColor.White;
        }

        public BoneAttack(Vector2 velocity, HitBox hitBox)
        {
            Velocity = velocity;
            HitBox = hitBox;
            Enabled = true;
        }

        public override void Update(TimeSpan elapsedTime)
        {
            base.Update(elapsedTime);
            Vector2 farCorner = Position + HitBox.Size;
            /*if ((farCorner.X >= Console.BufferWidth || farCorner.Y >= Console.BufferHeight || Position.X < 0 || Position.Y < 0) && Enabled)
            {
                Enabled = false;
                HitBox.Enabled = false;
                World.MarkDirty(HitBox.AllPositions());
            }*/
        }
    }
}
