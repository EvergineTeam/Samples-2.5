#region Using Statements
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
#endregion

namespace MixedRealitySample.Drawables
{
    public enum LineType
    {
        LineStrip,
        LineList
    }

    public struct LineVertexInfo 
    {
        public Vector3 Position;
        public float Size;
        public Color Color;
        public Vector2 UVShift;     
    }
}
