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

namespace CustomMaterial
{
    public class MyScene : Scene
    {
        protected override void CreateScene()
        {
            this.Load(WaveContent.Scenes.MyScene);

            Entity testShape = new Entity("TestShape")
                   .AddComponent(new Transform3D())
                   .AddComponent(Model.CreateSphere(5, 32))
                   .AddComponent(new MaterialsMap(new MyMaterial(WaveContent.Assets.DefaultTexture_png)))
                   .AddComponent(new ModelRenderer());

            this.EntityManager.Add(testShape);
        }
    }
}
