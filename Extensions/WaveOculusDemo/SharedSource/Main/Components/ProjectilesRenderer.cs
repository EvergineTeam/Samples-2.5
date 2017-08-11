using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Graphics.VertexFormats;
using WaveEngine.Common.Math;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Diagnostic;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;

namespace WaveOculusDemoProject.Components
{
    /// <summary>
    /// Projectile renderer
    /// </summary>
    [DataContract]
    public class ProjectilesRenderer : Drawable3D
    {
        private const int VerticesPerProjectile = 4;
        private const int IndicesPerProjectile = 6;

        private const float TrailFactor = 0.065f;
        private const float thickness = 4f;
        private const float halfThickness = thickness * 0.5f;

        private static Matrix identity = Matrix.Identity;

        [RequiredComponent]
        private ProjectileManager manager = null;
        
        [RequiredComponent]
        private MaterialComponent materialComponent = null;

        private int maxProjectiles;
        private VertexPositionColorTexture[] vertices;
        private Mesh mesh;

        /// <summary>
        /// Initializes the projectile renderer
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            this.BuildMesh();
        }

        /// <summary>
        /// Build the mesh used to draw the particles
        /// </summary>
        private void BuildMesh()
        {
            if (this.mesh != null)
            {
                this.DestroyMesh();
            }

            this.maxProjectiles = this.manager.Capacity;

            int nVertices = VerticesPerProjectile * this.maxProjectiles;
            int nIndices = IndicesPerProjectile * this.maxProjectiles;

            // Indices
            ushort[] indices = new ushort[nIndices];
            for (int i = 0; i < this.maxProjectiles; i++)
            {
                indices[(i * IndicesPerProjectile) + 0] = (ushort)(i * 4);
                indices[(i * IndicesPerProjectile) + 1] = (ushort)((i * 4) + 1);
                indices[(i * IndicesPerProjectile) + 2] = (ushort)((i * 4) + 2);
                indices[(i * IndicesPerProjectile) + 3] = (ushort)((i * 4) + 2);
                indices[(i * IndicesPerProjectile) + 4] = (ushort)((i * 4) + 3);
                indices[(i * IndicesPerProjectile) + 5] = (ushort)(i * 4);
            }

            IndexBuffer indexBuffer = new IndexBuffer(indices);
            this.RenderManager.GraphicsDevice.BindIndexBuffer(indexBuffer);

            // Vertices
            this.vertices = new VertexPositionColorTexture[nVertices];
            for (int i = 0; i < this.maxProjectiles; i++)
            {
                vertices[(i * VerticesPerProjectile) + 0].TexCoord = Vector2.Zero;
                vertices[(i * VerticesPerProjectile) + 0].Color = Color.White;
                vertices[(i * VerticesPerProjectile) + 1].TexCoord = Vector2.UnitX;
                vertices[(i * VerticesPerProjectile) + 1].Color = Color.White;
                vertices[(i * VerticesPerProjectile) + 2].TexCoord = Vector2.One;
                vertices[(i * VerticesPerProjectile) + 2].Color = Color.White;
                vertices[(i * VerticesPerProjectile) + 3].TexCoord = Vector2.UnitY;
                vertices[(i * VerticesPerProjectile) + 3].Color = Color.White;
            }

            DynamicVertexBuffer vertexBuffer = new DynamicVertexBuffer(VertexPositionColorTexture.VertexFormat);
            vertexBuffer.SetData(this.vertices);
            this.GraphicsDevice.BindVertexBuffer(vertexBuffer);

            this.mesh = new Mesh(0, nVertices, 0, nIndices / 3, vertexBuffer, indexBuffer, PrimitiveType.TriangleList)
                {
                    DisableBatch = true
                };
        }

        /// <summary>
        /// Draw projectiles
        /// </summary>
        /// <param name="gameTime">The current gameTime</param>
        public override void Draw(TimeSpan gameTime)
        {
            if (this.manager.BusyProjectiles.Count == 0)
            {
                return;
            }

            if (this.manager.Capacity > this.maxProjectiles)
            {
                this.BuildMesh();
            }

            ProjectileController particle;
            Vector3 start;
            Vector3 end;
            Vector3 velocity;           
            Vector3 perpendicular;
            Vector3 cameraPosition = this.RenderManager.CurrentDrawingCamera3D.Position;
            Vector3 eyeDir;
            float velocityLength;
            
            int nProjectiles = this.manager.BusyProjectiles.Count;
            for (int i = 0; i < nProjectiles; i++)
            {
                particle = this.manager.BusyProjectiles[i];

                eyeDir = cameraPosition - particle.Position;
                
                // Velocity
                velocity = particle.Velocity;
                velocityLength = velocity.Length();
                velocity.X = ((velocity.X * TrailFactor + (velocity.X / velocityLength * thickness)));
                velocity.Y = ((velocity.Y * TrailFactor + (velocity.Y / velocityLength * thickness)));
                velocity.Z = ((velocity.Z * TrailFactor + (velocity.Z / velocityLength * thickness)));
                
                // End
                end = particle.Position + velocity * 0.5f;

                // Start
                start = end - velocity;

                perpendicular = Vector3.Cross(eyeDir, velocity);
                perpendicular.Normalize();

                // Update vertex
                vertices[(i * VerticesPerProjectile) + 0].Position = start - (perpendicular * halfThickness);
                vertices[(i * VerticesPerProjectile) + 1].Position = end - (perpendicular * halfThickness);
                vertices[(i * VerticesPerProjectile) + 2].Position = end + (perpendicular * halfThickness);
                vertices[(i * VerticesPerProjectile) + 3].Position = start + (perpendicular * halfThickness);
            }

            this.mesh.NumPrimitives = IndicesPerProjectile * nProjectiles / 3;
            this.mesh.VertexBuffer.SetData(this.vertices, nProjectiles * VerticesPerProjectile, 0);
            this.RenderManager.GraphicsDevice.BindVertexBuffer(this.mesh.VertexBuffer);

            this.RenderManager.DrawMesh(this.mesh, this.materialComponent.Material, ref identity);
        }

        /// <summary>
        /// Draw debug lines
        /// </summary>
        protected override void DrawDebugLines()
        {
            base.DrawDebugLines();

            int nProjectiles = this.manager.BusyProjectiles.Count;
            for (int i = 0; i < nProjectiles; i++)
            {
                var particle = this.manager.BusyProjectiles[i];
                this.RenderManager.LineBatch3D.DrawPoint(particle.Position, 10, Color.Yellow);
            }
        }

        /// <summary>
        /// Destroy particle mesh
        /// </summary>
        private void DestroyMesh()
        {
            this.RenderManager.GraphicsDevice.DestroyIndexBuffer(mesh.IndexBuffer);
            this.RenderManager.GraphicsDevice.DestroyVertexBuffer(mesh.VertexBuffer);
        }

        /// <summary>
        /// Dispose the unmanaged resources
        /// </summary>
        /// <param name="disposing">Is disposing</param>
        protected override void Dispose(bool disposing)
        {
            if (this.mesh != null)
            {
                this.DestroyMesh();
            }
        }
    }
}
