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

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Input;
using WaveEngine.Common.Math;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Diagnostic;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics3D;
using WaveEngine.Framework.Services;
using WaveEngine.Materials;
#endregion

namespace StockRoomProject.Behaviors
{
    public class StackBehavior : Behavior
    {
        private const int NUM_LINES = 6;
        private const int NUM_COLUMNS = 10;
        private const int MASS = 1;
        private const int MASS_BIGBALL = 20;
        private Vector3 size = Vector3.One;  // Box size       
        private float offsetZ = -100;
        private List<Entity> boxes;
        private Entity anchor, bigBall;
        private Line rope;
        private Input inputService;
        private KeyboardState beforeKeyboardState;

        public StackBehavior()
            : base("StackBehavior")
        {
            boxes = new List<Entity>();
        }

        protected override void Initialize()
        {
            base.Initialize();

            // Create wall
            for (int i = NUM_LINES; i > 0; i--)
            {
                for (int j = 0; j < NUM_COLUMNS; j++)
                {
                    float offset = offsetZ;
                    if (i % 2 != 0)
                    {
                        offset += 10;
                    }

                    Entity box = Helpers.CreateBox("box" + i + "," + j,
                                                        new Vector3(0, 20 * i, offset + (20 * j)),
                                                        size, MASS,0.1f);
                    this.boxes.Add(box);
                    EntityManager.Add(box);
                }
            }
            Labels.Add("Wall Boxes", (NUM_LINES * NUM_COLUMNS).ToString());

            // Roff Ball
            rope = new Line(new Vector3(0, 230, 0), new Vector3(140, 140, 0), Color.Gray);
            anchor = new Entity("Anchor")
                .AddComponent(new Transform3D() { Position = rope.StartPoint, Scale = new Vector3(10) })
                //.AddComponent(new MaterialsMap(new List<IMaterial>() { new BasicMaterial(Color.Purple) }))
                //.AddComponent(new ModelRenderer())
                .AddComponent(Model.CreateCube())
                .AddComponent(new BoxCollider())
                .AddComponent(new RigidBody3D() { IsKinematic = true });                
            EntityManager.Add(anchor);

            bigBall = Helpers.CreateSphere("BigBall", rope.EndPoint, new Vector3(30), MASS_BIGBALL, Color.Gray);
            EntityManager.Add(bigBall);

            anchor.AddComponent(new JointMap3D()
                                        .AddJoint("jointA", new BallSocketJoint(bigBall, rope.StartPoint)));
        }

        protected override void Update(TimeSpan gameTime)
        {
            inputService = WaveServices.Input;

            if (inputService.KeyboardState.IsConnected)
            {
                if (inputService.KeyboardState.R == ButtonState.Pressed &&
                    beforeKeyboardState.R != ButtonState.Pressed)
                {
                    RemoveWall();
                    Initialize();
                }
            }

            beforeKeyboardState = inputService.KeyboardState;

            rope.EndPoint = this.bigBall.FindComponent<Transform3D>().Position;
            RenderManager.LineBatch3D.DrawLine(ref rope);
        }

        public void RemoveWall()
        {
            foreach (Entity box in boxes)
            {
                EntityManager.Remove(box);
            }
            boxes.Clear();
            Labels.Add("Wall Boxes", "0");

            EntityManager.Remove(this.anchor);
            EntityManager.Remove(this.bigBall);
        }
    }
}
