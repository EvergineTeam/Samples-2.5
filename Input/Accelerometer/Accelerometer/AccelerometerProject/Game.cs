#region Using Statements
using System;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;
#endregion

namespace AccelerometerProject
{
    public class Game : WaveEngine.Framework.Game
    {
        public override void Initialize(IApplication application)
        {
            base.Initialize(application);
            application.Adapter.SupportedOrientations = WaveEngine.Common.Input.DisplayOrientation.LandscapeRight;

            WaveServices.ScreenContextManager.To(new ScreenContext(new MainScene()));
        }
    }
}
