using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Common.Physics2D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics2D;
using WaveEngine.Framework.Diagnostic;

namespace Bouyance
{
    [DataContract]
    public class BouyanceRenderer : Drawable2D
    {
        [RequiredComponent]
        private BouyanceBehavior behavior;        

        public override void Draw(TimeSpan gameTime)
        {
            if (this.behavior == null)
            {
                return;
            }

            foreach (var tuple in this.behavior.fixturePairs)
            {
                ICollider2D fixtureA = tuple.Item1;
                ICollider2D fixtureB = tuple.Item2;

                float density = fixtureA.Density;

                Vector2[] intersectionPoints;
                Vector2[] polygonPointsA = BouyanceBehavior.GetPolygonPoints(fixtureA);
                Vector2[] polygonPointsB = BouyanceBehavior.GetPolygonPoints(fixtureB);

                intersectionPoints = BouyanceBehavior.GetIntersectedPolygon(polygonPointsA, polygonPointsB);
                if (intersectionPoints.Length > 0)
                {
                    // find centroid
                    float area = 0;
                    Vector2 centroid = BouyanceBehavior.ComputeCentroid(intersectionPoints, out area);                    

                    this.RenderManager.LineBatch2D.DrawPoint(centroid, 10, Color.Blue, 0);
                    //this.DrawPolygon(polygonPointsA, Color.Yellow);
                    //this.DrawPolygon(polygonPointsB, Color.Purple);
                    this.DrawPolygon(intersectionPoints, Color.Red);
                }
            }
        }

        private void DrawPolygon(Vector2[] vertices, Color color)
        {
            for (var i = 0; i < vertices.Length - 1; i++)
            {
                this.DrawSegmentInternal(vertices[i], vertices[i + 1], ref color);
            }

            this.DrawSegmentInternal(vertices[0], vertices[vertices.Length - 1], ref color);
        }

        private void DrawSegmentInternal(Vector2 p1, Vector2 p2, ref Color color)
        {
            if (this.RenderManager.LineBatch2D != null)
            {
                this.RenderManager.LineBatch2D.DrawLine(ref p1, ref p2, ref color, 0);
            }
        }

        protected override void Dispose(bool disposing)
        {            
        }
    }
}
