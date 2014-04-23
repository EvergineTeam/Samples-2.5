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
using WaveEngine.Components.Graphics2D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics2D;
using WaveEngine.Framework.Services;
#endregion

namespace AngleJoint2DSampleProject
{
    public class MainScene : Scene
    {
        private long repeats = 0;
        private Entity fallingCrate;

        protected override void CreateScene()
        {
            RenderManager.BackgroundColor = Color.CornflowerBlue;

            // Scene creation
            Entity Wheel1 = this.CreateWheel("Wheel1", 150, 300);
            EntityManager.Add(Wheel1);

            Entity Pin1 = this.CreatePin("Pin1", 200, 200);
            EntityManager.Add(Pin1);

            Pin1.AddComponent(new DistanceJoint2D(Wheel1, Vector2.Zero, Vector2.Zero));

            Entity Wheel2 = this.CreateWheel("Wheel2", 550, 300);
            EntityManager.Add(Wheel2);

            Entity Pin2 = this.CreatePin("Pin2", 600, 200);
            EntityManager.Add(Pin2);

            Pin2.AddComponent(new DistanceJoint2D(Wheel2, Vector2.Zero, Vector2.Zero));

            // ANGLE JOINT between wheels
            Wheel2.AddComponent(new AngleJoint2D(Wheel1));

            // Create Ground
            Entity ground = this.CreateGround("ground", 400, 500);
            EntityManager.Add(ground);

            Entity ground2 = this.CreateGround("ground2", 950, 400);
            EntityManager.Add(ground2);
            
            // Falling Crates controller
            WaveServices.TimerFactory.CreateTimer("CrateFallingTimer", TimeSpan.FromSeconds(2f), () =>
                {
                    this.CreateFallingCrate(255);

                    if (this.repeats == 10)
                    {
                        Pin2.RemoveComponent<DistanceJoint2D>();
                    }
                });
        }

        /// <summary>
        /// Creates a Falling Crate Entity
        /// </summary>
        /// <param name="x">X position</param>
        /// <returns>Falling Crate Entity Entity</returns>
        private void CreateFallingCrate(int x)
        {
            if (fallingCrate == null)
            {
                fallingCrate = new Entity("FallingCrate" + repeats++)
                    .AddComponent(new Transform2D() 
                    { 
                        Origin = Vector2.Center, 
                        Rotation = MathHelper.ToRadians(45), 
                        X = x, 
                        Y = -50 
                    })
                    .AddComponent(new RectangleCollider())
                    .AddComponent(new Sprite("Content/Crate.wpk"))
                    .AddComponent(new RigidBody2D() { IsKinematic = false, Restitution = 0, Friction = 100 })
                    .AddComponent(new SpriteRenderer(DefaultLayers.Opaque));
                
                EntityManager.Add(fallingCrate);
            }

            repeats++;
        }

        /// <summary>
        /// Creates a Kinematic Pin
        /// </summary>
        /// <param name="name">Entity name</param>
        /// <param name="x">X Position</param>
        /// <param name="y">Y Position</param>
        /// <returns>Pin Entity</returns>
        private Entity CreatePin(string name, int x, int y)
        {
            Entity sprite = new Entity(name)
                .AddComponent(new Transform2D() { X = x, Y = y, Origin = Vector2.Center })
                .AddComponent(new RectangleCollider())
                .AddComponent(new Sprite("Content/PinAzureA.wpk"))
                .AddComponent(new RigidBody2D() { IsKinematic = true })
                .AddComponent(new SpriteRenderer(DefaultLayers.Alpha));

            return sprite;
        }

        /// <summary>
        /// Creates a Wheel
        /// </summary>
        /// <param name="name">Entity Name</param>
        /// <param name="x">X Position</param>
        /// <param name="y">Y Position</param>
        /// <returns>Wheel Entity</returns>
        private Entity CreateWheel(string name, int x, int y)
        {
            Entity sprite = new Entity(name)
                .AddComponent(new Transform2D() { X = x, Y = y, Origin = Vector2.Center })
                .AddComponent(new CircleCollider())
                .AddComponent(new Sprite("Content/Wheel.wpk"))
                .AddComponent(new RigidBody2D() { IsKinematic = false})
                .AddComponent(new SpriteRenderer(DefaultLayers.Alpha));

            return sprite;
        }

        /// <summary>
        /// Creates a Kinematic Ground
        /// </summary>
        /// <param name="name">Entity name</param>
        /// <param name="x">X Position</param>
        /// <param name="y">Y Position</param>
        /// <returns>Ground Entity</returns>
        private Entity CreateGround(string name, int x, int y)
        {
            Entity sprite = new Entity(name)
                .AddComponent(new Transform2D() { X = x, Y = y, Origin = Vector2.Center })
                .AddComponent(new RectangleCollider())
                .AddComponent(new Sprite("Content/Ground.wpk"))
                .AddComponent(new RigidBody2D() { IsKinematic = true })
                .AddComponent(new SpriteRenderer(DefaultLayers.Opaque));

            return sprite;
        }
    }
}
