#region Using Statements
using System;
using System.Linq;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Models;
using WaveEngine.Framework.Resources;
using WaveEngine.Framework.Services;
using WaveEngine.NoesisGUI;
using static NoesisWPF.MainHUD;
#endregion

namespace NoesisSample
{
    public class MyScene : Scene
    {
        protected override void CreateScene()
        {
            this.Load(WaveContent.Scenes.MyScene);

            this.RenderManager.RegisterLayerBefore(new StarsLayer(this.RenderManager), DefaultLayers.Alpha);

            this.Assets.LoadModel<MaterialModel>(WaveContent.Assets.Materials.Stars).Material.LayerType = typeof(StarsLayer);

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
            this.RenderManager.SetActiveCamera3D(this.EntityManager.Find("camera3D"));
        }
    }
}
