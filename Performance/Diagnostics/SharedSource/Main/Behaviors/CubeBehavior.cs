using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Diagnostic;
using WaveEngine.Framework.Graphics;

namespace Diagnostics.Behaviors
{
    [DataContract]
    public class CubeBehavior : Behavior
    {
        [RequiredComponent]
        public Transform3D Transform;

        [DataMember]
        public float AngleStep { get; set; }

        [DataMember]
        public float Speed { get; set; }

        private float currentAngle;

        public CubeBehavior() { }

        protected override void Update(TimeSpan gameTime)
        {
            Timers.BeginTimer("Cube Behavior Update");
            var timeLapse = (gameTime.Milliseconds / 1000f);
            currentAngle = currentAngle + (AngleStep * Speed * timeLapse);

            Vector3 aux = this.Transform.Position;
            if (Math.Abs(Transform.Position.X) > 0.001)
            {
                aux.X = Transform.Position.X + ((float)Math.Cos(currentAngle) / 10);
            }
            if (Math.Abs(Transform.Position.Y) > 0.001)
            {
                aux.Y = Transform.Position.Y + ((float)Math.Sin(currentAngle) / 10);
            }
            if (Math.Abs(Transform.Position.Z) > 0.001)
            {
                aux.Z = Transform.Position.Z + ((float)Math.Cos(currentAngle) / 10);
            }
            this.Transform.Position = aux;

            Timers.EndTimer("Cube Behavior Update");
            Labels.Add("First cube position", this.Transform.Position.ToString());
        }
    }
}
