using System;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;

namespace PongProject
{
    class PlayerAIBehavior : Behavior
    {
        private const float SPEED = 5f;
        private const int BORDER_OFFSET = 25;

        private int direction;
        private bool move = false;

        [RequiredComponent]
        public Transform2D trans2D;

        public Entity ball;
        public Transform2D transBall2D;
        public BallBehavior ballBehavior;

        public PlayerAIBehavior(Entity ball)
            : base("PlayerIABehavior")
        {
            this.trans2D = null;
            this.ball = ball;
            this.transBall2D = ball.FindComponent<Transform2D>();
            this.ballBehavior = ball.FindComponent<BallBehavior>();
            this.direction = ballBehavior.HorizontalDirection;
        }

        protected override void Update(TimeSpan gameTime)
        {
            this.direction = ballBehavior.HorizontalDirection;

            // Move sprite
            if (this.direction > 0 && (Owner.Scene as GameScene).CurrentState == GameScene.State.Game)
                move = true;
            else
                move = false;

            if (trans2D.Y < transBall2D.Y && move)
                trans2D.Y += SPEED * (gameTime.Milliseconds / 10);
            else if (trans2D.Y > transBall2D.Y && move)
                trans2D.Y -= SPEED * (gameTime.Milliseconds / 10);

            // Check borders
            if (trans2D.Y < BORDER_OFFSET + trans2D.YScale + 80)
            {
                trans2D.Y = BORDER_OFFSET + trans2D.YScale + 80;
            }
            else if (trans2D.Y > WaveServices.Platform.ScreenHeight - BORDER_OFFSET)
            {
                trans2D.Y = WaveServices.Platform.ScreenHeight - BORDER_OFFSET;
            }
        }
    }
}
