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
using WaveEngine.Framework;
using WaveEngine.Framework.Services;
using WaveEngine.Components.Graphics3D;

#endregion

namespace PrimitivesProject
{
    public class ChangeModel : Behavior
    {
        private bool pressed;
        private int index;

        #region Initialize
        public ChangeModel()
            : base("ModelChange")
        {
        }
        #endregion

        #region Private Model
        protected override void Update(TimeSpan gameTime)
        {
            if (WaveServices.Input.TouchPanelState.Count > 0)
            {
                if (!pressed)
                {
                    pressed = true;
                    SwitchModel();
                }

            }
            else
            {
                pressed = false;
            }

        }

        private void SwitchModel()
        {
            Owner.RemoveComponent<Model>();

            index = ++index % 9;

            switch (index)
            {
                case 0:
                    Owner.AddComponent(Model.CreateCube());
                    break;
                case 1:
                    Owner.AddComponent(Model.CreateCone());
                    break;
                case 2:
                    Owner.AddComponent(Model.CreateTorus());
                    break;
                case 3:
                    Owner.AddComponent(Model.CreateSphere());
                    break;
                case 4:
                    Owner.AddComponent(Model.CreatePyramid());
                    break;
                case 5:
                    Owner.AddComponent(Model.CreateCylinder());
                    break;
                case 6:
                    Owner.AddComponent(Model.CreateCapsule());
                    break;
                case 7:
                    Owner.AddComponent(Model.CreateTeapot());
                    break;
                case 8:
                    Owner.AddComponent(Model.CreatePlane());
                    break;
                default:
                    throw new InvalidOperationException("Index invalid.");
            }

            Owner.RefreshDependencies();
        }
        #endregion
    }
}
