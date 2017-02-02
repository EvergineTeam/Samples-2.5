#region Using Statements
using System;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Resources;
using WaveEngine.Framework.Services;
using WaveEngine.Components.UI;
using WaveEngine.Framework.UI;
using System.Collections.Generic;
using WaveEngine.ImageEffects;
#endregion

namespace ImageEffects
{
    public class MyScene : Scene
    {
        private Entity camera3DEntity;
        private List<Lens> effects;

        private int currentEffectIndex;
        private Lens lastEffects;
        private Button effectButton;
        private bool flag;

        protected override void CreateScene()
        {
            this.Load(WaveContent.Scenes.MyScene);

            this.camera3DEntity = this.EntityManager.Find("defaultCamera3D");

            // Create effects
            this.CreateEffects();

            // Button next effect
            this.effectButton = new Button()
            {
                Text = "Original",
                VerticalTextAlignment = VerticalAlignment.Center,
                Width = 250,
                BackgroundColor = Color.Purple,
                HorizontalAlignment = HorizontalAlignment.Center,
            };
            this.effectButton.Click += (s, o) =>
            {
                if (!this.flag)
                {
                    WaveServices.Dispatcher.RunOnWaveThread(() =>
                    {
                        this.flag = true;

                        if (this.lastEffects != null)
                        {
                            this.camera3DEntity.RemoveComponent(this.lastEffects);
                        }

                        this.lastEffects = this.effects[this.currentEffectIndex];
                        this.currentEffectIndex = (this.currentEffectIndex + 1) % this.effects.Count;
                        this.effectButton.Text = this.lastEffects.GetType().Name;
                        WaveServices.Layout.PerformLayout();

                        if (this.lastEffects != null)
                        {
                            this.camera3DEntity.AddComponent(this.lastEffects);
                        }

                        this.flag = false;
                    });
                }
            };
            this.EntityManager.Add(this.effectButton);
        }

        private void CreateEffects()
        {
            this.effects = new List<Lens>();

            this.effects.Add(new AntialiasingLens());
            this.effects.Add(new BloomLens());
            this.effects.Add(new BokehLens());
            this.effects.Add(new ChromaticAberrationLens());
            this.effects.Add(new ConvolutionLens());
            this.effects.Add(new DepthOfFieldLens());
            this.effects.Add(new DistortionLens());
            this.effects.Add(new FastBlurLens());
            this.effects.Add(new FilmGrainLens());
            this.effects.Add(new FishEyeLens());
            this.effects.Add(new FogLens());
            this.effects.Add(new GaussianBlurLens());
            this.effects.Add(new GlowLens());
            this.effects.Add(new GrayScaleLens());
            this.effects.Add(new InvertLens());
            this.effects.Add(new LensFlareLens());
            this.effects.Add(new LightShaftLens());
            this.effects.Add(new MotionBlurLens());
            this.effects.Add(new PixelateLens());
            this.effects.Add(new PosterizeLens());
            this.effects.Add(new RadialBlurLens());
            this.effects.Add(new ScanlinesLens());
            this.effects.Add(new ScreenOverlayLens());
            this.effects.Add(new SepiaLens());
            this.effects.Add(new SobelLens());
            this.effects.Add(new SSAOLens());
            this.effects.Add(new TilingLens());
            this.effects.Add(new TiltShiftLens());
            this.effects.Add(new ToneMappingLens());
            this.effects.Add(new VignetteLens());
        }
    }
}
