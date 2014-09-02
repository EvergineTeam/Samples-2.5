using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;

namespace CloneProject
{
    public class CubeBehavior : Behavior
    {
        [RequiredComponent()]
        public Transform3D Transform;

        private float angleStep;
        private float speed;
        private float currentAngle;
        private float timeLapse;

        public CubeBehavior(string name, float angleStep, float speed)
            : base("CubeBehavior" + name)
        {
            this.angleStep = angleStep;
            this.speed = speed;
        }

        protected override void Update(TimeSpan gameTime)
        {
            timeLapse = (gameTime.Milliseconds / 1000f);
            currentAngle = currentAngle + (angleStep * speed * timeLapse);

            Vector3 aux = this.Transform.Position;
            if (Transform.Position.X != 0f)
            {                
                aux.X = Transform.Position.X + ((float)Math.Cos(currentAngle));
            }

            if (Transform.Position.Y != 0f)
            {
                aux.Y = Transform.Position.Y + ((float)Math.Sin(currentAngle));
            }

            if (Transform.Position.Z != 0f)
            {
                aux.Z = Transform.Position.Z + ((float)Math.Cos(currentAngle));
            }

            this.Transform.Position = aux;
        }
    }
}
