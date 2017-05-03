using System;
using WaveEngine.Adapter;
using WaveEngine.Common;
using WaveEngine.Common.Input;
using Windows.System.Display;
using Windows.UI.Xaml.Controls;

namespace XamarinFormsProfileSample
{
    public class WaveApplication : Application, IApplication
    {
        private DisplayRequest _displayRequest;
        private SwapChainPanel _swapChainPanel;
        private IGame _game;

        public override bool FullScreen { get; set; }

        public WaveApplication(SwapChainPanel panel)
            : base(panel)
        {
            _swapChainPanel = panel;
            this.FullScreen = true;
        }

        public override void Update(TimeSpan gameTime)
        {
            _game.UpdateFrame(gameTime);
        }

        public override void Draw(TimeSpan gameTime)
        {
            _game.DrawFrame(gameTime);
        }

        public override void Initialize()
        {
            base.Initialize();

            this.Adapter.SupportedOrientations = DisplayOrientation.Portrait | DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;

            _displayRequest = new DisplayRequest();
            _displayRequest.RequestActive();
        }

        public void Initialize(IGame theGame)
        {
            _game = theGame;
            Initialize();
            _game.Initialize(this);
        }

        public override void OnResuming()
        {
            base.OnResuming();

            _game.OnActivated();
        }

        public override void OnSuspending()
        {
            base.OnSuspending();

            _game.OnDeactivated();
        }
    }
}