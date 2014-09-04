#region Using Statements
using System;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;
#endregion

namespace ParallaxCamera2DProject
{
    public class Game : WaveEngine.Framework.Game
    {
        public override void Initialize(IApplication application)
        {
            base.Initialize(application);

            WaveServices.ViewportManager.Activate(1280, 720, ViewportManager.StretchMode.UniformToFill);

            ScreenContext screenContext = new ScreenContext(new MyScene());
            WaveServices.ScreenContextManager.To(screenContext);
        }
    }
}
