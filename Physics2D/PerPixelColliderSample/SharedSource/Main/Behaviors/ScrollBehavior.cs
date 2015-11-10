using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;

namespace PerPixelColliderSample
{
    [DataContract]
    public class ScrollBehavior : Behavior
    {
        private static int instances = 0;
        private static float scrollSpeed;
        private const float MAXSPEED = 0.5f;
        
        [DataMember]
        private float sceneWidth;

        public float SceneWidth
        {
            get { return sceneWidth; }
            set { sceneWidth = value; }
        }

        [DataMember]
        private bool randomSpawn;

        public bool RandomSpawn
        {
            get { return randomSpawn; }
            set { randomSpawn = value; }
        }

        [DataMember]
        private bool rotate;

        public bool Rotate
        {
            get { return rotate; }
            set { rotate = value; }
        }

        private float rotationSpeed;
        private float leftCorner;
        public static float ScrollSpeed
        {
            get
            {
                return scrollSpeed;
            }

            set
            {
                scrollSpeed = value;
            }
        }

        public ScrollBehavior(): this(3072, false, false)
        { }

        public ScrollBehavior(float sceneWidth, bool randomSpawn, bool rotate)
            : base("ObstacleBehavior_" + instances++)
        {
            this.sceneWidth = sceneWidth;
            this.randomSpawn = randomSpawn;
            this.rotate = rotate;
            this.rotationSpeed = ((float)WaveServices.Random.NextDouble() * 2 * MAXSPEED) - MAXSPEED;

            Vector2 topLeftCorner = Vector2.Zero;
            WaveServices.ViewportManager.RecoverPosition(ref topLeftCorner);
            this.leftCorner = topLeftCorner.X;
        }

        protected override void Update(TimeSpan gameTime)
        {
            // Adds the lateral scroll speed.
            var transform2D = this.Owner.FindComponent<Transform2D>();
            transform2D.X += (float)(ScrollSpeed * gameTime.TotalSeconds);

            // If element dissapears on the left side of screen, it's moved to the right
            if (transform2D.X + (transform2D.Rectangle.Width * transform2D.XScale) < this.leftCorner)
            {
                transform2D.X += sceneWidth;
                if (this.randomSpawn)
                {
                    transform2D.Y = (float)WaveServices.Random.NextDouble() * WaveServices.ViewportManager.VirtualHeight / WaveServices.ViewportManager.RatioY;
                }
            }

            if (this.rotate)
            {
                transform2D.Rotation += (float)gameTime.TotalSeconds * this.rotationSpeed;
            }
        }
    }
}
