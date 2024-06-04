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
        Texture2D normalButton;
        Texture2D hoverButton;

        public Vector2 btnPosition { get; set; }
        string text;

        bool IsHovered = false;

        // Delegate and event for click action
        public delegate void ClickAction();
        public event ClickAction OnClick;

        public Button(int x, int y, string text)
        {
            btnPosition = new Vector2(x, y);
            this.text = text;

            normalButton = Globals.Content.Load<Texture2D>("UI/btn_normal");
            hoverButton = Globals.Content.Load<Texture2D>("UI/btn_hover");
        }

        public void Update(GameTime gameTime)
        {
            Point pt = MouseHandler.GetMousePosition();
            Rectangle rect = new Rectangle((int)btnPosition.X, (int)btnPosition.Y, this.normalButton.Width, this.normalButton.Height);

            IsHovered = rect.Contains(pt);

            if (IsHovered && MouseHandler.IsMouseLeftPressed())
            {
                if (OnClick != null)
                    OnClick.Invoke();
            }
        }

        public void Draw()
        {
            Globals.SpriteBatch.Draw(normalButton, new Vector2((int)btnPosition.X, (int)btnPosition.Y), Color.White);
            if (IsHovered)
                Globals.SpriteBatch.Draw(hoverButton, new Vector2((int)btnPosition.X, (int)btnPosition.Y), Color.White);

            Globals.SpriteBatch.DrawString(Globals.SpriteFont, text, new Vector2(((int)btnPosition.X + 128 / 2) - Globals.SpriteFont.MeasureString(text).X / 2, ((int)btnPosition.Y + 32 / 2) - Globals.SpriteFont.MeasureString(text).Y / 2), Color.White);
        }

        public void UnloadContent()
        {
            normalButton.Dispose();
            hoverButton.Dispose();
        }

    }
}
