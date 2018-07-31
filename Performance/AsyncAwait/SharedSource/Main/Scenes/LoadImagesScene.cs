using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WaveEngine.Common.Math;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Components.Toolkit;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Threading;

namespace AsyncAwait.Scenes
{
    public class LoadImagesScene : Scene
    {
        private CancellationTokenSource disposedCancellationTokenSource = new CancellationTokenSource();

        private CancellationToken DisposedCancellationToken { get { return this.disposedCancellationTokenSource.Token; } }

        private int totalImages;
        private int downloadedImages;

        protected override void CreateScene()
        {
            this.Load(WaveContent.Scenes.LoadImagesScene);
        }

        protected override void Start()
        {
            base.Start();
            this.StartLoading();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            this.disposedCancellationTokenSource.Cancel();
        }

        private async void StartLoading()
        {
            try
            {
                var textures = ImagesHelper.Images.Select(x => this.LoadTextureAsync(x, this.DisposedCancellationToken)).ToList();
                this.totalImages = textures.Count;
                while (textures.Count > 0)
                {
                    var textureTask = await Task.WhenAny(textures).ConfigureAwait(false);
                    textures.Remove(textureTask);

                    var texture = await textureTask.ConfigureWaveAwait(WaveTaskContinueOn.Foreground);

                    this.DisposedCancellationToken.ThrowIfCancellationRequested();

                    if (texture != null)
                    {
                        await this.CreateImageSprite(texture).ConfigureAwait(false);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // The scene has been disposed
            }
        }

        private async Task CreateImageSprite(Texture2D texture)
        {
            this.EntityManager.Remove("loadedSprite");

            var spriteComponent = new Sprite();
            var sprite = new Entity("loadedSprite")
                .AddComponent(new Transform2D { LocalDrawOrder = 10, TranformMode = Transform2D.TransformMode.Screen, ScreenPosition = new Vector2(0.5f, 0.5f), Origin = new Vector2(0.5f, 0.5f) })
                .AddComponent(spriteComponent)
                .AddComponent(new SpriteRenderer());

            this.EntityManager.Add(sprite);

            spriteComponent.Texture = texture;

            await Task.Delay(2000);
        }

        private async Task<Texture2D> LoadTextureAsync(string imageUrl, CancellationToken cancellationToken)
        {
            using (var imageStreamResult = await ImagesHelper.LoadImageStreamAsync(imageUrl, cancellationToken).ConfigureWaveAwait(WaveTaskContinueOn.Background))
            {
                if (!imageStreamResult.IsSuccess)
                {
                    return null;
                }

                cancellationToken.ThrowIfCancellationRequested();
                var texture = Texture2D.FromFile(this.RenderManager.GraphicsDevice, imageStreamResult.Stream);
                this.downloadedImages++;
                await WaveForegroundTask.Run((Action)UpdateText, cancellationToken).ConfigureAwait(false);
                return texture;
            }
        }

        private void UpdateText()
        {
            this.EntityManager.FindComponentFromEntityPath<TextComponent>("label.text").Text = string.Format("Loaded {0} of {1} images", this.downloadedImages, this.totalImages);
        }
    }
}
