#region Using Statements
using Networking_ClientServer.Scenes;
using WaveEngine.Common;
using WaveEngine.Framework.Services;
#endregion

namespace Networking_ClientServer
{
    public class Game : WaveEngine.Framework.Game
    {
        public override void Initialize(IApplication application)
        {
            base.Initialize(application);

            this.Load(WaveContent.GameInfo);

            ScreenContext screenContext = new ScreenContext(new MainScene());
            WaveServices.ScreenContextManager.To(screenContext);
        }
    }
}
