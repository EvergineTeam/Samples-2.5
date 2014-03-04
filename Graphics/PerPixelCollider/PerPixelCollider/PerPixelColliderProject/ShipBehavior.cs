using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;

namespace PerPixelColliderProject
{
    public class ShipBehavior : Behavior
    {
        private const float GRAVITY = 1000f;
        private const float JETPOWER = -2500f;
        private float speed;
        private float topMargin;

        public ShipBehavior()
            : base("ShipBehaviour")
        {
            this.speed = 0;
            Vector2 topLeftCorner = Vector2.Zero;
            WaveServices.ViewportManager.RecoverPosition(ref topLeftCorner);
            this.topMargin = topLeftCorner.Y;
        }

        public void Reset()
        {
            this.speed = 0;
        }

        protected override void Update(TimeSpan gameTime)
        {
            var burst = this.Owner.ChildEntities.First();

            // Add the jet power if user presses the screen.
            if (WaveServices.Input.TouchPanelState.Count() > 0)
            {
                this.speed += JETPOWER * (float)gameTime.TotalSeconds;
                burst.Enabled = true;
            }
            else
            {
                burst.Enabled = false;
            }

            // Adds the gravity
            this.speed += GRAVITY * (float)gameTime.TotalSeconds;

            // Adds the speed to the owner entity
            var transform = this.Owner.FindComponent<Transform2D>();
            transform.Rotation = this.speed * 0.0005f;
            transform.Y += (float)(this.speed * gameTime.TotalSeconds);
            if (transform.Y < this.topMargin)
            {
                transform.Y = this.topMargin;
                this.speed = 0;
            }
        }
    }
}
