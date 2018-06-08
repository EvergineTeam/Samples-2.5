using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Common.Attributes;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics3D;
using WaveEngine.Framework.Services;

namespace Physics3DSample.Behaviors
{
    /// <summary>
    /// Component that fires a ball
    /// </summary>
    [DataContract]
    public class GunBehavior : Behavior
    {
        private bool pressed;
        private RigidBody3D ballRigidBody;
        private string firedEntityPath;

        [RequiredComponent(false)]
        private Transform3D transform = null;

        [DataMember]
        public float Impulse
        {
            get;
            set;
        }

        [RenderPropertyAsEntity(new string[] { "WaveEngine.Framework.Physics3D.RigidBody3D" })]
        [DataMember]
        public string FiredEntityPath
        {
            get
            {
                return this.firedEntityPath;
            }

            set
            {
                this.firedEntityPath = value;
                if (this.isInitialized)
                {
                    this.RefreshFiredEntity();
                }
            }
        }

        protected override void DefaultValues()
        {
            base.DefaultValues();
            this.Impulse = 100;
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.RefreshFiredEntity();
        }

        private void RefreshFiredEntity()
        {
            if (!string.IsNullOrEmpty(this.firedEntityPath))
            {
                var ball = this.EntityManager.Find(this.firedEntityPath);
                if (ball != null)
                {
                    this.ballRigidBody = ball.FindComponent<RigidBody3D>();
                }
            }
        }

        protected override void Update(TimeSpan gameTime)
        {
            if (this.ballRigidBody == null)
            {
                return;
            }

            var touches = WaveServices.Input.TouchPanelState;

            if (touches.Count > 0)
            {
                if (!pressed)
                {
                    pressed = true;

                    // Launches the ball
                    var position = this.transform.Position;
                    var scale = this.ballRigidBody.Transform3D.Scale;
                    var direction = this.transform.WorldTransform.Forward;
                    direction.Normalize();

                    this.ballRigidBody.InternalBody.SetTransform(position, Quaternion.Identity, scale);
                    
                    this.ballRigidBody.AngularVelocity = Vector3.Zero;
                    this.ballRigidBody.LinearVelocity = Vector3.Zero;
                    this.ballRigidBody.ClearForces();
                    this.ballRigidBody.ApplyImpulse(this.Impulse * direction);
                }
            }
            else
            {
                pressed = false;
            }
        }
    }
}
