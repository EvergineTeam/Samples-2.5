#region Using Statements
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Gestures;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Components.UI;
using WaveEngine.Framework;
using WaveEngine.Framework.Diagnostic;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics2D;
using WaveEngine.Framework.Services;
using WaveEngine.Framework.UI;
#endregion

namespace InternetTextureLoad
{
    public class MyScene : Scene
    {
        protected override void CreateScene()
        {
            this.Load("Content/Scenes/MyScene.wscene");
        }
    }
}
