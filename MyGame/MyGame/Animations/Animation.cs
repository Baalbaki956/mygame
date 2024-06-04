using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame.World
{
    public class Animation
    {
        private Texture2D spriteSheet;
        private int frameWidth;
        private int frameHeight;
        private int frameCountX; // Number of frames in the X direction
        private int frameCountY; // Number of frames in the Y direction
        private int currentFrame;
        private float frameTime;
        private float timer;

        public Vector2 Position { get; set; }

        public Animation(Texture2D spriteSheet, int frameCountX, int frameCountY, float frameTime)
        {
            this.spriteSheet = spriteSheet;
            this.frameWidth = 32;
            this.frameHeight = 32;
            this.frameCountX = frameCountX;
            this.frameCountY = frameCountY;
            this.frameTime = frameTime;

            this.currentFrame = 0;
            this.timer = 0f;
        }

        public void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timer > frameTime)
            {
                currentFrame++;
                if (currentFrame >= frameCountX * frameCountY)
                    currentFrame = 0;
                timer = 0f;
            }
        }

        public void Draw()
        {
            Rectangle sourceRect = new Rectangle(frameCountX * frameWidth, frameCountY * frameHeight, frameWidth, frameHeight);
            Globals.SpriteBatch.Draw(spriteSheet, Position, sourceRect, Color.White);
        }
    }
}
