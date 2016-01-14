#region Using Statements
using System;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;
#endregion

namespace Touch
{
    public class Game : WaveEngine.Framework.Game
    {
        public override void Initialize(IApplication application)
        {
            base.Initialize(application);

            ViewportManager vm = WaveServices.ViewportManager;
            vm.Activate(1280, 720, ViewportManager.StretchMode.UniformToFill);

            ScreenContext screenContext = new ScreenContext(new MyScene());	
			WaveServices.ScreenContextManager.To(screenContext);
        }
    }
}
