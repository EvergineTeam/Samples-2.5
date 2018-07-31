using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.ImageEffects;

namespace Physics3DSample.Behaviors
{
    /// <summary>
    /// Component that set the focus distance of the BokehLens to focus the owner's transform
    /// </summary>
    [DataContract]
    public class FocusBehavior : Behavior
    {
        [RequiredComponent]
        protected Transform3D transform;

        private BokehLens lens;

        private float velocity;

        [DataMember]
        public float FocusTime;

        protected override void DefaultValues()
        {
            base.DefaultValues();

            this.FocusTime = 0.2f;
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.lens = this.EntityManager.FindFirstComponentOfType<BokehLens>();
        }
        protected override void Update(TimeSpan gameTime)
        {
            var distance = Vector3.Distance(this.RenderManager.ActiveCamera3D.Position, this.transform.Position);

            this.lens.FocalDistance = MathHelper.SmoothDamp(
                this.lens.FocalDistance, 
                distance, 
                ref this.velocity, 
                this.FocusTime, 
                (float)gameTime.TotalSeconds); 
        }
    }
}
