using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Navier_Boats.Engine.Entities;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using libXNADeveloperConsole;
using Navier_Boats.Engine.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Navier_Boats.Engine.Inventory;
using Navier_Boats.Engine.System;
using Navier_Boats.Engine.Level;


namespace Navier_Boats.Game.Entities
{
    public class Weapon : Entity, IInteractable
    {
        enum WeaponState
        {
            left,
            right,
            swinging
        }

        #region Properties
        private WeaponState currentState;
        private int range;

        public int Range
        {
            get { return range; }
            set { range = value; }
        }


        private WeaponState CurrentState
        {
            get { return currentState; }
            set { currentState = value; }
        }
        #endregion

        public Weapon(int baseRange) : base()
        {
            currentState = WeaponState.right;
            range = baseRange;
            this.Position = new Vector2((int)Math.Round(EntityManager.GetInstance().Player.Position.X), (int)Math.Round(EntityManager.GetInstance().Player.Position.Y));
        }

        public override void Update(GameTime gameTime)
        {
            this.Position = new Vector2((int)Math.Round(EntityManager.GetInstance().Player.Position.X), (int)Math.Round(EntityManager.GetInstance().Player.Position.Y));
            if (currentState == WeaponState.swinging)
            {

            }
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
