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
using StockRoom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics3D;
using WaveEngine.Framework.Services;
using WaveEngine.Materials;
#endregion

namespace StockRoomProject
{
    public class Helpers
    {
        /// <summary>
        /// Create a box
        /// </summary>
        public static Entity CreateBox(string name, Vector3 position, Vector3 scale, float mass, float friction)
        {
            Entity primitive = new Entity(name)
                .AddComponent(new Transform3D() { Position = position, Scale = scale })
                .AddComponent(new MaterialComponent() { MaterialPath = WaveContent.Assets.Material.CrateMat })
                .AddComponent(new CubeMesh())
                .AddComponent(new BoxCollider3D())
                .AddComponent(new RigidBody3D()
                {
                    EnableContinuousContact = true,
                    Mass = mass,
                    KineticFriction = friction,
                    StaticFriction = friction
                })
                .AddComponent(new MeshRenderer());

            return primitive;
        }

        public static Entity CreateSphere(string name, Vector3 position, Vector3 scale, float mass, Color color)
        {
            Entity primitive = new Entity(name)
               .AddComponent(new Transform3D() { Position = position, Scale = scale })
               .AddComponent(new MaterialsMap(new StandardMaterial(color, DefaultLayers.Opaque) { LightingEnabled = false }))
               .AddComponent(Model.CreateSphere())
               .AddComponent(new SphereCollider3D())
               .AddComponent(new RigidBody3D() { EnableContinuousContact = true, Mass = mass })
               .AddComponent(new ModelRenderer());

            return primitive;
        }

        public static Color RandomColor()
        {
            var random = WaveServices.Random;
            return new Color((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble(), 1f);
        }
    }
}
