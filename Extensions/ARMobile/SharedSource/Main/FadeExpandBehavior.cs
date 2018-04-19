using System;
using System.Runtime.Serialization;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;

namespace ARMobile
{
    [DataContract]
    public class FadeExpandBehavior : Behavior
    {
        [RequiredComponent]
        protected Transform3D transform;

        private Vector3 currentVelocity;

        [DataMember]
        private Vector3 endScale;

        [DataMember]
        private Vector3 startScale;

        [DataMember]
        public TimeSpan AnimationTime { get; set; }

        public Vector3 StartScale { get => startScale; set => startScale = value; }

        public Vector3 EndScale { get => endScale; set => endScale = value; }

        protected override void DefaultValues()
        {
            base.DefaultValues();

            this.endScale = Vector3.One;
            this.AnimationTime = TimeSpan.FromSeconds(1);
        }

        protected override void Update(TimeSpan gameTime)
        {
            var localScale = this.transform.LocalScale;

            localScale = Vector3.SmoothDamp(localScale, this.endScale, ref this.currentVelocity, (float)this.AnimationTime.TotalSeconds, (float)gameTime.TotalSeconds);

            if (Vector3.DistanceSquared(localScale, this.EndScale) < 0.0001f)
            {
                localScale = this.StartScale;
            }

            this.transform.LocalScale = localScale;
        }
    }
}
