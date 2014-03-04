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
// MainScene
//
// Copyright © 2013 Weekend Game Studio. All rights reserved.
// Use is subject to license terms.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics3D;
using WaveEngine.Framework.Services;
using WaveEngine.Materials;
#endregion

namespace Entity3DCollisionProject
{
    /// <summary>
    /// Main Scene Class
    /// </summary>
    public class MainScene : Scene
    {
        /// <summary>
        /// Entities Instance Count
        /// </summary>
        private long instances = 0;

        private DrawableLines drawableLines;

        /// <summary>
        /// Create Scene
        /// </summary>
        protected override void CreateScene()
        {
            RenderManager.BackgroundColor = Color.CornflowerBlue;
            RenderManager.DebugLines = true;

            // Createa a Free Camera (W, A, S, D Keyboard controlled)
            FreeCamera camera = new FreeCamera("MainCamera", new Vector3(0, 5, 10), Vector3.Zero);
            EntityManager.Add(camera.Entity);
            RenderManager.SetActiveCamera(camera.Entity);

            // Creates a plane ground
            this.CreateGround();

            // Creates Line Drawing Entity
            drawableLines = new DrawableLines();
            Entity lines = new Entity("Drawables")
                .AddComponent(drawableLines);
            EntityManager.Add(lines);

            // Create a Timer to instance new Sphere each 0.5 seconds
            WaveServices.TimerFactory.CreateTimer("MainTimer", TimeSpan.FromSeconds(0.5f), () =>
                {
                    // new sphere in (random(0,1), 5, random(0, 1)) position
                    this.CreateSphere(Vector3.Up * 5 + Vector3.Forward*(float)WaveServices.Random.NextDouble() + Vector3.Left*(float)WaveServices.Random.NextDouble());
                });
        }

        /// <summary>
        /// Handles Collision Event
        /// </summary>
        /// <param name="sender">Sender RigidBody3D</param>
        /// <param name="args">Event Args</param>
        private void rigidBody3D_OnPhysic3DCollision(object sender, Physic3DCollisionEventArgs args)
        {
            // Creates a normal line mark
            Line l = drawableLines.AddNewLine(args.Position, args.Normal);
            string timername = "RemoveLineTimer" + instances++;

            // Line Time To Live
            WaveServices.TimerFactory.CreateTimer(timername, TimeSpan.FromSeconds(1), () =>
            {
                drawableLines.Remove(l);
                WaveServices.TimerFactory.RemoveTimer(timername);
            });
        }


        /// <summary>
        /// Creates a Box Ground
        /// </summary>
        private void CreateGround()
        {
            Entity ground = new Entity("ground")
                .AddComponent(new Transform3D() { Position = new Vector3(0, -1, 0), Scale = new Vector3(20, 1, 20) })
                .AddComponent(new BoxCollider())
                .AddComponent(Model.CreateCube())
                .AddComponent(new RigidBody3D() { IsKinematic = true })
                .AddComponent(new MaterialsMap(new BasicMaterial(Color.Gray)))
                .AddComponent(new ModelRenderer());

            EntityManager.Add(ground);
        }

        /// <summary>
        /// Create Sphere
        /// </summary>
        /// <param name="name"></param>
        /// <param name="position"></param>
        private void CreateSphere(Vector3 position)
        {
            Entity primitive = new Entity("Sphere" + instances++)
                .AddComponent(new Transform3D() { Position = position, Scale = Vector3.One / 2 })
                .AddComponent(new SphereCollider())
                .AddComponent(Model.CreateSphere())
                .AddComponent(new RigidBody3D() { KineticFriction = 10, Restitution = 1 })
                .AddComponent(new MaterialsMap(new BasicMaterial(GetRandomColor())))
                .AddComponent(new ModelRenderer());

            RigidBody3D rigidBody3D = primitive.FindComponentOfType<RigidBody3D>();
            if (rigidBody3D != null)
            {
                rigidBody3D.OnPhysic3DCollision += rigidBody3D_OnPhysic3DCollision;
            }

            EntityManager.Add(primitive);

            // Sets sphere Time To Live. Remove timer from WaveServices after use.
            WaveServices.TimerFactory.CreateTimer("Timer" + primitive.Name, TimeSpan.FromSeconds(10), () =>
            {
                EntityManager.Remove(primitive);
                WaveServices.TimerFactory.RemoveTimer("Timer" + primitive.Name);
            }); 
        }

        /// <summary>
        /// Gets random color.
        /// </summary>
        /// <returns>Random Color.</returns>
        private Color GetRandomColor()
        {
            var random = WaveServices.Random;
            return new Color((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble(), 1f);
        }
    }
}
