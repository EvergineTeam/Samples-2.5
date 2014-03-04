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
#endregion

namespace MotorRevoluteJoint2DSampleProject
{
    public class MainScene : Scene
    {
        protected override void CreateScene()
        {
            RenderManager.BackgroundColor = Color.CornflowerBlue;

            // Creates Car Body
            Entity body = this.CreateBody("body", 90, 100);
            EntityManager.Add(body);

            // Creates Motor Wheel
            Entity wheel1 = this.CreateWheel("Wheel1", 90, 100);
            EntityManager.Add(wheel1);
            wheel1.AddComponent(new RevoluteJoint2D(body, Vector2.Zero, new Vector2(-70, 25))
                {
                    MotorEnabled = true,
                    MotorMaxTorque = 1000f,
                    MotorTorque = 100f
                });
            wheel1.AddComponent(new MotorBehavior("MotorBehavior"));

            // Creates Second WHeel
            Entity wheel2 = this.CreateWheel("Wheel2", 210, 100); 
            EntityManager.Add(wheel2);
            wheel2.AddComponent(new RevoluteJoint2D(body, Vector2.Zero, new Vector2(70, 25)));

            // Create Ground and ramps
            this.CreateWalls();
        }

        /// <summary>
        /// Create Terrain
        /// </summary>
        private void CreateWalls()
        {
            Entity bottom = this.CreateKinematicBox("bottom", 400, 500, 0);
            EntityManager.Add(bottom);

            Entity leftramp = this.CreateKinematicBox("leftRamp", -100, 389, MathHelper.ToRadians(25));
            EntityManager.Add(leftramp);

            Entity rightramp = this.CreateKinematicBox("rightRamp", 900, 389, MathHelper.ToRadians(-25));
            EntityManager.Add(rightramp);

            Entity leftWall = this.CreateKinematicBox("leftWall", -100, 300, MathHelper.ToRadians(90));
            EntityManager.Add(leftWall);

            Entity rightWall = this.CreateKinematicBox("rightWall", 900, 300, MathHelper.ToRadians(90));
            EntityManager.Add(rightWall);

        }

        /// <summary>
        /// Creates a Wheel
        /// </summary>
        /// <param name="name">Entity Name</param>
        /// <param name="x">X Position</param>
        /// <param name="y">Y Position</param>
        /// <returns>Wheel Entity</returns>
        private Entity CreateWheel(string name, float x, float y)
        {
            Entity sprite = new Entity(name)
                .AddComponent(new Transform2D() { X = x, Y = y })
                .AddComponent(new CircleCollider())
                .AddComponent(new Sprite("Content/Wheel.wpk"))
                .AddComponent(new RigidBody2D() { IsKinematic = false, Friction = 1, Damping = 0 })
                .AddComponent(new SpriteRenderer(DefaultLayers.Alpha));

            return sprite;
        }

        /// <summary>
        /// Creates a Body Box
        /// </summary>
        /// <param name="name">Entity Name</param>
        /// <param name="x">X Position</param>
        /// <param name="y">Y Position</param>
        /// <returns>Body Box Entity</returns>
        private Entity CreateBody(string name, float x, float y)
        {
            Entity sprite = new Entity(name)
                .AddComponent(new Transform2D() { X = x, Y = y })
                .AddComponent(new RectangleCollider())
                .AddComponent(new Sprite("Content/RectangleBody.wpk"))
                .AddComponent(new RigidBody2D() { IsKinematic = false})
                .AddComponent(new SpriteRenderer(DefaultLayers.Opaque));

            return sprite;
        }

        /// <summary>
        /// Creates a kinematic Box
        /// </summary>
        /// <param name="name">Entity Name</param>
        /// <param name="x">X Position</param>
        /// <param name="y">Y Position</param>
        /// <returns>Kinematic Box Entity</returns>
        private Entity CreateKinematicBox(string name, float x, float y, float angle)
        {
            Entity sprite = new Entity(name)
                .AddComponent(new Transform2D() { X = x, Y = y })
                .AddComponent(new RectangleCollider())
                .AddComponent(new Sprite("Content/Ground.wpk"))
                .AddComponent(new RigidBody2D() { IsKinematic = true, Rotation = angle })
                .AddComponent(new SpriteRenderer(DefaultLayers.Opaque));

            return sprite;
        }
    }
}
