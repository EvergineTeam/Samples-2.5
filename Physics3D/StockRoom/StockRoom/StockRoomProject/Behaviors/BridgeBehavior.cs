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
    public class BridgeBehavior : Behavior
    {
        private const int HIGH = 100;
        private const int NUMTABLES = 12;
        private Vector3 tableSize = new Vector3(20, 1, 50);
        private Entity anchor1, anchor2;
        private List<Entity> tables;

        private const int INTERVAL = 250;
        private const int NUM_BOXES = 20;
        private const int MASS = 2;
        private Vector3 box_size = Vector3.One;
        private List<Entity> boxes;
        private int secondsTime;
        private int index;
        private WaveEngine.Framework.Services.Random random;

        private Input inputService;
        private KeyboardState beforeKeyboardState;

        public BridgeBehavior()
            : base("BridgeBehavior")
        {
            this.tables = new List<Entity>();

            this.secondsTime = 0;
            this.index = 0;
            this.random = WaveServices.Random;
            this.boxes = new List<Entity>(NUM_BOXES);
        }

        protected override void Initialize()
        {
            base.Initialize();

            int offset = -140;
            int space = 20 + 3;
            for (int i = 0; i < NUMTABLES; i++)
            {
                tables.Add(CreateTable("anchor" + i, new Vector3(offset + space * (i + 1), HIGH, 0), tableSize, 1f));

                if (i == 0)
                {
                    this.anchor1 = CreateAnchor("Anchor1", new Vector3(offset, HIGH, 0), new Vector3(5, 5, 50));
                    this.anchor1.AddComponent(new JointMap3D().AddJoint("joint",new HingeJoint(tables[i], new Vector3(offset + 10, HIGH, 0), Vector3.UnitZ)));
                }
                else if (i == NUMTABLES - 1)
                {
                    this.anchor2 = CreateAnchor("Anchor2", new Vector3(offset + 20 + space * NUMTABLES, HIGH, 0), new Vector3(5, 5, 50));
                    tables[i - 1].AddComponent(new JointMap3D().AddJoint("joint", new HingeJoint(tables[i], new Vector3(offset + 10 + (space * i), HIGH, 0), Vector3.UnitZ)));
                    tables[i].AddComponent(new JointMap3D().AddJoint("joint", new HingeJoint(this.anchor2, new Vector3(offset + 10 + space * NUMTABLES, HIGH, 0), Vector3.UnitZ)));
                }
                else
                {
                    tables[i - 1].AddComponent(new JointMap3D().AddJoint("joint", new HingeJoint(tables[i], new Vector3(offset + 10 + (space * i), HIGH, 0), Vector3.UnitZ)));
                }
            }
        }

        private Entity CreateTable(string name, Vector3 position, Vector3 scale, float mass)
        {
            Entity table = new Entity(name)
                 .AddComponent(new Transform3D()
                 {
                     Position = position,
                     Scale = scale
                 })
                .AddComponent(new MaterialsMap(new BasicMaterial(Color.DarkBlue)))
                .AddComponent(Model.CreateCube())
                .AddComponent(new BoxCollider())
                .AddComponent(new RigidBody3D() { Mass = mass })
                .AddComponent(new ModelRenderer());

            EntityManager.Add(table);

            return table;
        }

        private Entity CreateAnchor(string name, Vector3 position, Vector3 scale)
        {
            Entity table = new Entity(name)
                 .AddComponent(new Transform3D()
                 {
                     Position = position,
                     Scale = scale
                 })
                //.AddComponent(new MaterialsMap(new List<IMaterial>() { new BasicMaterial(Helpers.RandomColor()) }))
                //.AddComponent(new ModelRenderer())
                .AddComponent(Model.CreateCube())
                .AddComponent(new BoxCollider())
                .AddComponent(new RigidBody3D() { IsKinematic = true });                

            EntityManager.Add(table);

            return table;
        }

        protected override void Update(TimeSpan gameTime)
        {
            this.secondsTime += gameTime.Milliseconds;
            if (this.index < NUM_BOXES && secondsTime > INTERVAL)
            {
                this.secondsTime = 0;
                Entity box = Helpers.CreateBox("box" + this.index,
                                                new Vector3(this.random.Next(-50, 50), 150, 0),
                                                box_size, MASS,0.1f);
                boxes.Add(box);
                EntityManager.Add(box);
                this.index++;
            }

            inputService = WaveServices.Input;

            if (inputService.KeyboardState.IsConnected)
            {
                if (inputService.KeyboardState.R == ButtonState.Pressed &&
                    beforeKeyboardState.R != ButtonState.Pressed)
                {
                    Reset();
                    Initialize();
                }
                else if (inputService.KeyboardState.B == ButtonState.Pressed &&
                   beforeKeyboardState.B != ButtonState.Pressed)
                {
                    Entity tablet = tables[NUM_BOXES / 2];
                    //HingeJoint joint = tablet.FindComponent<HingeJoint>();
                    tablet.FindComponent<JointMap3D>().RemoveJoint("joint");
                    tablet.RefreshDependencies();
                }
            }

            beforeKeyboardState = inputService.KeyboardState;
        }

        public void Reset()
        {
            EntityManager.Remove(this.anchor1);
            EntityManager.Remove(this.anchor2);

            // Tables
            foreach (Entity table in tables)
            {
                EntityManager.Remove(table);
            }
            tables.Clear();

            // Boxes
            foreach (Entity box in boxes)
            {
                EntityManager.Remove(box);
            }

            this.boxes.Clear();
            this.index = 0;
        }
    }
}
