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

#region File Description
//-----------------------------------------------------------------------------
// FireBehavior
//
// Copyright © 2012 Weekend Game Studio. All rights reserved.
// Use is subject to license terms.
//-----------------------------------------------------------------------------
#endregion

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

namespace VuforiaProject
{
    public class FireBehavior : Behavior
    {
        private bool pressed;
        private Material ballMaterial;
        private Entity ball;
        private Physic3DCollisionGroup collisionGroup;

        [RequiredComponent(false)]
        private Camera3D camera = null;

        public FireBehavior(Physic3DCollisionGroup collisionGroup)
            : base("FireBehavior")
        {
            this.collisionGroup = collisionGroup;
            this.ballMaterial = new NormalMappingMaterial("Content/Textures/ball_diffuse.png", "Content/Textures/ball_normal_spec.png")
                {
                    AmbientColor = Color.White * 0.4f
                };
        }

        protected override void Update(TimeSpan gameTime)
        {
            var touches = WaveServices.Input.TouchPanelState;

            if (touches.Count > 0)
            {
                if (!pressed)
                {
                    pressed = true;

                    RigidBody3D rigidBody;

                    if (this.ball == null || this.ball.IsDisposed)
                    {
                        this.ball = new Entity() { Tag = "Removable" }
                               .AddComponent(new Transform3D() { Scale = new Vector3(0.25f) })
                               .AddComponent(new MaterialsMap(this.ballMaterial))
                               .AddComponent(new Model("Content/Models/ball.FBX"))
                               .AddComponent(new SphereCollider())
                               .AddComponent(rigidBody = new RigidBody3D()
                               {
                                   Mass = 2,
                                   EnableContinuousContact = true,
                                   CollisionGroup = this.collisionGroup
                               })
                               .AddComponent(new ModelRenderer());

                        EntityManager.Add(this.ball);
                    }
                    else
                    {
                        rigidBody = this.ball.FindComponent<RigidBody3D>();
                    }

                    var position = camera.Position;
                    var direction = camera.LookAt - camera.Position;
                    direction.Normalize();

                    rigidBody.ResetPosition(position);
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
