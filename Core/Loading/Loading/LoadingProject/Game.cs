#region Using Statements
using System;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;
#endregion

namespace LoadingProject
{
    public class Game : WaveEngine.Framework.Game
    {
        public override void Initialize(IApplication application)
        {
            base.Initialize(application);

            WaveServices.ViewportManager.Activate(800, 480, ViewportManager.StretchMode.UniformToFill);

            ScreenContext screenContext = new ScreenContext(new Loading());
            WaveServices.ScreenContextManager.To(screenContext);
        }
    }
}
