using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Navier_Boats.Engine.Entities;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Navier_Boats.Game.Entities
{
    class Wanderer : HostileLivingEntity
    {
        private Random randy;

        public Wanderer(Vector2 position, int randomSeed)
            : base(100)
        {
            Position = position;
            randy = new Random(randomSeed);
            Speed = 50;
        }

        public override void Update(GameTime gameTime)
        {
            // Change the player's velocity based on inputs
            Acceleration = new Vector2((float)(randy.NextDouble() - 0.5) * 1.0f, (float)(randy.NextDouble() - 0.5) * 1.0f); 

            // Make the Head look at the mouse
            headSprite.Rotation = Rotation;

            base.Update(gameTime);
        }
    }
}
