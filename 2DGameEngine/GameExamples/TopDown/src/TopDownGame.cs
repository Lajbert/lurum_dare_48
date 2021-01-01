﻿using GameEngine2D.Engine.src.Graphics.Primitives;
using GameEngine2D.Engine.src.Physics.Raycast;
using GameEngine2D.Engine.src.Util;
using GameEngine2D.Entities;
using GameEngine2D.GameExamples.SideScroller.src.Hero;
using GameEngine2D.GameExamples.TopDown.src.Hero;
using GameEngine2D.Global;
using GameEngine2D.src;
using GameEngine2D.src.Camera;
using GameEngine2D.src.Entities.Animation;
using GameEngine2D.src.Level;
using GameEngine2D.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameEngine2D.GameExamples.TopDown.src
{
    class TopDownGame : Game
    {
        private GraphicsDeviceManager graphics;
        private ContentManager contentManager;
        private SpriteFont font;
        private Camera camera;
        private Random random;
        private Color background1;
        private Color background2;
        private float sin;
        private MapSerializer mapSerializer;
        private float elapsedTime = 0;
        private SpriteBatch spriteBatch;
        private Ray2D ray;
        private Line line;

        public TopDownGame()
        {
            // >>>>>>> set framerate >>>>>>>>>>
            this.IsFixedTimeStep = true;//false;
            this.TargetElapsedTime = TimeSpan.FromSeconds(1d / Config.FPS); //60);

            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = Config.FULLSCREEN;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            random = new Random();
            background1 = GetRandomColor();
            background2 = GetRandomColor();
            // uncapped framerate
            //graphics.SynchronizeWithVerticalRetrace = false;
            //this.IsFixedTimeStep = false;
            mapSerializer = new LDTKJsonMapSerializer();
            contentManager = Content;

            Config.GRAVITY_ON = false;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Entity.SetGraphicsDeviceManager(graphics);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            camera = new Camera();
            font = Content.Load<SpriteFont>("DefaultFont");
            /*Entity child = new Entity(hero, graphics.GraphicsDevice, CreateRectangle(Constants.GRID, Color.Black), new Vector2(1 , 0) * Constants.GRID, font);
            Entity child2 = new Entity(child, graphics.GraphicsDevice, CreateRectangle(Constants.GRID, Color.Red), new Vector2(1, 0) * Constants.GRID, font);
            Entity child3 = new Entity(child2, graphics.GraphicsDevice, CreateRectangle(Constants.GRID, Color.Blue), new Vector2(1, 0) * Constants.GRID, font);*/
            // TODO: use this.Content to load your game content here
            graphics.PreferredBackBufferWidth = Config.RES_W;
            graphics.PreferredBackBufferHeight = Config.RES_H;
            graphics.ApplyChanges();
            CreateLevel();
            //public Knight(GraphicsLayer layer, Entity parent, GraphicsDeviceManager graphicsDevice, ContentManager content, SpriteBatch spriteBatch, Vector2 position, SpriteFont font)
            CubeGuy cube = new CubeGuy(new Vector2(5, 5) * Config.GRID, font);
            camera.trackTarget(cube, true);
            line = new Line(Scene.Instance.GetEntityLayer(), null, new Vector2(100, 100), 45, 1000, Color.Red);
            ray = new Ray2D(new Vector2(200, 100), 45);
        }

        private void CreateLevel()
        {



            /*for (int i = 2; i <= 300; i++)
            {
                Entity level = new Entity(graphics, Scene.Instance.GetColliderLayer(), null, new Vector2(i * Config.GRID, 25 * Config.GRID), font);
                level.SetSprite(SpriteUtil.CreateRectangle(graphics, Config.GRID, GetRandomColor()));
            }

            Scene.Instance.AddScrollableLayer(0.7f, true);
            Scene.Instance.AddScrollableLayer(0.5f, true);

            for (int i = 3; i <= 300; i++)
            {
                if (i % 15 == 0)
                {
                    for (int j = 22; j < 25; j++)
                    {
                        Entity level = new Entity(graphics, Scene.Instance.GetScrollableLayer(1), null, new Vector2(i * Config.GRID, j * Config.GRID), font);
                        level.SetSprite(SpriteUtil.CreateRectangle(graphics, Config.GRID, Color.Brown));
                    }
                }

            }

            for (int i = 3; i <= 300; i++)
            {
                if (i % 20 == 0)
                {
                    for (int j = 18; j < 25; j++)
                    {
                        Entity level = new Entity(graphics, Scene.Instance.GetScrollableLayer(0), null, new Vector2(i * Config.GRID, j * Config.GRID), font);
                        level.SetSprite(SpriteUtil.CreateRectangle(graphics, Config.GRID, Color.Black));
                    }
                }
            }

            for (int i = 2; i <= 300; i += 20)
            {
                for (int j = i; j <= i + 5; j++)
                {
                    Entity level = new Entity(graphics, Scene.Instance.GetColliderLayer(), null, new Vector2(j * Config.GRID, 20 * Config.GRID), font);
                    level.SetSprite(SpriteUtil.CreateRectangle(graphics, Config.GRID, GetRandomColor()));
                }

            }*/
            /*LDTKMap map = mapSerializer.Deserialize("D:/GameDev/MonoGame/2DGameEngine/2DGameEngine/Content/practise.json");
            HashSet<Vector2> collisions = map.GetCollisions();
            foreach (Vector2 coord in collisions) {
                Entity e =  new Entity(graphics, Scene.Instance.GetColliderLayer(), null , coord * Config.GRID, font);
                e.SetSprite(SpriteUtil.CreateRectangle(graphics, Config.GRID, Color.Black));

            }*/
            /*
            for (int i = 2 * Constants.GRID; i < 15 * Constants.GRID; i += Constants.GRID)
            {
                new Entity(null, graphics.GraphicsDevice, CreateRectangle(Constants.GRID, Color.Black), new Vector2(i, 17 * Constants.GRID), font);
            }

            for (int i = 16 * Constants.GRID; i < 27 * Constants.GRID; i += Constants.GRID)
            {
                new Entity(null, graphics.GraphicsDevice, CreateRectangle(Constants.GRID, Color.Black), new Vector2(i, 15 * Constants.GRID), font);
            }

            for (int i = 2 * Constants.GRID; i < 25 * Constants.GRID; i+= Constants.GRID)
            {
                new Entity(null, graphics.GraphicsDevice, CreateRectangle(Constants.GRID, Color.Black), new Vector2(i, 20 * Constants.GRID), font);
            }

            for (int i = 9 * Constants.GRID; i < 10 * Constants.GRID; i += Constants.GRID)
            {
                new Entity(null, graphics.GraphicsDevice, CreateRectangle(Constants.GRID, Color.Black), new Vector2(i, 19 * Constants.GRID), font);
            }

            for (int i = 25 * Constants.GRID; i < 50 * Constants.GRID; i += Constants.GRID)
            {
                new Entity(null, graphics.GraphicsDevice, CreateRectangle(Constants.GRID, Color.Black), new Vector2(i, 19 * Constants.GRID), font);
            }*/
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            RootContainer.Instance.UpdateAll(gameTime);
            camera.update(gameTime);
            camera.postUpdate(gameTime);
            base.Update(gameTime);
        }

        private Color GetRandomColor()
        {
            return Color.FromNonPremultiplied(random.Next(256), random.Next(256), random.Next(256), 256);
        }

        protected override void Draw(GameTime gameTime)
        {
            /*elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            sin = (float)Math.Sin(elapsedTime);
            if (sin <= 0.01)
            {
                background2 = GetRandomColor();
                elapsedTime = 0;
            } else if (sin >= 0.99)
            {
                background1 = GetRandomColor();
            }
            
            GraphicsDevice.Clear(Color.Lerp(background1, background2, sin));
            */

            GraphicsDevice.Clear(Color.Black);

            ray.Cast(line);

            // TODO: Add your drawing code here
            RootContainer.Instance.DrawAll(gameTime);

            base.Draw(gameTime);
        }
    }
}
