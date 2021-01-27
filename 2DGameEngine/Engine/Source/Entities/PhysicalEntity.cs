﻿using GameEngine2D.Engine.Source.Entities;
using GameEngine2D.Engine.Source.Entities.Controller;
using GameEngine2D.Entities;
using GameEngine2D.Entities.Interfaces;
using GameEngine2D.Global;
using GameEngine2D.Source;
using GameEngine2D.Source.Layer;
using GameEngine2D.Source.Util;
using GameEngine2D.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace GameEngine2D
{
    public class PhysicalEntity : Entity
    {

        private Vector2 bump;

        protected UserInputController UserInput;
        protected float elapsedTime;
        private float steps;
        private float step;
        private float steps2;
        private float step2;
        private float t;

        public Vector2 Velocity = Vector2.Zero;

        protected float Friction = Config.FRICTION;
        protected float BumpFriction = Config.BUMP_FRICTION;

        protected float MovementSpeed = Config.CHARACTER_SPEED;

        public float GravityValue = Config.GRAVITY_FORCE;

        protected float FallSpeed { get; set; }

        public bool HasGravity { get; set; }  = Config.GRAVITY_ON;

        

        protected GameTime GameTime;

        protected Direction CurrentFaceDirection { get; set; } = Engine.Source.Entities.Direction.RIGHT;

        public PhysicalEntity(Layer layer, Entity parent, Vector2 startPosition, Texture2D texture = null, bool collider = false, SpriteFont font = null) : base(layer, parent, startPosition, texture, collider, font)
        {
            Active = true;
            ResetPosition(startPosition);
        }

        override public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (DEBUG_SHOW_PIVOT)
            {
                spriteBatch.DrawString(font, "Y: " + Velocity.Y, DrawPosition, Color.White);
            }
            
            base.Draw(spriteBatch, gameTime);
        }

        public override void PreUpdate(GameTime gameTime)
        {
            if (UserInput != null)
            {
                UserInput.Update();
            }
            base.PreUpdate(gameTime);
        }

        public override void Update(GameTime gameTime)
        {

            elapsedTime = TimeUtil.GetElapsedTime(gameTime);

            this.GameTime = gameTime;

            steps = (float)Math.Ceiling(Math.Abs((Velocity.X + bump.X) * elapsedTime));
            step = (float)(Velocity.X + bump.X) * elapsedTime / steps;
            while (steps > 0)
            {
                InCellLocation.X += step;

                if (CheckForCollisions && InCellLocation.X >= CollisionOffsetLeft && CollisionChecker.HasBlockingColliderAt(GridUtil.GetRightGrid(GridCoordinates)))
                {
                    InCellLocation.X = CollisionOffsetLeft;
                }

                if (CheckForCollisions && InCellLocation.X <= CollisionOffsetRight && CollisionChecker.HasBlockingColliderAt(GridUtil.GetLeftGrid(GridCoordinates)))
                {
                    InCellLocation.X = CollisionOffsetRight;
                }

                while (InCellLocation.X > 1) { InCellLocation.X--; GridCoordinates.X++; }
                while (InCellLocation.X < 0) { InCellLocation.X++; GridCoordinates.X--; }
                steps--;
            }
            Velocity.X *= (float)Math.Pow(Friction, elapsedTime);
            bump.X *= (float)Math.Pow(BumpFriction, elapsedTime);

            //rounding stuff
            if (Math.Abs(Velocity.X) <= 0.0005 * elapsedTime) Velocity.X = 0;
            if (Math.Abs(bump.X) <= 0.0005 * elapsedTime) bump.X = 0;

            // Y
            if (HasGravity && !OnGround())
            {
                if (FallSpeed == 0)
                {
                    FallSpeed = (float)gameTime.TotalGameTime.TotalSeconds;
                }
                ApplyGravity(gameTime);
            }

            steps2 = (float)Math.Ceiling(Math.Abs((Velocity.Y + bump.Y) * elapsedTime));
            step2 = (float)(Velocity.Y + bump.Y) * elapsedTime / steps2;
            while (steps2 > 0)
            {
                InCellLocation.Y += step2;

                if (CheckForCollisions && InCellLocation.Y > CollisionOffsetBottom && OnGround() && Velocity.Y > 0)
                {
                    if (HasGravity)
                    {
                        Velocity.Y = 0;
                        bump.Y = 0;
                    }

                    InCellLocation.Y = CollisionOffsetBottom;
                }

                if (CheckForCollisions && InCellLocation.Y < CollisionOffsetTop && CollisionChecker.HasBlockingColliderAt(GridUtil.GetUpperGrid(GridCoordinates)))
                {
                    Velocity.Y = 0;
                    InCellLocation.Y = CollisionOffsetTop;
                }
                   
                while (InCellLocation.Y > 1) { InCellLocation.Y--; GridCoordinates.Y++; }
                while (InCellLocation.Y < 0) { InCellLocation.Y++; GridCoordinates.Y--; }
                steps2--;
            }

            // workaround for bug when character ends up standing inside a collider or slightly above it
            // when the movement started from below or the fall started from height less than sprite graphics offset
            if (OnGround() && HasGravity && Math.Abs(Velocity.Y) < 0.05)
            {

                if (InCellLocation.Y > CollisionOffsetBottom) {
                    InCellLocation.Y -= 0.09f * elapsedTime * GravityValue;
                }
                if (InCellLocation.Y < CollisionOffsetBottom)
                {
                    InCellLocation.Y += 0.09f * elapsedTime * GravityValue;
                }
                //InCellLocation.Y = CollisionOffsetBottom;
            }

            Velocity.Y *= (float)Math.Pow(Friction, elapsedTime);
            bump.Y *= (float)Math.Pow(BumpFriction, elapsedTime);
            //rounding stuff
            if (Math.Abs(Velocity.Y) <= 0.0005 * elapsedTime) Velocity.Y = 0;
            if (Math.Abs(bump.Y) <= 0.0005 * elapsedTime) bump.Y = 0;
            base.Update(gameTime);
        }

        public void Bump(Vector2 direction)
        {
            bump = direction;
        }

        private void ApplyGravity(GameTime gameTime)
        {
            if (Config.INCREASING_GRAVITY)
            {
                t = (float)(gameTime.TotalGameTime.TotalSeconds - FallSpeed) * Config.GRAVITY_T_MULTIPLIER;
                Velocity.Y += GravityValue * t * elapsedTime;
            }
            else
            {
                Velocity.Y += GravityValue * elapsedTime;
            }
        }

        public override void PostUpdate(GameTime gameTime)
        {
            Position = (GridCoordinates + InCellLocation) * Config.GRID;
            base.PostUpdate(gameTime);
        }

        protected bool OnGround()
        {
            return CollisionChecker.HasBlockingColliderAt(GridUtil.GetBelowGrid(GridCoordinates));
        }

        public void ResetPosition(Vector2 position)
        {
            InCellLocation = Vector2.Zero;
            this.Position = StartPosition = position;
            this.FallSpeed = 0;
        }
    }
}
