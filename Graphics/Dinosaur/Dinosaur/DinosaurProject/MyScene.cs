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

namespace DinosaurProject
{
    public class MyScene : Scene
    {
        protected override void CreateScene()
        {            
            FreeCamera mainCamera = new FreeCamera("MainCamera", new Vector3(0, 50, 80), Vector3.Zero);
            mainCamera.BackgroundColor = Color.Black;
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
