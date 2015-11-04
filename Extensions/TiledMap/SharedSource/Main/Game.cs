#region Using Statements
using System;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;
#endregion

namespace TiledMap
{
    public class Game : WaveEngine.Framework.Game
    {
        public override void Initialize(IApplication application)
        {
            base.Initialize(application);

            // ViewportManager is used to automatically adapt resolution to fit screen size
            ViewportManager vm = WaveServices.ViewportManager;
            vm.Activate(1280, 720, ViewportManager.StretchMode.UniformToFill);

            ScreenContext screenContext = new ScreenContext(new MyScene(), new JoystickScene());
            WaveServices.ScreenContextManager.To(screenContext);
        }
    }
}
