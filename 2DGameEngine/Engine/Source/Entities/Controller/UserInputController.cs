﻿using MonolithEngine.Engine.Source.Util;
using MonolithEngine.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonolithEngine.Engine.Source.Entities.Controller
{
    public class UserInputController
    {

        private Dictionary<Keys, bool> pressedKeys = new Dictionary<Keys, bool>();
        private Dictionary<Buttons, bool> pressedButtons = new Dictionary<Buttons, bool>();
        private Dictionary<KeyMapping, Action<Vector2>> keyPressActions = new Dictionary<KeyMapping, Action<Vector2>>();
        private Dictionary<Keys, Action> keyReleaseActions = new Dictionary<Keys, Action>();
        private Dictionary<Buttons, Action> buttonReleaseActions = new Dictionary<Buttons, Action>();
        private KeyboardState currentKeyboardState;
        private KeyboardState? prevKeyboardState;
        private GamePadState? prevGamepadState;
        private MouseState mouseState;
        private MouseState? prevMouseState;
        private GamePadState currentGamepadState;

        private int? prevMouseScrollWheelValue = 0;
        private Action mouseWheelUpAction;
        private Action mouseWheelDownAction;
        private float scrollThreshold = 0f;

        public Action<Vector2> LeftClickDownAction;
        public Action<Vector2> LeftClickUpAction;
        public Action<Vector2> LeftClickPressedAction;

        public Action<Vector2> RightClickDownAction;
        public Action<Vector2> RightClickUpAction;
        public Action<Vector2> RightClickPressedAction;

        public Action<Vector2, Vector2> MouseMovedAction;

        private Vector2 leftThumbstick = Vector2.Zero;
        private Vector2 rightThumbStick = Vector2.Zero;

        public bool ControlsDisabled = false;

        public void RegisterKeyPressAction(Keys key, Buttons controllerButton, Action<Vector2> action, bool singlePressOnly = false, int pressCooldown = 0)
        {
            keyPressActions.Add(new KeyMapping(key, controllerButton, singlePressOnly, pressCooldown), action);
            pressedKeys[key] = false;
            pressedButtons[controllerButton] = false;
        }

        public void RegisterKeyReleaseAction(Keys key, Buttons controllerButton, Action action)
        {
            keyReleaseActions.Add(key, action);
            buttonReleaseActions.Add(controllerButton, action);
        }

        public void RegisterKeyReleaseAction(ICollection<Keys> keys, Action action)
        {
            foreach (Keys key in keys)
            {
                keyReleaseActions.Add(key, action);
            }
        }

        public void RegisterKeyReleaseAction(Keys key, Action action)
        {
            keyReleaseActions.Add(key, action);
        }

        public void RegisterKeyPressAction(Buttons controllerButton, Action<Vector2> action, bool singlePressOnly = false, int pressCooldown = 0)
        {
            keyPressActions.Add(new KeyMapping(null, controllerButton, singlePressOnly, pressCooldown), action);
            pressedButtons[controllerButton] = false;
        }

        public void RegisterKeyPressAction(Keys key, Action<Vector2> action, bool singlePressOnly = false, int pressCooldown = 0) {
            keyPressActions.Add(new KeyMapping(key, null, singlePressOnly), action);
            pressedKeys[key] = false;
        }

        public void RegisterKeyPressAction(ICollection<Keys> keys, Action<Vector2> action, bool singlePressOnly = false, int pressCooldown = 0)
        {
            foreach (Keys key in keys)
            {
                keyPressActions.Add(new KeyMapping(key, null, singlePressOnly), action);
                pressedKeys[key] = false;
            }
        }

        public void RegisterMouseActions(Action wheelUpAction, Action wheelDownAction, float scrollThreshold = 0)
        {
            mouseWheelUpAction = wheelUpAction;
            mouseWheelDownAction = wheelDownAction;
            this.scrollThreshold = scrollThreshold;
        }

        public bool IsKeyPressed(Keys key)
        {
            return pressedKeys[key];
        }

        public void Update()
        {
            if (ControlsDisabled)
            {
                foreach (Keys key in pressedKeys.Keys.ToList())
                {
                    pressedKeys[key] = false;
                }

                foreach (Buttons button in pressedButtons.Keys.ToList())
                {
                    pressedButtons[button] = false;
                }

                prevGamepadState = null;
                prevKeyboardState = null;
                return;
            }

            currentKeyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();
            currentGamepadState = GamePad.GetState(PlayerIndex.One);

            foreach (KeyValuePair<KeyMapping, Action<Vector2>> mapping in keyPressActions)
            {
                Keys? key = mapping.Key.Key;
                if (key.HasValue)
                {
                    if (currentKeyboardState.IsKeyDown(key.Value))
                    {
                        if (Timer.IsSet("INPUTPRESSED_" + key.Value.ToString())) {
                            continue;
                        }
                        if (mapping.Key.PressCooldown != 0)
                        {
                            Timer.SetTimer("INPUTPRESSED_" + key.Value.ToString(), mapping.Key.PressCooldown);
                        }
                        if (mapping.Key.SinglePressOnly && (prevKeyboardState != null && (prevKeyboardState == currentKeyboardState || pressedKeys[key.Value])))
                        {
                            continue;
                        }
                        pressedKeys[key.Value] = true;
                        mapping.Value.Invoke(Vector2.Zero);
                    }
                    else
                    {
                        if (pressedKeys[key.Value] && keyReleaseActions.ContainsKey(key.Value))
                        {
                            keyReleaseActions[key.Value].Invoke();
                        }
                        pressedKeys[key.Value] = false;
                    }
                }

                Buttons? button = mapping.Key.Button;
                if (button.HasValue)
                {
                    if (currentGamepadState.IsButtonDown(button.Value))
                    {
                        if (mapping.Key.SinglePressOnly && (prevGamepadState != null && (prevGamepadState == currentGamepadState || pressedButtons[button.Value])))
                        {
                            continue;
                        }
                        if (Timer.IsSet("INPUTPRESSED_" + button.Value.ToString()))
                        {
                            continue;
                        }
                        if (mapping.Key.PressCooldown != 0)
                        {
                            Timer.SetTimer("INPUTPRESSED_" + button.Value.ToString(), mapping.Key.PressCooldown);
                        }
                        pressedButtons[button.Value] = true;
                        if (button.Value == Buttons.LeftThumbstickLeft || button.Value == Buttons.LeftThumbstickRight)
                        {
                            leftThumbstick.X = currentGamepadState.ThumbSticks.Left.X;
                            mapping.Value.Invoke(leftThumbstick);
                        } else if (button.Value == Buttons.LeftThumbstickUp || button.Value == Buttons.LeftThumbstickDown)
                        {
                            leftThumbstick.Y = currentGamepadState.ThumbSticks.Left.Y;
                            mapping.Value.Invoke(leftThumbstick);
                        }
                        else if (button.Value == Buttons.RightThumbstickLeft || button.Value == Buttons.RightThumbstickRight)
                        {
                            rightThumbStick.X = currentGamepadState.ThumbSticks.Right.X;
                            mapping.Value.Invoke(rightThumbStick);
                        }
                        else if (button.Value == Buttons.RightThumbstickUp || button.Value == Buttons.RightThumbstickDown)
                        {
                            rightThumbStick.Y = currentGamepadState.ThumbSticks.Right.Y;
                            mapping.Value.Invoke(rightThumbStick);
                        }
                        else
                        {
                            mapping.Value.Invoke(Vector2.Zero);
                        }


                    }
                    else
                    {
                        if (pressedButtons[button.Value] && buttonReleaseActions.ContainsKey(button.Value))
                        {
                            buttonReleaseActions[button.Value].Invoke();
                        }
                        pressedButtons[button.Value] = false;
                    }
                }
                
            }

            if (mouseState.ScrollWheelValue > prevMouseScrollWheelValue)
            {
                if (mouseWheelUpAction != null && (mouseState.ScrollWheelValue - prevMouseScrollWheelValue) >= scrollThreshold)
                {
                    mouseWheelUpAction.Invoke();
                    prevMouseScrollWheelValue = mouseState.ScrollWheelValue;
                }
            } 
            else if (mouseState.ScrollWheelValue < prevMouseScrollWheelValue)
            {
                if (mouseWheelDownAction != null && (prevMouseScrollWheelValue - mouseState.ScrollWheelValue) >= scrollThreshold)
                {
                    mouseWheelDownAction.Invoke();
                    prevMouseScrollWheelValue = mouseState.ScrollWheelValue;
                }
            }

            if (prevMouseState?.LeftButton != ButtonState.Pressed && mouseState.LeftButton == ButtonState.Pressed)
            {
                LeftClickDownAction?.Invoke(mouseState.Position.ToVector2());
            }
            else if (prevMouseState?.LeftButton == ButtonState.Pressed && mouseState.LeftButton != ButtonState.Pressed)
            {
                LeftClickUpAction?.Invoke(mouseState.Position.ToVector2());
            }

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                LeftClickPressedAction?.Invoke(mouseState.Position.ToVector2());
            }

            if (prevMouseState?.RightButton != ButtonState.Pressed && mouseState.RightButton == ButtonState.Pressed)
            {
                RightClickDownAction?.Invoke(mouseState.Position.ToVector2());
            }
            else if (prevMouseState?.RightButton == ButtonState.Pressed && mouseState.RightButton != ButtonState.Pressed)
            {
                RightClickUpAction?.Invoke(mouseState.Position.ToVector2());
            }

            if (mouseState.RightButton == ButtonState.Pressed)
            {
                RightClickPressedAction?.Invoke(mouseState.Position.ToVector2());
            }

            if (prevMouseState != null && prevMouseState?.Position != mouseState.Position)
            {
                MouseMovedAction?.Invoke(prevMouseState.Value.Position.ToVector2(), mouseState.Position.ToVector2());
            }



            prevKeyboardState = currentKeyboardState;
            prevGamepadState = currentGamepadState;
            prevMouseState = mouseState;
        }

        private class KeyMapping
        {
            public Keys? Key;
            public Buttons? Button;
            public bool SinglePressOnly;
            public int PressCooldown;

            public KeyMapping(Keys? key, Buttons? button, bool singlePressOnly = false, int pressCooldown = 0)
            {
                Key = key;
                this.Button = button;
                SinglePressOnly = singlePressOnly;
                PressCooldown = pressCooldown;
            }

            public override bool Equals(object obj)
            {
                return obj is KeyMapping mapping &&
                       Key == mapping.Key &&
                       Button == mapping.Button &&
                       SinglePressOnly == mapping.SinglePressOnly &&
                       PressCooldown == mapping.PressCooldown;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(Key, Button, SinglePressOnly, PressCooldown);
            }
        }
    }
}
