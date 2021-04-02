﻿using MonolithEngine.Engine.Source.MyGame;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonolithEngine.Engine.Source.MyGame
{
    public class VideoConfiguration
    {
        internal static MonolithGame GameInstance;

        public static int RESOLUTION_WIDTH;
        public static int RESOLUTION_HEIGHT;
        public static bool VSYNC;
        public static int FRAME_LIMIT;
        public static bool FULLSCREEN;

        public static void Apply()
        {
            GameInstance.ApplyVideoConfiguration();
        }
    }
}
