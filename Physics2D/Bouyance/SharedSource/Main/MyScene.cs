#region Using Statements
using System;
using System.Collections.Generic;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Common.Physics2D;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics2D;
using WaveEngine.Framework.Resources;
using WaveEngine.Framework.Services;
#endregion

namespace Bouyance
{
    public class MyScene : Scene
    {       
        protected override void CreateScene()
        {
            //WaveServices.ScreenContextManager.SetDiagnosticsActive(true);
            //this.RenderManager.DebugLines = true;
            this.Load(WaveContent.Scenes.MyScene);
      
        }      
    }
}
