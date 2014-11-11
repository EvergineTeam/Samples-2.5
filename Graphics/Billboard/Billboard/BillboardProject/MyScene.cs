#region Using Statements
using System;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Components.Shared.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Resources;
using WaveEngine.Framework.Services;
using WaveEngine.Materials;
#endregion

namespace BillboardProject
{
    public class MyScene : Scene
    {
        protected override void CreateScene()
        {
            //Create a 3D camera
            var camera3D = new FreeCamera("Camera3D", new Vector3(0, 2, 4), Vector3.Zero) 
            { 
                BackgroundColor = Color.CornflowerBlue, 
                Speed = 2,
                NearPlane = 0.1f
            };
            EntityManager.Add(camera3D); 

            // Ground
            Entity cube = new Entity()
                .AddComponent(new Transform3D())
                .AddComponent(Model.CreatePlane())
                .AddComponent(new MaterialsMap(new BasicMaterial("Content/ground.png") { SamplerMode = AddressMode.PointClamp }))
                .AddComponent(new ModelRenderer());

            EntityManager.Add(cube);

            // Tree Billboard 
            Entity tree = new Entity()
                .AddComponent(new Transform3D()
                {
                    LocalPosition = new Vector3(0, 0.75f, 0),
                    LocalScale = new Vector3(1.5f, 1.5f, 1.5f)
                })
                .AddComponent(new Billboard("Content/tree.png") { BillboardType = BillboardType.Axial_Orientation })
                .AddComponent(new BillboardRenderer(DefaultLayers.Alpha, AddressMode.PointClamp));

            EntityManager.Add(tree);


            // Flares
            int nflares = 6;

            for (int i = 0; i < nflares; i++)
            {
                float angle = ((float)i / 6) * MathHelper.TwoPi;

                Entity flare = new Entity()
                    .AddComponent(new Transform3D()
                    {
                        LocalPosition = new Vector3(0, 0.75f, 0),
                        LocalScale = Vector3.One * 0.25f
                    })
                    .AddComponent(new FlareBehavior(angle, 1))
                    .AddComponent(new Billboard("Content/flare.png") { BillboardType = BillboardType.PointOrientation })
                    .AddComponent(new BillboardRenderer(DefaultLayers.Alpha, AddressMode.PointClamp)); 

                EntityManager.Add(flare);
            }
        }

        protected override void Start()
        {
            base.Start();

            // This method is called after the CreateScene and Initialize methods and before the first Update.
        }
    }
}
