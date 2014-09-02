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
using System;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;
using WaveEngine.Components;
using WaveEngine.Common.Math;
using WaveEngine.Framework.Graphics;
using WaveEngine.Materials;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework.Physics3D;
using WaveEngine.Components.Cameras;
#endregion

namespace EnvironmentMapProject
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
            RenderManager.BackgroundColor = Color.Black;
            RenderManager.DebugLines = true;

            FreeCamera camera = new FreeCamera("MainCamera", new Vector3(0, 4, 12), new Vector3(0, 4, 0));
            camera.Speed = 5.0f;
            EntityManager.Add(camera);

            RenderManager.SetActiveCamera(camera.Entity);

            DirectionalLight skylight = new DirectionalLight("SkyLight", new Vector3(1));
            EntityManager.Add(skylight);

            float value = 0.0f;
            Color c = new Color(value, value, value, 1);

            Entity cubeModel = new Entity("Sphere")
                .AddComponent(new Spinner() { AxisTotalIncreases = new Vector3(0.3f, 0.5f,0.4f) })
                .AddComponent(new Transform3D() { Scale = new Vector3(6) })
                .AddComponent(Model.CreateSphere())
                .AddComponent(new MaterialsMap(new EnvironmentMapMaterial("Content/tile1.wpk", "Content/Sky.wpk") {LightingEnabled = true}))
                .AddComponent(new ModelRenderer());

            EntityManager.Add(cubeModel);
        }
    }
}
