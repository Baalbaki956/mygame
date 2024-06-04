using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGame.States
{
    public interface IState
    {
        void Initialize();
        void UnloadContent();
        void Update(GameTime gameTime);
        void Draw();
    }
}
