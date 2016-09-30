#region Using Statements
using System;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Graphics.VertexFormats;
using WaveEngine.Common.Math;
using WaveEngine.Framework.Services;
using System.Runtime.Serialization;
using WaveEngine.Materials;
using WaveEngine.Framework.Resources;
#endregion

namespace NinePatchSample.NinePatch
{
    [DataContract]
    public class NinePatchRenderer : Drawable2D
    {
        /// <summary>
        /// Number of instances of this component created.
        /// </summary>
        private static int instances;

        /// <summary>
        /// The entity transform.
        /// </summary>
        [RequiredComponent]
        protected Transform2D transform2D;

        /// <summary>
        /// Materials used rendering the particle system.
        /// </summary>
        [RequiredComponent(isExactType: false)]
        protected NinePatchBase ninePatch;

        /// <summary>
        /// The vertices
        /// </summary>
        private VertexPositionColorTexture[] vertices;

        /// <summary>
        /// The disposed
        /// </summary>
        protected bool disposed;

        /// <summary>
        /// The quad mesh.
        /// </summary>
        private Mesh mesh;

        #region Dirty members
        private Vector2 lastTransformRectangleCenter;

        private float lastTransformOpacity;

        private string lastTexturePath;
        #endregion

        #region Initialize
        /// <summary>
        /// Initializes a new instance of the <see cref="QuadRenderer" /> class.
        /// </summary>
        public NinePatchRenderer()
            : base("NinePatchRenderer" + instances, DefaultLayers.Alpha)
        {
            instances++;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Allows to perform custom drawing.
        /// </summary>
        /// <param name="gameTime">The elapsed game time.</param>
        /// <remarks>
        /// This method will only be called if all the following points are true:
        /// <list type="bullet">
        /// <item>
        /// <description>The parent of the owner <see cref="Entity" /> of the <see cref="Drawable" /> cascades its visibility to its children and it is visible.</description>
        /// </item>
        /// <item>
        /// <description>The <see cref="Drawable" /> is active.</description>
        /// </item>
        /// <item>
        /// <description>The owner <see cref="Entity" /> of the <see cref="Drawable" /> is active and visible.</description>
        /// </item>
        /// </list>
        /// </remarks>
        public override void Draw(TimeSpan gameTime)
        {
            if (this.lastTransformRectangleCenter != this.transform2D.Rectangle.Center
                 || this.lastTransformOpacity != this.transform2D.GlobalOpacity
                 || this.lastTexturePath != this.ninePatch.TexturePath)
            {
                this.RefreshMesh();
            }

            this.ninePatch.Material.LayerType = this.LayerType;

            if (this.transform2D.GlobalOpacity > 0
             && this.mesh != null)
            {
                this.mesh.ZOrder = this.transform2D.DrawOrder;
                Matrix worldTransform = this.transform2D.WorldTransform;

                // WORKAROUND: Force spritebatch to start a new render command
                this.layer.SpriteBatch.Draw(StaticResources.WhitePixel, -Vector2.One, null, Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, this.mesh.ZOrder);

                // Draw mesh
                this.RenderManager.DrawMesh(this.mesh, this.ninePatch.Material, ref worldTransform, false);
            }
        }

        protected override void DrawDebugLines()
        {
            base.DrawDebugLines();

            if (this.transform2D.GlobalOpacity > 0
             && this.mesh != null)
            {
                foreach (var vertex in this.vertices)
                {
                    var position = Vector3.Transform(vertex.Position, this.transform2D.WorldTransform);
                    this.RenderManager.LineBatch2D.DrawPoint(position.ToVector2(), 10, Color.Red, -1f);
                }
            }
        }

        #endregion

        #region Private Methods
        protected override void ResolveDependencies()
        {
            base.ResolveDependencies();

            if (this.isInitialized)
            {
                this.RefreshMesh();
            }
        }

        /// <summary>
        /// Performs further custom initialization for this instance.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            this.RefreshMesh();
        }

        /// <summary>
        /// Refresh the mesh
        /// </summary>
        private void RefreshMesh()
        {
            this.lastTransformOpacity = this.transform2D.GlobalOpacity;

            if (this.lastTransformOpacity <= 0)
            {
                return;
            }

            if (this.mesh != null)
            {
                this.GraphicsDevice.DestroyIndexBuffer(this.mesh.IndexBuffer);
                this.GraphicsDevice.DestroyVertexBuffer(this.mesh.VertexBuffer);
                this.mesh = null;
            }

            this.GenerateVertices();

            VertexBuffer vertexBuffer = new VertexBuffer(VertexPositionColorTexture.VertexFormat);
            vertexBuffer.SetData(this.vertices);

            var indexBuffer = new IndexBuffer(new ushort[]
            {
                0,1,2,0,2,3,

                1,4,7,1,7,2,

                4,5,6,4,6,7,

                3,2,9,3,9,8,

                2,7,12,2,12,9,

                7,6,13,7,13,12,

                8,9,10,8,10,11,

                9,12,15,9,15,10,

                12,13,14,12,14,15
            });

            this.mesh = new Mesh(0, vertexBuffer.VertexCount, 0, indexBuffer.IndexCount / 3, vertexBuffer, indexBuffer, PrimitiveType.TriangleList);
        }

        private void GenerateVertices()
        {
            var texture = (this.ninePatch.Material as StandardMaterial).Diffuse;

            this.lastTexturePath = this.ninePatch.TexturePath;

            if (this.transform2D.Rectangle.IsEmpty)
            {
                var rectangle = this.transform2D.Rectangle;
                rectangle.Width = texture.Width;
                rectangle.Height = texture.Height;
                this.transform2D.Rectangle = rectangle;
            }

            this.lastTransformRectangleCenter = this.transform2D.Rectangle.Center;

            Vector2 origin = this.transform2D.Origin;
            float halfWidth = this.transform2D.Rectangle.Width;
            float halfHeight = this.transform2D.Rectangle.Height;
            float originCorrectionWidth = origin.X * halfWidth;
            float originCorrectionHeight = origin.Y * halfHeight;

            Color color = new Color(this.lastTransformOpacity, this.lastTransformOpacity, this.lastTransformOpacity, this.lastTransformOpacity);

            var outputRectangle = new RectangleF(
                    0,
                    0,
                    halfWidth,
                    halfHeight);
            var outputRectangleCoords = new RectangleF(0, 0, 1, 1);

            var stretchableRectangleCoords = this.ninePatch.StretchableRectangleCoords;
            var stretchableRectangle = this.ninePatch.StretchableRectangle;

            // Ajust stretchable rectangle to rectangle size
            stretchableRectangle.Width = outputRectangle.Width - (texture.Width - stretchableRectangle.Width);
            stretchableRectangle.Height = outputRectangle.Height - (texture.Height - stretchableRectangle.Height);

            // Origin Offset
            outputRectangle.Offset(-originCorrectionWidth, -originCorrectionHeight);
            stretchableRectangle.Offset(-originCorrectionWidth, -originCorrectionHeight);

            this.vertices = new VertexPositionColorTexture[16];

            // Top-Left Vertices
            vertices[0].Position = new Vector3(outputRectangle.Left, outputRectangle.Top, 0);
            vertices[0].Color = color;
            vertices[0].TexCoord = new Vector2(outputRectangleCoords.Left, outputRectangleCoords.Top);

            vertices[1].Position = new Vector3(stretchableRectangle.Left, outputRectangle.Top, 0);
            vertices[1].Color = color;
            vertices[1].TexCoord = new Vector2(stretchableRectangleCoords.Left, outputRectangleCoords.Top);

            vertices[2].Position = new Vector3(stretchableRectangle.Left, stretchableRectangle.Top, 0);
            vertices[2].Color = color;
            vertices[2].TexCoord = new Vector2(stretchableRectangleCoords.Left, stretchableRectangleCoords.Top);

            vertices[3].Position = new Vector3(outputRectangle.Left, stretchableRectangle.Top, 0);
            vertices[3].Color = color;
            vertices[3].TexCoord = new Vector2(outputRectangleCoords.Left, stretchableRectangleCoords.Top);

            // Top-Right Vertices
            vertices[4].Position = new Vector3(stretchableRectangle.Right, outputRectangle.Top, 0);
            vertices[4].Color = color;
            vertices[4].TexCoord = new Vector2(stretchableRectangleCoords.Right, outputRectangleCoords.Top);

            vertices[5].Position = new Vector3(outputRectangle.Right, outputRectangle.Top, 0);
            vertices[5].Color = color;
            vertices[5].TexCoord = new Vector2(outputRectangleCoords.Right, outputRectangleCoords.Top);

            vertices[6].Position = new Vector3(outputRectangle.Right, stretchableRectangle.Top, 0);
            vertices[6].Color = color;
            vertices[6].TexCoord = new Vector2(outputRectangleCoords.Right, stretchableRectangleCoords.Top);

            vertices[7].Position = new Vector3(stretchableRectangle.Right, stretchableRectangle.Top, 0);
            vertices[7].Color = color;
            vertices[7].TexCoord = new Vector2(stretchableRectangleCoords.Right, stretchableRectangleCoords.Top);

            // Bottom-Left Vertices
            vertices[8].Position = new Vector3(outputRectangle.Left, stretchableRectangle.Bottom, 0);
            vertices[8].Color = color;
            vertices[8].TexCoord = new Vector2(outputRectangleCoords.Left, stretchableRectangleCoords.Bottom);

            vertices[9].Position = new Vector3(stretchableRectangle.Left, stretchableRectangle.Bottom, 0);
            vertices[9].Color = color;
            vertices[9].TexCoord = new Vector2(stretchableRectangleCoords.Left, stretchableRectangleCoords.Bottom);

            vertices[10].Position = new Vector3(stretchableRectangle.Left, outputRectangle.Bottom, 0);
            vertices[10].Color = color;
            vertices[10].TexCoord = new Vector2(stretchableRectangleCoords.Left, outputRectangleCoords.Bottom);

            vertices[11].Position = new Vector3(outputRectangle.Left, outputRectangle.Bottom, 0);
            vertices[11].Color = color;
            vertices[11].TexCoord = new Vector2(outputRectangleCoords.Left, outputRectangleCoords.Bottom);

            // Bottom-Right Vertices
            vertices[12].Position = new Vector3(stretchableRectangle.Right, stretchableRectangle.Bottom, 0);
            vertices[12].Color = color;
            vertices[12].TexCoord = new Vector2(stretchableRectangleCoords.Right, stretchableRectangleCoords.Bottom);

            vertices[13].Position = new Vector3(outputRectangle.Right, stretchableRectangle.Bottom, 0);
            vertices[13].Color = color;
            vertices[13].TexCoord = new Vector2(outputRectangleCoords.Right, stretchableRectangleCoords.Bottom);

            vertices[14].Position = new Vector3(outputRectangle.Right, outputRectangle.Bottom, 0);
            vertices[14].Color = color;
            vertices[14].TexCoord = new Vector2(outputRectangleCoords.Right, outputRectangleCoords.Bottom);

            vertices[15].Position = new Vector3(stretchableRectangle.Right, outputRectangle.Bottom, 0);
            vertices[15].Color = color;
            vertices[15].TexCoord = new Vector2(stretchableRectangleCoords.Right, outputRectangleCoords.Bottom);
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
                    if (this.mesh != null)
                    {
                        GraphicsDevice.DestroyIndexBuffer(this.mesh.IndexBuffer);
                        GraphicsDevice.DestroyVertexBuffer(this.mesh.VertexBuffer);
                    }

                    this.disposed = true;
                }
            }
        }

        #endregion
    }
}
