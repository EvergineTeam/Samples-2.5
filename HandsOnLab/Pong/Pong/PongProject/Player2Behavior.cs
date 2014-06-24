using System;
using WaveEngine.Common.Input;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;

namespace PongProject
{
    class Player2Behavior : Behavior
    {
        private const int SPEED = 5;
        private const int UP = -1;
        private const int DOWN = 1;
        private const int NONE = 0;
        private const int BORDER_OFFSET = 25;

        [RequiredComponent]
        public Transform2D trans2D;

        /// <summary>
        /// 1 or -1 indicating up or down respectively
        /// </summary>
        private int direction;
        private PlayerState currentState, lastState;
        private enum PlayerState { Idle, Up, Down };

        public Player2Behavior()
            : base("Player2Behavior")
        {
            this.direction = NONE;
            this.trans2D = null;
            this.currentState = PlayerState.Idle;
        }

        protected override void Update(TimeSpan gameTime)
        {
            currentState = PlayerState.Idle;

            // Keyboard
            var keyboard = WaveServices.Input.KeyboardState;
            if (keyboard.Up == ButtonState.Pressed)
            {
                currentState = PlayerState.Up;
            }
            else if (keyboard.Down == ButtonState.Pressed)
            {
                currentState = PlayerState.Down;
            }

            // Set current state if that one is diferent
            if (currentState != lastState)
            {
                switch (currentState)
                {
                    case PlayerState.Idle:
                        direction = NONE;
                        break;
                    case PlayerState.Up:
                        direction = UP;
                        break;
                    case PlayerState.Down:
                        direction = DOWN;
                        break;
                }
            }

            lastState = currentState;

            // Move sprite
            trans2D.Y += direction * SPEED * (gameTime.Milliseconds / 10);

            // Check borders
            if (trans2D.Y < BORDER_OFFSET + trans2D.YScale + 80)
            {
                trans2D.Y = BORDER_OFFSET + trans2D.YScale + 80;
            }
            else if (trans2D.Y > WaveServices.Platform.ScreenHeight - BORDER_OFFSET)
            {
                trans2D.Y = WaveServices.Platform.ScreenHeight - BORDER_OFFSET;
            }
        }
    }
}

