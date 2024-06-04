using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGame.States
{
    public class StateManager
    {
        Stack<IState> stateStack;

        public StateManager()
        {
            stateStack = new Stack<IState>();
        }

        public void PushState(IState newState)
        {
            if (stateStack.Count > 0 && stateStack.Peek() == newState)
                return;

            stateStack.Push(newState);
            newState.Initialize();
        }

        public void PopState(IState newState)
        {
            if (stateStack.Count > 0)
            {
                stateStack.Peek().UnloadContent();
                stateStack.Pop();
            }
        }

        public void UnloadContent()
        {
            if (stateStack.Count > 0)
                stateStack.Peek().UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            if (stateStack.Count > 0)
                stateStack.Peek().Update(gameTime);
        }

        public void Draw()
        {
            if (stateStack.Count > 0)
                stateStack.Peek().Draw();
        }
    }
}
