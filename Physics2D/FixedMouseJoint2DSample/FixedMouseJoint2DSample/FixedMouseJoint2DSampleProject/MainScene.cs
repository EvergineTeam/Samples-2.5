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

namespace FixedMouseJoint2DSampleProject
{
    public class MainScene : Scene
    {
        // Contants
        private const int STACKWIDTH = 5;
        private const int STACKHEIGHT = 5;

        // Box Instance Count
        private long instances = 0;

        protected override void CreateScene()
        {
            RenderManager.BackgroundColor = Color.CornflowerBlue;

            // Create Phsyic Borders
            this.CreateClosures();

            // Creates Box Stack
            for (int i = 0; i < STACKHEIGHT; i++)
            {
                for (int j = 0; j < STACKWIDTH; j++)
                {
                    Entity box = this.CreateCrate(200 + j * 45, 100 + (STACKHEIGHT - i) * 45);
                    EntityManager.Add(box);
                }
            }

            // Adds Mouse control. See MouseBehavior.cs for details.
            this.AddSceneBehavior(new MouseBehavior(), SceneBehavior.Order.PostUpdate);
        }

        /// <summary>
        /// Creates a Physic Crate
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private Entity CreateCrate(float x, float y)
        {
            Entity box = new Entity("Crate" + this.instances++)
                .AddComponent(new Transform2D() { X = x, Y = y, Origin = Vector2.Center })
                .AddComponent(new RectangleCollider())
                .AddComponent(new Sprite("Content/boxSprite.wpk"))
                .AddComponent(new RigidBody2D() { IsKinematic = false })
                .AddComponent(new SpriteRenderer(DefaultLayers.Opaque));

            return box;
        }

        /// <summary>
        /// Creates Physic Borders
        /// </summary>
        private void CreateClosures()
        {
            EntityManager.Add(this.CreateGround("top1", 150, -10, 0));
            EntityManager.Add(this.CreateGround("top2", 650, -10, 0));

            EntityManager.Add(this.CreateGround("bottom1", 150, 610, 0));
            EntityManager.Add(this.CreateGround("bottom2", 650, 610, 0));

            EntityManager.Add(this.CreateGround("left", 0, 300, MathHelper.ToRadians(90)));
            EntityManager.Add(this.CreateGround("right", 800, 300, MathHelper.ToRadians(90)));
        }

        /// <summary>
        /// Creates a Physic Ground.
        /// </summary>
        /// <param name="name">Entity Name.</param>
        /// <param name="x">X Position.</param>
        /// <param name="y">Y Position.</param>
        /// <param name="angle">Ground Angle.</param>
        /// <returns>Ground Entity.</returns>
        private Entity CreateGround(string name, float x, float y, float angle)
        {
            Entity sprite = new Entity(name)
                .AddComponent(new Transform2D() { X = x, Y = y, Origin = Vector2.Center, Rotation = angle })
                .AddComponent(new RectangleCollider())
                .AddComponent(new Sprite("Content/groundSprite.wpk"))
                .AddComponent(new RigidBody2D() { IsKinematic = true, Friction = 1 })
                .AddComponent(new SpriteRenderer(DefaultLayers.Opaque));

            return sprite;
        }
    }
}
