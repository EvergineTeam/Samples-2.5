using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaveEngine.Framework.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Common.Graphics;

namespace ProjectProject
{
    public class DrawableLine: Drawable3D
    {
        private Line line;

        public DrawableLine()
            : base("DrawableLine", Layer.Opaque)
        {
            line = new Line();
            line.StartColor = Color.Yellow;
            line.EndColor = Color.Yellow;
        }

        public override void Draw(TimeSpan gameTime)
        {
            RenderManager.LineBatch.DrawLine(ref line);
        }

        protected override void Dispose(bool disposing)
        {

        }

        public void SetLinePosition(ref Vector3 startPoint, ref Vector3 endPoint)
        {
            line.StartPoint = startPoint;
            line.EndPoint = endPoint;
        }
    }
}
