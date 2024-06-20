using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MyGame.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGame.UI.Chat
{
    class TextBox
    {
        public Vector2 Position { get; private set; }
        private Texture2D sprite;

        private bool isClicked;

        public bool IsClicked
        {
            get { return isClicked; }
            set { isClicked = value; }
        }

        public TextBox(int x, int y)
        {
            Position = new Vector2(x, y);
            sprite = Globals.Content.Load<Texture2D>("UI/chatBox");
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw()
        {
            if (isClicked)
                Globals.SpriteBatch.Draw(sprite, Position, Color.White * 0.5f);
            else
                Globals.SpriteBatch.Draw(sprite, Position, Color.White * 0.2f);
        }
    }
}
