using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Navier_Boats.Engine.Entities
{
    class HostileLivingEntity : LivingEntity
    {
        public HostileLivingEntity(int initialHealth)
            : base(initialHealth)
        {
            TintColor = Color.Red;
            headSprite.TintColor = TintColor;
        }
    }
}
