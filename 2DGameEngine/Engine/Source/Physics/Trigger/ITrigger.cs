﻿using GameEngine2D.Engine.Source.Entities.Abstract;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameEngine2D.Engine.Source.Physics.Trigger
{
    public interface ITrigger
    {
        public bool IsInsideTrigger(IGameObject otherObject);

        public string GetTag();
    }
}
