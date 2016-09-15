using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using System.Text;
using WaveEngine.Common.Input;
using WaveEngine.Common.Math;
using WaveEngine.Common.Physics2D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics2D;
using WaveEngine.Framework.Services;
using WaveEngine.Components.Toolkit;

namespace VehiclePhysics2D
{
    /// <summary>
    /// Controller class of the car
    /// </summary>
    [DataContract]
    public class CarController : Behavior
    {
        [RequiredComponent]
        protected WheelJoint2D[] wheels;

        [DataMember]
        public float MaxSpeed { get; set; }

        [DataMember]
        public float MinSpeed { get; set; }

        protected override void Initialize()
        {
            base.Initialize();

            var text = this.EntityManager.FindAllByTag("InstructionsLabel").FirstOrDefault() as Entity;

            if (text != null)
            {
                var textComponent = text.FindComponent<TextComponent>();

                if (textComponent != null)
                {
                    var keyboard = WaveServices.Input.KeyboardState;
                    var touchs = WaveServices.Input.TouchPanelState;

                    if (keyboard.IsConnected)
                    {
                        textComponent.Text = "Cursor ↑: Accelerate\nCursor ↓: Brake";
                    }
                    else if (touchs.IsConnected)
                    {
                        textComponent.Text = "Touch Right Side: Accelerate\nTouch Left Side: Brake";
                    }
                    else
                    {
                        text.Enabled = false;
                    }
                }
            }
        }

        protected override void Update(TimeSpan gameTime)
        {
            var keyboard = WaveServices.Input.KeyboardState;


            bool enabled = false;
            float speed = 0;

            if (keyboard.IsConnected)
            {
                if (keyboard.Up == WaveEngine.Common.Input.ButtonState.Pressed)
                {
                    enabled = true;
                    speed = MaxSpeed;
                }
                else if (keyboard.Down == WaveEngine.Common.Input.ButtonState.Pressed)
                {
                    enabled = true;
                    speed = MinSpeed;
                }
            }

            var touchs = WaveServices.Input.TouchPanelState;

            if ((touchs.IsConnected) && (touchs.Count > 0))
            {
                enabled = true;

                var touch = touchs[0];
                if (touch.Position.X > WaveServices.Platform.ScreenWidth * 0.5f)
                {
                    speed = MaxSpeed;
                }
                else
                {
                    speed = MinSpeed;
                }
            }

            foreach (var wheel in this.wheels)
            {
                wheel.EnableMotor = enabled;
                wheel.MotorSpeed = speed;
            }
        }
    }
}
