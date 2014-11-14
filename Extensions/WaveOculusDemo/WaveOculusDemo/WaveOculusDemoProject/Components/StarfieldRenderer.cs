using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Graphics.VertexFormats;
using WaveEngine.Common.Math;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;

namespace WaveOculusDemoProject.Components
{
    /// <summary>
    /// Starfield renderer.
    /// </summary>
    public class StarfieldRenderer : Drawable3D
    {
        private struct StarColorRamp
        {
            public float Lerp;
            public Color Color;
        };

        private readonly VertexPositionColorTexture[] referenceData = new VertexPositionColorTexture[]
        {
            new VertexPositionColorTexture(new Vector3( 0.5f, -0.5f, 1), Color.White, new Vector2(1,0)),
            new VertexPositionColorTexture(new Vector3( 0.5f,  0.5f, 1), Color.White, new Vector2(1,1)),
            new VertexPositionColorTexture(new Vector3(-0.5f,  0.5f, 1), Color.White, new Vector2(0,1)),
            new VertexPositionColorTexture(new Vector3(-0.5f, -0.5f, 1), Color.White, new Vector2(0,0)),
        };

        private readonly StarColorRamp[] colorRamp = new StarColorRamp[]
        {
            new StarColorRamp() {Lerp = 0, Color = Color.Blue },
            new StarColorRamp() {Lerp = 0.25f, Color = Color.Cyan },
            new StarColorRamp() {Lerp = 0.5f, Color = Color.White },
            new StarColorRamp() {Lerp = 0.85f, Color = Color.Yellow },
            new StarColorRamp() {Lerp = 1f, Color = Color.Orange },
        };

        private const float minScale = 1f;
        private const float maxScale = 2f;
        private const float minDepth = 100;
        private const float maxDepth = 400;
        private const float minAlpha = 0.2f;
        private const float maxAlpha = 0.85f;
        private const float whiteProportion = 0.6f;


        private const float scaleInterval = maxScale - minScale;
        private const float depthInterval = maxDepth - minDepth;
        private const float alphaInterval = maxAlpha - minAlpha;

        private int nStars;
        private Mesh mesh;

        [RequiredComponent]
        private MaterialsMap materials = null;

        [RequiredComponent]
        private Transform3D transform = null;

        /// <summary>
        /// Init a starfield renderer 
        /// </summary>
        /// <param name="nStars">The number of stars to render</param>
        public StarfieldRenderer(int nStars)
        {
            this.nStars = nStars;
        }

        /// <summary>
        /// Initialize the starfield mesh
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            // Indices
            var indices = new ushort[this.nStars * 6];
            for (int i = 0; i < this.nStars; i++)
            {
                indices[i * 6] = (ushort)(i * 4);
                indices[(i * 6) + 1] = (ushort)((i * 4) + 1);
                indices[(i * 6) + 2] = (ushort)((i * 4) + 2);
                indices[(i * 6) + 3] = (ushort)((i * 4) + 3);
                indices[(i * 6) + 4] = (ushort)(i * 4);
                indices[(i * 6) + 5] = (ushort)((i * 4) + 2);
            }

            var indexBuffer = new IndexBuffer(indices);

            // Vertices
            var vertices = new VertexPositionColorTexture[this.nStars * 4];

            System.Random rand = new System.Random();

            for (int i = 0; i < this.nStars; i++)
            {
                float scale = (float)(rand.NextDouble() * scaleInterval) + minScale;
                float depth = (float)(rand.NextDouble() * depthInterval) + minDepth;
                // scale
                Matrix scaleTransform = Matrix.CreateScale(new Vector3(
                    scale,
                    scale,
                    depth
                    ));

                // rotation
                Matrix rotationTransform = Matrix.CreateFromYawPitchRoll(
                    (float)(rand.NextDouble() * MathHelper.TwoPi),
                    (float)(Math.Asin((rand.NextDouble() * 2) - 1)),
                    0);
                Matrix transform = scaleTransform * rotationTransform;
                int quadSize = this.referenceData.Length;
                Color color = this.ColorFromLerp((float)rand.NextDouble());
                float alpha = (float)(rand.NextDouble() * alphaInterval + minAlpha);

                color = Color.White * whiteProportion + color * (1 - whiteProportion);
                color = color * alpha;

                for (int j = 0; j < quadSize; j++)
                {
                    Vector3.Transform(ref this.referenceData[j].Position, ref transform, out vertices[i * quadSize + j].Position);
                    vertices[i * quadSize + j].TexCoord = this.referenceData[j].TexCoord;
                    vertices[i * quadSize + j].Color = color;
                }
            }

            var vertexBuffer = new VertexBuffer(VertexPositionColorTexture.VertexFormat);
            vertexBuffer.SetData(vertices);

            this.mesh = new Mesh(0, vertices.Length, 0, this.nStars * 2, vertexBuffer, indexBuffer, PrimitiveType.TriangleList);
        }

        /// <summary>
        /// Draw the starfield mesh
        /// </summary>
        /// <param name="gameTime">The current game time</param>
        public override void Draw(TimeSpan gameTime)
        {
            Matrix worldTranasform = this.transform.WorldTransform;
            this.RenderManager.DrawMesh(this.mesh, this.materials.DefaultMaterial, ref worldTranasform);
        }

        /// <summary>
        /// Dispose the starfield mesh
        /// </summary>
        /// <param name="disposing">Is disposing</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.mesh != null)
                {
                    WaveServices.GraphicsDevice.DestroyVertexBuffer(this.mesh.VertexBuffer);
                    WaveServices.GraphicsDevice.DestroyIndexBuffer(this.mesh.IndexBuffer);
                }
            }
        }

        /// <summary>
        /// Get color from a lerp position.
        /// </summary>
        /// <param name="lerp">Lerp value</param>
        /// <returns>The color</returns>
        private Color ColorFromLerp(float lerp)
        {
            int index = 0;

            for (int i = 0; i < this.colorRamp.Length; i++)
            {
                index = i;
                if (this.colorRamp[i].Lerp >= lerp)
                {
                    break;
                }
            }

            lerp = (lerp - this.colorRamp[index - 1].Lerp) * (this.colorRamp[index].Lerp - this.colorRamp[index - 1].Lerp);
            return this.colorRamp[index].Color * lerp + this.colorRamp[index - 1].Color * (1 - lerp);
        }
    }
}
