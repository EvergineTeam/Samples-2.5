using System;
using Foundation;
using UIKit;
using WaveEngine.Common;

namespace Sample.Adapter
{
    public abstract class GameApplication : UIViewController, IApplication
    {
        public GameApplication(IntPtr handle)
            : base(handle)
        {
            this.ModalTransitionStyle = UIModalTransitionStyle.CoverVertical;
            this.ModalPresentationStyle = UIModalPresentationStyle.CurrentContext;
        }

        protected abstract GameView GLView { get; }

        public IAdapter Adapter
        {
            get
            {
                return this.GLView.Adapter;
            }
        }

        public string WindowTitle
        {
            get
            {
                return string.Empty;
            }
        }

        public int Width
        {
            get
            {
                return this.Adapter.Width;
            }
        }

        public int Height
        {
            get
            {
                return this.Adapter.Height;
            }
        }

        public bool FullScreen
        {
            get
            {
                return UIApplication.SharedApplication.StatusBarHidden;
            }

            set
            {
                UIApplication.SharedApplication.SetStatusBarHidden(value, false);
            }
        }

        public override void DidRotate(UIInterfaceOrientation fromInterfaceOrientation)
        {
            UIView.AnimationsEnabled = true;
            base.DidRotate(fromInterfaceOrientation);
        }

        public override bool ShouldAutorotateToInterfaceOrientation(UIInterfaceOrientation toInterfaceOrientation)
        {
            return true;
        }

        public override void ViewDidLoad()
        {
            this.GLView.Application = this;
            base.ViewDidLoad();

            this.View.ContentScaleFactor = UIScreen.MainScreen.Scale;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            NSNotificationCenter.DefaultCenter.RemoveObserver(this);
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();

            GC.Collect();
        }

        public override void ViewDidAppear(bool animated)
        {
            this.GLView.Activate();
            base.ViewDidAppear(animated);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            this.GLView.Deactivate();
        }

        public abstract void Initialize();

        public abstract void Update(TimeSpan elapsedTime);

        public abstract void Draw(TimeSpan elapsedTime);

        public void Exit()
        {
        }
    }
}
