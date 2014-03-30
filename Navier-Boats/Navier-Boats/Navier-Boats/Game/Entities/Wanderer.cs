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
        private double timeUntilNewAccel = 0f;

        public Wanderer(Vector2 position)
            : base(100)
        {
            Position = position;
            initialSpeed = 50;
        }

        public override void Update(GameTime gameTime)
        {
            timeUntilNewAccel -= gameTime.ElapsedGameTime.TotalSeconds;
            if (timeUntilNewAccel <= 0)
            {
                Velocity = new Vector2((float)(CurrentLevel.GetRandom().NextDouble() - 0.5) * 5.0f, (float)(CurrentLevel.GetRandom().NextDouble() - 0.5) * 5.0f);
                timeUntilNewAccel = CurrentLevel.GetRandom().NextDouble() * 3.0;
            }

            // Make the Head look at the mouse
            headSprite.Rotation = Rotation;

            base.Update(gameTime);
        }
    }
}
