// Copyright (C) 2014 Weekend Game Studio
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
using System.Collections.Generic;
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

namespace DualTextureProject
{
    public class MyScene : Scene
    {
        protected override void CreateScene()
        {            
            FreeCamera mainCamera = new FreeCamera("MainCamera", new Vector3(-15, 8, 18), Vector3.Zero)
            {
                Speed = 20f,
            };
            mainCamera.BackgroundColor = Color.CornflowerBlue;
            EntityManager.Add(mainCamera.Entity);            

            Entity river = new Entity("River")
                .AddComponent(new Transform3D())
                .AddComponent(new ModelRenderer())
                .AddComponent(new Model("Content/River.wpk"))
                .AddComponent(new WaterAnimationBehavior())
                .AddComponent(new MaterialsMap(new Dictionary<string, Material>()
                    {
                        { "GroundMesh", new DualTextureMaterial("Content/GroundTexture.wpk", "Content/GrassTexture.wpk", DefaultLayers.Opaque)
                                   {
                                       SamplerMode = AddressMode.LinearWrap,
                                       EffectMode = DualTextureMode.Mask
                                   }
                        },
                        { "RiverMesh", new DualTextureMaterial("Content/RiverWater.wpk", "Content/RiverLeafs.wpk", DefaultLayers.Opaque)
                                   {
                                       SamplerMode = AddressMode.LinearWrap,
                                       EffectMode = DualTextureMode.Mask
                                   }
                        },
                    }));

            EntityManager.Add(river);
        }

        protected override void Start()
        {
            base.Start();

            Model m = EntityManager.Find("River").FindComponent<Model>();
        }
    }
}
