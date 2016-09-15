using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Managers;
using WaveEngine.Framework.Services;
using WaveEngine.Framework.UI;

namespace ParticleSystem2DProject
{
    [DataContract]
    public class MeteorBehavior : Behavior
    {
        private Thickness MARGIN;
        private Vector2 velocity;
        private ExplosionBehavior explosionBehavior;
        private const float RANDOMX = 0.2f;
        private const float FALLSPEED = 0.3f;
        private VirtualScreenManager vsm;

        [RequiredComponent]
        public Transform2D transform;

        protected override void DefaultValues()
        {
            base.DefaultValues();

            velocity = new Vector2(0.3f, FALLSPEED);
            MARGIN = new Thickness(200, -40, 200, 40);
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.vsm = this.Owner.Scene.VirtualScreenManager;

            var explosion = this.Owner.Scene.EntityManager.Find("explosion");
            this.explosionBehavior = explosion.FindComponent<ExplosionBehavior>();
        }

        protected override void Update(TimeSpan gameTime)
        {
            if (WaveServices.Input.TouchPanelState.Count > 0)
            {
                var touchPosition = WaveServices.Input.TouchPanelState.First().Position;
                this.vsm.ToVirtualPosition(ref touchPosition);
                this.transform.X = touchPosition.X;
                this.transform.Y = touchPosition.Y;
            }
            else
            {

                if (this.transform.Y > WaveServices.Platform.ScreenHeight - MARGIN.Bottom)
                {
                    this.ResetPosition();
                }

                this.transform.X = this.transform.X + this.velocity.X * (float)gameTime.TotalMilliseconds;
                this.transform.Y = this.transform.Y + this.velocity.Y * (float)gameTime.TotalMilliseconds;
            }
        }

        private void ResetPosition()
        {
            if (this.explosionBehavior != null)
            {
                this.explosionBehavior.Explode(this.transform.X, this.transform.Y);
            }

            this.velocity.X = RANDOMX * (float)(WaveServices.Random.NextDouble() - 0.5);
            this.transform.Y = MARGIN.Top;
            this.transform.X = WaveServices.Random.Next((int)MARGIN.Left, WaveServices.Platform.ScreenWidth - (int)MARGIN.Right);
        }
    }
}
