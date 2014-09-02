using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveEngine.Common.Graphics;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Managers;

namespace CityDemoProject
{
    public class CustomLayer : OpaqueLayer
    {
        public CustomLayer(RenderManager renderManager)
            : base(renderManager)
        {
        }

        protected override void SetDevice()
        {
            base.SetDevice();
            this.renderState.BlendMode = BlendMode.AlphaBlend;
        }

        protected override void RestoreDevice()
        {
            base.RestoreDevice();
        }        
    }
}
