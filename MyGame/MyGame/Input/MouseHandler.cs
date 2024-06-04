using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGame.Input
{
    public class MouseHandler
    {
        private static MouseState currentMouseState;

        public static MouseState CurrentMouseState
        {
            get { return MouseHandler.currentMouseState; }
            set { MouseHandler.currentMouseState = value; }
        }
        private static MouseState previousMouseState;

        public static MouseState PreviousMouseState
        {
            get { return MouseHandler.previousMouseState; }
            set { MouseHandler.previousMouseState = value; }
        }

        public static void Update()
        {
            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();
        }

        public static bool IsMouseLeftPressed()
        {
            return currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released;
        }

        public static bool IsMouseRightPressed()
        {
            return currentMouseState.RightButton == ButtonState.Pressed && previousMouseState.RightButton == ButtonState.Released;
        }

        public static Point GetMousePosition()
        {
            return new Point(currentMouseState.X, currentMouseState.Y);
        }
    }
}
