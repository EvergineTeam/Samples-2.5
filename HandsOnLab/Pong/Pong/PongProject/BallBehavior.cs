using System;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics2D;
using WaveEngine.Framework.Services;

namespace PongProject
{
    class BallBehavior : Behavior
    {
        private const float SPEED = 4f;
        private const int BORDER_OFFSET = 20;

        private Entity player;
        private RectangleCollider rectPlayer;

        private Entity player2;
        private RectangleCollider rectPlayer2;

        private Entity barBot;
        private RectangleCollider rectBarBot;

        private Entity barTop;
        private RectangleCollider rectBarTop;

        private int verticalDirection = -1;
        private int horizontalDirection = -1;
        private float speed = SPEED;
        private float incrementSpeed = 0.5f;
        private int goals1 = 0;
        private int goals2 = 0;
        private bool checkGoal = false;

        [RequiredComponent]
        public Transform2D trans2D;

        public int Goal1 { get { return goals1; } private set { goals1 = value; } }
        public int Goal2 { get { return goals2; } private set { goals2 = value; } }
        public int HorizontalDirection { get {return horizontalDirection; } }

        public BallBehavior(Entity player, Entity barBot, Entity barTop, Entity playerIA)
            : base("BallBehavior")
        {
            this.trans2D = null;
            this.player = player;
            this.rectPlayer = player.FindComponent<RectangleCollider>();
            this.player2 = playerIA;
            this.rectPlayer2 = playerIA.FindComponent<RectangleCollider>();
            this.barBot = barBot;
            this.rectBarBot = barBot.FindComponent<RectangleCollider>();
            this.barTop = barTop;
            this.rectBarTop = barTop.FindComponent<RectangleCollider>();
        }

        protected override void Update(TimeSpan gameTime)
        {
            //Check Goals
            if (trans2D.X <= 0 && !checkGoal)
            {

                (Owner.Scene as GameScene).CurrentState = GameScene.State.Goal;
                checkGoal = true;
                goals2++;
                StartBall();
            }

            if (trans2D.X >= WaveServices.Platform.ScreenWidth && !checkGoal)
            {

                (Owner.Scene as GameScene).CurrentState = GameScene.State.Goal;
                checkGoal = true;
                goals1++;
                StartBall();
            }
  
            //Move Ball
            if (trans2D.X > 0 && trans2D.X < WaveServices.Platform.ScreenWidth)
            {
                trans2D.X += horizontalDirection * speed * (gameTime.Milliseconds / 10);
                trans2D.Y += verticalDirection * speed * (gameTime.Milliseconds / 10);
            }
             

            // Check collisions
            if (rectPlayer.Contain(new Vector2(trans2D.X, trans2D.Y)))
            {
                horizontalDirection = 1;
                speed += incrementSpeed;
                (Owner.Scene as GameScene).PlaySoundCollision();
            }

            if (rectPlayer2.Contain(new Vector2(trans2D.X, trans2D.Y)))
            {
                horizontalDirection = -1;
                speed += incrementSpeed;
                (Owner.Scene as GameScene).PlaySoundCollision();
            }

            if (rectBarBot.Contain(new Vector2(trans2D.X, trans2D.Y)))
            {
                verticalDirection = -1;
                (Owner.Scene as GameScene).PlaySoundCollision();
            }

            if (rectBarTop.Contain(new Vector2(trans2D.X, trans2D.Y - 15)))
            {
                verticalDirection = 1;
                (Owner.Scene as GameScene).PlaySoundCollision();
            }
        }

        //Start new ball
        public void StartBall()
        {
            trans2D.X = WaveServices.Platform.ScreenWidth / 2;
            trans2D.Y = WaveServices.Platform.ScreenHeight / 2;
            checkGoal = false;
            speed = SPEED;
        }

    }
}
