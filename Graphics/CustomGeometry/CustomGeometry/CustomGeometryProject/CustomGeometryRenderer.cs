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

        /// <summary>
        /// The vertex buffer
        /// </summary>
        private VertexBuffer vertexBuffer;

        /// <summary>
        /// The index buffer
        /// </summary>
        private IndexBuffer indexBuffer;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomGeometryRenderer"/> class.
        /// </summary>
        public CustomGeometryRenderer()
            : base("CustomGeometryRenderer" + instances)
        {
            instances++;
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
            Layer layer = RenderManager.FindLayer(DefaultLayers.Opaque);

            layer.AddDrawable(0, this, this.SortId);

            this.Material.DefaultMaterial.Matrices.World = Transform3D.LocalWorld;
        }

        /// <summary>
        /// Draws the basic unit.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        protected override void DrawBasicUnit(int parameter)
        {
            this.Material.DefaultMaterial.Apply(this.RenderManager);

            this.GraphicsDevice.DrawVertexBuffer(
                4,
                2,
                PrimitiveType.TriangleList,
                this.vertexBuffer,
                this.indexBuffer);
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

            this.vertexBuffer = new VertexBuffer(VertexPositionColor.VertexFormat);

            this.vertexBuffer.SetData(vertices, 3);
            this.GraphicsDevice.BindVertexBuffer(this.vertexBuffer);

            ushort[] indices = new ushort[3];
            indices[0] = 0;
            indices[1] = 1;
            indices[2] = 2;

            this.indexBuffer = new IndexBuffer(indices);
            this.GraphicsDevice.BindIndexBuffer(this.indexBuffer);
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
