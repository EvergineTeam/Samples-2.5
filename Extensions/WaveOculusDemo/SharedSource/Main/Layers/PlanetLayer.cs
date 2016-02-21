using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveEngine.Common.Graphics;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Managers;

namespace WaveOculusDemoProject.Layers
{
    /// <summary>
    /// Layer used to draw background planet scene
    /// </summary>
    public class PlanetLayer : AlphaLayer
    {
        /// <summary>
        /// The old view port
        /// </summary>
        private Viewport oldViewport;

        public PlanetLayer(RenderManager renderManager)
            : base(renderManager)
        {
        }

        /// <summary>
        /// Sets the device.
        /// </summary>
        protected override void SetDevice()
        {
            // Get previous viewport
            this.oldViewport = this.renderManager.GraphicsDevice.RenderState.Viewport;

            this.renderState.CullMode = CullMode.None;

            // Set the skybox viewport
            this.renderState.Viewport = new Viewport(
                this.oldViewport.X,
                this.oldViewport.Y,
                this.oldViewport.Width,
                this.oldViewport.Height,
                0.999f,
                1);

            // Disable Depth Write state
            this.renderState.BlendMode = BlendMode.AlphaBlend;
            this.renderState.DepthMode = DepthMode.Read;
            this.renderState.DepthBias = DepthBias.Zero;
        }

        /// <summary>
        /// Restores the device.
        /// </summary>
        protected override void RestoreDevice()
        {
            // Restore previous viewport
            this.renderState.Viewport = this.oldViewport;
        }
    }
}
