#region Using Statements
using System;
using System.Runtime.Serialization;
using WaveEngine.Common.Attributes;
using WaveEngine.Common.Math;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
#endregion

namespace NinePatchSample.NinePatch
{
    [DataContract]
    public class NinePatchSprite : NinePatchBase
    {
        /// <summary>
        /// The texture path in use
        /// </summary>
        private string texturePathInUse;

        #region Properties
        /// <summary>
        /// Gets or sets the texture path.
        /// Such path is platform agnostic, and will always start with "Content/".
        /// Example: "Content/Characters/Tim.wpk"
        /// </summary>
        /// <value>
        ///     The texture path.
        /// </value>
        [RenderPropertyAsAsset(AssetType.Texture)]
        public new string TexturePath
        {
            get
            {
                return base.TexturePath;
            }

            set
            {
                base.TexturePath = value;
            }
        }
        #endregion

        #region Private Methods
        protected override void InternalLoadTexture(out string textureId, out Texture2D sourceTexture, out Rectangle sourceRecangle)
        {
            if (this.texturePathInUse != null)
            {
                throw new InvalidOperationException("Previous sprite must be unloaded first");
            }

            textureId = this.TexturePath;

            if (base.IsGlobalAsset)
            {
                sourceTexture = WaveServices.Assets.Global.LoadAsset<Texture2D>(this.TexturePath);
            }
            else
            {
                sourceTexture = this.Assets.LoadAsset<Texture2D>(this.TexturePath);
            }

            this.texturePathInUse = this.TexturePath;

            sourceRecangle = new Rectangle(0, 0, sourceTexture.Width, sourceTexture.Height);
        }

        protected override void InternalUnloadTexture()
        {
            if (base.IsGlobalAsset)
            {
                WaveServices.Assets.Global.UnloadAsset(this.texturePathInUse);
            }
            else
            {
                this.Assets.LoadAsset<Texture2D>(this.texturePathInUse);
            }

            this.texturePathInUse = null;
        }
        #endregion
    }
}
