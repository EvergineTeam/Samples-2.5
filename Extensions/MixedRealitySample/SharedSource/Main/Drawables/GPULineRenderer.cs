#region Using Statements
using MixedRealitySample.Materials;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using WaveEngine.Common.Attributes;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
#endregion

namespace MixedRealitySample.Drawables
{
    [DataContract]
    public class GPULineRenderer : Drawable3D
    {
        public event EventHandler OnPreDraw;

        private const int VerticesPerPoint = 2;
        private const int IndicesPerPoint = 3;
        private const int MaxLinePointsPerMesh = ushort.MaxValue / VerticesPerPoint;

        [RequiredComponent]
        protected Transform3D transform = null;

        private List<Mesh> meshes;
        private LineMaterial material;        
        private List<LineVertexInfo> lineVertices;

        [DataMember]
        private LineType lineType;

        [DataMember]
        public bool WorldSpace { get; set; }

        #region Properties

        [DontRenderProperty]
        public List<LineVertexInfo> LineVertices
        {
            get
            {
                return this.lineVertices;
            }

            set
            {
                this.SetLineVertices(value);
            }
        }

        public LineType LineType
        {
            get
            {
                return this.lineType;
            }

            set
            {
                this.lineType = value;
                if (this.isInitialized)
                {
                    this.RefreshMeshes();
                }
            }
        } 

        /// <summary>
        /// Line Material 
        /// </summary>
        [DontRenderProperty]
        public LineMaterial LineMaterial
        {
            get { return this.material; }
            set { this.material = value; }
        }

        #endregion

        /// <summary>
        /// Default values method
        /// </summary>
        protected override void DefaultValues()
        {
            base.DefaultValues();
            this.meshes = new List<Mesh>();
            this.WorldSpace = false;
            this.LineType = LineType.LineList;
        }

        /// <summary>
        /// Intialize method
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            this.RefreshMeshes();
        }

        /// <summary>
        /// Set vertices
        /// </summary>
        /// <param name="points"></param>
        /// <param name="lineType"></param>
        public void SetLineVertices(List<LineVertexInfo> points, bool sequential = true)
        {
            if (this.lineVertices == null)
            {
                this.lineVertices = new List<LineVertexInfo>();
            }
            else
            {
                this.lineVertices.Clear();
            }

            if (points.Count == 0)
            {
                this.lineVertices.Clear();
                this.DisposeMeshes();
            }
            else
            {

                if (sequential)
                {
                    for (int i = 0; i < points.Count; i++)
                    {
                        if (i >= 2)
                        {
                            this.lineVertices.Add(points[i - 1]);
                        }

                        this.lineVertices.Add(points[i]);
                    }
                }
                else
                {
                    for (int i = 0; i < points.Count; i++)
                    {                       
                        this.lineVertices.Add(points[i]);
                    }
                }

                if (this.isInitialized)
                {
                    this.RefreshMeshes();
                }
            }            
        }

        /// <summary>
        /// Refresh meshes method
        /// </summary>
        private void RefreshMeshes()
        {
            if (this.meshes.Count > 0)
            {
                this.DisposeMeshes();
            }

            if (this.lineVertices == null || this.lineVertices.Count == 0)
            {
                return;
            }            

            switch (this.lineType)
            {
                case LineType.LineStrip:
                    this.FillStripLines();
                    break;
                case LineType.LineList:
                    this.FillLineList();
                    break;
            }
        }

        /// <summary>
        /// Strip lines
        /// </summary>
        private void FillStripLines()
        {
            for (int startIndex = 0; startIndex < (lineVertices.Count - 1); startIndex += MaxLinePointsPerMesh)
            {
                int nPoints = Math.Min(MaxLinePointsPerMesh, lineVertices.Count - startIndex);

                // Set vertex buffer
                var vertices = new LineVertexFormat[nPoints * VerticesPerPoint];

                var startPoint = this.lineVertices[startIndex + 0];
                var endPoint = this.lineVertices[startIndex + 1];

                float uCoord = 0;

                this.AddVertex(ref startPoint, endPoint.Position - startPoint.Position, uCoord, 0, vertices);

                float curveLength = 0;
                for (int i = 1; i < nPoints; i++)
                {
                    startPoint = this.lineVertices[startIndex + i - 1];
                    endPoint = this.lineVertices[startIndex + i];

                    curveLength += Vector3.Distance(startPoint.Position, endPoint.Position);
                }

                float currentLength = 0;

                for (int i = 1; i < nPoints; i++)
                {
                    startPoint = this.lineVertices[startIndex + i - 1];
                    endPoint = this.lineVertices[startIndex + i];

                    currentLength += Vector3.Distance(startPoint.Position, endPoint.Position);

                    uCoord = currentLength / curveLength;

                    this.AddVertex(ref endPoint, endPoint.Position - startPoint.Position, uCoord, i * 2, vertices);
                }

                VertexBuffer vertexBuffer = new VertexBuffer(LineVertexFormat.VertexFormat);
                vertexBuffer.SetData(vertices);

                // Set index buffer
                var indices = new ushort[vertices.Length];

                for (int i = 1; i < vertices.Length; i++)
                {
                    indices[i] = (ushort)i;
                }

                IndexBuffer indexBuffer = new IndexBuffer(indices);

                this.meshes.Add(new Mesh(0, vertices.Length, 0, indices.Length - 2, vertexBuffer, indexBuffer, PrimitiveType.TriangleStrip));
            }
        }

        /// <summary>
        /// Triangle lines
        /// </summary>
        private void FillLineList()
        {
            for (int startIndex = 0; startIndex < lineVertices.Count - 1; startIndex += MaxLinePointsPerMesh)
            {
                int nPoints = Math.Min(MaxLinePointsPerMesh, lineVertices.Count - startIndex);

                // Set vertex buffer
                var vertices = new LineVertexFormat[nPoints * VerticesPerPoint];
                var indices = new ushort[nPoints * IndicesPerPoint];

                var startPoint = this.lineVertices[startIndex + 0];
                var endPoint = this.lineVertices[startIndex + 1];
                float uCoord = startIndex / (float)lineVertices.Count;
                Vector3 axis;
                int pointIndex = 0;
                int indexArray = 0;

                for (int i = 0; i < nPoints - 1; i += 2)
                {
                    startPoint = this.lineVertices[startIndex + i];
                    endPoint = this.lineVertices[startIndex + i + 1];
                    axis = endPoint.Position - startPoint.Position;

                    int vertexIndex = pointIndex;

                    // Vertices 0 - 1
                    this.AddVertex(ref startPoint, axis, 0, pointIndex, vertices);
                    pointIndex += 2;

                    // Vertices 2 - 3
                    this.AddVertex(ref endPoint, axis, 1, pointIndex, vertices);
                    pointIndex += 2;

                    indices[indexArray + 0] = (ushort)(vertexIndex + 0);
                    indices[indexArray + 1] = (ushort)(vertexIndex + 1);
                    indices[indexArray + 2] = (ushort)(vertexIndex + 2);

                    indices[indexArray + 3] = (ushort)(vertexIndex + 2);
                    indices[indexArray + 4] = (ushort)(vertexIndex + 1);
                    indices[indexArray + 5] = (ushort)(vertexIndex + 3);
                    indexArray += IndicesPerPoint * 2;
                }

                VertexBuffer vertexBuffer = new VertexBuffer(LineVertexFormat.VertexFormat);
                vertexBuffer.SetData(vertices);

                IndexBuffer indexBuffer = new IndexBuffer(indices);

                this.meshes.Add(new Mesh(0, vertices.Length, 0, indices.Length / 3, vertexBuffer, indexBuffer, PrimitiveType.TriangleList));
            }
        }

        /// <summary>
        /// Add vertex method
        /// </summary>
        /// <param name="info"></param>
        /// <param name="axis"></param>
        /// <param name="uCoord"></param>
        /// <param name="vertexIndex"></param>
        /// <param name="vertices"></param>
        private void AddVertex(ref LineVertexInfo info, Vector3 axis, float uCoord, int vertexIndex, LineVertexFormat[] vertices)
        {
            float halfSize = info.Size / 2f;

            // 0
            vertices[vertexIndex].Position = info.Position;
            vertices[vertexIndex].Color = info.Color;
            vertices[vertexIndex].TexCoord = new Vector2(uCoord, 0);
            vertices[vertexIndex].AxisSize = new Vector4(axis.X, axis.Y, axis.Z, halfSize);
            vertices[vertexIndex].UVShift = info.UVShift;
            vertexIndex++;

            // 1
            vertices[vertexIndex].Position = info.Position;
            vertices[vertexIndex].Color = info.Color;
            vertices[vertexIndex].TexCoord = new Vector2(uCoord, 1);
            vertices[vertexIndex].AxisSize = new Vector4(axis.X, axis.Y, axis.Z, -halfSize);
            vertices[vertexIndex].UVShift = info.UVShift;
        }

        /// <summary>
        /// Draw method
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(TimeSpan gameTime)
        { 
            if (this.OnPreDraw != null)
            {
                this.OnPreDraw(this, null);
            }

            if (this.material == null && this.lineVertices != null)
            {
                RenderLayer layer = this.RenderManager.FindLayer(DefaultLayers.Additive);

                if (this.lineType == LineType.LineList)
                {
                    if (this.WorldSpace)
                    {
                        for (int i = 0; i < this.lineVertices.Count - 1; i += 2)
                        {
                            layer.LineBatch3D.DrawLine(this.lineVertices[i].Position, this.lineVertices[i + 1].Position, this.lineVertices[i].Color);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < this.lineVertices.Count - 1; i += 2)
                        {
                            var pos1 = Vector3.Transform(this.lineVertices[i].Position, this.transform.WorldTransform);
                            var pos2 = Vector3.Transform(this.lineVertices[i + 1].Position, this.transform.WorldTransform);
                            layer.LineBatch3D.DrawLine(pos1, pos2, this.lineVertices[i].Color);
                        }
                    }
                }
                else
                {
                    if (this.WorldSpace)
                    {
                        for (int i = 0; i < this.lineVertices.Count - 1; i++)
                        {
                            layer.LineBatch3D.DrawLine(this.lineVertices[i].Position, this.lineVertices[i + 1].Position, this.lineVertices[i].Color);
                        }
                    }
                    else
                    {
                        var pos1 = Vector3.Transform(this.lineVertices[0].Position, this.transform.WorldTransform);
                        for (int i = 0; i < this.lineVertices.Count - 1; i++)
                        {
                            var pos2 = Vector3.Transform(this.lineVertices[i + 1].Position, this.transform.WorldTransform);
                            layer.LineBatch3D.DrawLine(pos1, pos2, this.lineVertices[i].Color);

                            pos1 = pos2;
                        }
                    }
                }
            }

            if (this.meshes != null && this.material != null)
            {
                Matrix transform = this.WorldSpace ? Matrix.Identity : this.transform.WorldTransform;
                foreach (var mesh in this.meshes)
                {
                    this.RenderManager.DrawMesh(mesh, this.material, ref transform);
                }
            }
        }       

        /// <summary>
        /// Dispose method
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            this.OnPreDraw = null;
            this.DisposeMeshes();
        }

        /// <summary>
        /// Dispose meshes
        /// </summary>
        private void DisposeMeshes()
        {
            if (this.meshes != null)
            {
                foreach (var mesh in this.meshes)
                {
                    this.GraphicsDevice.DestroyIndexBuffer(mesh.IndexBuffer);
                    this.GraphicsDevice.DestroyVertexBuffer(mesh.VertexBuffer);
                }

                this.meshes.Clear();
            }
        }

        ////protected override void DrawDebugLines()
        ////{
        ////    base.DrawDebugLines();

        ////    if (this.lineVertices != null)
        ////    {
        ////        if (this.lineType == LineType.LineStrip)
        ////        {
        ////            for (int i = 1; i < this.lineVertices.Count; i++)
        ////            {
        ////                var p1 = this.lineVertices[i - 1];
        ////                var p2 = this.lineVertices[i];
        ////                this.RenderManager.LineBatch3D.DrawLine(p1.Position, p2.Position, p1.Color);
        ////            }
        ////        }
        ////        else if (this.lineType == LineType.LineList)
        ////        {
        ////            ////for (int i = 1; i < this.lineVertices.Count; i += 2)
        ////            ////{
        ////            ////    var p1 = this.lineVertices[i - 1];
        ////            ////    var p2 = this.lineVertices[i];
        ////            ////    this.RenderManager.LineBatch3D.DrawLine(p1.Position, p2.Position, p1.Color);
        ////            ////}
        ////        }
        ////    }
        ////}      
    }
}
