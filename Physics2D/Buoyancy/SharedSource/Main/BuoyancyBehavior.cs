#region Using Statements
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
#endregion

namespace Buoyancy
{
    [DataContract]
    public class BuoyancyBehavior : Behavior
    {
        private class Edge
        {
            public readonly Vector2 From;
            public readonly Vector2 To;

            public Edge(Vector2 from, Vector2 to)
            {
                this.From = from;
                this.To = to;
            }
        }

        public List<Tuple<ICollider2D, ICollider2D>> fixturePairs;
        private Vector2 gravity;     

        #region Properties

        [DataMember]
        public float DragMod { get; set; }

        [DataMember]
        public float LiftMod { get; set; }

        [DataMember]
        public float MaxDrag { get; set; }

        [DataMember]
        public float MaxLift { get; set; }

        [DataMember]
        public float AreaDivisor { get; set; }

        #endregion
        
        protected override void DefaultValues()
        {
            base.DefaultValues();

            this.fixturePairs = new List<Tuple<ICollider2D, ICollider2D>>();
            this.DragMod = 0.25f;
            this.LiftMod = 0.25f;
            this.MaxDrag = 2000;
            this.MaxLift = 500;
            this.AreaDivisor = 400;
        }

        protected override void Initialize()
        {
            base.Initialize();

            var simulate2D = this.Owner.Scene.PhysicsManager.Simulation2D;
            if (simulate2D != null)
            {
                this.gravity = simulate2D.Gravity;
                simulate2D.BeginCollision += Simulate2D_BeginCollision;
                simulate2D.EndCollision += Simulate2D_EndCollision;
            }
        }

        private void Simulate2D_BeginCollision(WaveEngine.Common.Physics2D.ICollisionInfo2D contact)
        {
            var fixtureA = contact.ColliderA;
            var fixtureB = contact.ColliderB;

            if (fixtureA.IsSensor && fixtureB.RigidBody.Type == WaveEngine.Common.Physics2D.RigidBodyType2D.Dynamic)
            {
                this.fixturePairs.Add(new Tuple<ICollider2D, ICollider2D>(fixtureA, fixtureB));
            }
            else if (fixtureB.IsSensor && fixtureA.RigidBody.Type == RigidBodyType2D.Dynamic)
            {
                this.fixturePairs.Add(new Tuple<ICollider2D, ICollider2D>(fixtureB, fixtureA));
            }
        }

        private void Simulate2D_EndCollision(WaveEngine.Common.Physics2D.ICollisionInfo2D contact)
        {
            var fixtureA = contact.ColliderA;
            var fixtureB = contact.ColliderB;

            if (fixtureA.IsSensor && fixtureB.RigidBody.Type == RigidBodyType2D.Dynamic)
            {
                var tuple = this.fixturePairs.Find(pair => pair.Item1 == fixtureA && pair.Item2 == fixtureB);
                this.fixturePairs.Remove(tuple);
            }
            else if (fixtureB.IsSensor && fixtureA.RigidBody.Type == RigidBodyType2D.Dynamic)
            {
                var tuple = this.fixturePairs.Find(pair => pair.Item1 == fixtureB && pair.Item2 == fixtureA);
                this.fixturePairs.Remove(tuple);
            }
        }

        protected override void Update(TimeSpan gameTime)
        {
            foreach (var tuple in this.fixturePairs)
            {
                ICollider2D fixtureA = tuple.Item1;
                ICollider2D fixtureB = tuple.Item2;

                float density = fixtureA.Density;

                Vector2[] intersectionPoints;
                Vector2[] polygonPointsA = GetPolygonPoints(fixtureA);
                Vector2[] polygonPointsB = GetPolygonPoints(fixtureB);

                intersectionPoints = GetIntersectedPolygon(polygonPointsA, polygonPointsB);
                if (intersectionPoints.Length > 0)
                {
                    // find centroid
                    float area = 0;
                    Vector2 centroid = ComputeCentroid(intersectionPoints, out area);
                    area = Math.Abs(area) / this.AreaDivisor;
                    Labels.Add("Area", area);

                    // Apply buoyancy force
                    float displacedMass = fixtureA.Density * area;
                    Vector2 buoyancyForce = displacedMass * -this.gravity;
                    fixtureB.RigidBody.ApplyForce(buoyancyForce, centroid);
                    Labels.Add("buoyancyForce", buoyancyForce);

                    // Apply complex drag                   
                    for (int i = 0; i < intersectionPoints.Length; i++)
                    {
                        Vector2 v0 = intersectionPoints[i];
                        Vector2 v1 = intersectionPoints[(i + 1) % intersectionPoints.Length];
                        Vector2 midPoint = 0.5f * (v0 + v1);

                        //find relative velocity between object and fluid at edge midpoint
                        Vector2 velDir = fixtureB.RigidBody.GetLinearVelocityFromWorldPoint(midPoint) - fixtureA.RigidBody.GetLinearVelocityFromWorldPoint(midPoint);
                        float vel = velDir.Normalize();

                        Vector2 edge = v1 - v0;
                        float edgeLength = edge.Normalize();
                        Vector2 normal = b2Cross(-1, ref edge);
                        float dragDot = Vector2.Dot(normal, velDir);
                        if (dragDot < 0)
                            continue;//normal points backwards - this is not a leading edge

                        //apply drag
                        float dragMag = dragDot * this.DragMod * edgeLength * density * vel * vel;
                        dragMag = Math.Min(dragMag, this.MaxDrag);
                        Vector2 dragForce = dragMag * -velDir;
                        if (!float.IsNaN(dragForce.X) && !float.IsNaN(dragForce.Y))
                        {
                            fixtureB.RigidBody.ApplyForce(dragForce, midPoint);
                            Labels.Add("dragForce", dragForce);
                        }

                        //apply lift
                        float liftDot = Vector2.Dot(edge, velDir);
                        float liftMag = dragDot * liftDot * this.LiftMod * edgeLength * density * vel * vel;
                        liftMag = Math.Min(liftMag, this.MaxLift);
                        Vector2 liftDir = b2Cross(1, ref velDir);
                        Vector2 liftForce = liftMag * liftDir;
                        if (!float.IsNaN(liftForce.X) && !float.IsNaN(liftForce.Y))
                        {
                            fixtureB.RigidBody.ApplyForce(liftForce, midPoint);
                            Labels.Add("liftForce", liftForce);
                        }
                    }
                }
            }
        }

        Vector2 b2Cross(float s, ref Vector2 a)
        {
            return new Vector2(-s * a.Y, s * a.X);
        }


        public static Vector2[] GetPolygonPoints(ICollider2D collider)
        {
            IPolygonColliderShape2D shape = (IPolygonColliderShape2D)collider.Shape;
            Vector2[] result = new Vector2[shape.Vertices.Length];
            var transform = ((Collider2D)collider.UserData).Owner.FindComponent<Transform2D>();
            for (int i = 0; i < shape.Vertices.Length; i++)
            {
                //result[i] = shape.Vertices[i];                
                Vector2 size = new Vector2(transform.Rectangle.Width, transform.Rectangle.Height);
                result[i] = Vector2.Transform(shape.Vertices[i] - (transform.Origin * size), transform.WorldTransform); //collider.RigidBody.GetWorldVector(shape.Vertices[i]);
            }

            return result;
        }

        /// <summary>
        /// This clips the subject polygon against the clip polygon (gets the intersection of the two polygons)
        /// </summary>
        /// <remarks>
        /// Based on the psuedocode from:
        /// http://en.wikipedia.org/wiki/Sutherland%E2%80%93Hodgman
        /// </remarks>
        /// <param name="subjectPoly">Can be concave or convex</param>
        /// <param name="clipPoly">Must be convex</param>
        /// <returns>The intersection of the two polygons (or null)</returns>
        public static Vector2[] GetIntersectedPolygon(Vector2[] subjectPoly, Vector2[] clipPoly)
        {
            if (subjectPoly.Length < 3 || clipPoly.Length < 3)
            {
                throw new ArgumentException(string.Format("The polygons passed in must have at least 3 Vector2s: subject={0}, clip={1}", subjectPoly.Length.ToString(), clipPoly.Length.ToString()));
            }

            List<Vector2> outputList = subjectPoly.ToList();

            //	Make sure it's clockwise
            if (!IsClockwise(subjectPoly))
            {
                outputList.Reverse();
            }

            //	Walk around the clip polygon clockwise
            foreach (Edge clipEdge in IterateEdgesClockwise(clipPoly))
            {
                List<Vector2> inputList = outputList.ToList();		//	clone it
                outputList.Clear();

                if (inputList.Count == 0)
                {
                    //	Sometimes when the polygons don't intersect, this list goes to zero.  Jump out to avoid an index out of range exception
                    break;
                }

                Vector2 S = inputList[inputList.Count - 1];

                foreach (Vector2 E in inputList)
                {
                    if (IsInside(clipEdge, E))
                    {
                        if (!IsInside(clipEdge, S))
                        {
                            Vector2? Vector2 = GetIntersect(S, E, clipEdge.From, clipEdge.To);
                            if (Vector2 == null)
                            {
                                throw new ApplicationException("Line segments don't intersect");		//	may be colinear, or may be a bug
                            }
                            else
                            {
                                outputList.Add(Vector2.Value);
                            }
                        }

                        outputList.Add(E);
                    }
                    else if (IsInside(clipEdge, S))
                    {
                        Vector2? Vector2 = GetIntersect(S, E, clipEdge.From, clipEdge.To);
                        if (Vector2 == null)
                        {
                            throw new ApplicationException("Line segments don't intersect");		//	may be colinear, or may be a bug
                        }
                        else
                        {
                            outputList.Add(Vector2.Value);
                        }
                    }

                    S = E;
                }
            }

            //	Exit Function
            return outputList.ToArray();
        }

        public static Vector2 ComputeCentroid(Vector2[] vs, out float area)
        {
            int count = (int)vs.Length;

            Vector2 c = Vector2.Zero;
            area = 0.0f;

            // pRef is the reference point for forming triangles.
            // It's location doesn't change the result (except for rounding error).
            Vector2 pRef = vs[0];

            const float inv3 = 1.0f / 3.0f;

            for (int i = 0; i < count; ++i)
            {
                // Triangle vertices.
                Vector2 p1 = pRef;
                Vector2 p2 = vs[i];
                Vector2 p3 = i + 1 < count ? vs[i + 1] : vs[0];

                Vector2 e1 = p2 - p1;
                Vector2 e2 = p3 - p1;

                float D = Vector2.Cross(e1, e2);

                float triangleArea = 0.5f * D;
                area += triangleArea;

                // Area weighted centroid
                c += triangleArea * inv3 * (p1 + p2 + p3);
            }

            // Centroid
            if (!IsNearZero(area))
                c *= 1.0f / area;
            else
                area = 0;
            return c;
        }

        /// <summary>
        /// This iterates through the edges of the polygon, always clockwise
        /// </summary>
        private static IEnumerable<Edge> IterateEdgesClockwise(Vector2[] polygon)
        {
            if (IsClockwise(polygon))
            {
                #region Already clockwise

                for (int cntr = 0; cntr < polygon.Length - 1; cntr++)
                {
                    yield return new Edge(polygon[cntr], polygon[cntr + 1]);
                }

                yield return new Edge(polygon[polygon.Length - 1], polygon[0]);

                #endregion
            }
            else
            {
                #region Reverse

                for (int cntr = polygon.Length - 1; cntr > 0; cntr--)
                {
                    yield return new Edge(polygon[cntr], polygon[cntr - 1]);
                }

                yield return new Edge(polygon[0], polygon[polygon.Length - 1]);

                #endregion
            }
        }

        /// <summary>
        /// Returns the intersection of the two lines (line segments are passed in, but they are treated like infinite lines)
        /// </summary>
        /// <remarks>
        /// Got this here:
        /// http://stackoverflow.com/questions/14480124/how-do-i-detect-triangle-and-rectangle-intersection
        /// </remarks>
        private static Vector2? GetIntersect(Vector2 line1From, Vector2 line1To, Vector2 line2From, Vector2 line2To)
        {
            Vector2 direction1 = line1To - line1From;
            Vector2 direction2 = line2To - line2From;
            double dotPerp = (direction1.X * direction2.Y) - (direction1.Y * direction2.X);

            // If it's 0, it means the lines are parallel so have infinite intersection Vector2s
            if (IsNearZero(dotPerp))
            {
                return null;
            }

            Vector2 c = line2From - line1From;
            double t = (c.X * direction2.Y - c.Y * direction2.X) / dotPerp;
            //if (t < 0 || t > 1)
            //{
            //    return null;		//	lies outside the line segment
            //}

            //double u = (c.X * direction1.Y - c.Y * direction1.X) / dotPerp;
            //if (u < 0 || u > 1)
            //{
            //    return null;		//	lies outside the line segment
            //}

            //	Return the intersection Vector2
            return line1From + ((float)t * direction1);
        }

        private static bool IsInside(Edge edge, Vector2 test)
        {
            bool? isLeft = IsLeftOf(edge, test);
            if (isLeft == null)
            {
                //	Colinear Vector2s should be considered inside
                return true;
            }

            return !isLeft.Value;
        }
        private static bool IsClockwise(Vector2[] polygon)
        {
            for (int cntr = 2; cntr < polygon.Length; cntr++)
            {
                bool? isLeft = IsLeftOf(new Edge(polygon[0], polygon[1]), polygon[cntr]);
                if (isLeft != null)		//	some of the Vector2s may be colinear.  That's ok as long as the overall is a polygon
                {
                    return !isLeft.Value;
                }
            }

            return false;
            //throw new ArgumentException("All the Vector2s in the polygon are colinear");
        }

        /// <summary>
        /// Tells if the test Vector2 lies on the left side of the edge line
        /// </summary>
        private static bool? IsLeftOf(Edge edge, Vector2 test)
        {
            Vector2 tmp1 = edge.To - edge.From;
            Vector2 tmp2 = test - edge.To;

            double x = (tmp1.X * tmp2.Y) - (tmp1.Y * tmp2.X);       //	dot product of perpendicular?

            if (x < 0)
            {
                return false;
            }
            else if (x > 0)
            {
                return true;
            }
            else
            {
                //	Colinear Vector2s;
                return null;
            }
        }

        private static bool IsNearZero(double testValue)
        {
            return Math.Abs(testValue) <= .000000001d;
        }
    }
}