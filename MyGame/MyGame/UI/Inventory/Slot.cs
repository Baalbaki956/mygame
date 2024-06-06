using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyGame.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGame.UI
{
    class Slot
    {
        public Vector2 Position { get; set; }
        Texture2D bgSlot;
        Texture2D bgSlotHovered;
        bool slotHover;

        public Slot(Vector2 position)
        {
            Position = position;
            //slotPosition = new Vector2(Globals.screenWidth - 216 + 6, Globals.screenHeight / 2 + 8);
            bgSlot = Globals.Content.Load<Texture2D>("UI/bgSlot");
            bgSlotHovered = Globals.Content.Load<Texture2D>("UI/slotHovered"); 
            slotHover = false;
        }

        public void Update(GameTime gameTime)
        {
            if (new Rectangle((int)Position.X, (int)Position.Y, bgSlot.Width, bgSlot.Height).Contains(MouseHandler.GetMousePosition()))
            {
                slotHover = true;
            }
            else
            {
                slotHover = false;
            }
        }

        public void UpdatePosition(Vector2 newPosition)
        {
            Position = newPosition;
        }

        public void Draw()
        {
            if (slotHover)
            {
                Globals.SpriteBatch.Draw(bgSlotHovered, Position, Color.White * .5f);
            }

            else
            {
                Globals.SpriteBatch.Draw(bgSlot, Position, Color.White);
            }
        }
    }
}
