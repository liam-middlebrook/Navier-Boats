using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Navier_Boats.Engine.Entities
{
    public interface IInteractable
    {
        /// <summary>
        /// Handles the interaction between two IInteractables
        /// </summary>
        /// <param name="interactor">The IIteractable that interacted with the other</param>
        /// <remarks>This is called TWICE for each interaction</remarks>
        void Interact(IInteractable interactor);

        /// <summary>
        /// Checks if an entity is interacting with this instance of IInteractable
        /// </summary>
        /// <param name="entities"></param>
        void CheckInteractions(List<Entity> entities);
    }
}
