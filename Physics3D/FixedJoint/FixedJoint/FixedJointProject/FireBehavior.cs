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
using System.Collections.Generic;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
using WaveEngine.Materials;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework.Physics3D;

#endregion

namespace FixedJointProject
{
    public class FireBehavior : Behavior
    {
        private bool pressed;
        Entity sphere;

        [RequiredComponent]
        public Camera Camera;

        public FireBehavior()
            : base("FireBehavior")
        {
            Camera = null;
        }

        protected override void Update(TimeSpan gameTime)
        {
            var touches = WaveServices.Input.TouchPanelState;

            if (touches.Count > 0)
            {
                if (!pressed)
                {
                    pressed = true;

                    if (sphere == null)
                    {
                        sphere = new Entity("ball")
                           .AddComponent(new Transform3D() { Scale = new Vector3(1) })
                           .AddComponent(new MaterialsMap(new BasicMaterial(Color.Red) ))
                           .AddComponent(Model.CreateSphere())
                           .AddComponent(new SphereCollider())
                           .AddComponent(new RigidBody3D() { Mass = 2, EnableContinuousContact = true})
                           .AddComponent(new ModelRenderer());

                        EntityManager.Add(sphere);
                    }

                    RigidBody3D rigidBody = sphere.FindComponent<RigidBody3D>();
                    rigidBody.ResetPosition(Camera.Position);
                    var direction = Camera.LookAt - Camera.Position;
                    direction.Normalize();
                    rigidBody.ApplyLinearImpulse(100 * direction);
                }
            }
            else
            {
                pressed = false;
            }
        }
    }
}
