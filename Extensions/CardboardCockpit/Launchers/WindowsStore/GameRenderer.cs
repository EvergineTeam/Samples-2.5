using System;
using WaveEngine.Adapter;
using WaveEngine.Adapter.CommonDX;
using WaveEngine.Common.Input;
using Windows.UI.Xaml.Controls;

namespace CardboardCockpit
{
    public class GameRenderer : Application
    {
        public int Width { get; set; }
        public int Height { get; set; }

        private CardboardCockpit.Game game;

        public GameRenderer(SwapChainBackgroundPanel panel)
            : base(panel)
        {
        }

        public override void Update(TimeSpan gameTime)
        {
            game.UpdateFrame(gameTime);
        }

        public override void Draw(TimeSpan gameTime)
        {
            game.DrawFrame(gameTime);
        }

        public override void Initialize()
        {
            base.Initialize();

            this.Adapter.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;

            game = new CardboardCockpit.Game();
            game.Initialize(this);
        }

        public override void OnResuming()
        {
            base.OnResuming();

            game.OnActivated();
        }

        public override void OnSuspending()
        {
            base.OnSuspending();

            game.OnDeactivated();
        }        
    }
}
