#region Using Statements
using System;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Resources;
using WaveEngine.Framework.Services;
using WaveEngine.Hololens.Interaction;
using WaveEngine.Hololens.Speech;
#endregion

namespace HololensSample
{
    public class MyScene : Scene
    {
        protected override void CreateScene()
        {
            this.Load(WaveContent.Scenes.MyScene);

#if !UWP
            this.EntityManager.Remove("CameraRig");

            FreeCamera3D mainCamera = new FreeCamera3D("main", Vector3.Zero, -Vector3.UnitZ);
            this.EntityManager.Add(mainCamera);
#endif
        }
    }
}
