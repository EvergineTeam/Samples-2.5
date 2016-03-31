using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using WaveEngine.Common.Attributes;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Graphics.VertexFormats;
using WaveEngine.Common.Math;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Models;

namespace WaveOculusDemoProject.Components
{
    /// <summary>
    /// Trails renderer drawable
    /// </summary>
    [DataContract]
    public class TrailsRenderer : Drawable3D
    {
        private const int InitialTrailPoints = 120;
        private const int VerticesStep = 64;

        private const int VerticesPerTrailPoint = 3;
        private const int IndicesPerTrailPoint = 3;

        [RequiredComponent]
        private TrailManager manager = null;

        [RequiredComponent]
        private MaterialsMap materials = null;

        private Material material;

        private VertexPositionColorTexture[] vertices;
        private ushort[] indices;
        private Mesh mesh;

        private int currentVertexId;
        private int currentIndexId;

        private int nVertices;
        private int nIndices;

        private static Matrix identity = Matrix.Identity;

        [RenderPropertyAsAsset(AssetType.Material)]
        [DataMember]
        public string MaterialPath { get; set; }

        /// <summary>
        /// Initializes the trail renderer
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            this.nVertices = InitialTrailPoints * VerticesPerTrailPoint;
            this.nIndices = InitialTrailPoints * IndicesPerTrailPoint;


            if (!string.IsNullOrEmpty(this.MaterialPath))
            {
                this.material = this.Assets.LoadModel<MaterialModel>(this.MaterialPath).Material;
                this.material.Initialize(this.Assets);
            }

            this.BuildMesh();
        }

        /// <summary>
        /// Build the mesh used to draw all trails
        /// </summary>
        private void BuildMesh()
        {
            if (this.mesh != null)
            {
                this.DestroyMesh();
            }

            // Indices
            this.indices = new ushort[this.nIndices];
            DynamicIndexBuffer indexBuffer = new DynamicIndexBuffer(this.indices);
            this.RenderManager.GraphicsDevice.BindIndexBuffer(indexBuffer);

            // Vertices
            this.vertices = new VertexPositionColorTexture[this.nVertices];
            DynamicVertexBuffer vertexBuffer = new DynamicVertexBuffer(VertexPositionColorTexture.VertexFormat);
            vertexBuffer.SetData(this.vertices);
            this.GraphicsDevice.BindVertexBuffer(vertexBuffer);

            this.mesh = new Mesh(0, nVertices, 0, nIndices / 3, vertexBuffer, indexBuffer, PrimitiveType.TriangleStrip)
            {
                DisableBatch = true
            };
        }

        /// <summary>
        /// Destroy trail mesh
        /// </summary>
        private void DestroyMesh()
        {
            this.GraphicsDevice.DestroyIndexBuffer(mesh.IndexBuffer);
            this.GraphicsDevice.DestroyVertexBuffer(mesh.VertexBuffer);
        }

        /// <summary>
        /// Draw debug lines
        /// </summary>
        protected override void DrawDebugLines()
        {
            base.DrawDebugLines();
            return;

            if (this.manager.NumTrails == 0)
            {
                return;
            }

            int trailCount = this.manager.NumTrails;
            for (int i = 0; i < trailCount; i++)
            {
                TrailSetting trail = this.manager.BusyTrails[i];
                int numPoints = trail.Points.Count;
                
                for (int j = 0; j < trail.Points.Count; j++)
                {
                    this.RenderManager.LineBatch3D.DrawPoint(trail.Points[j].Position, 4, Color.Yellow);
                }
            }
        }

        /// <summary>
        /// Draw the trail mesh
        /// </summary>
        /// <param name="gameTime">The current game time</param>
        public override void Draw(TimeSpan gameTime)
        {            
            if (this.manager.NumTrails == 0)
            {
                return;
            }

            Vector3 cameraPos = this.RenderManager.CurrentDrawingCamera3D.Position;

            int vertexPrediction = this.manager.TrailPointsCount * VerticesPerTrailPoint;
            int indexPrediction = vertexPrediction + (this.manager.NumTrails * 4);
            
            if (indexPrediction >= this.nIndices || vertexPrediction >= this.nVertices)
            {             
                this.nIndices = indexPrediction + VerticesStep;
                this.nVertices = vertexPrediction + VerticesStep;

                this.BuildMesh();
            }

            this.currentVertexId = 0;
            this.currentIndexId = 0;
            int trailCount = this.manager.NumTrails;

            for (int i = 0; i < trailCount; i++)
            {
                TrailSetting trail = this.manager.BusyTrails[i];
                int numPoints = trail.Points.Count;


                if (numPoints < 2)
                {
                    continue;
                }

                this.ProcessStartPoint(trail);

                for (int j = 1; j < trail.Points.Count; j++)
                {
                    this.ProcessTrailPoint(trail, j);
                }

                this.ProcessEndPoint(trail);
            }

            this.mesh.VertexBuffer.SetData(this.vertices, this.currentVertexId);
            this.mesh.IndexBuffer.SetData(this.indices, this.currentIndexId);
            this.GraphicsDevice.BindVertexBuffer(this.mesh.VertexBuffer);
            this.GraphicsDevice.BindIndexBuffer(this.mesh.IndexBuffer);
            this.mesh.NumPrimitives = this.currentIndexId - 2;

            this.RenderManager.DrawMesh(this.mesh, this.material, ref identity);
        }

        /// <summary>
        /// Process a start point of a trail
        /// </summary>
        /// <param name="trail">The current trail</param>
        private void ProcessStartPoint(TrailSetting trail)
        {
            if (this.currentIndexId != 0)
            {
                // Concatenate two triangle strips...
                this.indices[this.currentIndexId + 0] = (ushort)(this.currentVertexId - 1);
                this.indices[this.currentIndexId + 1] = (ushort)(this.currentVertexId);
                this.indices[this.currentIndexId + 2] = (ushort)(this.currentVertexId);
                this.indices[this.currentIndexId + 3] = (ushort)(this.currentVertexId + 1);
                this.currentIndexId += 4;
            }

            Vector3 start = trail.Points[0].Position;
            Vector3 end = trail.Points[1].Position;
            double pointTime = trail.Points[1].Time;
            float lerp = (float)((trail.CurrentTime - pointTime) / trail.ExpirationTime);

            ProcessTrailPoint(trail, ref start, ref end, 0);
        }

        /// <summary>
        /// Process the end point of a trail
        /// </summary>
        /// <param name="trail">The current trail</param>
        private void ProcessEndPoint(TrailSetting trail)
        {
            Vector3 start = trail.Points.Last().Position;
            Vector3 end = trail.Transform.Position;
            Color c = Color.White;
            float lerp = 0;

            if (trail.Finished)
            {
                double pointTime = trail.Points.Last().Time;
                lerp = (float)((trail.CurrentTime - pointTime) / trail.ExpirationTime);
            }

            ProcessTrailPoint(trail, ref start, ref end, lerp);
        }


        /// <summary>
        /// Process a middle trail point
        /// </summary>
        /// <param name="trail">The current trail</param>
        private void ProcessTrailPoint(TrailSetting trail, int pointId)
        {
            Vector3 start = trail.Points[pointId - 1].Position;
            Vector3 end = trail.Points[pointId].Position;
            double pointTime = trail.Points[pointId].Time;
            float lerp = (float)((trail.CurrentTime - pointTime) / trail.ExpirationTime);

            ProcessTrailPoint(trail, ref start, ref end, lerp);
        }

        
        /// <summary>
        /// Process a trail point
        /// </summary>
        /// <param name="trail">The trail setting</param>
        /// <param name="start">start position</param>
        /// <param name="end">end position</param>
        /// <param name="lerp">Lerp factor</param>
        private void ProcessTrailPoint(TrailSetting trail, ref Vector3 start, ref Vector3 end, float lerp)
        {
            Vector3 diff = end - start;
            Vector3 eyeDir = start - this.RenderManager.ActiveCamera3D.Position;
            float halfThickness = trail.Thickness;
            
            Vector3 perpendicular;            
            Vector3.Cross(ref eyeDir, ref diff, out perpendicular);
            perpendicular.Normalize();

            this.vertices[this.currentVertexId].Position = end + (perpendicular * halfThickness);
            this.vertices[this.currentVertexId].TexCoord = new Vector2(lerp, 0);
            this.vertices[this.currentVertexId].Color = Color.White * lerp;
            this.indices[this.currentIndexId] = (ushort)this.currentVertexId;
            this.currentVertexId++;
            this.currentIndexId++;

            this.vertices[this.currentVertexId].Position = end - (perpendicular * halfThickness);
            this.vertices[this.currentVertexId].TexCoord = new Vector2(lerp, 1);
            this.vertices[this.currentVertexId].Color = Color.White * lerp;
            this.indices[this.currentIndexId] = (ushort)this.currentVertexId;
            this.currentVertexId++;
            this.currentIndexId++;
        }

        protected override void Dispose(bool disposing)
        {
        }
    }
}
