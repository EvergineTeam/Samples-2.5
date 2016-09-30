#region Using Statements
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using WaveEngine.Common.Attributes;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
using WaveEngine.Materials;
#endregion

namespace NinePatchSample.NinePatch
{
    [DataContract]
    public abstract class NinePatchBase : Component
    {
        private static Dictionary<string, NinePatchTextureInfo> innerTextureInfoCache = new Dictionary<string, NinePatchTextureInfo>();

        /// <summary>
        /// Required 2D transform.
        /// See <see cref="Transform2D"/> for more information.
        /// </summary>
        [RequiredComponent]
        protected Transform2D Transform2D;
        
        /// <summary>
        /// The texture is loaded in global asset manager.
        /// </summary>
        [DataMember]
        private bool isGlobalAsset;

        /// <summary>
        /// Texture path
        /// </summary>
        [DataMember]
        private string texturePath;

        /// <summary>
        /// The size
        /// </summary>
        [DataMember]
        private Vector2 size;

        /// <summary>
        /// The nine patch texture info
        /// </summary>
        private NinePatchTextureInfo ninePatchTexture;

        /// <summary>
        /// The disposed
        /// </summary>
        protected bool disposed;

        /// <summary>
        /// Sprite material
        /// </summary>
        private Material material;
        
        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether this asset is global.
        /// By "global" it is meant this asset will be consumed anywhere else. It implies 
        /// once this component is disposed, the asset it-self will not be unload from memory.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this asset is global; otherwise, <c>false</c>.
        /// </value>
        public bool IsGlobalAsset
        {
            get
            {
                return this.isGlobalAsset;
            }

            set
            {
                if (this.isInitialized)
                {
                    throw new InvalidOperationException("Asset has already initialized.");
                }

                this.isGlobalAsset = value;
            }
        }

        /// <summary>
        /// Gets or sets the texture path.
        /// Such path is platform agnostic, and will always start with "Content/".
        /// Example: "Content/Characters/Tim.wpk"
        /// </summary>
        /// <value>
        ///     The texture path.
        /// </value>
        [DontRenderProperty]
        public string TexturePath
        {
            get
            {
                return this.texturePath;
            }

            set
            {
                this.texturePath = value;

                if (this.isInitialized)
                {
                    this.RefreshTexture();
                }
            }
        }

        /// <summary>
        /// Gets or sets the tint color.
        /// Each pixel of the sprite will be multiplied by such color during the drawing.
        /// By default, it is white.
        /// </summary>
        /// <value>
        /// The tint color.
        /// </value>
        [DataMember]
        public Color TintColor { get; set; }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        /// <value>
        /// The size.
        /// </value>
        public Vector2 Size
        {
            get
            {
                return this.size;
            }

            set
            {
                this.size = value;

                if (this.isInitialized)
                {
                    this.UpdateTransformRectangleSize();
                }
            }
        }

        /// <summary>
        /// Gets or sets the material used to render the sprite
        /// </summary>
        [DontRenderProperty]
        public Material Material
        {
            get
            {
                return this.material;
            }
        }

        /// <summary>
        /// The stretchable rectangle
        /// </summary>
        [DontRenderProperty]
        public RectangleF StretchableRectangle
        {
            get
            {
                if (this.ninePatchTexture != null)
                {
                    return this.ninePatchTexture.StretchableRectangle;
                }
                else
                {
                    return RectangleF.Empty;
                }
            }
        }

        /// <summary>
        /// The stretchable rectangle coordinates
        /// </summary>
        [DontRenderProperty]
        public RectangleF StretchableRectangleCoords
        {
            get
            {
                if (this.ninePatchTexture != null)
                {
                    return this.ninePatchTexture.StretchableRectangleCoords;
                }
                else
                {
                    return RectangleF.Empty;
                }
            }
        }
        #endregion

        #region Initialize
        /// <summary>
        /// The default values
        /// </summary>
        protected override void DefaultValues()
        {
            base.DefaultValues();

            this.isGlobalAsset = false;
            this.TintColor = Color.White;
        }

        /// <summary>
        /// Performs further custom initialization for this instance.
        /// </summary>
        protected override void Initialize()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException("NinePatchBase");
            }

            this.InitMaterial();
            this.LoadTexture();
            this.UpdateTransformRectangleSize();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Updates the size of the transform's rectangle.
        /// </summary>
        private void UpdateTransformRectangleSize()
        {
            var rectangle = this.Transform2D.Rectangle;
            rectangle.Width = this.size.X;
            rectangle.Height = this.size.Y;
            this.Transform2D.Rectangle = rectangle;
        }

        /// <summary>
        /// Refresh the sprite texture
        /// </summary>
        private void RefreshTexture()
        {
            this.UnloadTexture();
            this.LoadTexture();
        }

        protected abstract void InternalLoadTexture(out string textureId, out Texture2D sourceTexture, out Rectangle sourceRecangle);

        protected abstract void InternalUnloadTexture();

        protected void RefreshInnerTexture(string textureId, Texture2D sourceTexture, Rectangle sourceRecangle)
        {
            this.UnloadTexture(innerTextureOnly: true);
            this.UpdateNinePatchData(textureId, sourceTexture, sourceRecangle);
        }

        private void LoadTexture()
        {
            if (this.ninePatchTexture == null
             && !string.IsNullOrEmpty(this.texturePath))
            {
                Texture2D sourceTexture;
                Rectangle sourceRecangle;
                string innerTextureId;

                this.InternalLoadTexture(out innerTextureId, out sourceTexture, out sourceRecangle);

                this.UpdateNinePatchData(innerTextureId, sourceTexture, sourceRecangle);
            }
        }

        /// <summary>
        /// Unload texture
        /// </summary>
        private void UnloadTexture(bool innerTextureOnly = false)
        {
            if (this.ninePatchTexture != null)
            {
                //WaveServices.GraphicsDevice.Textures.DestroyTexture(this.ninePatchTexture.InnerTexture);
                this.ninePatchTexture = null;

                if (!innerTextureOnly)
                {
                    this.InternalUnloadTexture();
                }
            }
        }

        /// <summary>
        /// Updates the strechable rectangles and inner texture.
        /// </summary>
        /// <param name="textureId">The nine patch texture id</param>
        /// <param name="sourceTexture">The source texture.</param>
        /// <param name="sourceRectangle">The source rectangle.</param>
        private void UpdateNinePatchData(string textureId, Texture sourceTexture, Rectangle sourceRectangle)
        {
            NinePatchTextureInfo textureInfo;

            if (innerTextureInfoCache.TryGetValue(textureId, out textureInfo))
            {
                this.ninePatchTexture = textureInfo;
            }
            else
            {
                var stretchableRectangle = RectangleF.Empty;
                var stretchableRectangleCoords = RectangleF.Empty;

                var innerTexture = new Texture2D()
                {
                    Width = sourceRectangle.Width - 2,
                    Height = sourceRectangle.Height - 2,
                    Format = PixelFormat.R8G8B8A8,
                    Levels = 1,
                    Data = new byte[1][][]
                };

                var textureData = sourceTexture.GetData();

                // Horizontal stretchable area search
                int index = 0;
                bool startFound = false;
                var rectangleOffset = (sourceTexture.Width * sourceRectangle.Y) + sourceRectangle.X;
                for (; index < sourceRectangle.Width; index++)
                {
                    var pixelColor = textureData[rectangleOffset + index];

                    if (startFound ^ (pixelColor == Color.Black))
                    {
                        if (!startFound)
                        {
                            stretchableRectangle.X = index - 1;
                            stretchableRectangleCoords.X = stretchableRectangle.X / innerTexture.Width;
                            startFound = true;
                        }
                        else
                        {
                            stretchableRectangle.Width = index - stretchableRectangle.X - 1;
                            stretchableRectangleCoords.Width = stretchableRectangle.Width / innerTexture.Width;
                            break;
                        }
                    }
                }

                // Vertical stretchable area search
                index = 0;
                startFound = false;
                for (; index < sourceRectangle.Height; index++)
                {
                    var pixelColor = textureData[rectangleOffset + (index * sourceTexture.Width)];

                    if (startFound ^ (pixelColor == Color.Black))
                    {
                        if (!startFound)
                        {
                            stretchableRectangle.Y = index - 1;
                            stretchableRectangleCoords.Y = stretchableRectangle.Y / innerTexture.Height;
                            startFound = true;
                        }
                        else
                        {
                            stretchableRectangle.Height = index - stretchableRectangle.Y - 1;
                            stretchableRectangleCoords.Height = stretchableRectangle.Height / innerTexture.Height;

                            break;
                        }
                    }
                }

                // Fill innerTexture data
                innerTexture.Data[0] = new byte[1][];
                innerTexture.Data[0][0] = new byte[4 * innerTexture.Width * innerTexture.Height];

                int destinationIndex = 0;
                for (int i = 1; i < sourceRectangle.Height - 1; i++)
                {
                    for (int j = 1; j < sourceRectangle.Width - 1; j++)
                    {
                        var pixelColor = textureData[rectangleOffset + (i * (sourceTexture.Width)) + j];

                        innerTexture.Data[0][0][destinationIndex++] = pixelColor.R;
                        innerTexture.Data[0][0][destinationIndex++] = pixelColor.G;
                        innerTexture.Data[0][0][destinationIndex++] = pixelColor.B;
                        innerTexture.Data[0][0][destinationIndex++] = pixelColor.A;
                    }
                }

                WaveServices.GraphicsDevice.Textures.UploadTexture(innerTexture);

                this.ninePatchTexture = new NinePatchTextureInfo()
                {
                    InnerTexture = innerTexture,
                    StretchableRectangle = stretchableRectangle,
                    StretchableRectangleCoords = stretchableRectangleCoords
                };
                innerTextureInfoCache.Add(textureId, this.ninePatchTexture);
            }

            (this.material as StandardMaterial).Diffuse = this.ninePatchTexture.InnerTexture;

            if (this.size == Vector2.Zero)
            {
                this.size = new Vector2(this.ninePatchTexture.InnerTexture.Width, this.ninePatchTexture.InnerTexture.Height);
            }
        }

        /// <summary>
        /// Init material
        /// </summary>
        private void InitMaterial()
        {
            if (this.material == null)
            {
                var innerTexture = this.ninePatchTexture != null ? this.ninePatchTexture.InnerTexture : null;

                this.material = new StandardMaterial()
                {
                    Diffuse = innerTexture,
                    SamplerMode = AddressMode.LinearClamp,
                    LightingEnabled = false,
                    VertexColorEnable = true
                };

                AssetsContainer assets;

                if (this.IsGlobalAsset)
                {
                    assets = WaveServices.Assets.Global;
                }
                else
                {
                    assets = this.Assets;
                }

                this.material.Initialize(assets);
            }
        }

        /// <summary>
        ///     Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing">
        ///     <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.UnloadTexture();

                    this.disposed = true;
                }
            }
        }
        #endregion
    }
}
