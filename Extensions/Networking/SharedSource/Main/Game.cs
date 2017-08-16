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
            
            this.Load(WaveContent.GameInfo);
          
			ScreenContext screenContext = new ScreenContext(new MainScene());	
			WaveServices.ScreenContextManager.To(screenContext);
        }
    }
}
