namespace Xamarin_iOS
{
    using System;
    using System.CodeDom.Compiler;
    using Foundation;
    using UIKit;
    using WaveEngine.Adapter;

    public partial class MainController : WaveEngine.Adapter.Application
    {
        private Xamarin_iOS.Game game;

        public MainController(IntPtr handle)
            : base(handle)
        {
        }

        public override void Initialize()
        {
            this.game = new Xamarin_iOS.Game();
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

            if (this.game != null)
            {
                this.game.OnActivated();
            }
        }

        public override void ViewWillUnload()
        {
            base.ViewWillUnload();

            this.game.OnDeactivated();
        }

        private static void IncludeAssemblies()
        {
            // Include WaveEngine Components
            var a1 = typeof(WaveEngine.Components.Cameras.FreeCamera3D);
        }

		partial void AutoRotateSwitchChanged (UISwitch sender)
		{
			this.game.UpdateAutoRotation(sender.On);
		}
    }
}
