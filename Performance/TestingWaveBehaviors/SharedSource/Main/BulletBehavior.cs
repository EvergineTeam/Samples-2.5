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
        public float screenWidth;
        public float screenHeight;

        [RequiredComponent]
        private Transform2D transform = null;

        public BulletBehavior()
            : base()
        {
            this.SpeedY = 15;
        }

        protected override void ResolveDependencies()
        {
            base.ResolveDependencies();


            var viewportManager = this.Owner.Scene.VirtualScreenManager;
            this.screenWidth = viewportManager.ScreenWidth;
            this.screenHeight = viewportManager.ScreenHeight;
        }

        protected override void Update(TimeSpan gameTime)
        {
            float time = (float)gameTime.TotalMilliseconds;

            transform.Y -= SpeedY;
            transform.X += SpeedX;

            int limit = 10;
            if (transform.Y < -limit ||
                transform.X < -limit ||
                transform.Y > this.screenHeight + limit ||
                transform.X > this.screenWidth + limit)
            {
                EntityManager.Remove(Owner);
            }
        }
    }
}
