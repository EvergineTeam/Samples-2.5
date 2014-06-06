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
// Copyright © 2012 Weekend Game Studio. All rights reserved.
// Use is subject to license terms.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Framework.Physics2D;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
#endregion

namespace BasicRigidBody2DProject
{
    public class MyScene : Scene
    {
        protected override void CreateScene()
        {
            RenderManager.BackgroundColor = Color.White;
            //RenderManager.DebugLines = true;

            Entity ground = new Entity("ground")
                .AddComponent(new Transform2D() {X = 400, Y = 400, Origin = Vector2.Center })
                .AddComponent(new Sprite("Content/groundSprite.wpk"))
                .AddComponent(new RectangleCollider())
                .AddComponent(new RigidBody2D() { IsKinematic = true })
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

        private Entity CreateBox(string name, float x, float y)
        {
            return new Entity(name)
              .AddComponent(new Transform2D() { X = x, Y = y, Origin = Vector2.Center })
              .AddComponent(new Sprite("Content/boxSprite.wpk"))
              .AddComponent(new RectangleCollider())
              .AddComponent(new RigidBody2D() { EnableContinuousContact = true, Mass = 4 })
              .AddComponent(new SpriteRenderer(DefaultLayers.Opaque));
        }
    }
}
