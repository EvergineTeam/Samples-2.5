// Copyright (C) 2012-2013 Weekend Game Studio
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to
// deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
// sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.

#region Using Statements
using CityDemoProject.Behaviors;
using System;
using System.Collections.Generic;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Components.Particles;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
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
            FreeCamera cameraEntity = new FreeCamera("camera", new Vector3(-152, 180, 1000), new Vector3(0, 180, 0))
            {
                NearPlane = 10,
                FarPlane = 65000,
                FieldOfView = MathHelper.ToRadians(65),
                Speed = 300
            };
            cameraEntity.Entity.AddComponent(new CameraBehavior());
            EntityManager.Add(cameraEntity);

            RenderManager.SetActiveCamera(cameraEntity.Entity);
            RenderManager.BackgroundColor = Color.Black;
            WaveServices.GraphicsDevice.RenderState.BlendMode = BlendMode.AlphaBlend;


            this.LoadModel("buildingModels", "buildings");
            this.LoadModel("buildingModels2", "buildings2");
            this.LoadModel("skyModel", "Sky");
            this.LoadModel("cityBackground", "skylineAlpha");
            this.LoadModel("balcony", "balconyMapRadiosity");
            Entity lightFoot = this.LoadModel("lightFoot", "lightFootMap");
            lightFoot.AddComponent(new RotationBehavior(false, true));
            lightFoot.FindComponent<Transform3D>().Position = new Vector3(36, 22, 549);
            Entity light = this.LoadModel("light", "lightMap", lightFoot);
            light.FindComponent<Transform3D>().Position = new Vector3(35, 207, 549);
            light.AddComponent(new RotationBehavior(true, true));
            Entity lightSignal = this.LoadModel("lightSignal", "WaveLight", lightFoot, true);
            lightSignal.FindComponent<Transform3D>().Position = new Vector3(35, 207, 549);
            lightSignal.AddComponent(new RotationBehavior(true, true));
            Camera cam = cameraEntity.Entity.FindComponent<Camera>();
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
                entity.AddComponent(new MaterialsMap(new BasicMaterial(string.Format(PATHMATERIAL, texture), DefaultLayers.Opaque)));
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
