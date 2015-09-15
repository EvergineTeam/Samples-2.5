#region Using Statements

using WaveEngine.Common;
using WaveEngine.Framework.Services;

#endregion

namespace SocialSampleProject
{
    public class Game : WaveEngine.Framework.Game
    {
        public override void Initialize(IApplication application)
        {
            base.Initialize(application);

            // ViewportManager is used to automatically adapt resolution to fit screen size
            ViewportManager vm = WaveServices.ViewportManager;
            vm.Activate(1280, 720, ViewportManager.StretchMode.Uniform);

            ScreenContext screenContext = new ScreenContext(new MyScene());
            WaveServices.ScreenContextManager.To(screenContext);
        }
    }
}
