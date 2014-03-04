using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;

namespace LensFlareProject
{
    internal class SkyboxBehavior : Behavior
    {
        private static int instances;

        [RequiredComponent]
        public Transform3D Transform { get; set; }

        [RequiredComponent]
        public ModelRenderer ModelRenderer { get; set; }

        public SkyboxBehavior()
            : base("Skybox" + instances)
        {
            instances++;
        }

        protected override void Initialize()
        {
            base.Initialize();

            ModelRenderer.FrustumCullingEnabled = false;
        }

        protected override void Update(TimeSpan gameTime)
        {
            Transform.Position = RenderManager.Camera.Position;
        }
    }
}
