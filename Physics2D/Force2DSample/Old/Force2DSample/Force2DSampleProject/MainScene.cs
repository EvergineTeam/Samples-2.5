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

namespace Force2DSampleProject
{
    public class MainScene : Scene
    {
        protected override void CreateScene()
        {
            RenderManager.BackgroundColor = Color.CornflowerBlue;

            this.CreateClosures();

            Entity wheel = new Entity("Wheel")
                .AddComponent(new Transform2D() { X = 400, Y = 300, Origin = Vector2.Center})
                .AddComponent(new CircleCollider())
                .AddComponent(new Sprite("Content/Wheel.wpk"))
                .AddComponent(new RigidBody2D() { IsKinematic = false, Friction = 1 })
                .AddComponent(new SpriteRenderer(DefaultLayers.Alpha))
                .AddComponent(new ForceBehavior("ForceBehavior"));

            EntityManager.Add(wheel);
        }

        private void CreateClosures()
        {
            EntityManager.Add(this.CreateGround("top", 400, 0, 0));
            EntityManager.Add(this.CreateGround("bottom", 400, 600, 0));
            EntityManager.Add(this.CreateGround("left", 50, 300, MathHelper.ToRadians(90)));
            EntityManager.Add(this.CreateGround("right", 750, 300, MathHelper.ToRadians(90)));
        }

        private Entity CreateGround(string name, float x, float y, float angle)
        {
            Entity sprite = new Entity(name)
                .AddComponent(new Transform2D() { X = x, Y = y, Origin = Vector2.Center, Rotation = angle})
                .AddComponent(new RectangleCollider())
                .AddComponent(new Sprite("Content/Ground.wpk"))
                .AddComponent(new RigidBody2D() { IsKinematic = true, Friction = 1 })
                .AddComponent(new SpriteRenderer(DefaultLayers.Opaque));
                        
            return sprite;
        }
    }
}
