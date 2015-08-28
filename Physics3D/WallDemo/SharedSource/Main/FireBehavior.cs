#region Using Statements

using System;
using System.Runtime.Serialization;
using WaveEngine.Common.Math;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics3D;
using WaveEngine.Framework.Services;

#endregion

namespace WallDemo
{
    [DataContract]
    public class FireBehavior : Behavior
    {
        private bool pressed;
        private Entity sphere;

        [RequiredComponent]
        public Camera3D Camera;

        public FireBehavior()
            : base("FireBehavior")
        {
            Camera = null;
        }

        protected override void Update(TimeSpan gameTime)
        {
            var touches = WaveServices.Input.TouchPanelState;

            if (touches.Count > 0)
            {
                if (!pressed)
                {
                    pressed = true;

                    if (sphere == null)
                    {
                        sphere = this.EntityManager.Find("ball");
                    }

                    var rigidBody = sphere.FindComponent<RigidBody3D>();
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
