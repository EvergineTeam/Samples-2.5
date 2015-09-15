using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics3D;
using WaveEngine.Framework.Services;
using WaveEngine.Materials;

namespace Jenga
{
    [DataContract]
    public class ShootBehavior : Behavior
    {
        [RequiredComponent]
        public Camera3D Camera;

        private bool pressed;

        protected override void Update(TimeSpan gameTime)
        {
            var touches = WaveServices.Input.TouchPanelState;

            if (touches.Count > 0)
            {
                if (!pressed)
                {
                    pressed = true;

                    var sphere = new Entity()
                          .AddComponent(new Transform3D() { Scale = new Vector3(1) })
                          .AddComponent(new MaterialsMap(new StandardMaterial(Color.Red, DefaultLayers.Opaque)))
                          .AddComponent(Model.CreateSphere())
                          .AddComponent(new SphereCollider3D())
                          .AddComponent(new RigidBody3D() { Mass = 2, EnableContinuousContact = true })
                          .AddComponent(new TimeAliveBehavior())
                          .AddComponent(new ModelRenderer());

                    this.EntityManager.Add(sphere);

                    RigidBody3D rigidBody = sphere.FindComponent<RigidBody3D>();
                    rigidBody.ResetPosition(Camera.Position);
                    var direction = Camera.Transform.WorldTransform.Forward;
                    direction.Normalize();
                    rigidBody.ApplyLinearImpulse(100 * direction);
                }
            }
            else
            {
                pressed = false;
            }
        }
    }
}
