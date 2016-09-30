#region Using Statements
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
#endregion

namespace NinePatchSample.NinePatch
{
    public class NinePatchTextureInfo
    {
        /// <summary>
        /// The inner texture
        /// </summary>
        public Texture InnerTexture { get; set; }

        /// <summary>
        /// The stretchable rectangle
        /// </summary>
        public RectangleF StretchableRectangle { get; set; }

        /// <summary>
        /// The stretchable rectangle coordinates
        /// </summary>
        public RectangleF StretchableRectangleCoords { get; set; }
    }
}
