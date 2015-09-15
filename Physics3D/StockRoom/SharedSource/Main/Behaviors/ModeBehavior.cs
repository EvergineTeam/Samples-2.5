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
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Services; 
#endregion

namespace StockRoom.Behaviors
{
    [DataContract(Namespace = "StockRoom.Behaviors")]
    public class ModeBehavior : Behavior
    {
        enum ModeState
        {
            EMITER,
            WALL,
            BRIDGE
        }

        [RequiredComponent]
        private BridgeBehavior bridgeBehavior;

        [RequiredComponent]
        private StackBehavior stackBehavior;

        [RequiredComponent]
        private EmiterBehavior emiterBehavior;
        
        private Input inputService;
        private KeyboardState beforeKeyboardState;
        private bool diagnostics;
        private ModeState modeState;
        private Vector3[] gravities;
        private int gravityIndex;

        public ModeBehavior()
            : base("ModeBehavior")
        {
        }
        protected override void DefaultValues()
        {
            base.DefaultValues();
            this.diagnostics = false;
            this.modeState = ModeState.EMITER;
            this.gravityIndex = 0;
            this.gravities = new Vector3[] { new Vector3(0,-120,0),
                                                        new Vector3(120,0,0),
                                                        new Vector3(-120,0,0),
                                                        new Vector3(0,120,0)};
        }

        protected override void Initialize()
        {
            base.Initialize();
            UpdateMode(this.modeState);
            WaveServices.ScreenContextManager.SetDiagnosticsActive(this.diagnostics);
        }

        protected override void Update(TimeSpan gameTime)
        {
            inputService = WaveServices.Input;

            if (inputService.KeyboardState.IsConnected)
            {
                // Key F1
                if (inputService.KeyboardState.F1 == ButtonState.Pressed &&
                    beforeKeyboardState.F1 != ButtonState.Pressed)
                {
                    this.diagnostics = !this.diagnostics;
                    WaveServices.ScreenContextManager.SetDiagnosticsActive(this.diagnostics);
                    this.Owner.Scene.RenderManager.DebugLines = this.diagnostics;
                }
                // Key F5
                else if (this.modeState != ModeState.EMITER &&
                         inputService.KeyboardState.F5 == ButtonState.Pressed &&
                         beforeKeyboardState.F5 != ButtonState.Pressed)
                {
                    this.modeState = ModeState.EMITER;
                    this.UpdateMode(this.modeState);
                }
                // Key F6
                else if (this.modeState != ModeState.WALL &&
                         inputService.KeyboardState.F6 == ButtonState.Pressed &&
                         beforeKeyboardState.F6 != ButtonState.Pressed)
                {
                    this.modeState = ModeState.WALL;
                    this.UpdateMode(this.modeState);
                }
                // Key F7
                else if (this.modeState != ModeState.BRIDGE &&
                         inputService.KeyboardState.F7 == ButtonState.Pressed &&
                         beforeKeyboardState.F7 != ButtonState.Pressed)
                {
                    this.modeState = ModeState.BRIDGE;
                    this.UpdateMode(this.modeState);
                }
                // Key G
                else if (inputService.KeyboardState.G == ButtonState.Pressed &&
                         beforeKeyboardState.G != ButtonState.Pressed)
                {
                    gravityIndex++;
                    this.Owner.Scene.PhysicsManager.Gravity3D = gravities[gravityIndex % gravities.Length];
                }
                // Key H
                else if (inputService.KeyboardState.H == ButtonState.Pressed &&
                         beforeKeyboardState.H != ButtonState.Pressed)
                {
                    MyScene scene = this.Owner.Scene as MyScene;
                    scene.helpText.Entity.Enabled = !scene.helpText.Entity.Enabled;
                }
            }

            beforeKeyboardState = inputService.KeyboardState;
        }

        private void UpdateMode(ModeState modeState)
        {
            if (modeState == ModeState.BRIDGE)
            {
                this.bridgeBehavior.IsActive = true;
                this.stackBehavior.IsActive = false;
                this.emiterBehavior.IsActive = false;
                this.stackBehavior.RemoveWall();
                this.emiterBehavior.ResetEmiter();
            }
            else if (modeState == ModeState.EMITER)
            {
                this.emiterBehavior.IsActive = true;
                this.bridgeBehavior.IsActive = false;
                this.stackBehavior.IsActive = false;
                this.stackBehavior.RemoveWall();
                this.bridgeBehavior.Reset();
            }
            else if (modeState == ModeState.WALL)
            {
                this.stackBehavior.IsActive = true;
                this.bridgeBehavior.IsActive = false;
                this.emiterBehavior.IsActive = false;
                this.emiterBehavior.ResetEmiter();
                this.bridgeBehavior.Reset();
            }
        }
    }
}
