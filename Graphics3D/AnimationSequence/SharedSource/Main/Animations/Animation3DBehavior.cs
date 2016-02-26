using System;
using System.Runtime.Serialization;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;

namespace AnimationSequence.Animations
{
    [DataContract]
    public class Animation3DBehavior : Behavior
    {
        public event EventHandler Completed;

        [RequiredComponent]
        private Transform3D transform3D = null;

        private TimeSpan time;
        private AnimationSlot animation;
        private bool completed;

        public AnimationSlot AnimationSlot
        {
            get
            {
                return this.animation;
            }

            private set
            {
                this.animation = value;
                
                if (this.animation.TransformationType.HasFlag(AnimationSlot.TransformationTypes.Position))
                {
                    this.transform3D.LocalPosition = this.animation.StartPosition;
                }
                if (this.animation.TransformationType.HasFlag(AnimationSlot.TransformationTypes.Rotation))
                {
                    this.transform3D.LocalRotation = this.animation.StartRotation;
                }
                if (this.animation.TransformationType.HasFlag(AnimationSlot.TransformationTypes.Scale))
                {
                    this.transform3D.LocalScale = this.animation.StartScale;
                }
            }
        }

        protected override void DefaultValues()
        {
            base.DefaultValues();
            this.completed = true;
        }

        public void BeginAnimation(AnimationSlot animatinoSlot)
        {
            this.AnimationSlot = animatinoSlot;
            this.time = TimeSpan.Zero;
            this.completed = false;
        }

        protected override void Update(TimeSpan gameTime)
        {
            if (completed)
            {
                return;
            }

            this.time += gameTime;
            if (time <= this.animation.TotalTime)
            {
                float amount = (float)(this.time.TotalSeconds / this.animation.TotalTime.TotalSeconds);

                if (this.animation.TransformationType.HasFlag(AnimationSlot.TransformationTypes.Position))
                {
                    Vector3 deltaPosition = Vector3.Lerp(this.animation.StartPosition, this.animation.EndPosition, amount);                    
                    this.transform3D.LocalPosition = deltaPosition;
                }
                if (this.animation.TransformationType.HasFlag(AnimationSlot.TransformationTypes.Rotation))
                {
                    Vector3 deltaRotation = Vector3.Lerp(this.animation.StartRotation, this.animation.EndRotation, amount);                    
                    this.transform3D.LocalRotation = deltaRotation;                    
                }
                if (this.animation.TransformationType.HasFlag(AnimationSlot.TransformationTypes.Scale))
                {
                    Vector3 deltaScale = Vector3.Lerp(this.animation.StartScale, this.animation.EndScale, amount);                    
                    this.transform3D.LocalScale = deltaScale;                    
                }
            }
            else
            {                
                if (this.animation.TransformationType.HasFlag(AnimationSlot.TransformationTypes.Position))
                {
                    this.transform3D.LocalPosition = this.animation.EndPosition;
                }
                if (this.animation.TransformationType.HasFlag(AnimationSlot.TransformationTypes.Rotation))
                {
                    this.transform3D.LocalRotation = this.animation.EndRotation;
                }
                if (this.animation.TransformationType.HasFlag(AnimationSlot.TransformationTypes.Scale))
                {
                    this.transform3D.LocalScale = this.animation.EndScale;
                }

                this.completed = true;
                if (this.Completed != null)
                {
                    this.Completed(this, new EventArgs());
                }
            }
        }
    }
}
