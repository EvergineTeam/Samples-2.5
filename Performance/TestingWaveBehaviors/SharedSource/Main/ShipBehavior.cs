using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Common.Input;
using WaveEngine.Common.Math;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;

namespace TestingWaveBehaviors
{
    [DataContract]
    public class ShipBehavior : Behavior, IFire
    {
        public GunController gunController;

        [RequiredComponent]
        public Transform2D Transform;

        protected override void DefaultValues()
        {
            base.DefaultValues();

            gunController = new GunController();
            gunController.fireController = this;
        }

        protected override void Update(TimeSpan gameTime)
        {
            var keyboardState = WaveServices.Input.KeyboardState;
            var mouseState = WaveServices.Input.MouseState;

            this.Transform.X = mouseState.X;
            this.Transform.Y = mouseState.Y;

            gunController.CountShooRate(gameTime);

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                gunController.ApplyFire();
            }
            if (mouseState.RightButton == ButtonState.Pressed)
            {
                gunController.Reload();
            }
        }

        public void Fire()
        {
            var bullet = new Entity()
                    .AddComponent(new Transform2D())
                    .AddComponent(new Sprite(WaveContent.Assets.Bullet_png))
                    .AddComponent(new SpriteRenderer());

            bullet.FindComponent<Transform2D>().Position = this.Transform.Position;
            bullet.FindComponent<Transform2D>().Origin = Vector2.One / 2;
            bullet.FindComponent<Transform2D>().DrawOrder = 10;


            bullet.AddComponent(new BulletBehavior());

            EntityManager.Add(bullet);
        }
    }
}
