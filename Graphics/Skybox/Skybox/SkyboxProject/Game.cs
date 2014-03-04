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
//
// Skybox texture (Sky.wpk) made by Emil 'Humus' Persson (http://www.humus.name).
// Licensed under Creative Commons Attribution 3.0 Unported License.

#region Using Statements
using System;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics3D;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Materials;
using WaveEngine.Common.Math;
using WaveEngine.Components.Cameras;
#endregion

namespace SkyboxProject
{
    public class Game : WaveEngine.Framework.Game
    {
        public override void Initialize(IApplication application)
        {
            base.Initialize(application);

            WaveServices.ScreenContextManager.To(new ScreenContext(new MyScene()));
        }
    }

    public class MyScene : Scene
    {
        protected override void CreateScene()
        {
            RenderManager.BackgroundColor = Color.CornflowerBlue;
            RenderManager.RegisterLayerBefore(new SkyLayer(this.RenderManager), DefaultLayers.Alpha);

            FreeCamera camera = new FreeCamera("MainCamera", new Vector3(5, 0, 0), new Vector3(-1, 0, 0));
            camera.Speed = 5.0f;
            camera.NearPlane = 0.1f;
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

        protected override void Draw(TimeSpan gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
