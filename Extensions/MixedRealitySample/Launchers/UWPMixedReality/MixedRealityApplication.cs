using System;
using WaveEngine.MixedReality;

namespace MixedRealitySample
{
    internal class MixedRealityApplication : BaseMixedRealityApplication
{
        private MixedRealitySample.Game game;

        public MixedRealityApplication() : base()
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            this.game = new MixedRealitySample.Game();
            this.game.Initialize(this);
        }

        public override void Update(TimeSpan gameTime)
        {
            this.game.UpdateFrame(gameTime);
        }

        public override void Draw(TimeSpan gameTime)
        {
            this.game.DrawFrame(gameTime);
        }

        public override void OnResuming()
        {
            base.OnResuming();

            this.game.OnActivated();
        }

        public override void OnSuspending()
        {
            base.OnSuspending();

            this.game.OnDeactivated();
        }
    }
}
