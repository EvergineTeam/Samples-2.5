using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
using WaveEngine.Framework.UI;

namespace ParticleSystem2DProject
{
    public class MeteorBehavior : Behavior
    {
        private static readonly Thickness MARGIN = new Thickness(200, -40, 200, 40);
        private const float RANDOMX = 0.2f;
        private const float FALLSPEED = 0.3f;
        private Vector2 velocity = new Vector2(0.3f, FALLSPEED);

        [RequiredComponent]
        public Transform2D transform;

        private ExplosionBehavior explosionBehavior;
        
        public MeteorBehavior(ExplosionBehavior explosionBehavior)
            : base("meteorBehavior")
        {
            this.explosionBehavior = explosionBehavior;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void Update(TimeSpan gameTime)
        {
            if (WaveServices.Input.TouchPanelState.Count > 0)
            {
                var touch = WaveServices.Input.TouchPanelState.First();
                this.transform.X = touch.Position.X;
                this.transform.Y = touch.Position.Y;
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
