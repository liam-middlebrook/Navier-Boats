using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Navier_Boats.Engine.Entities
{
    class LivingEntity : Entity
    {
        private double health;

        public LivingEntity(double initialHealth)
            : base()
        {
            health = initialHealth;
        }

        public void TakeDamage(double damage)
        {
            health -= damage;
        }
    }
}
