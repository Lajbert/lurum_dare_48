﻿using lurum_dare_48.Source.Entities.Traps;
using Microsoft.Xna.Framework;
using MonolithEngine.Engine.Source.Entities.Abstract;
using MonolithEngine.Engine.Source.Physics.Trigger;
using MonolithEngine.Engine.Source.Scene;
using MonolithEngine.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace lurum_dare_48.Source.Entities.Triggers
{
    class TrapTrigger : Entity
    {
        public TrapTrigger(AbstractScene scene, Vector2 position, int width, int height) : base(scene.LayerManager.EntityLayer, null, position)
        {
            Active = true;

            AddComponent(new BoxTrigger(this, width, height, Vector2.Zero, tag: ""));
            //(GetComponent<ITrigger>() as BoxTrigger).DEBUG_DISPLAY_TRIGGER = true;
            //DEBUG_SHOW_PIVOT = true;
        }

        public override void OnLeaveTrigger(string triggerTag, IGameObject otherEntity)
        {
            base.OnLeaveTrigger(triggerTag, otherEntity);
            if (otherEntity is Saw)
            {
                (otherEntity as Saw).Velocity *= -1;
            }
        }
    }
}
