using Sample.Adapter;
using System;
using UIKit;

namespace Sample
{
	partial class GameViewController : GameApplication
    {
        private Game game;

        protected override GameView GLView
        {
            get
            {
                return this.GameView;
            }
        }

        public GameViewController (IntPtr handle) : base (handle)
        {
            this.FullScreen = true;
        }

        public override void Initialize()
        {
            this.game = new Game();
            this.game.Initialize(this);
        }

        public override void Update(TimeSpan elapsedTime)
        {
            this.game.UpdateFrame(elapsedTime);
        }

        public override void Draw(TimeSpan elapsedTime)
        {
            this.game.DrawFrame(elapsedTime);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.NavigationController.NavigationBarHidden = true;
        }

        partial void AutoRotateSwitchChanged(UISwitch sender)
        {
            this.game.UpdateAutoRotation(sender.On);
        }
    }
}
