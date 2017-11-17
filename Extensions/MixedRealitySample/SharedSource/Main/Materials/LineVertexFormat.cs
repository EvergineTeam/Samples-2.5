#region Using Statements
using System.Runtime.InteropServices;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
#endregion

namespace MixedRealitySample.Materials
{
    [StructLayout(LayoutKind.Explicit, Size = 48)]
    public struct LineVertexFormat : IBasicVertex
    {
        /// <summary>
        /// Vertex position.
        /// </summary>
        [FieldOffset(0)]
        public Vector3 Position;

        /// <summary>
        /// Vertex color.
        /// </summary>
        [FieldOffset(12)]
        public Color Color;

        /// <summary>
        /// Vertex texture coordinate.
        /// </summary>
        [FieldOffset(16)]
        public Vector2 TexCoord;

        [FieldOffset(24)]
        public Vector4 AxisSize;

        [FieldOffset(40)]
        public Vector2 UVShift;

        /// <summary>
        /// Vertex format.
        /// </summary>
        public static readonly VertexBufferFormat VertexFormat;

        #region Properties
        /// <summary>
        /// Gets the vertex format.
        /// </summary>
        VertexBufferFormat IBasicVertex.VertexFormat
        {
            get
            {
                return VertexFormat;
            }
        }
        #endregion

        #region Initialize

        /// <summary>
        /// Initializes static members of the <see cref="BillboardVertexFormat"/> struct.
        /// </summary>
        static LineVertexFormat()
        {
            VertexFormat = new VertexBufferFormat(new VertexElementProperties[]
                {
                    new VertexElementProperties(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                    new VertexElementProperties(12, VertexElementFormat.Color, VertexElementUsage.Color, 0),
                    new VertexElementProperties(16, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
                    new VertexElementProperties(24, VertexElementFormat.Vector4, VertexElementUsage.TextureCoordinate, 1),
                    new VertexElementProperties(40, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 2),
                });
        }
        #endregion
    }
}
