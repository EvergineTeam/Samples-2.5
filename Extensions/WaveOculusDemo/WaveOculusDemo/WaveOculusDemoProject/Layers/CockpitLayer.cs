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
    /// Layer used to draw the cockpit mesh
    /// </summary>
    public class CockpitLayer : AlphaLayer
    {
        public CockpitLayer(RenderManager renderManager)
            : base(renderManager)
        {
        }
    }
}
