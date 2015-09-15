#region Using Statements
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using WaveEngine.Common.Input;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Physics2D;
using WaveEngine.Framework.Services;
#endregion

namespace Force2DSample
{
    [DataContract]
    public class ForceBehavior : Behavior
    {
        private Input input;
        private KeyboardState keyboardState;
        private float multiplicator;
        private float angularImpulse;
        private float torque;
        private Vector2 topDirection;
        private Vector2 leftDirection;

        public ForceBehavior()
            : base()
        {
        }

        protected override void DefaultValues()
        {
            base.DefaultValues();

            this.multiplicator = 2f;
            this.angularImpulse = 0.1f;
            this.torque = 1f;
        }

        protected override void Update(TimeSpan gameTime)
        {
            this.input = WaveServices.Input;
            if (this.input.KeyboardState.IsConnected)
            {
                this.keyboardState = this.input.KeyboardState;

                RigidBody2D rigidBody = Owner.FindComponent<RigidBody2D>();
                if (rigidBody != null)
                {
                    // W, A, S, D applies Directional Linear Impulses
                    Vector2 result = Vector2.Zero;
                    if (this.keyboardState.A == ButtonState.Pressed)
                    {
                        result += this.leftDirection;
                    }
                    if (this.keyboardState.D == ButtonState.Pressed)
                    {
                        result -= this.leftDirection;
                    }
                    if (this.keyboardState.W == ButtonState.Pressed)
                    {
                        result += this.topDirection;
                    }
                    if (this.keyboardState.S == ButtonState.Pressed)
                    {
                        result -= this.topDirection;
                    }

                    // Apply Linear Impulse
                    if (result != Vector2.Zero)
                    {
                        rigidBody.ApplyLinearImpulse(result);
                    }

                    // Left and Right arrow applies angular impulse
                    if (this.keyboardState.Left == ButtonState.Pressed)
                    {
                        rigidBody.ApplyAngularImpulse(-this.angularImpulse);
                    }
                    else if (this.keyboardState.Right == ButtonState.Pressed)
                    {
                        rigidBody.ApplyAngularImpulse(this.angularImpulse);
                    }

                    // J and K applies Torque
                    if (this.keyboardState.J == ButtonState.Pressed)
                    {
                        rigidBody.ApplyTorque(this.torque);
                    }
                    else if (keyboardState.K == ButtonState.Pressed)
                    {
                        rigidBody.ApplyTorque(-this.torque);
                    }
                }
            }
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.topDirection = -Vector2.UnitY * this.multiplicator;
            this.leftDirection = -Vector2.UnitX * this.multiplicator;
        }
    }
}
