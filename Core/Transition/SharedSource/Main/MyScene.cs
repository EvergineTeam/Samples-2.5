#region Using Statements
using System;
using System.Collections.Generic;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Components.Transitions;
using WaveEngine.Components.UI;
using WaveEngine.Framework;
using WaveEngine.Framework.Animation;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Resources;
using WaveEngine.Framework.Services;
using WaveEngine.Framework.UI;

#endregion

namespace Transition
{
    public class MyScene : Scene
    {
        private static readonly TimeSpan TRANSITIONTIME = new TimeSpan(0, 0, 0, 1, 200);

        private ScreenTransition transition;

        private static int sceneIndex = 0;

        private static EasingFunctionBase easeFunction = new CubicEase() { EasingMode = EasingMode.EaseInOut };

        public MyScene(int index)
        {
            switch (index)
            {
                case 0:
                    this.transition = new CrossFadeTransition(TRANSITIONTIME);
                    break;
                case 1:
                    this.transition = new PushTransition(TRANSITIONTIME, PushTransition.EffectOptions.FromLeft);
                    break;
                case 2:
                    this.transition = new ColorFadeTransition(Color.White, TRANSITIONTIME);
                    break;
                case 3:
                    this.transition = new DoorwayTransition(TRANSITIONTIME);
                    break;
                case 4:
                    this.transition = new CombTransition(TRANSITIONTIME, CombTransition.EffectOptions.Horizontal);
                    break;
                case 5:
                    this.transition = new CoverTransition(TRANSITIONTIME, CoverTransition.EffectOptions.FromLeft);
                    break;
                case 6:
                    this.transition = new CurtainsTransition(TRANSITIONTIME);
                    break;
                case 7:
                    this.transition = new FallingLinesTransition(TRANSITIONTIME);
                    break;
                case 8:
                    this.transition = new FanTransition(TRANSITIONTIME);
                    break;
                case 9:
                    this.transition = new RotateTransition(TRANSITIONTIME);
                    break;
                case 10:
                    this.transition = new ScaleTransition(TRANSITIONTIME);
                    break;
                case 11:
                    this.transition = new ShrinkAndSpinTransition(TRANSITIONTIME);
                    break;
                case 12:
                    this.transition = new SpinningSquaresTransition(TRANSITIONTIME);
                    break;
                case 13:
                    this.transition = new UncoverTransition(TRANSITIONTIME, UncoverTransition.EffectOptions.FromLeft);
                    break;
                case 14:
                    this.transition = new ZoomTransition(TRANSITIONTIME);
                    break;
                case 15:
                    this.transition = new ChequeredAppearTransition(TRANSITIONTIME);
                    break;
                default:
                    break;
            }

            this.transition.EaseFunction = easeFunction;
        }

        protected override void CreateScene()
        {
            var scene = string.Format(@"Content/Scenes/Scene{0}.wscene", sceneIndex);

            this.Load(scene);

            var button = new Button()
            {
                Text = string.Format("Next scene with {0}", this.transition.GetType().Name),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                Width = 400,
                Margin = new Thickness(10),
            };

            button.Click += this.OnClick;

            this.EntityManager.Add(button);
        }

        private void OnClick(object sender, EventArgs e)
        {
            NextScene();
        }

        public static void NextScene()
        {
            sceneIndex = (sceneIndex + 1) % 16;

            var context = new ScreenContext(new MyScene(sceneIndex));
            WaveServices.ScreenContextManager.To(context, (context[0] as MyScene).transition);
        }
    }
}
