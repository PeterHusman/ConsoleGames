using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnderEngine
{
    public class ProgressBar : ObjectBase
    {
        private float oldProg = 2;

        public float Progress
        {
            get => _progress;
            set
            {
                oldProg = _progress;
                _progress = value;
                if (oldProg == _progress)
                {
                    return;
                }
                World.MarkDirty(HitBox.AllPositions());
            }
        }

        public int Width { get; }

        public ConsoleColor BackColor { get; }

        public ConsoleColor ForeColor { get; }
        private float _progress = 2;

        public override ConsoleColor GetColor(Vector2 absolutePos)
        {
            return (absolutePos - Position).X >= Progress * HitBox.Size.X ? BackColor : ForeColor;
        }

        public ProgressBar(int width, int height, ConsoleColor back, ConsoleColor fore)
        {
            HitBox.Size.X = width;
            HitBox.Size.Y = height;
            BackColor = back;
            ForeColor = fore;
        }

        public override void Update(TimeSpan elapsedTime)
        {
            
        }
    }
}
