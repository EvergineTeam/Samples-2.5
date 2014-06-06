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
using WaveEngine.Components.Particles;
using WaveEngine.Components.UI;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics2D;
using WaveEngine.Framework.Services;
using WaveEngine.Framework.UI;
#endregion

namespace OnPhysics2DCollisionSampleProject
{
    public class MainScene : Scene
    {
        // Consts
        private const string GROUND_TEXTURE = "Content/GroundSprite.wpk";
        private const string CRATEA_TEXTURE = "Content/CrateA.wpk";
        private const string CIRCLE_TEXTURE = "Content/CircleSprite.wpk";
        private const string MARKER_TEXTURE = "Content/Mark.wpk";
        private const string ARROW_TEXTURE = "Content/Arrow.wpk";

        // Entity instances count
        private long instances = 0;

        // UI Components
        private TextBlock boxTextBlock;
        private TextBlock circleTextBlock;

        /// <summary>
        /// Creates Main Scene
        /// </summary>
        protected override void CreateScene()
        {
            RenderManager.BackgroundColor = Color.CornflowerBlue;

            // Border and UI
            this.CreateClosures();
            this.CreateUI();

            // Sample Crate and Collision Event Register
            Entity box = this.CreateCrate(200, 100);
            RigidBody2D boxRigidBody = box.FindComponent<RigidBody2D>();
            if (boxRigidBody != null)
            {
                boxRigidBody.OnPhysic2DCollision += BoxRigidBody_OnPhysic2DCollision;
            }
            EntityManager.Add(box);

            // Sample Circle and Collision Event register
            Entity circle = this.CreateCircle(600, 100);
            RigidBody2D circleRigidBody = circle.FindComponent<RigidBody2D>();
            if (circleRigidBody != null)
            {
                circleRigidBody.OnPhysic2DCollision += CircleRigidBody_OnPhysic2DCollision;
            }
            EntityManager.Add(circle);

            // Mouse Handler.
            this.AddSceneBehavior(new MouseBehavior(), SceneBehavior.Order.PostUpdate);
        }

        /// <summary>
        /// Handles Circle OnPhysicsCollision.
        /// </summary>
        /// <param name="sender">Sender Object.</param>
        /// <param name="args">Collision Arguments.</param>
        private void CircleRigidBody_OnPhysic2DCollision(object sender, Physic2DCollisionEventArgs args)
        {
            // Gets angle, updates Textblocks and Draws information marks
            float angle = (float)Math.Atan2(args.Normal.X, args.Normal.Y);
            this.circleTextBlock.Text = string.Format("Normal : {0}\nNormal Angle: {1}\nPointA : {2}\nPointB : {3}", args.Normal, MathHelper.ToDegrees(angle), args.PointA, args.PointB);
            this.CreateVolatileMark(args.PointA.Value.X, args.PointA.Value.Y, angle);
        }

        /// <summary>
        /// Handles Box On PhysicsCollision.
        /// </summary>
        /// <param name="sender">Sender Object.</param>
        /// <param name="args">Collision Arguments.</param>
        private void BoxRigidBody_OnPhysic2DCollision(object sender, Physic2DCollisionEventArgs args)
        {
            /// gets normal angle, updates textblocks and draws information marks
            float angle = (float)Math.Atan2(args.Normal.X, args.Normal.Y);
            this.boxTextBlock.Text = string.Format("Normal : {0}\nNormal Angle: {1}\nPointA : {2}\nPointB : {3}", args.Normal, MathHelper.ToDegrees(angle), args.PointA, args.PointB);
            this.CreateVolatileMark(args.PointA.Value.X, args.PointA.Value.Y, angle);
            
            // We could retrieve a second contact points on Collision. A circle only can get ONE contact point. 
            if (args.PointB != Vector2.Zero)
            {
                this.CreateVolatileMark(args.PointB.Value.X, args.PointB.Value.Y, angle);
            }
        }

        /// <summary>
        /// Creates User Interface
        /// </summary>
        private void CreateUI()
        {
            // Box coords TextBlock
            this.boxTextBlock = new TextBlock()
            {
                Margin = new Thickness(50)
            };
            EntityManager.Add(boxTextBlock.Entity);

            // Circle coords TextBlock
            this.circleTextBlock = new TextBlock()
            {
                Margin = new Thickness(400, 50, 0, 0)
            };
            EntityManager.Add(circleTextBlock.Entity);
        }

        /// <summary>
        /// Creates a Volatile Mark on X,Y position.
        /// </summary>
        /// <param name="x">X Position.</param>
        /// <param name="y">Y Position.</param>
        private void CreateVolatileMark(float x, float y, float angle)
        {
            Entity mark = new Entity("mark" + instances++)
                .AddComponent(new Transform2D() { X = x - 7, Y  = y-7})
                .AddComponent(new Sprite(MARKER_TEXTURE))
                .AddComponent(new SpriteRenderer(DefaultLayers.Alpha));
            EntityManager.Add(mark);

            Entity arrow = new Entity("mark" + instances++)
                .AddComponent(new Transform2D() { Origin = new Vector2(0.5f,1),  X = x, Y = y, Rotation = angle })
                .AddComponent(new Sprite(ARROW_TEXTURE))
                .AddComponent(new SpriteRenderer(DefaultLayers.Alpha));
            EntityManager.Add(arrow);

            // Sets Time To Live for the mark. Remove timer from WaveServices after use.
            WaveServices.TimerFactory.CreateTimer("Timer" + mark.Name, TimeSpan.FromSeconds(1), () =>
                {
                    EntityManager.Remove(mark);
                    EntityManager.Remove(arrow);
                    WaveServices.TimerFactory.RemoveTimer("Timer" + mark.Name);
                });
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

            EntityManager.Add(this.CreateGround("Ramp", 0, 500, MathHelper.ToRadians(45)));
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
                .AddComponent(new Transform2D() { X = x, Y = y, Origin = Vector2.Center, Rotation = angle})
                .AddComponent(new RectangleCollider())
                .AddComponent(new Sprite(GROUND_TEXTURE))
                .AddComponent(new RigidBody2D() { IsKinematic = true, Friction = 1, CollisionCategories = Physic2DCategory.All })
                .AddComponent(new SpriteRenderer(DefaultLayers.Opaque));

            return sprite;
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
                .AddComponent(new Sprite(CRATEA_TEXTURE))
                .AddComponent(new RigidBody2D() { IsKinematic = false })
                .AddComponent(new SpriteRenderer(DefaultLayers.Opaque));

            return box;
        }

        /// <summary>
        /// Creates a Physic Circle
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private Entity CreateCircle(float x, float y)
        {
            Entity box = new Entity("Circle" + this.instances++)
                .AddComponent(new Transform2D() { X = x, Y = y, Origin = Vector2.Center })
                .AddComponent(new CircleCollider())
                .AddComponent(new Sprite(CIRCLE_TEXTURE))
                .AddComponent(new RigidBody2D() { IsKinematic = false })
                .AddComponent(new SpriteRenderer(DefaultLayers.Alpha));

            return box;
        }
    }
}
