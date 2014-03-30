using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Navier_Boats.Engine.Entities;
using Navier_Boats.Engine.Level;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Navier_Boats.Game.Entities
{
    public class Wanderer : HostileLivingEntity
    {
        public Wanderer(Vector2 position)
            : base(100)
        {
            Position = position;
            Speed = 50;
        }

        public override void Update(GameTime gameTime)
        {
            Acceleration = new Vector2((float)(CurrentLevel.GetRandom().NextDouble() - 0.5) * 1.0f, (float)(CurrentLevel.GetRandom().NextDouble() - 0.5) * 1.0f); 

            // Make the Head look at the mouse
            headSprite.Rotation = Rotation;

            base.Update(gameTime);
        }
    }
}
