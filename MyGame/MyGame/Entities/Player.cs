using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MyGame;
using MyGame.Input;
using MyGame.World;
using System;
using System.Collections.Generic;

namespace MyGame.Entities
{
    class Player
    {
        public Vector2 Position { get; set; }
        public Vector2 Target { get; set; }

        private bool moving;
        private Dictionary<Keys, bool> keys;
        private OrthogonalMap map;

        private Rectangle healthRect;
        private Texture2D healthTexture;
        // private int slotSize = 42;

        private Animation animUp;
        private Animation animDown;
        private Animation animLeft;
        private Animation animRight;

        Animation currentAnim;

        public Player(Vector2 position, OrthogonalMap map)
        {
            this.map = map;
            Position = position * 32; // Adjust based on tile size
            Target = Position;
            moving = false;
            keys = new Dictionary<Keys, bool>();

            // Health
            healthRect = new Rectangle(Globals.screenWidth / 2 - 256 / 2, Globals.screenHeight - 24 - 8, 250, 18);
            healthTexture = Globals.Content.Load<Texture2D>("UI/healthBar");

            // Setup input handling
            KeyboardState currentState = Keyboard.GetState();
            keys[Keys.Up] = false;
            keys[Keys.Down] = false;
            keys[Keys.Left] = false;
            keys[Keys.Right] = false;

            animUp = new Animation(Globals.Content.Load<Texture2D>("Character/GMOutfit"), 0, 1, 0.1f);
            animDown = new Animation(Globals.Content.Load<Texture2D>("Character/GMOutfit"), 0, 0, 0.1f);
            animLeft = new Animation(Globals.Content.Load<Texture2D>("Character/GMOutfit"), 0, 3, 0.1f);
            animRight = new Animation(Globals.Content.Load<Texture2D>("Character/GMOutfit"), 0, 2, 0.1f);

            currentAnim = animDown;
            currentAnim.Position = Position;
        }

        private void OnKeyDown(Keys key)
        {
            keys[key] = true;
        }

        private void OnKeyUp(Keys key)
        {
            keys[key] = false;
        }

        public void Update(GameTime gameTime)
        {
            currentAnim.Update(gameTime);

            // Check if any movement key is pressed
            bool anyKeyPressed = KeyboardHandler.IsKeyPressed(Keys.Up) || KeyboardHandler.IsKeyPressed(Keys.Down) ||
                                 KeyboardHandler.IsKeyPressed(Keys.Left) || KeyboardHandler.IsKeyPressed(Keys.Right);

            // Update target based on key presses
            if (anyKeyPressed && !moving)
            {
                if (KeyboardHandler.IsKeyPressed(Keys.Up))
                {
                    Move(0, -1);
                    currentAnim = animUp;
                }
                else if (KeyboardHandler.IsKeyPressed(Keys.Down))
                {
                    Move(0, 1);
                    currentAnim = animDown;
                }
                else if (KeyboardHandler.IsKeyPressed(Keys.Left))
                {
                    Move(-1, 0);
                    currentAnim = animLeft;
                }
                else if (KeyboardHandler.IsKeyPressed(Keys.Right))
                {
                    Move(1, 0);
                    currentAnim = animRight;
                }
            }

            // Move towards the target
            if (Position != Target)
            {
                Vector2 direction = Vector2.Normalize(Target - Position);
                float speed = 2f; // Adjust speed as needed
                Position += direction * speed;
            }

            // Check if arrived at the target
            if (Vector2.DistanceSquared(Position, Target) < 1) // Adjust threshold as needed
            {
                Position = Target;
                moving = false;
            }

            // Update animation position if moving
            currentAnim.Position = Position;
        }

        public void Draw()
        {
            // Draw player sprite at current position
            // Globals.SpriteBatch.Draw(player, Position, Color.White);
            currentAnim.Draw();
        }

        public void DrawHealth()
        {
            // Player Health
            Globals.SpriteBatch.Draw(healthTexture, healthRect, Color.White);
        }

        private void Move(int dx, int dy)
        {
            int tileSize = 32; // Adjust based on tile size
            int newX = (int)Position.X + dx * tileSize;
            int newY = (int)Position.Y + dy * tileSize;

            // Set new target position
            if (IsValidMove(newX, newY))
            {
                Target = new Vector2(newX, newY);
                moving = true;
            }
        }

        private bool IsValidMove(int newX, int newY)
        {
            Rectangle newBoundingBox = new Rectangle(newX, newY, 32, 32); // Assuming player size is 32x32

            // Check collision with unpassable tiles
            foreach (var tile in map.Tiles)
            {
                foreach (var tileId in tile.ID)
                {
                    var obj = Array.Find(map.Objects, o => o.ID == tileId);
                    if (obj != null && obj.IsUnpassable && newBoundingBox.Intersects(tile.BoundingBox))
                    {
                        return false;
                    }
                }
            }

            return true; // No collision detected
        }
    }
}