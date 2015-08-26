#region Using Statements
using System;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics3D;
using WaveEngine.Framework.Resources;
using WaveEngine.Framework.Services;
#endregion

namespace ServoMotor3D
{
    public class MyScene : Scene
    {
        private Entity ball;
        private Vector3 ballInitialPosition;
        private Transform3D ballTransform3D;

        protected override void CreateScene()
        {
            this.Load(WaveContent.Scenes.MyScene);

            this.ball = this.EntityManager.Find("ball");
            this.ballTransform3D = this.ball.FindComponent<Transform3D>();
            this.ballInitialPosition = this.ballTransform3D.Position;

            WaveServices.TimerFactory.CreateTimer(TimeSpan.FromSeconds(3), 
                () =>
                {
                    this.ball.RemoveComponent<RigidBody3D>();
                    this.ballTransform3D.Position = this.ballInitialPosition;
                    this.ball.AddComponent(new RigidBody3D());
                    this.ball.RefreshDependencies();
                },
                true);
        }
    }
}
