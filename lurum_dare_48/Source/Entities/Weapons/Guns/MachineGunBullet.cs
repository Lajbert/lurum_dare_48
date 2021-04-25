﻿using Microsoft.Xna.Framework;
using MonolithEngine;
using MonolithEngine.Engine.Source.Entities;
using MonolithEngine.Engine.Source.Scene;
using System;
using System.Collections.Generic;
using System.Text;

namespace lurum_dare_48.Source.Entities.Weapons.Guns
{
    class MachineGunBullet : PhysicalEntity, IBullet
    {
        public MachineGunBullet(AbstractScene scene, Vector2 position, Vector2 direction) : base(scene.LayerManager.EntityLayer, null, position)
        {

        }

        public Vector2 GetImpactForce()
        {
            throw new NotImplementedException();
        }

        public Vector2 GetPosition()
        {
            throw new NotImplementedException();
        }
    }
}
