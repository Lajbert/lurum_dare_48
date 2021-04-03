﻿using Microsoft.Xna.Framework;
using MonolithEngine.Engine.Source.Asset;
using MonolithEngine.Engine.Source.Scene;
using MonolithEngine.Engine.Source.Scene.Transition;
using MonolithEngine.Engine.Source.UI;
using MonolithEngine.Global;
using MonolithEngine.Source.Camera2D;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForestPlatformerExample.Source.Scenes
{
    class PauseMenuScene : AbstractScene
    {
        public PauseMenuScene() : base("PauseMenu", true)
        {

        }

        public override ICollection<object> ExportData()
        {
            return null;
        }

        public override ISceneTransitionEffect GetTransitionEffect()
        {
            return null;
        }

        public override void ImportData(ICollection<object> state)
        {
            
        }

        public override void Load()
        {
            SelectableImage continueGame = new SelectableImage(Assets.GetTexture("HUDContinueBase"), Assets.GetTexture("HUDContinueSelected"), new Vector2(150, 150), scale: 0.25f);
            continueGame.HoverSound = Assets.GetSoundEffect("MenuHover");
            continueGame.SelectSound = Assets.GetSoundEffect("MenuSelect");

            continueGame.OnClick = () =>
            {
                SceneManager.StartScene("Level1");
            };

            SelectableImage settings = new SelectableImage(Assets.GetTexture("HUDSettingsBase"), Assets.GetTexture("HUDSettingsSelected"), new Vector2(150, 200), scale: 0.25f);
            settings.HoverSound = Assets.GetSoundEffect("MenuHover");
            settings.SelectSound = Assets.GetSoundEffect("MenuSelect");

            settings.OnClick = () =>
            {
                SceneManager.StartScene("Settings");
            };

            SelectableImage quit = new SelectableImage(Assets.GetTexture("HUDQuitBase"), Assets.GetTexture("HUDQuitSelected"), new Vector2(150, 250), scale: 0.25f);
            quit.HoverSound = Assets.GetSoundEffect("MenuHover");
            quit.SelectSound = Assets.GetSoundEffect("MenuSelect");

            quit.OnClick = Config.ExitAction;

            UI.AddUIElement(quit);
            UI.AddUIElement(continueGame);
            UI.AddUIElement(settings);
        }

        public override void OnEnd()
        {
            
        }

        public override void OnStart()
        {
            ForestPlatformerGame.Paused = true;
        }
    }
}
