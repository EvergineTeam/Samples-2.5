#region Using Statements
using System;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;
#endregion

namespace AnimationSequence
{
    public class Game : WaveEngine.Framework.Game
    {
        public override void Initialize(IApplication application)
        {
            base.Initialize(application);

			ScreenContext screenContext = new ScreenContext(new Scene2());	
			WaveServices.ScreenContextManager.To(screenContext);
        }
    }
}
