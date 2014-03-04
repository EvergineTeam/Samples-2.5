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

#region Using Statements
using System;
using System.Collections.Generic;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
using WaveEngine.Common.Graphics;
using WaveEngine.Framework.Managers;
#endregion

namespace IBLSampleProject
{
    internal class SkyLayer : Layer
    {
        private Viewport skyBoxViewport;
        private Viewport oldViewport;

        #region Properties
        public SkyLayer(RenderManager renderManager)
            : base(renderManager)
        {
            // Create the skybox viewport (project depth to [0.999, 1] range)
            this.skyBoxViewport = new Viewport(0, 0, WaveServices.Platform.ScreenWidth, WaveServices.Platform.ScreenHeight, 0.999f, 1.0f);
        }
        #endregion

        #region Methods
        protected override void SetDevice()
        {
            // Get previous viewport
            this.oldViewport = this.renderManager.GraphicsDevice.RenderState.Viewport;

            // Set the skybox viewport
            this.renderState.Viewport = this.skyBoxViewport;

            // Disable Depth Write state
            this.renderState.DepthMode = DepthMode.Read;
        }

        protected override void RestoreDevice()
        {
            // Restore previous viewport
            this.renderState.Viewport = this.oldViewport;
        }
        #endregion
    }
}
