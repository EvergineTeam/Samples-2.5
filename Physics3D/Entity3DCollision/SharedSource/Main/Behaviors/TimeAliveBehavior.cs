using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Framework;
using WaveEngine.Framework.Physics3D;

namespace Entity3DCollision
{
    [DataContract]
    public class TimeAliveBehavior : Behavior
    {
        private TimeSpan aliveTime;
        private TimeSpan remainingTime;

        protected override void DefaultValues()
        {
            base.DefaultValues();

            this.aliveTime = TimeSpan.FromSeconds(10);
            this.remainingTime = this.aliveTime;
        }

        protected override void Update(TimeSpan gameTime)
        {
            this.remainingTime -= gameTime;

            if (this.remainingTime <= TimeSpan.Zero)
            {
                this.EntityManager.Remove(this.Owner);
            }
        }
    }
}
