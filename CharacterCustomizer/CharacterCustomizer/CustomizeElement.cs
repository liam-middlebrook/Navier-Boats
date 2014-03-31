using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CharacterCustomizer
{
    class CustomizeElement
    {
        private int scale;

        /// <summary>
        /// get
        /// </summary>
        public int Scale
        {
            get { return scale; }
        }

        public CustomizeElement(int s)
        {
            scale = s;
        }

        public int ConvertPixelsToScale(int span)
        {
            return span * scale;
        }
    }
}
