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

        private RigidBody3D[] rigidBodyPool;

        private int numBalls;

        private string firedPrefabPath;

        private int maxBalls;

        [RequiredComponent(false)]
        private Transform3D transform = null;

        [DataMember]
        public float Impulse;

        [DataMember]
        public int MaxBalls
        {
            get
            {
                return this.maxBalls;
            }

            set
            {
                this.maxBalls = value;
                if (this.isInitialized)
                {
                    this.RefreshPool();
                }
            }
        }

        [RenderPropertyAsAsset(AssetType.Prefab)]
        [DataMember]
        public string FiredPrefabPath
        {
            get
            {
                return this.firedPrefabPath;
            }

            set
            {
                this.firedPrefabPath = value;
                if (this.isInitialized)
                {
                    this.RefreshPool();
                }
            }
        }

        protected override void DefaultValues()
        {
            base.DefaultValues();
            this.Impulse = 100;
            this.MaxBalls = 5;
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.RefreshPool();
        }

        private void RefreshPool()
        {
            if (!string.IsNullOrEmpty(this.firedPrefabPath))
            {
                if (this.rigidBodyPool != null)
                {
                    foreach (var b in this.rigidBodyPool)
                    {
                        this.EntityManager.Remove(b.Owner);
                    }
                }

                this.rigidBodyPool = new RigidBody3D[this.MaxBalls];

                for (int i = 0; i < this.MaxBalls; i++)
                {
                    var ball = this.EntityManager.Instantiate(this.firedPrefabPath);
                    ball.Name = $"ball{i}";

                    this.rigidBodyPool[i] = ball?.FindComponent<RigidBody3D>();
                }
            }
            else
            {
                this.rigidBodyPool = null;
            }

            this.numBalls = 0;
        }

        protected override void Update(TimeSpan gameTime)
        {
            if (this.rigidBodyPool == null)
            {
                return;
            }

            var touches = WaveServices.Input.TouchPanelState;

            if (touches.Count > 0)
            {
                if (!pressed)
                {
                    pressed = true;

                    this.Launch();
                }
            }
            else
            {
                pressed = false;
            }
        }

        private void Launch()
        {
            var mustBeAdded = this.numBalls < this.MaxBalls;

            var index = this.numBalls++ % this.MaxBalls;

            var ballRigidBody = this.rigidBodyPool[index];

            if (mustBeAdded)
            {
                this.EntityManager.Add(ballRigidBody.Owner);
            }

            var position = this.transform.Position;
            var scale = ballRigidBody.Transform3D.Scale;
            var direction = this.transform.WorldTransform.Forward;
            direction.Normalize();

            ballRigidBody.InternalBody.SetTransform(position, Quaternion.Identity, scale);

            ballRigidBody.AngularVelocity = Vector3.Zero;
            ballRigidBody.LinearVelocity = Vector3.Zero;
            ballRigidBody.ClearForces();
            ballRigidBody.ApplyImpulse(this.Impulse * direction);
        }
    }
}
