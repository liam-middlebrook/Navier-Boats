using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Navier_Boats.Game.Inventory
{
    public class InventoryException : Exception
    {
        public InventoryException()
            : base()
        {
        }

        public InventoryException(string message)
            : base(message)
        {
        }

        public InventoryException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
