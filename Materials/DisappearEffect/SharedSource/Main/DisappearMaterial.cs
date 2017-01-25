#region Using Statements
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using WaveEngine.Common.Graphics;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
using WaveEngine.Materials.VertexFormats;
#endregion

namespace DisappearEffect
{
    public class DisappearMaterial : Material
    {
        private static ShaderTechnique[] techniques =
        {
            new ShaderTechnique("DissapearMaterialTechnique",
                "vsDisappear",
                "psDisappear",
                VertexPositionDualTexture.VertexFormat),
        };

        [StructLayout(LayoutKind.Explicit, Size = 16)]
        private struct DisappearMaterialParameters
        {
            [FieldOffset(0)]
            public float Threshold;

            [FieldOffset(4)]
            public float BurnSize;
        }

        private DisappearMaterialParameters shaderParameters;
        private string diffuseMapPath;
        private string opacityMapPath;
        private string burnMapPath;

        #region Properties

        public override string CurrentTechnique
        {
            get { return techniques[0].Name; }
        }

        public Texture DiffuseMap { get; set; }

        public Texture OpacityMap { get; set; }

        public Texture BurnMap { get; set; }

        public float Threshold { get; set; }

        public float BurnSize { get; set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="DisappearMaterial"/> class.
        /// </summary>
        /// <param name="diffuseMap">The diffuse map.</param>
        public DisappearMaterial(string diffuseMap, string opacityMap, string burnMap)
            : base(DefaultLayers.Opaque)
        {
            this.SamplerMode = AddressMode.LinearClamp;

            this.diffuseMapPath = diffuseMap;
            this.opacityMapPath = opacityMap;
            this.burnMapPath = burnMap;
            this.BurnSize = 10f;
            this.shaderParameters = new DisappearMaterialParameters();
            this.Parameters = this.shaderParameters;

            this.InitializeTechniques(techniques);
        }

        /// <summary>
        /// Initializes the specified assets.
        /// </summary>
        /// <param name="assets">The assets.</param>
        /// <exception cref="System.InvalidOperationException">Disappear Material needs a valid texture.</exception>
        public override void Initialize(AssetsContainer assets)
        {
            try
            {
                this.DiffuseMap = assets.LoadAsset<Texture2D>(this.diffuseMapPath);
                this.OpacityMap = assets.LoadAsset<Texture2D>(this.opacityMapPath);
                this.BurnMap = assets.LoadAsset<Texture2D>(this.burnMapPath);
            }
            catch (Exception)
            {
                throw new InvalidOperationException("DisappearMaterial needs a valid texture.");
            }
        }

        /// <summary>
        /// Sets the parameters.
        /// </summary>
        /// <param name="cached">if set to <c>true</c> [cached].</param>
        public override void SetParameters(bool cached)
        {
            base.SetParameters(cached);

            this.shaderParameters.Threshold = this.Threshold / 255f;
            this.shaderParameters.BurnSize = this.BurnSize / 100.0f;

            this.Parameters = shaderParameters;

            this.graphicsDevice.SetTexture(this.DiffuseMap, 0);
            this.graphicsDevice.SetTexture(this.OpacityMap, 1);
            this.graphicsDevice.SetTexture(this.BurnMap, 2);
        }
    }
}