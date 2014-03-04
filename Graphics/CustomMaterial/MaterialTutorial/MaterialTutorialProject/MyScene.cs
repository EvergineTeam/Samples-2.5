#region Using Statements
using System;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
using WaveEngine.Materials;
#endregion

namespace MaterialTutorialProject
{
    public class MyScene : Scene
    {
        protected override void CreateScene()
        {
            RenderManager.BackgroundColor = Color.CornflowerBlue;

            FreeCamera mainCamera = new FreeCamera("Camera", new Vector3(0, 0, 10), Vector3.Zero);
            this.EntityManager.Add(mainCamera);

            this.RenderManager.SetActiveCamera(mainCamera.Entity);

            Entity testShape = new Entity("TestShape")
                    .AddComponent(new Transform3D())
                    .AddComponent(Model.CreateSphere(5, 32))
                    .AddComponent(new MaterialsMap(new MyMaterial("Content/DefaultTexture.wpk")))
                    .AddComponent(new ModelRenderer());

            this.EntityManager.Add(testShape);
        }
    }
}
