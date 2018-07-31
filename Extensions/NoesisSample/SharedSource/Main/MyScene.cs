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
#endregion

namespace NoesisSample
{
    public class MyScene : Scene
    {
        protected override void CreateScene()
        {
            this.Load(WaveContent.Scenes.MyScene);

            // Remove the camera components  of all the path cameras
            var pathCameras = this.EntityManager.FindAllByTag("PathCamera");
            foreach (var cam in pathCameras)
            {
                if (cam is Entity)
                {
                    var camEntity = cam as Entity;
                    camEntity.RemoveComponent<Camera3D>();
                }
            }

            // The camera3D must be the default camera
            this.RenderManager.SetActiveCamera3D(this.EntityManager.Find("camera3D"));
        }
    }
}
