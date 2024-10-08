﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MyGame.Entities;
using MyGame.Input;
using MyGame.UI;
using MyGame.World;
using System;

namespace MyGame.States
{
    public class GameState : IState
    {
        Player player;
        Camera camera;
        OrthogonalMap map;
        Icon icon;

        Vector2 mouseInWorldToTilePos;

        // Panel
        Texture2D panel;
        Texture2D lineBReak;

        public void Initialize()
        {
            Console.WriteLine("GameState Initialized!");

            // Initialize camera and set its initial position to the player position
            camera = new Camera();
            camera.Zoom = 2f; // Ensure the camera zoom is set properly
            camera.Rotation = 0.0f; // Ensure the camera rotation is set properly
            camera.Position = new Vector2(570, 610);

            map = new OrthogonalMap();
            map.LoadContent();

            player = new Player(new Vector2(0, 0), map);
            icon = new Icon(Globals.screenWidth - 24, 0);
            icon.OnClick += MenuPressed;

            panel = Globals.Content.Load<Texture2D>("UI/panel");
            lineBReak = Globals.Content.Load<Texture2D>("UI/lineBreak");
        }

        private void MenuPressed()
        {
            icon.isPressed = !icon.isPressed;
            
        }

        private void GoBack()
        {
            Console.WriteLine("Back Button pressed.");
            Game1.stateManager.PopState(this);
            Game1.stateManager.PushState(new MenuState());
        }

        public void UnloadContent()
        {
            icon.OnClick -= MenuPressed;
            Console.WriteLine("Disposing GameState.");
        }

        public void Update(GameTime gameTime)
        {
            // update map
            map.Update(gameTime);
            if (icon.isPressed)
                icon.Position = new Vector2(Globals.screenWidth - 216 - 24, 0);
            else
                icon.Position = new Vector2(Globals.screenWidth - 24, 0);

            // update player
            player.Update(gameTime);

            camera.Position = player.Position; // Adjust this offset as needed

            // Mouse to World position;
            Matrix inverseMatrix = Matrix.Invert(camera.GetTransformation(Globals.Graphics));
            Vector2 mouseInWorld = Vector2.Transform(new Vector2(MouseHandler.GetMousePosition().X, MouseHandler.GetMousePosition().Y), inverseMatrix);
            mouseInWorldToTilePos = new Vector2(mouseInWorld.X / 32, mouseInWorld.Y / 32);

            if (MouseHandler.IsMouseRightPressed())
            {
                if ((int)(mouseInWorldToTilePos.X) == 0 && (int)(mouseInWorldToTilePos.Y) == 0)
                {
                    map.ChangeTile(0, 0);
                }
            }
            icon.Update(gameTime);
            Console.WriteLine(Globals.screenWidth);
        }

        public void Draw()
        {
            // Apply camera transformation when drawing the map and player
            Globals.SpriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, camera.GetTransformation(Globals.Graphics));
            map.Draw();
            player.Draw();
            Globals.SpriteBatch.End();

            // Draw UI elements without camera transformation
            Globals.SpriteBatch.Begin();
            Globals.SpriteBatch.DrawString(Globals.SpriteFont, "Pos X: " + ((int)player.Position.X / (int)32) + ", Y: " + ((int)player.Position.Y / (int)32), new Vector2(0, 0), Color.White);
            Globals.SpriteBatch.DrawString(Globals.SpriteFont, "MPos X: " + ((int)mouseInWorldToTilePos.X + ", Y: " + (int)mouseInWorldToTilePos.Y), new Vector2(0, 32), Color.White);

            player.DrawHealth();
            if (icon.isPressed)
            {
                Globals.SpriteBatch.Draw(panel, new Rectangle(Globals.screenWidth - 216, 0, panel.Width, Globals.screenHeight), Color.White);
                Globals.SpriteBatch.Draw(lineBReak, new Vector2(Globals.screenWidth - 216, Globals.screenHeight / 2), Color.White);
            }

            // Line break
            icon.Draw();

            Globals.SpriteBatch.End();
        }
    }
}
