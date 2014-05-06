using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Navier_Boats.Engine.System
{
    public class InputStateHelper
    {
        private KeyboardState keyState;
        private KeyboardState prevKeyState;
        private MouseState mouseState;
        private MouseState prevMouseState;
        private GamePadState gamePadState;
        private GamePadState prevGamePadState;

        public KeyboardState KeyState { get { return keyState; } }
        public KeyboardState PrevKeyState { get { return prevKeyState; } }
        public MouseState MouseState { get { return mouseState; } }
        public MouseState PrevMouseState { get { return prevMouseState; } }
        public GamePadState GamePadState { get { return gamePadState; } }
        public GamePadState PrevGamePadState { get { return prevGamePadState; } }

        public InputStateHelper()
        {
            keyState = Keyboard.GetState();
            mouseState = Mouse.GetState();
            gamePadState = GamePad.GetState(Microsoft.Xna.Framework.PlayerIndex.One);
        }

        public void Update()
        {
            prevKeyState = keyState;
            prevMouseState = mouseState;
            prevGamePadState = gamePadState;

            keyState = Keyboard.GetState();
            mouseState = Mouse.GetState();
            gamePadState = GamePad.GetState(Microsoft.Xna.Framework.PlayerIndex.One);
        }
    }
}
