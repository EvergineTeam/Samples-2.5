using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Managers;

namespace ParallaxCamera2D.Layers
{
    public class SunLayer : AdditiveLayer
    {
        public SunLayer(RenderManager renderManager)
            :base(renderManager)
        {
        }
    }
}
