using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Common.Input;
using WaveEngine.Common.Math;
using WaveEngine.Common.Physics2D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics2D;
using WaveEngine.Framework.Services;

namespace VehiclePhysics
{
    /// <summary>
    /// Controller class of the car
    /// </summary>
    [DataContract]
    public class CarController : Behavior
    {
        [RequiredComponent]
        private Transform2D transform;

        [RequiredComponent]
        private RigidBody2D rigidBody;

        [RequiredComponent]
        protected WheelJoint2D[] wheels;

        [DataMember]
        public float MaxSpeed { get; set; }

        [DataMember]
        public float MinSpeed { get; set; }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void Update(TimeSpan gameTime)
        {
            var keyboard = WaveServices.Input.KeyboardState;

            bool enabled;
            float speed;

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
            else
            {

                enabled = false;
                speed = 0;
            }

            foreach (var wheel in this.wheels)
            {
                wheel.EnableMotor = enabled;
                wheel.MotorSpeed = speed;
            }
        }
    }
}
