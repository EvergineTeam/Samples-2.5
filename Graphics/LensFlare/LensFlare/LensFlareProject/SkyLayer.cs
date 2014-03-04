using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaveEngine.Common.Graphics;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Managers;
using WaveEngine.Framework.Services;

namespace LensFlareProject
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
