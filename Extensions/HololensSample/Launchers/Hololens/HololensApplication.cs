using System;
using WaveEngine.Hololens;

namespace HololensSample
{
    internal class HololensApplication : BaseHololensApplication
    {
        private HololensSample.Game game;

        public HololensApplication() : base()
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            this.game = new HololensSample.Game();
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
