using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AccidentallyLasers.WorldObjects.PhysicalObjects
{
    public abstract class PhysicalWorldObject : InWorldObject
    {
        protected PhysicalWorldObject(int xPos, int yPos)
            : base()
        {
            this.xPosition = xPos;
            this.yPosition = yPos;
        }

        private int xPosition { get; set; }
        private int yPosition { get; set; }

        public override int xPositionWorld { get { return xPosition; } }
        public override int yPositionWorld { get { return yPosition; } }

        protected abstract int HalfWidth { get; }
        protected abstract int HalfHeight { get; }
        protected int Width { get { return HalfWidth * 2; } }
        protected int Height { get { return Height * 2; } }
    }
}
