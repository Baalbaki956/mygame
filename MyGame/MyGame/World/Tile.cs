using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGame.World
{
    public class Tile
    {
        public int[] ID
        {
            get;
            set;
        }

        public Rectangle BoundingBox
        {
            get
            {
                return new Rectangle(X * 32, Y * 32, 32, 32); // Assuming tile size is 32x32
            }
        }

        public int X
        {
            get;
            private set;
        }

        public int Y
        {
            get;
            private set;
        }

        public Tile(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}
