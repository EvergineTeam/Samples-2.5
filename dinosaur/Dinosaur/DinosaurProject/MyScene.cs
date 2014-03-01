#region Using Statements
using System;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics3D;
using WaveEngine.Framework.Services;
using WaveEngine.Materials;
#endregion

namespace DinosaurProject
{
    public class MyScene : Scene
    {
        protected override void CreateScene()
        {
            RenderManager.BackgroundColor = Color.Black;
            //RenderManager.DebugLines = true;

            FreeCamera mainCamera = new FreeCamera("MainCamera", new Vector3(0, 50, 80), Vector3.Zero);
            EntityManager.Add(mainCamera);

            Entity velociraptor = new Entity("Velociraptor")
                        .AddComponent(new Transform3D())
                        .AddComponent(new BoxCollider())
                        .AddComponent(new Model("Content/Models/velociraptor.wpk"))
                        .AddComponent(new MaterialsMap(new BasicMaterial("Content/Textures/VelociraptorTexture3.wpk") { ReferenceAlpha = 0.5f }))
                        .AddComponent(new ModelRenderer());

            EntityManager.Add(velociraptor);

            //velociraptor.IsVisible = false;

            Entity floor = new Entity("Floor")
                       .AddComponent(new Transform3D())
                       .AddComponent(new BoxCollider())
                       .AddComponent(new Model("Content/Models/floor.wpk"))
                       .AddComponent(new MaterialsMap(new BasicMaterial("Content/Textures/floorNight.wpk")))
                       .AddComponent(new ModelRenderer());


            EntityManager.Add(floor);

            Entity fern = new Entity("Fern")
                     .AddComponent(new Transform3D() { Position = new Vector3(0, -8, 0) })
                     .AddComponent(new BoxCollider())
                     .AddComponent(new Model("Content/Models/fern.wpk"))
                     .AddComponent(new MaterialsMap(new BasicMaterial("Content/Textures/FernTexture.wpk") { ReferenceAlpha = 0.5f }))
                     .AddComponent(new ModelRenderer());

            EntityManager.Add(fern);
        }
    }
}
