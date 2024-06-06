using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGame.UI
{
    class InventoryTab
    {
        public Vector2 Position;
        public bool IsVisible { get; set; }

        private List<Slot> slots;

        public InventoryTab(Vector2 position)
        {
            this.Position = position;

            slots = new List<Slot>();
            IsVisible = false;

            int rows = 6;
            int columns = 5;
            int slotWidth = 32;
            int slotHeight = 32;
            int xSpacing = 8;
            int ySpacing = 8;

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    float x = Position.X + col * (slotWidth + xSpacing / 2);
                    float y = Position.Y + row * (slotHeight + ySpacing / 2);

                    slots.Add(new Slot(new Vector2(x, y)));
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (Slot slot in slots)
            {
                slot.Update(gameTime);
            }
        }

        public void UpdatePosition(Vector2 position)
        {
            int rows = 6;
            int columns = 5;
            int slotWidth = 32;
            int slotHeight = 32;
            int xSpacing = 8;
            int ySpacing = 8;

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    int index = row * columns + col;
                    if (index < slots.Count)
                    {
                        float x = position.X + col * (slotWidth + xSpacing / 2);
                        float y = position.Y + row * (slotHeight + ySpacing / 2);
                        slots[index].UpdatePosition(new Vector2(x, y));
                    }
                }
            }
        }

        public void Draw()
        {
            if (IsVisible)
            {
                foreach (Slot slot in slots)
                {
                    slot.Draw();
                }
            }
        }
    }
}
