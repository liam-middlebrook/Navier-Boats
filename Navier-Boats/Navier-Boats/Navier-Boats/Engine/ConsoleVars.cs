using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Navier_Boats.Engine
{
    public class ConsoleVars
    {
        private bool debugDraw;

        public bool DebugDraw { get { return debugDraw; } set { debugDraw = value; } }

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
