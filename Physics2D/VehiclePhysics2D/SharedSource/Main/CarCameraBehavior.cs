using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Common.Attributes;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;

namespace VehiclePhysics
{
    [DataContract]
    public class CarCameraBehavior : Behavior
    {
        [RequiredComponent]
        protected Transform2D transform;

        [DataMember]
        public float MinX { get; set; }

        [DataMember]
        public float MaxX { get; set; }

        [DataMember]
        [RenderPropertyAsSlider(0, 1, 0.1f)]
        public float Smooth { get; set; }

        [DataMember]
        public Vector2 Offset { get; set; }

        [DataMember]
        [RenderPropertyAsEntity(new string[] { "WaveEngine.Framework.Graphics.Transform2D" })]
        public string TargetPath { get; set; }

        private Transform2D targetTrasnform;

        protected override void Initialize()
        {
            base.Initialize();

            if (!string.IsNullOrEmpty(this.TargetPath))
            {
                this.targetTrasnform = this.EntityManager.Find(this.TargetPath)?.FindComponent<Transform2D>();
            }
        }

        protected override void Update(TimeSpan gameTime)
        {
            if (this.targetTrasnform == null)
            {
                return;
            }

            var position = this.targetTrasnform.Position + this.Offset;

            if (position.X > this.MaxX)
            {
                position.X = this.MaxX;
            }
            else if (position.X < this.MinX)
            {
                position.X = this.MinX;
            }

            var currentPosition = this.transform.Position;

            Vector2.SmoothStep(ref currentPosition, ref position, this.Smooth, out position);

            this.transform.Position = position;
        }
    }
}
