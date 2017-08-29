using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WaveEngine.Common.Math;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Components.Toolkit;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;

namespace AsyncAwait.Scenes
{
    public class HeavyLoadScene : Scene
    {
        private List<Texture2D> textures;

        protected override void CreateScene()
        {
            this.Load(WaveContent.Scenes.HeavyLoadScene);
            this.textures = ImagesHelper.Images.Select(this.LoadTexture).ToList();
            this.EntityManager.FindComponentFromEntityPath<TextComponent>("label.text").Text = string.Format("Loaded {0} of {0} images", this.textures.Count);
        }

        protected override async void Start()
        {
            base.Start();

            while (this.textures.Count > 0 && !this.IsDisposed)
            {
                var texture = this.textures[0];
                this.textures.Remove(texture);
                this.CreateImageSprite(texture);

                await Task.Delay(2000);
            }
        }

        private void CreateImageSprite(Texture2D texture)
        {
            this.EntityManager.Remove("loadedSprite");

            var spriteComponent = new Sprite();
            var sprite = new Entity("loadedSprite")
                .AddComponent(new Transform2D { LocalDrawOrder = 10, TranformMode = Transform2D.TransformMode.Screen, ScreenPosition = new Vector2(0.5f, 0.5f), Origin = new Vector2(0.5f, 0.5f) })
                .AddComponent(spriteComponent)
                .AddComponent(new SpriteRenderer());

            this.EntityManager.Add(sprite);

            spriteComponent.Texture = texture;
        }

        private Texture2D LoadTexture(string imageUrl)
        {
            using (var imageStreamResult = ImagesHelper.LoadImageStream(imageUrl))
            {
                var texture = Texture2D.FromFile(this.RenderManager.GraphicsDevice, imageStreamResult.Stream);
                return texture;
            }
        }
    }
}
