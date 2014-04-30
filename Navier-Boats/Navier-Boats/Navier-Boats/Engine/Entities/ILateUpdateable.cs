using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Navier_Boats.Engine.Entities
{
    public interface ILateUpdateable
    {
        /// <summary>
        /// A second update call that disregards player input
        /// </summary>
        /// <param name="gameTime">Data about the time between update cycles for our game</param>
        void LateUpdate(GameTime gameTime);
    }
}
