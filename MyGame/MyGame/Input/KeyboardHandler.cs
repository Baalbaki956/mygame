using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGame.Input
{
    public class KeyboardHandler
    {
        private static KeyboardState currentKeyboardState;

        public static KeyboardState CurrentKeyboardState
        {
            get { return KeyboardHandler.currentKeyboardState; }
            private set { KeyboardHandler.currentKeyboardState = value; }
        }
        private static KeyboardState previousKeyboardState;

        public static KeyboardState PreviousKeyboardState
        {
            get { return KeyboardHandler.previousKeyboardState; }
            private set { KeyboardHandler.previousKeyboardState = value; }
        }

        public static void Update()
        {
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();
        }

        public static bool IsKeyPressed(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key);
        }

        public static bool IsKeyJustPressed(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key) && previousKeyboardState.IsKeyUp(key);
        }
    }
}
