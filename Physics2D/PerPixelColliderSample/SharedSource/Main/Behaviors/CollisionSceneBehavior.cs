using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics2D;
using WaveEngine.Framework.Services;

namespace PerPixelColliderSample
{
    public class CollisionSceneBehavior : SceneBehavior
    {
        public MyScene myScene;

        protected override void ResolveDependencies()
        {
            myScene = (MyScene)this.Scene;
        }

        protected override void Update(TimeSpan gameTime)
        {
            if (myScene.State == SampleState.Playing)
            {
                // Playing update
                ScrollBehavior.ScrollSpeed += MyScene.SCROLLACCELERATION * (float)gameTime.TotalSeconds;

                // Gets the elements colliders
                PerPixelCollider2D shipCollider = myScene.ship.FindComponent<PerPixelCollider2D>();
                PerPixelCollider2D groundCollider = myScene.ground.FindComponent<PerPixelCollider2D>();
                PerPixelCollider2D groundCollider2 = myScene.ground2.FindComponent<PerPixelCollider2D>();
                PerPixelCollider2D groundCollider3 = myScene.ground3.FindComponent<PerPixelCollider2D>();
                PerPixelCollider2D obstacleCollider;
                bool collision = false;

                if (shipCollider.Intersects(groundCollider) || shipCollider.Intersects(groundCollider2) || shipCollider.Intersects(groundCollider3))
                {
                    // Checks collision of the ground.
                    collision = true;
                }
                else
                {

                    // Iterates through the obstacles to detect intersection
                    foreach (var obstacle in this.myScene.obstacles)
                    {
                        obstacleCollider = obstacle.FindComponent<PerPixelCollider2D>();

                        if (shipCollider.Intersects(obstacleCollider))
                        {
                            collision = true;
                            break;
                        }
                    }
                }

                if (collision)
                {
                    this.myScene.Explosion();
                    this.myScene.State = SampleState.Waiting;
                }

                if (this.myScene.ship.FindComponent<Transform2D>().Y > this.Scene.VirtualScreenManager.ScreenHeight)
                {
                    this.myScene.State = SampleState.Waiting;
                }
            }
            else
            {
                this.myScene.countDown += (int)gameTime.TotalMilliseconds;
                if (this.myScene.countDown > MyScene.WAITINGTIME)
                {
                    this.myScene.State = SampleState.Playing;
                }
            }
        }
    }
}
