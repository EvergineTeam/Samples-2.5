using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Managers;

namespace ParallaxCamera2D.Layers
{
    public class ForegroundLayer : AlphaLayer
    {
        public ForegroundLayer(RenderManager renderManager)
            :base(renderManager)
        {
        }

        protected override void SetDevice()
        {
            base.SetDevice();

            this.renderState.DepthMode = WaveEngine.Common.Graphics.DepthMode.None;
        }
    }
}
