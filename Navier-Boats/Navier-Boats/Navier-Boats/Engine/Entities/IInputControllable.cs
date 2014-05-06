using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Navier_Boats.Engine.System;

namespace Navier_Boats.Engine.Entities
{
    public interface IInputControllable
    {
        /// <summary>
        /// Handles Input from the Player
        /// </summary>
        /// <param name="inputHelper">The input state from the player</param>
        void HandleInput(InputStateHelper inputHelper);
    }
}
