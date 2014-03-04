#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Input;
using WaveEngine.Common.Math;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
#endregion

namespace XboxControllerProject
{
    public class GamePadSceneBehavior : SceneBehavior
    {
        /// <summary>
        /// The before gamepad state
        /// </summary>
        private GamePadState beforeGamepadState;

        /// <summary>
        /// Resolves the dependencies needed for this instance to work.
        /// </summary>
        protected override void ResolveDependencies()
        {
        }

        /// <summary>
        /// Allows this instance to execute custom logic during its <c>Update</c>.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        /// <remarks>
        /// This method will not be executed if it are not <c>Active</c>.
        /// </remarks>
        protected override void Update(TimeSpan gameTime)
        {
            MyScene myscene = this.Scene as MyScene;

            // Set Default
            myscene.buttonA.IsVisible = false;
            myscene.buttonB.IsVisible = false;
            myscene.buttonX.IsVisible = false;
            myscene.buttonY.IsVisible = false;
            myscene.buttonBack.IsVisible = false;
            myscene.buttonStart.IsVisible = false;
            myscene.dpadUp.IsVisible = false;
            myscene.dpadDown.IsVisible = false;
            myscene.dpadLeft.IsVisible = false;
            myscene.dpadRight.IsVisible = false;
            myscene.rightJoystick.FindComponent<SpriteAtlas>().TextureName = "JoyStick";
            myscene.leftJoystick.FindComponent<SpriteAtlas>().TextureName = "JoyStick";
            myscene.leftShoulder.IsVisible = false;
            myscene.rightShoulder.IsVisible = false;
            myscene.leftTrigger.IsVisible = false;
            myscene.rightTrigger.IsVisible = false;

            var inputService = WaveServices.Input;
            if (inputService.GamePadState.IsConnected)
            {
                // A
                if (inputService.GamePadState.Buttons.A == ButtonState.Pressed)
                {
                    myscene.buttonA.IsVisible = true;
                }

                // B
                if (inputService.GamePadState.Buttons.B == ButtonState.Pressed)
                {
                    myscene.buttonB.IsVisible = true;
                }

                // X
                if (inputService.GamePadState.Buttons.X == ButtonState.Pressed)
                {
                    myscene.buttonX.IsVisible = true;
                }

                // Y
                if (inputService.GamePadState.Buttons.Y == ButtonState.Pressed)
                {
                    myscene.buttonY.IsVisible = true;
                }

                // RightShoulder
                if (inputService.GamePadState.Buttons.RightShoulder == ButtonState.Pressed)
                {
                    myscene.rightShoulder.IsVisible = true;
                }

                // LeftShoulder
                if (inputService.GamePadState.Buttons.LeftShoulder == ButtonState.Pressed)
                {
                    myscene.leftShoulder.IsVisible = true;
                }

                // RightStick
                if (inputService.GamePadState.Buttons.RightStick == ButtonState.Pressed)
                {
                    myscene.rightJoystick.FindComponent<SpriteAtlas>().TextureName = "JoyStickPressed";
                }

                // LeftStick
                if (inputService.GamePadState.Buttons.LeftStick == ButtonState.Pressed)
                {
                    myscene.leftJoystick.FindComponent<SpriteAtlas>().TextureName = "JoyStickPressed";
                }

                // Start
                if (inputService.GamePadState.Buttons.Start == ButtonState.Pressed)
                {
                    myscene.buttonStart.IsVisible = true;
                }

                // Back
                if (inputService.GamePadState.Buttons.Back == ButtonState.Pressed)
                {
                    myscene.buttonBack.IsVisible = true;
                }

                // DPad Right
                if (inputService.GamePadState.DPad.Right == ButtonState.Pressed)
                {
                    myscene.dpadRight.IsVisible = true;
                }

                // DPad Left
                if (inputService.GamePadState.DPad.Left == ButtonState.Pressed)
                {
                    myscene.dpadLeft.IsVisible = true;
                }

                // DPad Up
                if (inputService.GamePadState.DPad.Up == ButtonState.Pressed)
                {
                    myscene.dpadUp.IsVisible = true;
                }

                // DPad Down
                if (inputService.GamePadState.DPad.Down == ButtonState.Pressed)
                {
                    myscene.dpadDown.IsVisible = true;
                }

                // Triggers
                if (inputService.GamePadState.Triggers.Right > 0)
                {
                    myscene.rightTrigger.IsVisible = true;
                    myscene.rightTriggerText.Text = "RightTrigger: " + inputService.GamePadState.Triggers.Right;
                }
                else
                {
                    myscene.rightTriggerText.Text = "RightTrigger: 0";
                }

                if (inputService.GamePadState.Triggers.Left > 0)
                {
                    myscene.leftTrigger.IsVisible = true;
                    myscene.leftTriggerText.Text = "LeftTrigger: " + inputService.GamePadState.Triggers.Left;
                }
                else
                {
                    myscene.leftTriggerText.Text = "LeftTrigger: 0";
                }

                // Sticks                              
                myscene.leftJoystick.FindComponent<Transform2D>().X = inputService.GamePadState.ThumbStricks.Left.X * 10;
                myscene.leftJoystick.FindComponent<Transform2D>().Y = -inputService.GamePadState.ThumbStricks.Left.Y * 10;
                myscene.leftStickText.Text = "LeftStick: " + inputService.GamePadState.ThumbStricks.Left;

                myscene.rightJoystick.FindComponent<Transform2D>().X = inputService.GamePadState.ThumbStricks.Right.X * 10;
                myscene.rightJoystick.FindComponent<Transform2D>().Y = -inputService.GamePadState.ThumbStricks.Right.Y * 10;
                myscene.rightStickText.Text = "RightStick: " + inputService.GamePadState.ThumbStricks.Right;


                beforeGamepadState = inputService.GamePadState;
            }
        }
    }
}
