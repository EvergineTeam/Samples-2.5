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

namespace CustomMaterialProject
{
    public class MyScene : Scene
    {
        protected override void CreateScene()
        {            
            FreeCamera mainCamera = new FreeCamera("Camera", new Vector3(0, 0, 10), Vector3.Zero);
            mainCamera.BackgroundColor = Color.CornflowerBlue;
            this.EntityManager.Add(mainCamera);            

            Entity testShape = new Entity("TestShape")
                    .AddComponent(new Transform3D())
                    .AddComponent(Model.CreateSphere(5, 32))
                    .AddComponent(new MaterialsMap(new MyMaterial("Content/DefaultTexture.wpk")))
                    .AddComponent(new ModelRenderer());

            this.EntityManager.Add(testShape);
        }
    }
}
