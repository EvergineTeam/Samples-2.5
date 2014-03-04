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

namespace DistanceJoint2DSampleProject
{
    public class MainScene : Scene
    {
        protected override void CreateScene()
        {
            RenderManager.BackgroundColor = Color.CornflowerBlue;

            Entity crate = new Entity("crate")
                .AddComponent(new Transform2D() { X = 300, Y = 150 })
                .AddComponent(new RectangleCollider())
                .AddComponent(new Sprite("Content/Crate.wpk"))
                .AddComponent(new RigidBody2D())
                .AddComponent(new SpriteRenderer(DefaultLayers.Opaque));

            Entity Wheel1 = new Entity("wheel1")
                .AddComponent(new Transform2D() { X = 300, Y = 400 })
                .AddComponent(new CircleCollider())
                .AddComponent(new Sprite("Content/Wheel.wpk"))
                .AddComponent(new RigidBody2D())
                .AddComponent(new SpriteRenderer(DefaultLayers.Alpha));
               //.AddComponent(new DistanceJoint2D(crate, Vector2.Zero, Vector2.Zero));

            Entity Wheel2 = new Entity("wheel2")
                .AddComponent(new Transform2D() { X = 450, Y = 400 })
                .AddComponent(new CircleCollider())
                .AddComponent(new Sprite("Content/Wheel.wpk"))
                .AddComponent(new RigidBody2D())
                .AddComponent(new SpriteRenderer(DefaultLayers.Alpha))
                .AddComponent(new DistanceJoint2D(Wheel1, Vector2.Zero, Vector2.Zero));

            Entity ground = new Entity("ground")
                .AddComponent(new Transform2D() { X = 400, Y = 500 })
                .AddComponent(new RectangleCollider())
                .AddComponent(new Sprite("Content/Ground.wpk"))
                .AddComponent(new RigidBody2D() { IsKinematic = true })
                .AddComponent(new SpriteRenderer(DefaultLayers.Alpha));
                

            EntityManager.Add(crate);
            EntityManager.Add(Wheel1);
            EntityManager.Add(Wheel2);
            EntityManager.Add(ground);
        }
    }
}
