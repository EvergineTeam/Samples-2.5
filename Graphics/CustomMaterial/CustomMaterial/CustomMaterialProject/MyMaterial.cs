using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Graphics.VertexFormats;
using WaveEngine.Common.Math;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;

namespace CustomMaterialProject
{
    public class MyMaterial : Material
    {
        private static ShaderTechnique[] techniques = 
        {
            new ShaderTechnique("MyMaterialTechnique",
                "MyMaterialvs",
                "MyMaterialps",
                VertexPositionNormalTexture.VertexFormat),
        };

        [StructLayout(LayoutKind.Sequential, Size = 16)]
        private struct MyMaterialParameters
        {
            public float Time;
        }

        private string diffuseMapPath;

        public Texture DiffuseMap
        {
            get;
            set;
        }

        public override string CurrentTechnique
        {
            get { return techniques[0].Name; }
        }

        private MyMaterialParameters shaderParameters = new MyMaterialParameters();

        public MyMaterial(string diffuseMap)
            : base(DefaultLayers.Opaque)
        {
            this.diffuseMapPath = diffuseMap;
            this.Parameters = this.shaderParameters;

            this.InitializeTechniques(techniques);
        }

        public override void Initialize(AssetsContainer assets)
        {
            try
            {
                this.DiffuseMap = assets.LoadAsset<Texture2D>(this.diffuseMapPath);
            }
            catch (Exception)
            {
                throw new InvalidOperationException("MyMaterial needs a valid texture.");
            }
        }

        public override void SetParameters(bool cached)
        {
            base.SetParameters(cached);

            this.shaderParameters.Time = (float)DateTime.Now.TimeOfDay.TotalSeconds;

            this.Parameters = shaderParameters;

            this.graphicsDevice.SetTexture(this.DiffuseMap, 0);
        }
    }
}
