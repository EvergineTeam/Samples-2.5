using System;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;

namespace PongProject
{
    public class MySceneBehavior : SceneBehavior
    {
        private const int GOALS_TO_WIN = 2;
        private int time = 3;
        Entity ball;
        Entity bg;
        BallBehavior ballBehavior;
        Timer timer;

        public MySceneBehavior()
            : base("PongBehavior")
        { }

        protected override void Update(TimeSpan gameTime)
        {

            var state = (this.Scene as GameScene).CurrentState;

            switch (state)
            {
                case GameScene.State.Init:

                    var textBlock = (this.Scene as GameScene).textblockInit;
                    textBlock.IsVisible = true;
                    textBlock.Text = time.ToString();
                    ball.IsActive = false;
                    bg.IsVisible = false;

                    if (timer == null)
                    {
                        timer = WaveServices.TimerFactory.CreateTimer("Init", TimeSpan.FromSeconds(1f), () =>
                        {
                            textBlock.Text = time.ToString();
                            time--;
                        });
                    }

                    if (time <= 0)
                    {
                        time = 3;
                        WaveServices.TimerFactory.RemoveTimer("Init");
                        timer = null;
                        textBlock.IsVisible = false;
                        bg.IsVisible = true;
                        SetState(GameScene.State.Game);
                    }

                    break;

                case GameScene.State.Game:

                    ball.IsActive = true;

                    break;

                case GameScene.State.Goal:

                    (this.Scene as GameScene).PlaySoundGoal();

                    ball.IsActive = false;

                    var textBlock1 = (this.Scene as GameScene).textblockGoal1;
                    textBlock1.Text = ballBehavior.Goal1.ToString();

                    var textBlock2 = (this.Scene as GameScene).textblockGoal2;
                    textBlock2.Text = ballBehavior.Goal2.ToString();

                    if (ballBehavior.Goal1 == GOALS_TO_WIN || ballBehavior.Goal2 == GOALS_TO_WIN)
                    {
                        SetState(GameScene.State.Win);
                        break;
                    }

                    SetState(GameScene.State.Init);

                    break;

                case GameScene.State.Win:

                    (this.Scene as GameScene).trophy.IsVisible = true;
                    ball.IsActive = false;

                    if (ballBehavior.Goal1 == GOALS_TO_WIN)
                    {
                        (this.Scene as GameScene).trophy.FindComponent<Transform2D>().X = WaveServices.Platform.ScreenWidth / 2 - 300;
                        (this.Scene as GameScene).trophy.FindComponent<Transform2D>().Y = WaveServices.Platform.ScreenHeight / 2 - 100;
                    }
                    else
                    {
                        (this.Scene as GameScene).trophy.FindComponent<Transform2D>().X = WaveServices.Platform.ScreenWidth / 2 + 100;
                        (this.Scene as GameScene).trophy.FindComponent<Transform2D>().Y = WaveServices.Platform.ScreenHeight / 2 - 100;
                    }

                    break;
            }
        }

        public void SetState(GameScene.State _State)
        {
            (this.Scene as GameScene).CurrentState = _State;
        }

        protected override void ResolveDependencies()
        {

            ball = (this.Scene as GameScene).EntityManager.Find<Entity>("Ball");
            bg = (this.Scene as GameScene).EntityManager.Find<Entity>("Background");
            ballBehavior = ball.FindComponent<BallBehavior>();
        }
    }
}
