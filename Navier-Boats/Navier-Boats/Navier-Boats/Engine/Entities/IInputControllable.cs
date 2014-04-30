using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Navier_Boats.Engine.Entities
{
    public interface IInputControllable
    {
        /// <summary>
        /// Handles Input from the Player
        /// </summary>
        /// <param name="keyState">The current state of the keyboard</param>
        /// <param name="prevKeyState">The previous state of the keyboard</param>
        /// <param name="mouseState">The current state of the mouse</param>
        /// <param name="prevMouseState">The previous state of the mouse</param>
        void HandleInput(KeyboardState keyState, KeyboardState prevKeyState, MouseState mouseState, MouseState prevMouseState);
    }
}
