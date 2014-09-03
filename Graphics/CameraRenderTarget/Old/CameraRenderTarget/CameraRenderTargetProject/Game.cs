#region Using Statements
using System;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;
#endregion

namespace CameraRenderTargetProject
{
    public class Game : WaveEngine.Framework.Game
    {
        public override void Initialize(IApplication application)
        {
            base.Initialize(application);

            WaveServices.ViewportManager.Activate(800, 600, ViewportManager.StretchMode.Uniform);

            WaveServices.ScreenContextManager.To(new ScreenContext(new MyScene()));
        }
    }
}
