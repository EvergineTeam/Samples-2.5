using System;
using System.Runtime.Serialization;
using WaveEngine.Common.Attributes;
using WaveEngine.Common.Graphics;
using WaveEngine.Framework;
using WaveEngine.Materials;

namespace MixedRealitySample.Components
{
    [DataContract]
    public class ButtonComponentBase : Component
    {
        protected Entity buttonBase;
        protected StandardMaterial buttonBaseMaterial;
        protected bool hover;
        protected float hoverScale;

        public event EventHandler Touched;

        [DataMember]
        public Color ReleaseColor { get; set; }

        [DataMember]
        public Color HoverColor { get; set; }

        [DontRenderProperty]
        public bool Hover
        {
            get
            {
                return this.hover;
            }

            set
            {
                if (this.hover != value)
                {
                    this.hover = value;

                    if (this.isInitialized)
                    {
                        this.HoverAction(this.hover);
                    }
                }
            }
        }

        [DataMember]
        public float HoverScale
        {
            get
            {
                return this.hoverScale;
            }

            set
            {
                if (this.hoverScale != value)
                {
                    this.hoverScale = value;

                    if (this.isInitialized)
                    {
                        this.HoverAction(this.hover);
                    }
                }
            }
        }

        protected override void DefaultValues()
        {
            base.DefaultValues();
        }

        protected override void ResolveDependencies()
        {
            base.ResolveDependencies();
        }

        public void Touch()
        {
            this.CustomTouch();
            this.Touched?.Invoke(this, null);
        }

        private void HoverAction(bool isHover)
        {
            this.CustomHover(isHover);
        }

        protected virtual void CustomTouch()
        {
        }

        protected virtual void CustomHover(bool isHover)
        {
        }
    }
}