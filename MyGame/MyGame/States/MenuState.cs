using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MyGame.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGame.States
{
    public class MenuState : IState
    {
        Texture2D menuPanel;
        Vector2 panelPosition;
        Texture2D bg;

        Button btnStart;
        Button btnExit;

        public void Initialize()
        {
            Console.WriteLine("MenuState Inisialized!.");
            menuPanel = Globals.Content.Load<Texture2D>("UI/menu_panel");
            bg = Globals.Content.Load<Texture2D>("UI/background");
            
            btnStart = new Button((int)panelPosition.X + 77, (int)panelPosition.Y + 61, "Start");
            btnStart.OnClick += StartGame;
            btnExit = new Button((int)panelPosition.X + 77, (int)panelPosition.Y + 61 + 64, "Exit");
            btnExit.OnClick += ExitGame;
        }

        private void ExitGame()
        {
            Game1.Instance.Exit();
        }

        public void UnloadContent()
        {
            Console.WriteLine("Disposing MenuState.");
            btnStart.OnClick -= StartGame;
            btnExit.OnClick -= ExitGame;
        }

        public void Update(GameTime gameTime)
        {
            btnStart.Update(gameTime);
            btnExit.Update(gameTime);

            panelPosition = new Vector2(Globals.screenWidth / 2 - menuPanel.Width / 2, Globals.screenHeight / 2 - menuPanel.Height / 2);
            btnStart.btnPosition = new Vector2((int)panelPosition.X + 77, (int)panelPosition.Y + 61);
            btnExit.btnPosition = new Vector2((int)panelPosition.X + 77, (int)panelPosition.Y + 61 + 64);
        }

        public void Draw()
        {
            Globals.SpriteBatch.Begin();
            Globals.SpriteBatch.Draw(bg, new Rectangle(0, 0, Globals.screenWidth, Globals.screenHeight), Color.White);
            Globals.SpriteBatch.Draw(menuPanel, panelPosition, Color.White);
            btnStart.Draw();
            btnExit.Draw();
            Globals.SpriteBatch.End();
        }

        private void StartGame()
        {
            Console.WriteLine("Start Button pressed.");
            Game1.stateManager.PopState(this);
            Game1.stateManager.PushState(new GameState());
        }
    }
}
