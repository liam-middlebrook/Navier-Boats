using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Navier_Boats.Engine.Inventory
{
    public interface IItemFactory
    {
        IGameItem CreateItem();
    }
}
