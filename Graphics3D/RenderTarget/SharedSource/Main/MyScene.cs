#region Using Statements
using WaveEngine.Framework;
using WaveEngine.Framework.Models;
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

            // Updates the cube Material to use the RenderTarget as diffuse texture.
            var materialModel = this.Assets.LoadModel<MaterialModel>(WaveContent.Assets.RenderTargetMaterial);
            var renderTargetMaterial = materialModel.Material as StandardMaterial;

            renderTargetMaterial.Diffuse1 = renderTargetScene.SmallTarget;
        }
    }
}
