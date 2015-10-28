#region File Description
//-----------------------------------------------------------------------------
// FireBehavior
//
// Copyright © 2012 Weekend Game Studio. All rights reserved.
// Use is subject to license terms.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
using WaveEngine.Materials;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework.Physics3D;
using System.Runtime.Serialization;
using WaveEngine.Common.Attributes;

#endregion

namespace VuforiaTest
{
    [DataContract]
    public class FireBehavior : Behavior
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

                    Vector3 position = this.transform.Position;
                    Vector3 direction = this.transform.WorldTransform.Forward;
                    direction.Normalize();

                    this.ballRigidBody.ResetPosition(position);
                    this.ballRigidBody.ApplyLinearImpulse(this.Impulse * direction);
                }
            }
            else
            {
                pressed = false;
            }
        }
    }
}
