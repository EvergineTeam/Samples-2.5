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
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Common.Input;
using WaveEngine.Framework;
using WaveEngine.Framework.Diagnostic;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
#endregion

namespace StockRoom.Behaviors
{
    [DataContract(Namespace = "StockRoom.Behaviors")]
    public class CombineBehavior : Behavior
    {
        enum State
        {
            LAUNCH,
            PICK
        }

        [RequiredComponent]
        private PickingBehavior pickingBehavior;

        [RequiredComponent]
        private LaunchBehavior launchBehavior;

        private KeyboardState lastKeyboardState;
        private State state;

        public CombineBehavior()
            : base("CombineBehavior")
        {
        }

        protected override void DefaultValues()
        {
            base.DefaultValues();
            this.state = State.PICK;
        }

        protected override void Initialize()
        {
            base.Initialize();
            UpdateBehaviorsState(this.state);
        }

        protected override void Update(TimeSpan gameTime)
        {
            var inputService = WaveServices.Input;

            if (inputService.KeyboardState.IsConnected)
            {
                if (inputService.KeyboardState.D1 == ButtonState.Pressed && this.lastKeyboardState.D1 != ButtonState.Pressed && this.state != State.PICK)
                {
                    this.state = State.PICK;
                    UpdateBehaviorsState(this.state);
                }
                else if (inputService.KeyboardState.D2 == ButtonState.Pressed && this.lastKeyboardState.D2 != ButtonState.Pressed && this.state != State.LAUNCH)
                {
                    this.state = State.LAUNCH;
                    UpdateBehaviorsState(this.state);
                }

                Labels.Add("CameraState", this.state.ToString());
            }

            this.lastKeyboardState = inputService.KeyboardState;
        }

        private void UpdateBehaviorsState(State state)
        {
            if (state == State.PICK)
            {
                this.pickingBehavior.IsActive = true;
                this.launchBehavior.IsActive = false;
                this.launchBehavior.Reset();
            }
            else if (state == State.LAUNCH)
            {
                this.launchBehavior.IsActive = true;
                this.pickingBehavior.IsActive = false;
                this.pickingBehavior.Reset();
            }
        }
    }
}
