using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGame.Entities
{
    class GameObject : Entity
    {
        protected Vector2 Position { get; set; }
        protected Vector2 Target { get; set; }

        protected Texture2D sprite;
        protected bool moving;

        public GameObject(int x, int y)
        {
            this.Position = new Vector2(x * 32, y * 32);
            sprite = Globals.Content.Load<Texture2D>("Character/3398");
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw()
        {
            Globals.SpriteBatch.Draw(sprite, Position, Color.White);
        }
    }
}
