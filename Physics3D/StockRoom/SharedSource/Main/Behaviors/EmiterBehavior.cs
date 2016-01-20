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
using StockRoomProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
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

namespace StockRoom.Behaviors
{
    [DataContract(Namespace = "StockRoom.Behaviors")]
    public class EmiterBehavior : Behavior
    {
        private List<Entity> boxes;
        private double secondsTime;
        private int index;
        private System.Random random;
        private Input inputService;
        private KeyboardState beforeKeyboardState;

        [DataMember]
        public float Interval
        {
            get;
            set;
        }

        [DataMember]
        public float Mass
        {
            get;
            set;
        }

        [DataMember]
        public float ItemSize
        {
            get;
            set;
        }

        [DataMember]
        public int Count
        {
            get;
            set;
        }

        [DataMember]
        public Vector3 PositionRandom
        {
            get;
            set;
        }

        [RequiredComponent]
        private Transform3D transform;

        /// <summary>
        /// Constructor
        /// </summary>
        public EmiterBehavior()
            : base("emiterBehavior")
        {
        }

        protected override void DefaultValues()
        {
            base.DefaultValues();

            this.Mass = 1;
            this.Count = 50;
            this.Interval = 0.25f;
            this.ItemSize = 1.5f;
            this.PositionRandom = new Vector3(20, 0, 0.5f);

            this.secondsTime = 0;
            this.index = 0;
            this.random = new System.Random();
            this.boxes = new List<Entity>();
        }

        /// <summary>
        /// Update method
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Update(TimeSpan gameTime)
        {
            this.secondsTime += gameTime.TotalSeconds;
            if (this.index < this.Count && secondsTime > this.Interval)
            {
                this.secondsTime = 0;
                var offsetPosition = new Vector3((float)((0.5f * this.random.NextDouble() - 0.5f) * this.PositionRandom.X), 
                                                (float)((0.5f * this.random.NextDouble() - 0.5f) * this.PositionRandom.Y), 
                                                (float)((0.5f * this.random.NextDouble() - 0.5f) * this.PositionRandom.Z));
                var position = this.transform.Position + offsetPosition;

                Entity box = Helpers.CreateBox("box" + this.index, position, Vector3.One * this.ItemSize, this.Mass, 3f);
                boxes.Add(box);
                EntityManager.Add(box);
                this.index++;
                Labels.Add("Emiter boxes", boxes.Count.ToString());
            }

            inputService = WaveServices.Input;

            if (inputService.KeyboardState.IsConnected)
            {
                if (inputService.KeyboardState.R == ButtonState.Pressed &&
                    beforeKeyboardState.R != ButtonState.Pressed)
                {
                    ResetEmiter();
                }
            }

            beforeKeyboardState = inputService.KeyboardState;
        }

        /// <summary>
        /// Reset emiter
        /// </summary>
        public void ResetEmiter()
        {
            foreach (Entity box in boxes)
            {
                EntityManager.Remove(box);
            }

            this.boxes.Clear();
            this.index = 0;
            Labels.Add("Emiter boxes", boxes.Count.ToString());
        }
    }
}
