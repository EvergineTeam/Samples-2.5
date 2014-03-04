#region Using Statements
using System;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;
#endregion

namespace ParticleSystem2DProject
{
    public class Game : WaveEngine.Framework.Game
    {
        public override void Initialize(IApplication application)
        {
            base.Initialize(application);

            WaveServices.ScreenContextManager.To(new ScreenContext(new MyScene())/*, new MyTransition(TimeSpan.FromSeconds(2))*/);
        }
    }

    public class MyTransition : ScreenContextTransition
    {
        private TimeSpan timeAcumulator;

        private TimeSpan duration;

        public MyTransition(TimeSpan duration)
            :base()
        {
            this.duration = duration;
        }

        protected override void Initialize()
        {
            this.timeAcumulator = TimeSpan.Zero;
        }

        public override void Update(TimeSpan gameTime)
        {
            this.timeAcumulator += gameTime;

            if(this.timeAcumulator > this.duration)
            {
                this.Finished = true;
            }
        }

        public override void Draw(TimeSpan gameTime)
        {
            double total = this.duration.TotalSeconds;
            double current = this.timeAcumulator.TotalSeconds;
            float lerp = (float)(current / total);
            Color c = Color.White * lerp;

            Vector2 pos = Vector2.UnitY * WaveServices.Platform.ScreenWidth * lerp;

            this.SpriteBatch.Begin(BlendMode.AlphaBlend, DepthMode.None);
            this.SpriteBatch.Draw(this.FromTexture, Vector2.Zero, null, Color.White, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, 1);
            this.SpriteBatch.Draw(this.ToTexture, pos, null, Color.White, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, 0);

            this.SpriteBatch.End();
        }
    }
}
