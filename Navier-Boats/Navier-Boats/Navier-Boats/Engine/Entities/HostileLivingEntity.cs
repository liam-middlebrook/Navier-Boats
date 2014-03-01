﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Navier_Boats.Engine.Entities
{
    class HostileLivingEntity : LivingEntity, IInteractable
    {
        public HostileLivingEntity(int initialHealth)
            : base(initialHealth)
        {
            TintColor = Color.Red;
            headSprite.TintColor = TintColor;
        }

        public void Interact(IInteractable interactor)
        {
            TakeDamage(1);
            if (interactor is LivingEntity)
            {
                ((LivingEntity)interactor).TakeDamage(2);
            }
        }
    }
}