using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Navier_Boats.Engine.Entities;
using Navier_Boats.Engine.Inventory;

namespace Navier_Boats.Game.Items
{
    [Serializable]
    class Weapon : BaseGameItem
    {
        public double Range
        {
            get;
            set;
        }

        public double Damage
        {
            get;
            set;
        }

        public void OnAction(LivingEntity executor)
        {
            throw new NotImplementedException();
        }

        public string getItemType()
        {
            return "Items.Weapon";
        }
    }
}
