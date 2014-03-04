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
using WaveEngine.Framework;
using WaveEngine.Framework.Services; 
#endregion

namespace StockRoomProject.Behaviors
{
    public class MySceneBehavior : SceneBehavior
    {
        enum SceneState
        {
            EMITER,
            WALL,
            BRIDGE
        }
        private Input inputService;
        private KeyboardState beforeKeyboardState;
        private bool diagnostics;
        private SceneState sceneState;
        private Vector3[] gravities = new Vector3[] { new Vector3(0,-120,0),
                                                        new Vector3(120,0,0),
                                                        new Vector3(-120,0,0),
                                                        new Vector3(0,120,0)};
        private int gravityIndex;

        public MySceneBehavior()
            : base("MySceneBehavior")
        {
            this.diagnostics = false;
            WaveServices.ScreenContextManager.SetDiagnosticsActive(this.diagnostics);
            this.sceneState = SceneState.EMITER;
            this.gravityIndex = 0;
        }

        protected override void ResolveDependencies()
        {

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
                    this.Scene.RenderManager.DebugLines = this.diagnostics;
                }
                // Key F5
                else if (this.sceneState != SceneState.EMITER &&
                         inputService.KeyboardState.F5 == ButtonState.Pressed &&
                         beforeKeyboardState.F5 != ButtonState.Pressed)
                {
                    this.RemoveBeforeBehaviors();

                    MyScene scene = this.Scene as MyScene;
                    scene.CreateEmiterBox();
                    this.sceneState = SceneState.EMITER;
                }
                // Key F6
                else if (this.sceneState != SceneState.WALL &&
                         inputService.KeyboardState.F6 == ButtonState.Pressed &&
                         beforeKeyboardState.F6 != ButtonState.Pressed)
                {
                    this.RemoveBeforeBehaviors();

                    MyScene scene = this.Scene as MyScene;
                    scene.CreateBoxStack();
                    this.sceneState = SceneState.WALL;
                }
                // Key F7
                else if (this.sceneState != SceneState.BRIDGE &&
                         inputService.KeyboardState.F7 == ButtonState.Pressed &&
                         beforeKeyboardState.F7 != ButtonState.Pressed)
                {
                    this.RemoveBeforeBehaviors();

                    MyScene scene = this.Scene as MyScene;                   
                    scene.CreateBridge();
                    this.sceneState = SceneState.BRIDGE;
                }
                // Key G
                else if (inputService.KeyboardState.G == ButtonState.Pressed &&
                         beforeKeyboardState.G != ButtonState.Pressed)
                {
                    gravityIndex++;
                    this.Scene.PhysicsManager.Gravity3D = gravities[gravityIndex % gravities.Length];
                }
                // Key H
                else if (inputService.KeyboardState.H == ButtonState.Pressed &&
                         beforeKeyboardState.H != ButtonState.Pressed)
                {
                    MyScene scene = this.Scene as MyScene;
                    scene.helpText.Entity.Enabled = !scene.helpText.Entity.Enabled;
                }
            }

            beforeKeyboardState = inputService.KeyboardState;
        }

        private void RemoveBeforeBehaviors()
        {
            MyScene scene = this.Scene as MyScene;

            // Remove EmiterBehavior
            if (scene.emiterBoxes != null)
            {
                EmiterBehavior emiter = scene.emiterBoxes.FindComponent<EmiterBehavior>();
                if (emiter != null)
                    emiter.ResetEmiter();
                scene.EntityManager.Remove(scene.emiterBoxes);
                scene.emiterBoxes = null;
            }

            // Remove StackBehavior
            if (scene.entityWall != null)
            {
                StackBehavior stack = scene.entityWall.FindComponent<StackBehavior>();
                if (stack != null)
                    stack.RemoveWall();
                scene.EntityManager.Remove(scene.entityWall);
                scene.entityWall = null;
            }

            // Remove BridgeBehavior
            if (scene.entityBridge != null)
            {
                BridgeBehavior bridge = scene.entityBridge.FindComponent<BridgeBehavior>();
                if (bridge != null)
                    bridge.Reset();
                scene.EntityManager.Remove(scene.entityBridge);
                scene.entityBridge = null;
            }
        }
    }
}
