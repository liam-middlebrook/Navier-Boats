using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Navier_Boats.Game.Inventory
{
    public class InventoryOutOfSpaceException : InventoryException
    {
        public InventoryOutOfSpaceException()
            : base()
        {
        }

        public InventoryOutOfSpaceException(string message)
            : base(message)
        {
        }

        public InventoryOutOfSpaceException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
