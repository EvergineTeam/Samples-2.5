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
        public TimeSpan AnimationTime { get; set; }

        [DataMember]
        public Vector3 StartScale { get; set; }

        [DataMember]
        public Vector3 EndScale { get; set; }

        protected override void DefaultValues()
        {
            base.DefaultValues();

            this.EndScale = Vector3.One;
            this.AnimationTime = TimeSpan.FromSeconds(1);
        }

        protected override void Update(TimeSpan gameTime)
        {
            var localScale = this.transform.LocalScale;

            localScale = Vector3.SmoothDamp(localScale, this.EndScale, ref this.currentVelocity, (float)this.AnimationTime.TotalSeconds, (float)gameTime.TotalSeconds);

            if (Vector3.DistanceSquared(localScale, this.EndScale) < 0.0001f)
            {
                localScale = this.StartScale;
            }

            this.transform.LocalScale = localScale;
        }
    }
}
