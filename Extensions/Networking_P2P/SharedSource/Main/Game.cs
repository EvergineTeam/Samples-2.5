#region Using Statements
using Networking_P2P.Scenes;
using System;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;
#endregion

namespace Networking_P2P
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
