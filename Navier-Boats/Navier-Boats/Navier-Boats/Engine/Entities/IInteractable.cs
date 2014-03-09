using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Navier_Boats.Engine.Entities
{
    public interface IInteractable
    {
        void Interact(IInteractable interactor);
        void CheckInteractions(List<Entity> entities);
    }
}
