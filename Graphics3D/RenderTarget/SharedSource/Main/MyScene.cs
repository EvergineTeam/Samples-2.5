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
using WaveEngine.Materials;
#endregion

namespace RenderTarget
{
    public class MyScene : Scene
    {
        protected override void CreateScene()
        {
            this.Load(WaveContent.Scenes.MyScene);
            
            // First, retrieve the RenderTargetScene from the current context.
            var renderTargetScene = WaveServices.ScreenContextManager.CurrentContext.FindScene<RenderTargetScene>();

            // Creates a Material that uses the RenderTarget as diffuse texture.
            var renderTargetMaterial = new StandardMaterial(DefaultLayers.Opaque, renderTargetScene.SmallTarget);

            // Finally, retrieve the cube from the current scene, and set the marial.
            var cube = this.EntityManager.Find("cube");
            cube.FindComponent<MaterialsMap>().DefaultMaterial = renderTargetMaterial;
        }
    }
}
