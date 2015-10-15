#region Using Statements
using System;
using WaveEngine.Analytics;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Components.UI;
using WaveEngine.Framework;
using WaveEngine.Framework.Animation;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Resources;
using WaveEngine.Framework.Services;
using WaveEngine.Framework.UI;
#endregion

namespace AnalyticsDemo
{
    public class MyScene : Scene
    {
        private SingleAnimation showSpanker, showColonel, showFuzz;
        private AnimationUI fuzzAnimation, colonelAnimation, spankerAnimation;

        protected override void CreateScene()
        {
            this.Load(WaveContent.Scenes.MyScene);
            // Sonic
            Button spankerButton = new Button()
            {
                Width = 226,
                Height = 226,
                Text = string.Empty,
                Margin = new Thickness(40, 500, 0, 0),
                BackgroundImage = WaveContent.Assets.spanker_png,
                PressedBackgroundImage = WaveContent.Assets.spankerPressed_png,
            };
            spankerButton.Entity.AddComponent(new AnimationUI());
            this.spankerAnimation = spankerButton.Entity.FindComponent<AnimationUI>();
            spankerButton.Click += (s, o) =>
            {
                WaveServices.GetService<AnalyticsManager>().TagEvent("CharacterSelection", "Character", "Spanker");
            };
            EntityManager.Add(spankerButton);

            // Link
            Button colonelButton = new Button()
            {
                Width = 226,
                Height = 226,
                Text = string.Empty,
                Margin = new Thickness(288, 500, 0, 0),
                BackgroundImage = WaveContent.Assets.colonel_png,
                PressedBackgroundImage = WaveContent.Assets.colonelPressed_png,
            };
            colonelButton.Entity.AddComponent(new AnimationUI());
            this.colonelAnimation = colonelButton.Entity.FindComponent<AnimationUI>();
            colonelButton.Click += (s, o) =>
            {
                WaveServices.GetService<AnalyticsManager>().TagEvent("CharacterSelection", "Character", "Colonel");
            };
            EntityManager.Add(colonelButton);

            // Mario
            Button fuzzButton = new Button()
            {
                Width = 226,
                Height = 226,
                Text = string.Empty,
                Margin = new Thickness(536, 500, 0, 0),
                BackgroundImage = WaveContent.Assets.fuzz_png,
                PressedBackgroundImage = WaveContent.Assets.fuzzPressed_png,
            };
            fuzzButton.Entity.AddComponent(new AnimationUI());
            this.fuzzAnimation = fuzzButton.Entity.FindComponent<AnimationUI>();
            fuzzButton.Click += (s, o) =>
            {
                WaveServices.GetService<AnalyticsManager>().TagEvent("CharacterSelection", "Character", "Fuzz");
            };
            EntityManager.Add(fuzzButton);

            this.showSpanker = new SingleAnimation(0, -374, 0.5f, EasingFunctions.Back);
            this.showColonel = new SingleAnimation(0, -374, 0.5f, EasingFunctions.Back);
            this.showFuzz = new SingleAnimation(0, -374, 0.5f, EasingFunctions.Back);
        }

        protected override void Start()
        {
            base.Start();

            //
            // Initial animation
            //
            this.spankerAnimation.BeginAnimation(Transform2D.YProperty, showSpanker);

            WaveServices.TimerFactory.CreateTimer("colonelShow", TimeSpan.FromSeconds(0.2f), () =>
            {
                this.colonelAnimation.BeginAnimation(Transform2D.YProperty, showColonel);
            }, false);

            WaveServices.TimerFactory.CreateTimer("fuzzShow", TimeSpan.FromSeconds(0.4f), () =>
            {
                this.fuzzAnimation.BeginAnimation(Transform2D.YProperty, showFuzz);
            }, false);
        }
    }
}
