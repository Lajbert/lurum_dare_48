﻿using _2DGameEngine.Global;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2DGameEngine.src.Util
{
    class TimeUtil
    {
        public static float GetElapsedTime(GameTime gameTime)
        {
            return (float)gameTime.ElapsedGameTime.TotalSeconds * Constants.TIME_OFFSET;
        }
    }
}
