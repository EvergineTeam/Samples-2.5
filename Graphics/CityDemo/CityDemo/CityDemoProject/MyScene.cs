#region Using Statements
using CityDemoProject.Behaviors;
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

namespace CityDemoProject
{
    public class MyScene : Scene
    {
        private const string PATHMATERIAL = "Content/Textures/{0}.wpk";
        private const string PATHMODELS = "Content/Models/{0}.wpk";

        protected override void CreateScene()
        {
            this.RenderManager.RegisterLayerBefore(new CustomLayer(this.RenderManager), DefaultLayers.Opaque);

            FreeCamera cameraEntity = new FreeCamera("camera", new Vector3(-152, 180, 1000), new Vector3(0, 180, 0))
            {
                NearPlane = 10,
                FarPlane = 65000,
                FieldOfView = MathHelper.ToRadians(65),
                Speed = 300
            };
            cameraEntity.BackgroundColor = Color.Black;            
            cameraEntity.Entity.AddComponent(new CameraBehavior());
            EntityManager.Add(cameraEntity);                                               

            this.LoadModel("buildingModels", "buildings");
            this.LoadModel("buildingModels2", "buildings2");
            this.LoadModel("skyModel", "Sky");
            this.LoadModel("cityBackground", "skylineAlpha");
            this.LoadModel("balcony", "balconyMapRadiosity");
            Entity lightFoot = this.LoadModel("lightFoot", "lightFootMap");
            lightFoot.AddComponent(new RotationBehavior(false, true));
            lightFoot.FindComponent<Transform3D>().Position = new Vector3(36, 22, 549);
            Entity light = this.LoadModel("light", "lightMap", lightFoot);
            light.FindComponent<Transform3D>().Position = new Vector3(-1, 185, 0);
            light.AddComponent(new RotationBehavior(true, true));
            Entity lightSignal = this.LoadModel("lightSignal", "WaveLight", lightFoot, true);
            lightSignal.FindComponent<Transform3D>().Position = new Vector3(-1, 185, 0);
            lightSignal.AddComponent(new RotationBehavior(true, true));
        }

        private Entity LoadModel(string model, string texture)
        {
            return LoadModel(model, texture, null, false);
        }

        private Entity LoadModel(string model, string texture, Entity parent)
        {
            return LoadModel(model, texture, parent, false);
        }

        private Entity LoadModel(string model, string texture, Entity parent, bool isAdditive)
        {
            Entity entity = new Entity(model)
                    .AddComponent(new Model(string.Format(PATHMODELS, model)))
                    .AddComponent(new Transform3D())
                    .AddComponent(new ModelRenderer());

            if (!isAdditive)
            {
                entity.AddComponent(new MaterialsMap(new BasicMaterial(string.Format(PATHMATERIAL, texture), typeof(CustomLayer) )));
            }
            else
            {
                entity.AddComponent(new MaterialsMap(new BasicMaterial(string.Format(PATHMATERIAL, texture), DefaultLayers.Additive)
                {
                    DiffuseColor = new Color(0.6f, 0.6f, 0.6f)
                }));
            }

            if (parent == null)
            {
                EntityManager.Add(entity);
            }
            else
            {
                parent.AddChild(entity);
            }

            return entity;
        }
    }
}
