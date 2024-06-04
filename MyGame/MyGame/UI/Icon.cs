using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyGame.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGame.UI
{
    class Icon
    {
        public Vector2 Position { get; set; }
        public Boolean IsHovering { get; set; }

        private Texture2D bgButton;
        private Texture2D bgButtonHovered;
        private Rectangle btnRectangle;
        //public Rectangle SourceRect { get; set; }
        //public Rectangle srcIconRect { get;set; }

        public Icon(int x, int y)
        {
            this.Position = new Vector2(x, y);
            this.IsHovering = false;
            //SourceRect = new Rectangle((int)Position.X, (int)Position.Y, 24, 24);
        }

        public void Update(GameTime gameTIme)
        {
            Rectangle MouseBounds = new Rectangle(MouseHandler.GetMousePosition().X, MouseHandler.GetMousePosition().Y, 1, 1);
            //if (SourceRect.Contains(MouseBounds))
            //{
            //    this.IsHovering = true;
            //}
            //else
            //{
            //    this.IsHovering = false;
            //}
        }

        public void Draw()
        {
            //if (!IsHovering)
            //    Globals.SpriteBatch.Draw(Globals.Content.Load<Texture2D>("UI/icons"), SourceRect, new Rectangle(2, 2, 24, 24), Color.White);
            //else
            //{
            //    Globals.SpriteBatch.Draw(Globals.Content.Load<Texture2D>("UI/icons"), SourceRect, new Rectangle(28, 2, 24, 24), Color.White);
            //    Globals.SpriteBatch.Draw(Globals.Content.Load<Texture2D>("UI/icons"), SourceRect, new Rectangle(134 - 4, 10 - 4, 24, 24), Color.White);
            //}
        }
    }
}
