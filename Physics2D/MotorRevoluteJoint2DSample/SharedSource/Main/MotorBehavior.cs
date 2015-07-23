using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Common.Input;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Physics2D;
using WaveEngine.Framework.Services;

namespace MotorRevoluteJoint2DSample
{
    /// <summary>
    /// Motor Behavior Class
    /// </summary>
    [DataContract]
    public class MotorBehavior : Behavior
    {
        private Vector2 initialPosition = new Vector2(90, 100);

        // Input variables
        private Input input;
        private KeyboardState keyboardState;

        // Motor Speed Increase
        private float motorSpeed;

        private float maxSpeed;

        // Motor Revolute Joint
        [RequiredComponent()]
        private JointMap2D jointMap;

        private RevoluteJoint2D revoluteJoint;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        public MotorBehavior()
            : base()
        {
            this.revoluteJoint = null;
        }

        protected override void DefaultValues()
        {
            base.DefaultValues();

            this.motorSpeed = 0.7f;
            this.maxSpeed = 12.0f;
        }

        protected override void ResolveDependencies()
        {
            base.ResolveDependencies();

            this.revoluteJoint = jointMap.Joints["joint"] as RevoluteJoint2D;
        }

        /// <summary>
        /// Update Method
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Update(TimeSpan gameTime)
        {
            this.input = WaveServices.Input;

            // if (this.input.KeyboardState.IsConnected)
            {
                this.keyboardState = this.input.KeyboardState;

                if (revoluteJoint != null)
                {
                    // A, D, S Keyboard Control (left, right, stop motor)
                    if (this.keyboardState.A == ButtonState.Pressed)
                    {
                        if (revoluteJoint.MotorSpeed + motorSpeed <= maxSpeed)
                        {
                            revoluteJoint.MotorSpeed += motorSpeed;
                        }
                    }
                    else if (this.keyboardState.D == ButtonState.Pressed)
                    {
                        if (revoluteJoint.MotorSpeed - motorSpeed >= -maxSpeed)
                        {
                            revoluteJoint.MotorSpeed -= motorSpeed;
                        }
                    }
                    else if (this.keyboardState.S == ButtonState.Pressed)
                    {
                        revoluteJoint.MotorSpeed = 0.0f;
                    }
                }
            }
        }
    }
}
