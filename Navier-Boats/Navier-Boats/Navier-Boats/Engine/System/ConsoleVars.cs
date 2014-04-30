using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Navier_Boats.Engine.System
{
    public class ConsoleVars
    {
        private int windowHeight;

        private int windowWidth;

        private bool debugDraw;

        public int WindowHeight { get { return windowHeight; } set { windowHeight = value; } }
       
        public int WindowWidth { get { return windowWidth; } set { windowWidth = value; } }

        public Rectangle WindowRect { get { return new Rectangle(0, 0, windowWidth, windowHeight); } }

        public bool DebugDraw { get { return debugDraw; } set { debugDraw = value; } }

        public bool DebugPathing { get; set; }

        public bool ShowChunkBorders { get; set; }

        public bool ShowRoadConnectors { get; set; }

        public bool ShowRoads { get; set; }

        public bool GodMode { get; set; }

        #region SINGLETON_MEMBERS

        private static ConsoleVars _instance;
        private ConsoleVars()
        {
            debugDraw = false;
            ShowRoads = true;
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
