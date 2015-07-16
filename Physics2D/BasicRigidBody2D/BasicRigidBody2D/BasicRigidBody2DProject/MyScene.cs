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
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics2D;
using WaveEngine.Framework.Resources;
using WaveEngine.Framework.Services;
#endregion

namespace BasicRigidBody2DProject
{
    public class MyScene : Scene
    {
        /// <summary>
        /// Creates the scene.
        /// </summary>
        /// <remarks>
        /// This method is called before all 
        /// <see cref="T:WaveEngine.Framework.Entity" /> instances in this instance are initialized.
        /// </remarks>
        protected override void CreateScene()
        {
            var camera2D = new FixedCamera2D("Camera2D")
            {
                BackgroundColor = Color.CornflowerBlue
            };
            EntityManager.Add(camera2D);


            Entity ground = new Entity("ground")
                .AddComponent(new Transform2D() { X = 400, Y = 400, Origin = Vector2.Center })
                .AddComponent(new Sprite("Content/groundSprite.wpk"))
                .AddComponent(new RectangleCollider())
                .AddComponent(new RigidBody2D() { PhysicBodyType = PhysicBodyType.Static })
                .AddComponent(new SpriteRenderer(DefaultLayers.Opaque));

            EntityManager.Add(ground);

            Entity circle = new Entity("Circle")
                .AddComponent(new Transform2D() { X = 450, Origin = Vector2.Center })
                .AddComponent(new Sprite("Content/circleSprite.wpk"))
                .AddComponent(new CircleCollider())
                .AddComponent(new RigidBody2D())
                .AddComponent(new SpriteRenderer(DefaultLayers.Alpha));

            EntityManager.Add(circle);

            for (int i = 0; i < 8; i++)
            {
                Entity box = CreateBox(i.ToString(), 300 + i * 5, i * -50);
                EntityManager.Add(box);
            }
        }


        /// <summary>
        /// Creates the box.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        private Entity CreateBox(string name, float x, float y)
        {
            return new Entity(name)
              .AddComponent(new Transform2D() { X = x, Y = y, Origin = Vector2.Center })
              .AddComponent(new Sprite("Content/boxSprite.wpk"))
              .AddComponent(new RectangleCollider())
              .AddComponent(new RigidBody2D() { EnableContinuousContact = true })
              .AddComponent(new SpriteRenderer(DefaultLayers.Opaque));
        }

        /// <summary>
        /// Allows to perform custom code when this instance is started.
        /// </summary>
        /// <remarks>
        /// This base method perfoms a layout pass.
        /// </remarks>
        protected override void Start()
        {
            base.Start();

            // This method is called after the CreateScene and Initialize methods and before the first Update.
        }
    }
}
