#region Using Statements
using System;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Diagnostic;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics2D;
using WaveEngine.Framework.Resources;
using WaveEngine.Framework.Services;
#endregion

namespace OnPhysics2dCollisionEvent
{
    public class MyScene : Scene
    {                
        protected override void CreateScene()
        {
            this.Load(WaveContent.Scenes.MyScene);

            var collidables = this.EntityManager.FindAllByTag("CollidableEntity");

            foreach (var collidable in collidables)
            {
                var entity = collidable as Entity;
                if (entity != null)
                {
                    var rigidBody = entity.FindComponent<RigidBody2D>();
                    if (rigidBody != null)
                    {
                        rigidBody.OnPhysic2DCollision += this.OnPhysic2DCollision;
                    }
                }
            }

            // Mouse Handler.
            this.AddSceneBehavior(new MouseBehavior(), SceneBehavior.Order.PostUpdate);
        }

        private void OnPhysic2DCollision(object sender, Physic2DCollisionEventArgs args)
        {
            // Gets angle, updates Textblocks and Draws information marks
            float angle = (float)Math.Atan2(args.Normal.X, args.Normal.Y);
            Labels.Add("Normal", args.Normal);
            Labels.Add("Angle", MathHelper.ToDegrees(angle));
            Labels.Add("PointA", args.PointA);
            Labels.Add("PointB", args.PointB);
            this.CreateVolatileMark(args.PointA.Value.X, args.PointA.Value.Y, angle);
        }

        /// <summary>
        /// Creates a Volatile Mark on X,Y position.
        /// </summary>
        /// <param name="x">X Position.</param>
        /// <param name="y">Y Position.</param>
        private void CreateVolatileMark(float x, float y, float angle)
        {
            Entity mark = new Entity()
                .AddComponent(new Transform2D() { X = x - 7, Y = y - 7 })
                .AddComponent(new Sprite(WaveContent.Assets.Mark_png))
                .AddComponent(new SpriteRenderer(DefaultLayers.Alpha));
            EntityManager.Add(mark);

            Entity arrow = new Entity()
                .AddComponent(new Transform2D() { Origin = new Vector2(0.5f, 1), X = x, Y = y, Rotation = angle })
                .AddComponent(new Sprite(WaveContent.Assets.Arrow_png))
                .AddComponent(new SpriteRenderer(DefaultLayers.Alpha));
            EntityManager.Add(arrow);

            // Sets Time To Live for the mark. Remove timer from WaveServices after use.
            WaveServices.TimerFactory.CreateTimer("Timer" + mark.Name, TimeSpan.FromSeconds(1), () =>
            {
                EntityManager.Remove(mark);
                EntityManager.Remove(arrow);
                WaveServices.TimerFactory.RemoveTimer("Timer" + mark.Name);
            });
        }
    }
}
