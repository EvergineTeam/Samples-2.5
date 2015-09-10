#region Using Statements
using Networking.Scenes;
using WaveEngine.Common;
using WaveEngine.Framework.Services;
using WaveEngine.Networking;
#endregion

namespace Networking
{
    public class Game : WaveEngine.Framework.Game
    {
        public override void Initialize(IApplication application)
        {
            base.Initialize(application);

            this.RegisterServices();

            ViewportManager vm = WaveServices.ViewportManager;
            vm.Activate(1280, 720, ViewportManager.StretchMode.Uniform);
			ScreenContext screenContext = new ScreenContext(new MainScene());	
			WaveServices.ScreenContextManager.To(screenContext);
        }

        private void RegisterServices()
        {
            WaveServices.RegisterService(new NetworkService());
        }
    }
}
