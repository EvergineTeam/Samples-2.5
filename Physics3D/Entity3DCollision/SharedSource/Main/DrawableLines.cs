using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Framework.Graphics;

namespace Entity3DCollision
{
    [DataContract]
    public class DrawableLines : Drawable3D
    {
        private List<Line> lines;

        protected override void DefaultValues()
        {
            base.DefaultValues();

            this.lines = new List<Line>();
        }

        public DrawableLines()
            : base("DrawableLines")
        {
        }

        public Line AddNewLine(Vector3 startPosition, Vector3 endDisplacement)
        {
            Line l = new Line();

            l.StartPoint = startPosition;
            l.EndPoint = new Vector3(startPosition.X + endDisplacement.X,
                                       startPosition.Y + endDisplacement.Y,
                                       startPosition.Z + endDisplacement.Z);

            l.Color = Color.Red;
            this.lines.Add(l);

            if (this.lines.Count > 50)
            {
                this.lines.RemoveAt(0);
            }

            return l;
        }

        public override void Draw(TimeSpan gameTime)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                Line l = lines[i];
                this.RenderManager.LineBatch3D.DrawLine(ref l);
            }
        }

        protected override void Dispose(bool disposing)
        {
        }
    }
}
