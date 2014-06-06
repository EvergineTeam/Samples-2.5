using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Graphics.VertexFormats;
using WaveEngine.Common.Math;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;

namespace CustomGeometryProject
{
    public class CustomGeometryRenderer : Drawable3D
    {
        /// <summary>
        /// Number of instances of this component created.
        /// </summary>
        private static int instances;

        /// <summary>
        /// The entity transform.
        /// </summary>
        [RequiredComponent]
        public Transform3D Transform3D;

        /// <summary>
        /// The material
        /// </summary>
        [RequiredComponent]
        public MaterialsMap Material;

        /// <summary>
        /// The disposed
        /// </summary>
        protected bool disposed;

        private Mesh mesh;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomGeometryRenderer"/> class.
        /// </summary>
        public CustomGeometryRenderer()
            : base("CustomGeometryRenderer" + instances)
        {
            instances++;
        }

        /// <summary>
        /// Performs further custom initialization for this instance.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            VertexPositionColor[] vertices = new VertexPositionColor[3];
            vertices[0].Position = new Vector3(-0.5f, -0.5f, 0f);
            vertices[0].Color = Color.Red;

            vertices[1].Position = new Vector3(0f, 0.5f, 0f);
            vertices[1].Color = Color.Green;

            vertices[2].Position = new Vector3(0.5f, -0.5f, 0f);
            vertices[2].Color = Color.Yellow;

            var vertexBuffer = new VertexBuffer(VertexPositionColor.VertexFormat);
            vertexBuffer.SetData(vertices, 3);
            
            ushort[] indices = new ushort[3];
            indices[0] = 0;
            indices[1] = 1;
            indices[2] = 2;

            var indexBuffer = new IndexBuffer(indices);

            this.mesh = new Mesh(0, vertices.Length, 0, 1, vertexBuffer, indexBuffer, PrimitiveType.TriangleList);
        }

        /// <summary>
        /// Allows to perform custom drawing.
        /// </summary>
        /// <param name="gameTime">The elapsed game time.</param>
        /// <remarks>
        /// This method will only be called if all the following points are true:
        /// <list type="bullet">
        /// <item>
        /// <description>The entity passes a frustrum culling test.</description>
        /// </item>
        /// <item>
        /// <description>The parent of the owner <see cref="T:WaveEngine.Framework.Entity" /> of the <see cref="T:WaveEngine.Framework.Drawable" /> cascades its visibility to its children and it is visible.</description>
        /// </item>
        /// <item>
        /// <description>The <see cref="T:WaveEngine.Framework.Drawable" /> is active.</description>
        /// </item>
        /// <item>
        /// <description>The owner <see cref="T:WaveEngine.Framework.Entity" /> of the <see cref="T:WaveEngine.Framework.Drawable" /> is active and visible.</description>
        /// </item>
        /// </list>
        /// </remarks>
        public override void Draw(TimeSpan gameTime)
        {
            this.RenderManager.DrawMesh(this.mesh, this.Material.DefaultMaterial, ref this.Transform3D.LocalWorld);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.disposed = true;
                }
            }
        }
    }
}
