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

        private Texture2D bgButton;
        private Rectangle btnRectangle;
        private Texture2D bgButtonHovered;
        private bool hover = false;
        public bool isPressed { get; set; }

        public delegate void ClickAction();
        public event ClickAction OnClick;

        public Icon(int x, int y)
        {
            this.Position = new Vector2(x, y);

            bgButton = Globals.Content.Load<Texture2D>("UI/bgButton");
            bgButtonHovered = Globals.Content.Load<Texture2D>("UI/bgButtonHover");
        }

        public void Update(GameTime gameTIme)
        {
            btnRectangle = new Rectangle((int)Position.X, (int)Position.Y, 24, 24);
            Point mousePoint = MouseHandler.GetMousePosition();
            if (btnRectangle.Contains(mousePoint))
            {
                hover = true;
            }
            else
            {
                hover = false;
            }

            if (hover && MouseHandler.IsMouseLeftPressed())
            {
                if (OnClick != null)
                {
                    OnClick.Invoke();
                }
            }
        }

        public void Draw()
        {
            if (hover)
            {
                Globals.SpriteBatch.Draw(bgButtonHovered, new Rectangle((int)Position.X, (int)Position.Y, 24, 24), Color.White);
            }
            else
            {
                Globals.SpriteBatch.Draw(bgButton, new Rectangle((int)Position.X, (int)Position.Y, 24, 24), Color.White);
            }
        }
    }
}
