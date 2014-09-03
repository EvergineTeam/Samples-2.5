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
using WaveEngine.Framework.Physics3D;
using WaveEngine.Framework.Resources;
using WaveEngine.Framework.Services;
using WaveEngine.Materials;
#endregion

namespace SkyboxProject
{
    public class MyScene : Scene
    {
        protected override void CreateScene()
        {            
            RenderManager.RegisterLayerBefore(new SkyLayer(this.RenderManager), DefaultLayers.Alpha);

            FreeCamera camera = new FreeCamera("MainCamera", new Vector3(5, 0, 0), new Vector3(-1, 0, 0))
            {
                Speed = 5.0f,
                NearPlane = 0.1f,
                BackgroundColor = Color.CornflowerBlue,
            };            
            EntityManager.Add(camera);

            Entity teapot = new Entity("Teapot")
                .AddComponent(new Transform3D())
                .AddComponent(new PointLightProperties() { Attenuation = 1, Falloff = 0.1f })
                .AddComponent(Model.CreateSphere())
                .AddComponent(new MaterialsMap(new EnvironmentMapMaterial("Content/DefaultTexture.wpk", "Content/Sky.wpk") { AmbientLightColor = Color.White }))
                .AddComponent(new ModelRenderer());

            EntityManager.Add(teapot);

            Entity skybox = new Entity("Skybox")
                .AddComponent(new Transform3D() { Scale = new Vector3(-1) })
                .AddComponent(new SkyboxBehavior())
                .AddComponent(new BoxCollider())
                .AddComponent(Model.CreateCube(1))
                .AddComponent(new MaterialsMap(new SkyboxMaterial("Content/Sky.wpk", typeof(SkyLayer))))
                .AddComponent(new ModelRenderer());

            EntityManager.Add(skybox);
        }      
    }
}
