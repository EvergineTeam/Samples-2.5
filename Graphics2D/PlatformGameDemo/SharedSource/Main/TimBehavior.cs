using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Input;
using WaveEngine.Components.Animation;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;

namespace PlatformGameDemo
{
    [DataContract]
    class TimBehavior : Behavior
    {
        private const int SPEED = 5;
        private const int RIGHT = 1;
        private const int LEFT = -1;
        private const int NONE = 0;
        private const int BORDER_OFFSET = 25;

        [RequiredComponent]
        public Animation2D anim2D;
        [RequiredComponent]
        public Transform2D trans2D;

        /// <summary>
        /// 1 or -1 indicating right or left respectively
        /// </summary>
        private int direction;
        private AnimState currentState, lastState;
        private enum AnimState { Idle, Right, Left };

        public TimBehavior()
            : base("TimBehavior")
        {
            this.direction = NONE;
            this.anim2D = null;
            this.trans2D = null;
            this.currentState = AnimState.Idle;
        }

        protected override void Update(TimeSpan gameTime)
        {
            currentState = AnimState.Idle;

            // touch panel
            var touches = WaveServices.Input.TouchPanelState;
            if (touches.Count > 0)
            {
                var firstTouch = touches[0];
                if (firstTouch.Position.X > WaveServices.Platform.ScreenWidth / 2)
                {
                    currentState = AnimState.Right;
                }
                else
                {
                    currentState = AnimState.Left;
                }
            }

            // Keyboard
            var keyboard = WaveServices.Input.KeyboardState;
            if (keyboard.Right == ButtonState.Pressed)
            {
                currentState = AnimState.Right;
            }
            else if (keyboard.Left == ButtonState.Pressed)
            {
                currentState = AnimState.Left;
            }

            // Set current animation if that one is diferent
            if (currentState != lastState)
            {
                switch (currentState)
                {
                    case AnimState.Idle:
                        anim2D.PlayAnimation("Idle", true);
                        direction = NONE;
                        break;
                    case AnimState.Right:
                        trans2D.Effect = SpriteEffects.None;
                        anim2D.PlayAnimation("Running", true);
                        direction = RIGHT;
                        break;
                    case AnimState.Left:
                        trans2D.Effect = SpriteEffects.FlipHorizontally;
                        anim2D.PlayAnimation("Running", true);
                        direction = LEFT;
                        break;
                }
            }

            lastState = currentState;

            // Move sprite
            trans2D.X += direction * SPEED * (gameTime.Milliseconds / 10);

            var screenMidLenght = WaveServices.Platform.ScreenWidth / 2;
            var left = -screenMidLenght + BORDER_OFFSET;
            var right = screenMidLenght - BORDER_OFFSET;
            
            // Check borders
            if (trans2D.X < left)
            {
                trans2D.X = left;
            }
            else if (trans2D.X > right)
            {
                trans2D.X = right;
            }
        }
    }
}

