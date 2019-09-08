using System;
using System.Collections.Generic;
using System.Linq;

namespace UnderEngine
{
    public abstract class BasePhysicsObject : ObjectBase
    {
        public Vector2 Velocity { get; set; } = Vector2.Zero;

        public Vector2 Acceleration { get; set; } = Vector2.Zero;

        public float Mass { get; set; }

        public bool Enabled
        {
            get => HitBox.Enabled;
            set => HitBox.Enabled = value;
        }

        private HitBox oldHB = new HitBox();

        public override void Update(TimeSpan elapsedTime)
        {
            if (Enabled)
            {
                Velocity += Acceleration * (float) elapsedTime.TotalSeconds;
                Position += Velocity * (float) elapsedTime.TotalSeconds;
            }
        }
    }
}
