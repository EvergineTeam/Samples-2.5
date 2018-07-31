using System;
using System.Threading;
using WaveEngine.Common.Math;
using WaveEngine.Components.GameActions;
using WaveEngine.Framework;
using WaveEngine.Framework.Animation;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
using WaveEngine.Framework.Threading;

namespace Loading
{
    public class LoadingScene : Scene
    {
        private Entity background;
        private Entity tower;
        private Entity text;

        private GameScene gameScene;

        private Transform2D backgroundTransform;
        private Transform2D towerTransform;
        private Transform2D textTransform;

        protected override void CreateScene()
        {
            this.Load(@"Content/Scenes/LoadingScene.wscene");

            this.background = this.EntityManager.Find("background");
            this.tower = this.EntityManager.Find("tower");
            this.text = this.EntityManager.Find("text");

            this.backgroundTransform = this.background.FindComponent<Transform2D>();
            this.towerTransform = this.tower.FindComponent<Transform2D>();
            this.textTransform = this.text.FindComponent<Transform2D>();
        }

        protected override void Start()
        {
            base.Start();

            this.StartAnimation();

            WaveBackgroundTask.Run(() =>
            {
                this.gameScene = new GameScene();
                this.gameScene.Initialize(WaveServices.GraphicsDevice);
                Thread.Sleep(5000);
            })
            .ContinueWith((t) =>
            {
                this.EndAnimation();
            });
        }

        private void StartAnimation()
        {
            var backgroundTransform = this.background.FindComponent<Transform2D>();
            var towerTransform = this.tower.FindComponent<Transform2D>();
            var textTransform = this.text.FindComponent<Transform2D>();

            var animation = this.CreateParallelGameActions(
                new FloatAnimationGameAction(this.background, 0.0f, 1.0f, TimeSpan.FromMilliseconds(2000), EaseFunction.CircleOutEase, (v) => { backgroundTransform.Opacity = v; }),
                new FloatAnimationGameAction(this.tower, 0.0f, 1.0f, TimeSpan.FromMilliseconds(1000), EaseFunction.ExponentialOutEase, (v) => { towerTransform.Opacity = v; }),
                new MoveTo2DGameAction(this.tower, new Vector2(0.0f, 77.0f), TimeSpan.FromMilliseconds(3000), EaseFunction.CircleOutEase, true)
                )
                .WaitAll()
                .ContinueWith(
                    new MoveTo2DGameAction(this.text, Vector2.Zero, TimeSpan.FromMilliseconds(500f), EaseFunction.BackOutEase, true));

            animation.Run();
        }

        private void EndAnimation()
        {
            var animation = new MoveTo2DGameAction(this.text, new Vector2(700, -700), TimeSpan.FromMilliseconds(500f), EaseFunction.BackInEase, true)
                           .ContinueWith(
                               this.CreateParallelGameActions(
                                   new FloatAnimationGameAction(this.background, 1.0f, 0.0f, TimeSpan.FromMilliseconds(2000), EaseFunction.CircleInOutEase, (v) => { backgroundTransform.Opacity = v; }),
                                   new FloatAnimationGameAction(this.tower, 1.0f, 0.0f, TimeSpan.FromMilliseconds(500), EaseFunction.CircleInEase, (v) => { towerTransform.Opacity = v; }),
                                   new MoveTo2DGameAction(this.tower, new Vector2(0.0f, 500.0f), TimeSpan.FromMilliseconds(500), EaseFunction.CircleInEase, true)
                                   )
                                   .WaitAll()
                            );

            animation.Completed += this.AnimationCompleted;
            animation.Run();
        }

        private void AnimationCompleted(IGameAction obj)
        {
            WaveServices.ScreenContextManager.To(new ScreenContext(gameScene));
        }
    }
}
