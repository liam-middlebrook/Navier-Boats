using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Navier_Boats.Engine.System
{
    public class ConsoleVars
    {
        private bool debugDraw;

        public bool DebugDraw { get { return debugDraw; } set { debugDraw = value; } }

        public bool DebugPathing { get; set; }

        #region SINGLETON_MEMBERS

        private static ConsoleVars _instance;
        private ConsoleVars()
        {
            debugDraw = false;
        }

        #endregion

        public static ConsoleVars GetInstance()
        {
            if(_instance!=null)
            {
                return _instance;
            }
            return (_instance = new ConsoleVars());
        }
    }
}
