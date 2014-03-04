#region Using Statements
using System;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;
#endregion

namespace FrustumCullingProject
{
    public class Game : WaveEngine.Framework.Game
    {
        public override void Initialize(IApplication application)
        {
            base.Initialize(application);

            WaveServices.ScreenContextManager.To(new ScreenContext(new MyScene()));
        }
    }
}
