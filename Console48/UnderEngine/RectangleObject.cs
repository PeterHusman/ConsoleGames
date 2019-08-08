using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnderEngine
{
    public class RectangleObject : ObjectBase
    {
        public ConsoleColor Color;

        public override ConsoleColor GetColor(Vector2 absolutePos)
        {
            return Color;
        }

        public override void Update(TimeSpan elapsedTime)
        {
            
        }
    }
}
