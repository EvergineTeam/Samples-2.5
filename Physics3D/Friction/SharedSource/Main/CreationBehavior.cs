using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Physics3D;

namespace Friction
{
    [DataContract]
    public class CreationBehavior : Behavior
    {
        private bool init;
        private Vector3 impulse;

        protected override void DefaultValues()
        {
            base.DefaultValues();

            this.impulse = new Vector3(-5, 0, 0);
        }

        protected override void Update(TimeSpan gameTime)
        {
            if (!this.init)
            {
                var entities = this.EntityManager.FindAllByTag("PhysicEntity");
                foreach (var entity in entities)
                {
                    Entity e = entity as Entity;
                    if (e != null)
                    {
                        var rigidBody3D = e.FindComponent<RigidBody3D>();
                        rigidBody3D.ApplyLinearImpulse(this.impulse);
                    }
                }

                this.init = true;
            }
        }
    }
}
