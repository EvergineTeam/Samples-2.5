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

#region Usings Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
using WaveEngine.Materials;
#endregion

namespace RimLightingProject
{
    public class BasicScene : Scene
    {
        protected override void CreateScene()
        {
            RenderManager.BackgroundColor = Color.CornflowerBlue;

            FreeCamera camera = new FreeCamera("MainCamera", new Vector3(0, 20, -30), Vector3.Zero);
            EntityManager.Add(camera.Entity);
            RenderManager.SetActiveCamera(camera.Entity);

            Entity box = new Entity("box")
               .AddComponent(new Transform3D() { Position = new Vector3(-10, 0, 0), Scale = new Vector3(5) })
               .AddComponent(new Spinner() { AxisTotalIncreases = new Vector3(1, 2, 1) })
               .AddComponent(Model.CreateCube())
               .AddComponent(new MaterialsMap(new RimLightMaterial("Content/BoxTexture.wpk")))
               .AddComponent(new ModelRenderer());

            EntityManager.Add(box);

            Entity Sphere = new Entity("sphere")
               .AddComponent(new Transform3D() { Position = new Vector3(0, 0, 5), Scale = new Vector3(5) })
               .AddComponent(new Spinner() { AxisTotalIncreases = new Vector3(1, 1, 2) })
               .AddComponent(Model.CreateSphere(1, 16))
               .AddComponent(new MaterialsMap(new RimLightMaterial("Content/SphereTexture.wpk")))
               .AddComponent(new ModelRenderer());

            EntityManager.Add(Sphere);

            Entity torus = new Entity("torus")
                .AddComponent(new Transform3D() { Position = new Vector3(10, 0, 0), Scale = new Vector3(5) })
                .AddComponent(new Spinner() { AxisTotalIncreases = new Vector3(2, 1, 1) })
                .AddComponent(Model.CreateTorus(1, 0.333f, 24))
                .AddComponent(new MaterialsMap(new RimLightMaterial() { DiffuseColor = Color.Purple }))
                .AddComponent(new ModelRenderer());

            EntityManager.Add(torus);
        }
    }
}
