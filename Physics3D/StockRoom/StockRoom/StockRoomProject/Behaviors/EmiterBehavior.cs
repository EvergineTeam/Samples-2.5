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
    public class EmiterBehavior : Behavior
    {
        private const int INTERVAL = 250;
        private const int NUM_BOXES = 50;
        private const int MASS = 1;
        private Vector3 box_size = Vector3.One;
        private List<Entity> boxes;
        private int secondsTime;
        private int index;
        private System.Random random;
        private Input inputService;
        private KeyboardState beforeKeyboardState;

        /// <summary>
        /// Constructor
        /// </summary>
        public EmiterBehavior()
            : base("emiterBehavior")
        {
            this.secondsTime = 0;
            this.index = 0;
            this.random = new System.Random();
            this.boxes = new List<Entity>(NUM_BOXES);
        }

        /// <summary>
        /// Update method
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Update(TimeSpan gameTime)
        {
            this.secondsTime += gameTime.Milliseconds;
            if (this.index < NUM_BOXES && secondsTime > INTERVAL)
            {
                this.secondsTime = 0;
                Entity box = Helpers.CreateBox("box" + this.index,
                                                new Vector3(this.random.Next(-100, 0), 150, this.random.Next(0, 10)),
                                                box_size, MASS, 3f);
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
