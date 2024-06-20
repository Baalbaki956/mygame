using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyGame.UI.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGame.UI
{
    class ChatBox
    {
        public Vector2 Position { get; private set; }

        Texture2D sprite;

        public ChatBox(int x, int y)
        {
            Position = new Vector2(x, y);
            sprite = Globals.Content.Load<Texture2D>("UI/textDisplay");
        }

        public void Draw()
        {
            Globals.SpriteBatch.Draw(sprite, Position, Color.White * 0.5f);

        }

        public void Update(GameTime gameTime)
        {
        }
    }
}
