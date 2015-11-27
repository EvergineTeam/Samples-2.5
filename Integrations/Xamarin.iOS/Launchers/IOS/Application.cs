using Foundation;
using UIKit;

namespace Sample
{
    [Register("AppDelegate")]
    public class Application : UIApplicationDelegate
    {
        private UIWindow window;
        private UINavigationController navigationController;

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            this.window = new UIWindow(UIScreen.MainScreen.Bounds);
            this.navigationController = new UINavigationController();
            this.window.RootViewController = this.navigationController;

            UIViewController viewController = UIStoryboard.FromName("Storyboard", null).InstantiateViewController("GameViewController");
            this.navigationController.PushViewController(viewController, true);

            this.window.MakeKeyAndVisible();

            return true;
        }
    }
}