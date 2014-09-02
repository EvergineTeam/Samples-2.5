using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Framework;
using WaveEngine.Framework.Animation;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;

namespace LoadingProject
{
    public class Loading : Scene
    {
        Entity background, tower, text;
        GameScene gameScene;

        protected override void CreateScene()
        {
            var camera2D = new FixedCamera2D("Camera2D");
            camera2D.BackgroundColor = Color.Black;
            EntityManager.Add(camera2D);

            background = new Entity()
                .AddComponent(new Transform2D()
                {
                    Origin = Vector2.Center,
                    DrawOrder = 1f,
                    XScale = 4f,
                    YScale = 4f,
                })
                .AddComponent(new Sprite("Content/Background")
                {
                    TintColor = new Color(255 - 71, 255 - 80, 255 - 87),
                })
                .AddComponent(new AnimationUI())
                .AddComponent(new SpriteRenderer(DefaultLayers.Alpha));

            EntityManager.Add(background);

            tower = new Entity()
                .AddComponent(new Transform2D()
                {
                    Origin = new Vector2(0, 1),
                    DrawOrder = 0.6f,
                    Y = 480,
                })
                .AddComponent(new Sprite("Content/LoadingSpankerImg"))
                .AddComponent(new AnimationUI())
                .AddComponent(new SpriteRenderer(DefaultLayers.Alpha));

            EntityManager.Add(tower);

            text = new Entity()
                .AddComponent(new Transform2D()
                {
                    DrawOrder = 0.9f,
                    Rotation = MathHelper.ToRadians(-45),
                    XScale = 2f,
                    YScale = 2f,
                })
                .AddComponent(new Sprite("Content/LoadingSpankerText"))
                .AddComponent(new AnimationUI())
                .AddComponent(new SpriteRenderer(DefaultLayers.Alpha));

            EntityManager.Add(text);
        }

        protected override void Start()
        {
            base.Start();

            StartAnimation();

            WaveServices.TaskScheduler.CreateTask(() =>
            {
                gameScene = new GameScene();
                gameScene.Initialize(WaveServices.GraphicsDevice);
                Thread.Sleep(5000);
            })
            .ContinueWith(() =>
            {
                EndAnimation();
            });
        }

        private void StartAnimation()
        {
            TimeSpan animationTime = TimeSpan.FromMilliseconds(600);

            background.FindComponent<AnimationUI>()
                    .BeginAnimation(
                        Transform2D.OpacityProperty,
                        new SingleAnimation(0f, 1f, TimeSpan.FromMilliseconds(2000)));

            tower.FindComponent<AnimationUI>()
                    .BeginAnimation(
                        Transform2D.XScaleProperty,
                        new SingleAnimation(1f, 1.2f, TimeSpan.FromMilliseconds(2000), EasingFunctions.Cubic))
                    .BeginAnimation(
                        Transform2D.YScaleProperty,
                        new SingleAnimation(1f, 1.2f, TimeSpan.FromMilliseconds(2000), EasingFunctions.Cubic))
                    .BeginAnimation(
                        Transform2D.XProperty,
                        new SingleAnimation(-400, 0, TimeSpan.FromSeconds(2), EasingFunctions.Cubic));

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
                                800,
                                300,
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

            tower.FindComponent<AnimationUI>()
                   .BeginAnimation(
                       Transform2D.XScaleProperty,
                       new SingleAnimation(1.2f, 1.6f, TimeSpan.FromSeconds(70)))
                   .BeginAnimation(
                       Transform2D.YScaleProperty,
                       new SingleAnimation(1.2f, 1.6f, TimeSpan.FromSeconds(70)));
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
