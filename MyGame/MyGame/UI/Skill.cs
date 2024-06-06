using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGame.UI
{
    class Skill
    {
        private string text;

        private Vector2 position;
        private Texture2D texture;

        public Skill(Vector2 position, string text)
        {
            this.position = position;
            this.text = text;

            texture = Globals.Content.Load<Texture2D>("UI/healthBar");
        }

        public void Draw()
        {
            Globals.SpriteBatch.DrawString(Globals.SpriteFont,
                               text,
                               new Vector2(position.X, position.Y + 32),  // Position
                               Color.White,            // Color
                               0,                      // Rotation
                               Vector2.Zero,           // Origin
                               new Vector2(0.5f, 0.5f),// Scale (this will double the size)
                               SpriteEffects.None,
                               0);

            // skillbar
            Globals.SpriteBatch.Draw(texture, new Vector2(position.X, position.Y + 32 + 14), Color.White);
        }

        public void UpdatePosition(Vector2 newPosition)
        {
            this.position = newPosition;
        }
    }
}
