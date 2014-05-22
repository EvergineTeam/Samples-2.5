#region Using Statements
using DolbySampleProject.Behaviors;
using System;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Components.UI;
using WaveEngine.Framework;
using WaveEngine.Framework.Animation;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
#endregion

namespace DolbySampleProject
{
    public class BackgroundScene : Scene
    {

        private SingleAnimation fadeIn;
        private SingleAnimation logoYTransform;
        private SingleAnimation backgoundYtransform;

        private SingleAnimation leftSpeakerXtransform;
        private SingleAnimation rightSpeakerXtransform;

        private Entity dolbyLogo;
        private Entity background;
        private Entity leftSpeaker;
        private Entity rightSpeaker;

        /// <summary>
        /// Creates the scene.
        /// </summary>
        /// <remarks>
        /// This method is called before all 
        /// <see cref="T:WaveEngine.Framework.Entity" /> instances in this instance are initialized.
        /// </remarks>
        protected override void CreateScene()
        {
            RenderManager.BackgroundColor = Color.Blue;
            //RenderManager.DebugLines = true;

            //// Entities
            // Left Speaker
            this.leftSpeaker = this.CreateSpeaker(WaveServices.ViewportManager.LeftEdge, WaveServices.ViewportManager.VirtualHeight / 2, false, new Vector2(0.0f, 0.5f), new Vector2(1.0f, 1.0f), "Content/speakera.wpk", 0.5f);
            Entity leftChild = this.CreateSpeaker(0.0f, 0.0f, false, new Vector2(0.0f, 0.5f), new Vector2(1.0f, 1.0f), "Content/speakerb.wpk", 0.0f);
            leftChild.AddComponent(new BlinkBehavior());
            this.leftSpeaker.AddChild(leftChild);
            EntityManager.Add(this.leftSpeaker);

            // Right Speaker
            this.rightSpeaker = this.CreateSpeaker(WaveServices.ViewportManager.RightEdge, WaveServices.ViewportManager.VirtualHeight / 2, true, new Vector2(1.0f, 0.5f), new Vector2(1.0f, 1.0f), "Content/speakera.wpk", 0.5f);
            Entity rightChild = this.CreateSpeaker(0.0f, 0.0f, true, new Vector2(1.0f, 0.5f), new Vector2(1.0f, 1.0f), "Content/speakerb.wpk", 0.0f);
            rightChild.AddComponent(new BlinkBehavior());
            this.rightSpeaker.AddChild(rightChild);
            EntityManager.Add(this.rightSpeaker);

            // Dolby Logo
            this.dolbyLogo = new Entity()
                            .AddComponent(new Transform2D() { X = WaveServices.ViewportManager.VirtualWidth / 2, Y = WaveServices.ViewportManager.VirtualHeight / 2, Origin = Vector2.Center, DrawOrder = 0.5f, Opacity = 1 })
                            .AddComponent(new AnimationUI())
                            .AddComponent(new Sprite("Content/logo.wpk"))
                            .AddComponent(new SpriteRenderer(DefaultLayers.GUI));
            EntityManager.Add(this.dolbyLogo);

            // Background
            this.background = new Entity()
                  .AddComponent(new Transform2D() { X = WaveServices.ViewportManager.VirtualWidth / 2, Y = 0, Origin = new Vector2(0.5f, 0), DrawOrder = 1.0f, XScale = 1.5f, YScale = 1.5f })
                  .AddComponent(new AnimationUI())
                  .AddComponent(new Sprite("Content/background.wpk"))
                  .AddComponent(new SpriteRenderer(DefaultLayers.GUI));
            EntityManager.Add(this.background);

            //// Animations
            this.fadeIn = new SingleAnimation(0, 1, TimeSpan.FromSeconds(3), EasingFunctions.Cubic);
            this.logoYTransform = new SingleAnimation(WaveServices.ViewportManager.VirtualHeight / 2, 50, TimeSpan.FromSeconds(3), EasingFunctions.Cubic);
            this.backgoundYtransform = new SingleAnimation(-50, -90, TimeSpan.FromSeconds(20), EasingFunctions.Cubic);
            this.leftSpeakerXtransform = new SingleAnimation(-500, 0, TimeSpan.FromSeconds(6));
            this.rightSpeakerXtransform = new SingleAnimation(WaveServices.ViewportManager.RightEdge + 500, WaveServices.ViewportManager.RightEdge, TimeSpan.FromSeconds(6));         
        }

        /// <summary>
        /// Creates the speaker.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="mirror">if set to <c>true</c> [mirror].</param>
        /// <param name="origin">The origin.</param>
        /// <param name="scale">The scale.</param>
        /// <param name="texture">The texture.</param>
        /// <param name="drawOrder">The draw order.</param>
        /// <returns></returns>
        private Entity CreateSpeaker(float x, float y, bool mirror, Vector2 origin, Vector2 scale, string texture, float drawOrder)
        {
            Transform2D transform = new Transform2D()
                            {
                                Origin = origin,
                                XScale = scale.X,
                                YScale = scale.Y,
                                X = x,
                                Y = y,
                                DrawOrder = drawOrder
                            };
            if (mirror)
            {
                transform.Effect = SpriteEffects.FlipHorizontally;
            }

            Entity speaker = new Entity()
                            .AddComponent(transform)
                            .AddComponent(new AnimationUI())
                            .AddComponent(new Sprite(texture))
                            .AddComponent(new SpriteRenderer(DefaultLayers.GUI));

            return speaker;
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

            var dolbyLogoAnimation = this.dolbyLogo.FindComponent<AnimationUI>();
            dolbyLogoAnimation.BeginAnimation(Transform2D.YProperty, this.logoYTransform);
            dolbyLogoAnimation.BeginAnimation(Transform2D.OpacityProperty, this.fadeIn);

            var backgroundAnimation = this.background.FindComponent<AnimationUI>();
            backgroundAnimation.BeginAnimation(Transform2D.YProperty, this.backgoundYtransform);

            var leftSpeakerAnimation = this.leftSpeaker.FindComponent<AnimationUI>();
            leftSpeakerAnimation.BeginAnimation(Transform2D.XProperty, this.leftSpeakerXtransform);

            var rightSpeakerAnimation = this.rightSpeaker.FindComponent<AnimationUI>();
            rightSpeakerAnimation.BeginAnimation(Transform2D.XProperty, this.rightSpeakerXtransform);
        }
    }
}
