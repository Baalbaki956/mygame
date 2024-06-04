using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MyGame.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGame.UI
{
    class Button
    {
        private int x, y;

        Texture2D normalButton;
        Texture2D hoverButton;

        Vector2 btnPosition { get; set; }
        string text;

        bool IsHovered = false;

        // Delegate and event for click action
        public delegate void ClickAction();
        public event ClickAction OnClick;

        public Button(int x, int y, string text)
        {
            this.x = x;
            this.y = y;
            this.text = text;

            normalButton = Globals.Content.Load<Texture2D>("UI/btn_normal");
            hoverButton = Globals.Content.Load<Texture2D>("UI/btn_hover");
        }

        public void Update(GameTime gameTime)
        {
            Point pt = MouseHandler.GetMousePosition();
            Rectangle rect = new Rectangle(x, y, this.normalButton.Width, this.normalButton.Height);

            IsHovered = rect.Contains(pt);

            if (IsHovered && MouseHandler.IsMouseLeftPressed())
            {
                if (OnClick != null)
                    OnClick.Invoke();
            }
        }

        public void Draw()
        {
            Globals.SpriteBatch.Draw(normalButton, new Vector2(x, y), Color.White);
            if (IsHovered)
                Globals.SpriteBatch.Draw(hoverButton, new Vector2(x, y), Color.White);
           
            Globals.SpriteBatch.DrawString(Globals.SpriteFont, text, new Vector2((x + 128 / 2) - Globals.SpriteFont.MeasureString(text).X / 2, (y + 32 / 2) - Globals.SpriteFont.MeasureString(text).Y / 2), Color.White);
        }

        public void UnloadContent()
        {
            normalButton.Dispose();
            hoverButton.Dispose();
        }

    }
}
