#region Using Statements
using System;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Common.Media;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Components.UI;
using WaveEngine.Framework;
using WaveEngine.Framework.Animation;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Resources;
using WaveEngine.Framework.Services;
#endregion

namespace DolbySample
{
    public class UIScene : Scene
    {
        private SingleAnimation fadeIn;
        private SingleAnimation logoYTransform;
        private SingleAnimation leftSpeakerXtransform;
        private SingleAnimation rightSpeakerXtransform;

        protected override void CreateScene()
        {
            this.Load(WaveContent.Scenes.UIScene);

            this.fadeIn = new SingleAnimation(0, 1, TimeSpan.FromSeconds(2), EasingFunctions.Cubic);
            this.logoYTransform = new SingleAnimation(-WaveServices.ViewportManager.VirtualHeight, -WaveServices.ViewportManager.VirtualHeight / 3, TimeSpan.FromSeconds(3), EasingFunctions.Cubic);
        }

        /// <summary>
        /// Allows to perform custom code when this instance is started.
        /// </summary>
        /// <remarks>
        /// This base method perfoms a layout pass.
        /// </remarks>
        protected override void Start()
        {
            base.Start();

            var background = EntityManager.Find("background");            
            background.FindComponent<Transform2D>().Y -= WaveServices.ViewportManager.VirtualHeight/2;

            var dolbyLogoAnimation = EntityManager.Find("logo").FindComponent<AnimationUI>();
            dolbyLogoAnimation.BeginAnimation(Transform2D.YProperty, this.logoYTransform);
            dolbyLogoAnimation.BeginAnimation(Transform2D.OpacityProperty, this.fadeIn);
            
            var leftSpeaker = EntityManager.Find("leftSpeaker");
            
            var leftSpeakerTransform = leftSpeaker.FindComponent<Transform2D>();
            leftSpeakerTransform.X = -WaveServices.ViewportManager.VirtualWidth / 2;
            leftSpeakerTransform.Y = WaveServices.ViewportManager.VirtualHeight / 2;

            var leftSpeakerTexture = leftSpeaker.FindComponent<Sprite>().Texture;
            var leftSpeakerAnimation = leftSpeaker.FindComponent<AnimationUI>();
            this.leftSpeakerXtransform = new SingleAnimation(-WaveServices.ViewportManager.VirtualWidth / 2 - leftSpeakerTexture.Width, -WaveServices.ViewportManager.VirtualWidth / 2, TimeSpan.FromSeconds(2));
            leftSpeakerAnimation.BeginAnimation(Transform2D.XProperty, this.leftSpeakerXtransform);

            
            var rightSpeaker = EntityManager.Find("rightSpeaker");
            var rightSpeakerTransform = rightSpeaker.FindComponent<Transform2D>();
            rightSpeakerTransform.X = WaveServices.ViewportManager.VirtualWidth / 2;
            rightSpeakerTransform.Y = WaveServices.ViewportManager.VirtualHeight / 2;

            var rightSpeakerTexture = rightSpeaker.FindComponent<Sprite>().Texture;
            var rightSpeakerAnimation = rightSpeaker.FindComponent<AnimationUI>();
            this.rightSpeakerXtransform = new SingleAnimation(WaveServices.ViewportManager.VirtualWidth / 2 + rightSpeakerTexture.Width, WaveServices.ViewportManager.VirtualWidth / 2, TimeSpan.FromSeconds(2));
            rightSpeakerAnimation.BeginAnimation(Transform2D.XProperty, this.rightSpeakerXtransform);
        }

    }
}
