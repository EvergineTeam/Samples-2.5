using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;

namespace BillboardProject
{
    public class FlareBehavior : Behavior
    {
        [RequiredComponent]
        private Transform3D transform = null;

        private float speed;
        private float initY;
        private double angle;

        public FlareBehavior(float angle, float speed)
        {
            this.angle = angle;
            this.speed = speed;
        }

        protected override void Initialize()
        {
            base.Initialize();

            initY = transform.LocalPosition.Y;
        }

        protected override void Update(TimeSpan gameTime)
        {
            this.angle += gameTime.TotalSeconds * speed;

            this.transform.Position = new Vector3(
                (float)Math.Sin(angle),
                this.initY,
                (float)Math.Cos(angle));
        }
    }
}
