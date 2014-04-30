using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;

namespace Navier_Boats.Engine.Entities
{
    [Serializable]
    public class HostileLivingEntity : LivingEntity
    {
        /// <summary>
        /// Creates a new HostileLivingEntity
        /// </summary>
        /// <param name="initialHealth">The initial health of the HostileLivingEntity</param>
        public HostileLivingEntity(int initialHealth)
            : base(initialHealth)
        {
            TintColor = Color.Red;
            headSprite.TintColor = TintColor;
        }

        protected HostileLivingEntity(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Handles when another IInteractable interacts with this entity
        /// </summary>
        /// <param name="interactor">The IInteractable that interacted with this entity</param>
        public override void Interact(IInteractable interactor)
        {
            if (interactor is LivingEntity && !(interactor is HostileLivingEntity))
            {
                (interactor as LivingEntity).TakeDamage(.5);
            }
        }

        public override void OnDeath()
        {
            base.OnDeath();
        }
    }
}
