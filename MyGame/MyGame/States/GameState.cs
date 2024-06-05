using Microsoft.Xna.Framework;
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

        // icons with images
        Texture2D icons;

        Icon iconMenu;
        Icon backpackIcon;
        Icon skillsIcon;
        
        Icon gearIcon;
        Icon logoutIcon;

        Inventory inventory;

        Vector2 mouseInWorldToTilePos;

        // player preview panel
        Texture2D playerPreview;

        // equipment slots
        Slot equipmentSlot1;
        Slot equipmentSlot2;
        Slot equipmentSlot3;
        Slot equipmentSlot4;

        // Panel
        Texture2D panel;
        Texture2D lineBReak;

        public void Initialize()
        {
            Console.WriteLine("GameState Initialized!");
            Game1.Instance.Window.ClientSizeChanged += OnWindowResize;

            // Initialize camera and set its initial position to the player position
            camera = new Camera();
            camera.Zoom = 2f; // Ensure the camera zoom is set properly
            camera.Rotation = 0.0f; // Ensure the camera rotation is set properly
            camera.Position = new Vector2(570, 610);

            map = new OrthogonalMap();
            map.LoadContent();

            player = new Player(new Vector2(8, 4), map);

            // menu icon
            iconMenu = new Icon(Globals.screenWidth - 24, 0);
            iconMenu.OnClick += MenuPressed;

            backpackIcon = new Icon(Globals.screenWidth - 196 + 4, Globals.screenHeight / 2 - 28);
            backpackIcon.OnClick += OpenBackpack;

            skillsIcon = new Icon(Globals.screenWidth - 196 + 4 + 24 + 4, Globals.screenHeight / 2 - 28);
            gearIcon = new Icon(Globals.screenWidth - 24 - 4, Globals.screenHeight / 2 - 28);
            logoutIcon = new Icon(Globals.screenWidth - 24 - 4 - 24 - 4, Globals.screenHeight / 2 - 28);

            panel = Globals.Content.Load<Texture2D>("UI/panel");
            
            // player preview
            playerPreview = Globals.Content.Load<Texture2D>("Character/3398");

            // icons inside panel
            lineBReak = Globals.Content.Load<Texture2D>("UI/lineBreak");

            // slots
            inventory = new Inventory(new Vector2(Globals.screenWidth - 196 + 8, Globals.screenHeight / 2 + 8));

            // equipment slots
            equipmentSlot1 = new Slot(new Vector2(8 + Globals.screenWidth - 196 + 4 + 4, Globals.screenHeight / 2 - 28 - 24 - 18 - 16));
            equipmentSlot2 = new Slot(new Vector2(8 + Globals.screenWidth - 196 + 4 + 32 + 4 + 4 + 4 + 4, Globals.screenHeight / 2 - 28 - 24 - 18 - 16));
            equipmentSlot3 = new Slot(new Vector2(8 + Globals.screenWidth - 196 + 4 + 64 + 4 + 4 + 4 + 4 + 4 + 4 + 4, Globals.screenHeight / 2 - 28 - 24 - 18 - 16));
            equipmentSlot4 = new Slot(new Vector2(8 + Globals.screenWidth - 196 + 4 + 96 + 4 + 4 + 4 + 4 + 4 + 4 + 4 + 4 + 4 + 4, Globals.screenHeight / 2 - 28 - 24 - 18 - 16));

            // icon images
            icons = Globals.Content.Load<Texture2D>("UI/icons");
        }

        private void OpenBackpack()
        {
            inventory.IsVisible = !inventory.IsVisible;
        }

        private void OnWindowResize(object sender, EventArgs e)
        {
            inventory.UpdatePosition(new Vector2(Globals.screenWidth - 196 + 8, Globals.screenHeight / 2 + 8));
            equipmentSlot1.UpdatePosition(new Vector2(8 + Globals.screenWidth - 196 + 4 + 4, Globals.screenHeight / 2 - 28 - 24 - 18 - 16));
            equipmentSlot2.UpdatePosition(new Vector2(8 + Globals.screenWidth - 196 + 4 + 32 + 4 + 4 + 4 + 4, Globals.screenHeight / 2 - 28 - 24 - 18 - 16));
            equipmentSlot3.UpdatePosition(new Vector2(8 + Globals.screenWidth - 196 + 4 + 64 + 4 + 4 + 4 + 4 + 4 + 4 + 4, Globals.screenHeight / 2 - 28 - 24 - 18 - 16));
            equipmentSlot4.UpdatePosition(new Vector2(8 + Globals.screenWidth - 196 + 4 + 96 + 4 + 4 + 4 + 4 + 4 + 4 + 4 + 4 + 4 + 4, Globals.screenHeight / 2 - 28 - 24 - 18 - 16));
        }

        private void MenuPressed()
        {
            iconMenu.isPressed = !iconMenu.isPressed;
        }

        private void GoBack()
        {
            Console.WriteLine("Back Button pressed.");
            Game1.stateManager.PopState(this);
            Game1.stateManager.PushState(new MenuState());
        }

        public void UnloadContent()
        {
            iconMenu.OnClick -= MenuPressed;
            Game1.Instance.Window.ClientSizeChanged -= OnWindowResize;
            Console.WriteLine("Disposing GameState.");
        }

        public void Update(GameTime gameTime)
        {
            // update map
            map.Update(gameTime);
            if (iconMenu.isPressed)
                iconMenu.Position = new Vector2(Globals.screenWidth - 196 - 24, 0);
            else
                iconMenu.Position = new Vector2(Globals.screenWidth - 24, 0);

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

            iconMenu.Update(gameTime);
            
            backpackIcon.Update(gameTime);
            backpackIcon.Position = new Vector2(Globals.screenWidth - 196 + 4, Globals.screenHeight / 2 - 28);
            skillsIcon.Update(gameTime);
            skillsIcon.Position = new Vector2(Globals.screenWidth - 196 + 4 + 24 + 4, Globals.screenHeight / 2 - 28);
            gearIcon.Update(gameTime);
            gearIcon.Position = new Vector2(Globals.screenWidth - 24 - 4, Globals.screenHeight / 2 - 28);
            logoutIcon.Update(gameTime);
            logoutIcon.Position = new Vector2(Globals.screenWidth - 24 - 4 - 24 - 4, Globals.screenHeight / 2 - 28);
            
            inventory.Update(gameTime);
            inventory.UpdatePosition(new Vector2(Globals.screenWidth - 196 + 8, Globals.screenHeight / 2 + 8));
            Console.WriteLine(inventory.Position);

            // equipment slots
            equipmentSlot1.Update(gameTime);
            equipmentSlot2.Update(gameTime);
            equipmentSlot3.Update(gameTime);
            equipmentSlot4.Update(gameTime);
        }

        public void Draw()
        {
            // Apply camera transformation when drawing the map and player
            Globals.SpriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, camera.GetTransformation(Globals.Graphics));
            map.Draw();
            player.Draw();
            Globals.SpriteBatch.End();

            // Draw UI elements without camera transformation
            Globals.SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null);
            Globals.SpriteBatch.DrawString(Globals.SpriteFont, "Pos X: " + ((int)player.Position.X / (int)32) + ", Y: " + ((int)player.Position.Y / (int)32), new Vector2(0, 0), Color.White);
            Globals.SpriteBatch.DrawString(Globals.SpriteFont, "MPos X: " + ((int)mouseInWorldToTilePos.X + ", Y: " + (int)mouseInWorldToTilePos.Y), new Vector2(0, 32), Color.White);

            player.DrawHealth();
            if (iconMenu.isPressed)
            {
                Globals.SpriteBatch.Draw(panel, new Rectangle(Globals.screenWidth - 196, 0, 196, Globals.screenHeight), Color.White);
                Globals.SpriteBatch.Draw(lineBReak, new Vector2(Globals.screenWidth - 196, Globals.screenHeight / 2), Color.White);
                Globals.SpriteBatch.Draw(lineBReak, new Vector2(Globals.screenWidth - 196, Globals.screenHeight / 2 - 32), Color.White);

                // Slots here
                inventory.Draw();

                // icons
                backpackIcon.Draw();
                skillsIcon.Draw();
                gearIcon.Draw();
                logoutIcon.Draw();

                // equipment slot
                equipmentSlot1.Draw();
                equipmentSlot2.Draw();
                equipmentSlot3.Draw();
                equipmentSlot4.Draw();

                Globals.SpriteBatch.Draw(playerPreview, new Rectangle(8 + Globals.screenWidth - 196 + 32, Globals.screenHeight / 2 - 32 - 180, 100, 100), Color.White);

                // icon images
                Globals.SpriteBatch.Draw(icons, new Rectangle((int)backpackIcon.Position.X + 4, (int)backpackIcon.Position.Y + 4, 16, 16), new Rectangle(116, 10, 16, 16), Color.White);
                Globals.SpriteBatch.Draw(icons, new Rectangle((int)skillsIcon.Position.X + 4, (int)skillsIcon.Position.Y + 4, 16, 16), new Rectangle(134, 10, 16, 16), Color.White);
                Globals.SpriteBatch.Draw(icons, new Rectangle((int)gearIcon.Position.X + 4, (int)gearIcon.Position.Y + 4, 16, 16), new Rectangle(98, 10, 16, 16), Color.White);
                Globals.SpriteBatch.Draw(icons, new Rectangle((int)logoutIcon.Position.X + 4, (int)logoutIcon.Position.Y + 4, 16, 16), new Rectangle(80, 10, 16, 16), Color.White);

                // skills tab
                Globals.SpriteBatch.DrawString(Globals.SpriteFont,
                               "Experience",
                               new Vector2(backpackIcon.Position.X, backpackIcon.Position.Y + 32),  // Position
                               Color.White,            // Color
                               0,                      // Rotation
                               Vector2.Zero,           // Origin
                               new Vector2(0.5f, 0.5f),// Scale (this will double the size)
                               SpriteEffects.None,
                               0);
            }

            iconMenu.Draw();
            

            Globals.SpriteBatch.End();
        }
    }
}
