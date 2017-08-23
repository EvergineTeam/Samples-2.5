using System;
using System.Threading;
using WaveEngine.Framework;
using WaveEngine.Framework.Animation;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
using WaveEngine.Framework.Threading;

namespace Loading
{
    public class LoadingScene : Scene
    {
        Entity background, tower, text;
        GameScene gameScene;

        protected override void CreateScene()
        {
            this.Load(@"Content/Scenes/LoadingScene.wscene");

            background = EntityManager.Find("background");
            tower = EntityManager.Find("tower");
            text = EntityManager.Find("text");
        }

        protected override void Start()
        {
            base.Start();

            StartAnimation();

            WaveBackgroundTask.Run(() =>
            {
                gameScene = new GameScene();
                gameScene.Initialize(WaveServices.GraphicsDevice);
                Thread.Sleep(5000);
            }).ContinueWith((t) =>
            {
                EndAnimation();
            });
        }

        private void StartAnimation()
        {
            background.FindComponent<AnimationUI>()
                    .BeginAnimation(
                        Transform2D.OpacityProperty,
                        new SingleAnimation(0f, 1f, TimeSpan.FromMilliseconds(2000)));

            tower.FindComponent<AnimationUI>()                    
                    .BeginAnimation(
                        Transform2D.XProperty,
                        new SingleAnimation(-718, 0, TimeSpan.FromSeconds(2), EasingFunctions.Cubic));

            SingleAnimation positionX =
                new SingleAnimation(
                    -325,
                    300,
                    TimeSpan.FromMilliseconds(2200),
                    EasingFunctions.Circle);

            positionX.Completed += positionX_Completed;

            text.FindComponent<AnimationUI>()
                    .BeginAnimation(
                        Transform2D.OpacityProperty,
                        new SingleAnimation(0, 0.4f, TimeSpan.FromMilliseconds(400)))
                    .BeginAnimation(
                        Transform2D.XProperty,
                        positionX)
                    .BeginAnimation(
                        Transform2D.YProperty,
                        new SingleAnimation(
                                1300,
                                600,
                                TimeSpan.FromMilliseconds(2200),
                                EasingFunctions.Circle));
        }

        private void positionX_Completed(object sender, EventArgs e)
        {
            text.FindComponent<AnimationUI>()
                .BeginAnimation(
                    Transform2D.XProperty,
                    new SingleAnimation(
                            text.FindComponent<Transform2D>().X,
                            text.FindComponent<Transform2D>().X + 600,
                            TimeSpan.FromSeconds(70)))
                .BeginAnimation(
                    Transform2D.YProperty,
                    new SingleAnimation(
                            text.FindComponent<Transform2D>().Y,
                            text.FindComponent<Transform2D>().Y - 600,
                            TimeSpan.FromSeconds(70)));           
        }

        private void EndAnimation()
        {
            SingleAnimation getOutAnimation =
                new SingleAnimation(
                    0, -500, TimeSpan.FromSeconds(1), EasingFunctions.Back);

            getOutAnimation.Completed += getOutAnimation_Completed;

            tower.FindComponent<AnimationUI>()
                    .BeginAnimation(
                        Transform2D.XProperty,
                        getOutAnimation)
                    .BeginAnimation(
                        Transform2D.OpacityProperty,
                        new SingleAnimation(1, 0, TimeSpan.FromSeconds(1)));

            background.FindComponent<AnimationUI>()
                    .BeginAnimation(
                        Transform2D.OpacityProperty,
                        new SingleAnimation(1, 0, TimeSpan.FromSeconds(1)));

            text.FindComponent<AnimationUI>()
                    .BeginAnimation(
                        Transform2D.OpacityProperty,
                        new SingleAnimation(1, 0, TimeSpan.FromSeconds(1)));
        }

        private void getOutAnimation_Completed(object sender, EventArgs e)
        {
            WaveServices.ScreenContextManager.To(new ScreenContext(gameScene));
        }
    }
}
