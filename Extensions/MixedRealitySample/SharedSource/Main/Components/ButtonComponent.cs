#region Using Statements
using System;
using System.Runtime.Serialization;
using WaveEngine.Common.Math;
using WaveEngine.Components.GameActions;
using WaveEngine.Framework.Graphics;
#endregion

namespace MixedRealitySample.Components
{
    [DataContract]
    public class ButtonComponent : ButtonComponentBase
    {
        private Vector3 originalScale;
      
        protected override void ResolveDependencies()
        {
            base.ResolveDependencies();

            this.buttonBase = this.Owner.FindChild("base");
            this.originalScale = this.buttonBase.FindComponent<Transform3D>().LocalScale;           
        }

        protected override void DefaultValues()
        {
            base.DefaultValues();
            
            this.hoverScale = 1.1f;
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.Hover = false;
            this.UpdateHover();
        }

        protected override void CustomHover(bool isHover)
        {
            base.CustomHover(isHover);
            this.UpdateHover();
        }

        protected override void CustomTouch()
        {
            base.CustomTouch();

            // not hover, just click
            if (this.buttonBase != null && this.buttonBase.Parent != null)
            {              
                var time = TimeSpan.FromSeconds(0.1f);
                this.Owner.Scene.CreateGameAction(new ScaleTo3DGameAction(this.buttonBase, this.originalScale * 0.9f, time, EaseFunction.None, true))
                                                .ContinueWith(new ScaleTo3DGameAction(this.buttonBase, this.originalScale, TimeSpan.FromSeconds(0.1f), EaseFunction.None, true))
                                  .Run();
            }
        }

        private void UpdateHover()
        {
            Vector3 scale;

            if (this.hover)
            {
                scale = this.originalScale * this.HoverScale;              
            }
            else
            {
                scale = this.originalScale;                
            }

            if (this.buttonBase != null)
            {
                var time = TimeSpan.FromSeconds(0.1f);
                this.Owner.Scene.CreateGameAction(
                    new ScaleTo3DGameAction(this.buttonBase, scale, time, EaseFunction.None, true))
                    .Run();                
            }
        }                    
    }
}