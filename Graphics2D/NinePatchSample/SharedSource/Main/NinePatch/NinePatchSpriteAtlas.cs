#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using WaveEngine.Common.Attributes;
using WaveEngine.Common.Math;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
#endregion

namespace NinePatchSample.NinePatch
{
    [DataContract]
    public class NinePatchSpriteAtlas : NinePatchBase
    {
        /// <summary>
        /// The texture name.
        /// </summary>
        [DataMember]
        private string textureName;

        /// <summary>
        /// The texture index.
        /// </summary>
        private int textureIndex;
        
        #region Properties
        /// <summary>
        ///     Gets or sets the path to the atlas.
        /// </summary>
        /// <value>
        ///     The atlas path.
        /// </value>
        [RenderPropertyAsAsset(AssetType.Spritesheet)]
        public new string TexturePath
        {
            get { return base.TexturePath; }
            set { base.TexturePath = value; }
        }

        /// <summary>
        ///     Gets or sets the name of the texture from where this atlas is loaded.
        /// </summary>
        /// <value>
        ///     The name of the texture.
        /// </value>
        [RenderPropertyAsSelector("TextureNames")]
        public string TextureName
        {
            get
            {
                return this.textureName;
            }

            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.textureName = value;

                    if (this.isInitialized)
                    {
                        this.RefreshTextureIndex(true);
                    }
                }
            }
        }

        /// <summary>
        ///     Gets or sets the texture atlas.
        /// </summary>
        /// <value>
        ///     The texture atlas.
        /// </value>
        [DontRenderProperty]
        public SpriteSheet SpriteSheet { get; protected set; }


        /// <summary>
        /// Gets the texture names.
        /// </summary>
        /// <value>
        /// The texture names.
        /// </value>
        [DontRenderProperty]
        public IEnumerable<string> TextureNames
        {
            get
            {
                if (this.SpriteSheet != null)
                {
                    return this.SpriteSheet.SpriteDictionary
                                           .Keys
                                           .AsEnumerable<string>();
                }

                return null;
            }
        }
        #endregion

        protected override void Initialize()
        {
            base.Initialize();
        }

        #region Private Methods

        /// <summary>
        /// Refresh texture index from its name
        /// </summary>
        private void RefreshTextureIndex(bool refreshInnerTexture)
        {
            if (this.SpriteSheet != null)
            {
                bool setDefaultIndex = true;

                if (!string.IsNullOrEmpty(this.textureName))
                {
                    SpriteSheetResource resource;
                    setDefaultIndex = !this.SpriteSheet.SpriteDictionary.TryGetValue(this.textureName, out resource);

                    if (!setDefaultIndex)
                    {
                        this.textureIndex = resource.Index;
                        this.textureName = this.SpriteSheet.Sprites[this.textureIndex].Name;
                    }
                }

                if (setDefaultIndex)
                {
                    this.textureIndex = 0;
                    this.textureName = this.SpriteSheet.Sprites[this.textureIndex].Name;
                }

                this.RefreshSourceRectangle(refreshInnerTexture);
            }
        }

        /// <summary>
        /// Refresh the source rectangle of the sprite transform 2D.
        /// </summary>
        /// <exception cref="System.ObjectDisposedException">SpriteAtlas has been disposed.</exception>
        private void RefreshSourceRectangle(bool refreshInnerTexture)
        {
            if (this.SpriteSheet != null
             && this.SpriteSheet.Sprites.Length > 0)
            {
                if (this.textureIndex >= this.SpriteSheet.Sprites.Length)
                {
                    this.textureIndex = this.SpriteSheet.Sprites.Length - 1;
                }

                var sourceRectangle = this.SpriteSheet.Sprites[this.textureIndex].Rectangle;

                if (refreshInnerTexture)
                {
                    var textureId = this.TexturePath + this.textureName;

                    this.RefreshInnerTexture(textureId, this.SpriteSheet.Texture, sourceRectangle);
                }
            }
        }

        protected override void InternalLoadTexture(out string textureId, out Texture2D sourceTexture, out Rectangle sourceRecangle)
        {
            if(this.SpriteSheet != null)
            {
                throw new InvalidOperationException("Previous spritesheet must be unloaded first");
            }

            textureId = this.TexturePath + this.textureName;

            if (base.IsGlobalAsset)
            {
                this.SpriteSheet = WaveServices.Assets.Global.LoadAsset<SpriteSheet>(this.TexturePath);
            }
            else
            {
                this.SpriteSheet = this.Assets.LoadAsset<SpriteSheet>(this.TexturePath);
            }

            this.RefreshTextureIndex(refreshInnerTexture: false);

            sourceTexture = this.SpriteSheet.Texture;
            sourceRecangle = this.SpriteSheet.Sprites[this.textureIndex].Rectangle;
        }

        protected override void InternalUnloadTexture()
        {
            if (base.IsGlobalAsset)
            {
                WaveServices.Assets.Global.UnloadAsset(this.SpriteSheet.AssetPath);
            }
            else
            {
                this.Assets.UnloadAsset(this.SpriteSheet.AssetPath);
            }

            this.SpriteSheet = null;
        }
        #endregion
    }
}
