﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Navier_Boats.Engine.Entities
{
    public interface ILateUpdateable
    {
        void LateUpdate(GameTime gameTime);
    }
}
