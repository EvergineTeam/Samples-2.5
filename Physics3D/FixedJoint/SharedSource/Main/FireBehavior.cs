using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Common.Attributes;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics3D;
using WaveEngine.Framework.Services;
using WaveEngine.Materials;

namespace FixedJoint
{
    [DataContract(Namespace = "FixedJoint")]
    public class FireBehavior : Behavior
    {
        private bool pressed;

        private Entity linkedEntity;

        private string entityPath;

        [DataMember]
        [RenderPropertyAsEntity]
        public string EntityPath
        {
            get
            {
                return entityPath;
            }
            set
            {
                entityPath = value;
                RefreshEntity();
            }
        }

        [RequiredComponent]
        public Camera3D Camera;

        public FireBehavior()
            : base("FireBehavior")
        {
            Camera = null;
        }

        protected override void Initialize()
        {
            this.isInitialized = true;
            RefreshEntity();
        }

        private void RefreshEntity()
        {
            if (this.isInitialized)
            {
                if (this.EntityPath != null)
                {
                    linkedEntity = EntityManager.Find(this.EntityPath);
                }
            }
        }

        protected override void Update(TimeSpan gameTime)
        {
            var touches = WaveServices.Input.TouchPanelState;

            if (touches.Count > 0)
            {
                if (!pressed)
                {
                    pressed = true;
                    RigidBody3D rigidBody = linkedEntity.FindComponent<RigidBody3D>();
                    rigidBody.ResetPosition(Camera.Position);

                    var direction = Camera.Transform.WorldTransform.Forward;
                    direction.Normalize();
                    rigidBody.ApplyLinearImpulse(60 * direction);
                }
            }
            else
            {
                pressed = false;
            }
        }
    }
}
