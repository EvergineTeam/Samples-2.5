#region Using Statements
using System;
using System.Runtime.InteropServices;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
#endregion

namespace MixedRealitySample.Materials
{
    public class LineMaterial : Material
    {
        /// <summary>
        /// The techniques
        /// </summary>
        private static ShaderTechnique[] techniques =
        {
            new ShaderTechnique("LineMaterial", "vsLineMaterial", "psLineMaterial", LineVertexFormat.VertexFormat),

            // NOISE
            new ShaderTechnique("LineMaterialNoise", "vsLineMaterial", "psLineMaterial", LineVertexFormat.VertexFormat, new string[] { "NOISE" }),
            
            // UVSHIFT
            new ShaderTechnique("LineMaterialUVShift", "vsLineMaterial", "psLineMaterial", LineVertexFormat.VertexFormat, new string[] { "UVSHIFT" }),
            new ShaderTechnique("LineMaterialNoiseUVShift", "vsLineMaterial", "psLineMaterial", LineVertexFormat.VertexFormat, new string[] { "NOISE", "UVSHIFT" }),

            // APPEARING
            new ShaderTechnique("LineMaterialAppearing", "vsLineMaterial", "psLineMaterial", LineVertexFormat.VertexFormat, new string[] { "APPEARING" }),
            new ShaderTechnique("LineMaterialAppearingNoise", "vsLineMaterial", "psLineMaterial", LineVertexFormat.VertexFormat, new string[] { "APPEARING", "NOISE" }),

            new ShaderTechnique("LineMaterialAppearingUVShift", "vsLineMaterial", "psLineMaterial", LineVertexFormat.VertexFormat, new string[] { "APPEARING", "UVSHIFT" }),
            new ShaderTechnique("LineMaterialAppearingNoiseUVShift", "vsLineMaterial", "psLineMaterial", LineVertexFormat.VertexFormat, new string[] { "APPEARING", "NOISE", "UVSHIFT" }),

            // BIAS
            new ShaderTechnique("LineMaterialBias", "vsLineMaterial", "psLineMaterial", LineVertexFormat.VertexFormat, new string[] { "BIAS" }),
            new ShaderTechnique("LineMaterialBiasNoise", "vsLineMaterial", "psLineMaterial", LineVertexFormat.VertexFormat, new string[] { "BIAS", "NOISE" }),

            new ShaderTechnique("LineMaterialBiasUVShift", "vsLineMaterial", "psLineMaterial", LineVertexFormat.VertexFormat, new string[] { "BIAS", "UVSHIFT" }),
            new ShaderTechnique("LineMaterialBiasNoiseUVShift", "vsLineMaterial", "psLineMaterial", LineVertexFormat.VertexFormat, new string[] { "BIAS", "NOISE", "UVSHIFT" }),
                                        
            new ShaderTechnique("LineMaterialBiasAppearing", "vsLineMaterial", "psLineMaterial", LineVertexFormat.VertexFormat, new string[] { "BIAS", "APPEARING" }),
            new ShaderTechnique("LineMaterialBiasAppearingNoise", "vsLineMaterial", "psLineMaterial", LineVertexFormat.VertexFormat, new string[] { "BIAS", "APPEARING", "NOISE" }),

            new ShaderTechnique("LineMaterialBiasAppearingUVShift", "vsLineMaterial", "psLineMaterial", LineVertexFormat.VertexFormat, new string[] { "BIAS", "APPEARING", "UVSHIFT" }),
            new ShaderTechnique("LineMaterialBiasAppearingNoiseUVShift", "vsLineMaterial", "psLineMaterial", LineVertexFormat.VertexFormat, new string[] { "BIAS", "APPEARING", "NOISE", "UVSHIFT" }),
        };

        public override string CurrentTechnique
        {
            get
            {
                string technique = "LineMaterial";

                if (this.Bias != 0)
                {
                    technique += "Bias";
                }

                if (this.AppearingTexture != null)
                {
                    technique += "Appearing";
                }

                if (this.NoiseTexture != null)
                {
                    technique += "Noise";
                }

                if (this.UVShiftTexture != null)
                {
                    technique += "UVShift";
                }

                return technique;             
            }
        }

        public string TexturePath { get; set; }

        public Texture Texture { get; set; }

        public string NoiseTexturePath { get; set; }

        public Texture NoiseTexture { get; set; }

        public string UVShiftTexturePath { get; set; }

        public Texture UVShiftTexture { get; set; }

        public string AppearingTexturePath { get; set; }

        public Texture AppearingTexture { get; set; }

        public float Time { get; set; }


        public Vector3 NoiseScale { get; set; }

        public Vector3 TimeFactor { get; set; }

        public Vector2 TextureOffset { get; set; }
        public Vector2 TextureScale { get; set; }

        public float UVShiftScale { get; set; }

        public Vector2 UVShiftTimeFactor { get; set; }

        public Color TintColor { get; set; }

        public float Bias { get; set; }

        // Animation
        public float AnimationPlaybackTime { get; set; }

        public Vector3 AnimationCenter { get; set; }

        public float AnimationDisplace { get; set; }

        public float AnimationScale { get; set; }
        public float AnimationPlaybackRate { get; set; }

        public float AnimationRotationFactor { get; set; }

        #region Shader Parameters
        /// <summary>
        /// Parameters for LineMaterial.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct LineMaterialParameters
        {
            public Vector3 CameraPosition;

            public float Time;

            public Vector3 NoiseScale;

            public float Bias;

            public Vector3 TimeFactor;

            public float UVShiftScale;

            public Vector2 UVShiftTimeFactor;
            
            public Vector2 TextureOffset;

            public Vector3 AnimationCenter;

            public float AnimationPlaybackTime;

            public float AnimationDisplace;

            public float AnimationScale;

            public float AnimationPlaybackRate;

            public float AnimationRotationFactor;

            public Vector2 TextureScale;

            public Vector2 Dummy0;

            public Vector4 TintColor;
        }

        /// <summary>
        /// Parameters for this shader.
        /// </summary>
        private LineMaterialParameters shaderParameters;
        #endregion

        public LineMaterial(int layerType)
            : base(layerType)
        {

            this.shaderParameters = new LineMaterialParameters();
            this.Parameters = this.shaderParameters;
            this.InitializeTechniques(techniques);
            this.NoiseScale = Vector3.One * 0.2f;
            this.TimeFactor = Vector3.One * 0.01f;
            this.TextureOffset = Vector2.Zero;
            this.TextureScale = Vector2.One;
            this.UVShiftScale = 0.3f;
            this.UVShiftTimeFactor = Vector2.One * 0.056f;

            this.TintColor = Color.White;

            this.Bias = 0;

            this.AnimationPlaybackTime = 0;
            this.AnimationCenter = Vector3.Zero;
            this.AnimationDisplace = -0.05f;
            this.AnimationScale = 0.4f;
            this.AnimationPlaybackRate = 4f;
            this.AnimationRotationFactor = 0.025f;

            //this.SamplerMode = AddressMode.LinearWrap;
        }

        public override void Initialize(WaveEngine.Framework.Services.AssetsContainer assets)
        {
            base.Initialize(assets);

            if (this.Texture == null && !string.IsNullOrEmpty(this.TexturePath))
            {
                this.Texture = assets.LoadAsset<Texture2D>(this.TexturePath);
            }

            if (this.NoiseTexture == null && !string.IsNullOrEmpty(this.NoiseTexturePath))
            {
                this.NoiseTexture = assets.LoadAsset<Texture2D>(this.NoiseTexturePath);
            }

            if (this.UVShiftTexture == null && !string.IsNullOrEmpty(this.UVShiftTexturePath))
            {
                this.UVShiftTexture = assets.LoadAsset<Texture2D>(this.UVShiftTexturePath);
            }

            if (this.AppearingTexture == null && !string.IsNullOrEmpty(this.AppearingTexturePath))
            {
                this.AppearingTexture = assets.LoadAsset<Texture2D>(this.AppearingTexturePath);
            }
        }

        public override void SetParameters(bool cached)
        {
            base.SetParameters(cached);

            if (!cached)
            {
                this.shaderParameters.CameraPosition = Vector3.Transform(this.renderManager.CurrentDrawingCamera3D.Position, Matrix.Invert(this.Matrices.World));

                this.shaderParameters.NoiseScale = this.NoiseScale;
                this.shaderParameters.Time = (float)WaveServices.Clock.TotalTime.TotalSeconds;
                this.shaderParameters.TimeFactor = this.TimeFactor;
                this.shaderParameters.UVShiftScale = this.UVShiftScale;
                this.shaderParameters.UVShiftTimeFactor = this.UVShiftTimeFactor;
                this.shaderParameters.TextureOffset = this.TextureOffset;
                this.shaderParameters.TextureScale = this.TextureScale;

                this.shaderParameters.Bias = this.Bias;

                this.shaderParameters.AnimationCenter = this.AnimationCenter;
                this.shaderParameters.AnimationDisplace = this.AnimationDisplace;
                this.shaderParameters.AnimationPlaybackTime = this.AnimationPlaybackTime;
                this.shaderParameters.AnimationScale = this.AnimationScale;
                this.shaderParameters.AnimationPlaybackRate = this.AnimationPlaybackRate;
                this.shaderParameters.AnimationRotationFactor = this.AnimationRotationFactor;

                this.shaderParameters.TintColor = this.TintColor.ToVector4();

                this.Parameters = this.shaderParameters;

                if (this.Texture != null)
                {
                    graphicsDevice.SetTexture(this.Texture, 0, TextureSlotUsage.PixelShader);
                }

                if (this.NoiseTexture != null)
                {
                    graphicsDevice.SetTexture(this.NoiseTexture, 1, TextureSlotUsage.VertexShader);
                }

                if (this.UVShiftTexture != null)
                {
                    graphicsDevice.SetTexture(this.UVShiftTexture, 2, TextureSlotUsage.VertexShader);
                }

                if (this.AppearingTexture != null)
                {
                    graphicsDevice.SetTexture(this.AppearingTexture, 3, TextureSlotUsage.VertexShader);
                }
            }
        }
    }
}
