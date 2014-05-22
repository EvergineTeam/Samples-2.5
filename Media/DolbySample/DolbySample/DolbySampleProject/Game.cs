#region Using Statements
using System;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;
#endregion

namespace DolbySampleProject
{
    public class Game : WaveEngine.Framework.Game
    {
        public override void Initialize(IApplication application)
        {
            base.Initialize(application);

            // Use ViewportManager to ensure scaling in all devices
            WaveServices.ViewportManager.Activate(1024, 576, ViewportManager.StretchMode.Uniform);

            ScreenContext screenContext = new ScreenContext(new BackgroundScene(), new MainScene());
            WaveServices.ScreenContextManager.To(screenContext);
        }
    }
}
