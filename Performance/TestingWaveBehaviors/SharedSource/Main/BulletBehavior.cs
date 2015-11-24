using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;

namespace TestingWaveBehaviors
{
    [DataContract]
    public class BulletBehavior : Behavior
    {
        public float SpeedX;
        public float SpeedY;

        [RequiredComponent]
        private Transform2D transform;

        public BulletBehavior()
            : base()
        {
            this.SpeedY = 15;
        }

        protected override void Update(TimeSpan gameTime)
        {
            float time = (float)gameTime.TotalMilliseconds;

            transform.Y -= SpeedY;
            transform.X += SpeedX;

            int limit = 10;
            if (transform.Y < -limit ||
                transform.X < -limit ||
                transform.Y > WaveServices.ViewportManager.VirtualHeight + limit ||
                transform.X > WaveServices.ViewportManager.VirtualWidth + limit)
            {
                EntityManager.Remove(Owner);
            }
        }
    }
}
