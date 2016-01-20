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
    public class ModeBehavior : SceneBehavior
    {
        private Input inputService;
        private ScreenContextManager screenContextManager;
        private KeyboardState beforeKeyboardState;
        private bool diagnostics;
        private Vector3[] gravities;
        private int gravityIndex;
        

        public ModeBehavior()
            : base("ModeBehavior")
        {
            this.gravityIndex = 0;
            this.gravities = new Vector3[] { new Vector3(0,-14,0),
                                             new Vector3(14,0,0),
                                             new Vector3(-14,0,0),
                                             new Vector3(0,14,0)};

            this.diagnostics = false;
        }


        protected override void ResolveDependencies()
        {
            WaveServices.ScreenContextManager.SetDiagnosticsActive(this.diagnostics);
            this.inputService = WaveServices.Input;
            this.screenContextManager = WaveServices.ScreenContextManager;
        }

        protected override void Update(TimeSpan gameTime)
        {
            if (this.inputService.KeyboardState.IsConnected)
            {
                // Key F1
                if (this.inputService.KeyboardState.F1 == ButtonState.Pressed &&
                    beforeKeyboardState.F1 != ButtonState.Pressed)
                {
                    this.diagnostics = !this.diagnostics;
                    this.screenContextManager.SetDiagnosticsActive(this.diagnostics);
                    this.Scene.RenderManager.DebugLines = this.diagnostics;
                }
                // Key F5
                else if (this.inputService.KeyboardState.F5 == ButtonState.Pressed &&
                         beforeKeyboardState.F5 != ButtonState.Pressed)
                {
                    this.screenContextManager.To(new ScreenContext(new StockScene(WaveContent.Scenes.EmitterScene)));
                }
                // Key F6
                else if (this.inputService.KeyboardState.F6 == ButtonState.Pressed &&
                         beforeKeyboardState.F6 != ButtonState.Pressed)
                {
                    this.screenContextManager.To(new ScreenContext(new StockScene(WaveContent.Scenes.WallScene)));
                }
                // Key F7
                else if (this.inputService.KeyboardState.F7 == ButtonState.Pressed &&
                         beforeKeyboardState.F7 != ButtonState.Pressed)
                {
                    this.screenContextManager.To(new ScreenContext(new StockScene(WaveContent.Scenes.BridgeScene)));
                }
                // Key G
                else if (this.inputService.KeyboardState.G == ButtonState.Pressed &&
                         beforeKeyboardState.G != ButtonState.Pressed)
                {
                    gravityIndex++;
                    this.Scene.PhysicsManager.Gravity3D = gravities[gravityIndex % gravities.Length];
                }
                // Key H
                else if (inputService.KeyboardState.H == ButtonState.Pressed &&
                         beforeKeyboardState.H != ButtonState.Pressed)
                {
                    StockScene scene = this.Scene as StockScene;
                    scene.helpText.Entity.Enabled = !scene.helpText.Entity.Enabled;
                }
                // Key R
                else if (inputService.KeyboardState.R == ButtonState.Pressed &&
                         beforeKeyboardState.R != ButtonState.Pressed)
                {
                    StockScene scene = this.Scene as StockScene;
                    this.screenContextManager.To(new ScreenContext(new StockScene(scene.ScenePath)));
                }
            }

            beforeKeyboardState = inputService.KeyboardState;
        }
    }
}
