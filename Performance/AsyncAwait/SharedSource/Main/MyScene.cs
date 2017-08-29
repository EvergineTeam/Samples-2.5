using AsyncAwait.Scenes;
using System;
using WaveEngine.Common.Math;
using WaveEngine.Components.GameActions;
using WaveEngine.Components.Gestures;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
using WaveEngine.Framework.Threading;

namespace AsyncAwait
{
    public class MyScene : Scene
    {
        private Entity syncSceneLoadButton;
        private Entity asyncImagesLoadButton;
        private Entity asyncSceneLoadButton;

        protected override void CreateScene()
        {
            this.Load(WaveContent.Scenes.MyScene);

            this.syncSceneLoadButton = this.EntityManager.Find("buttons.syncSceneLoad");
            this.asyncImagesLoadButton = this.EntityManager.Find("buttons.asyncImagesLoad");
            this.asyncSceneLoadButton = this.EntityManager.Find("buttons.asyncSceneLoad");

            this.syncSceneLoadButton.FindComponent<TouchGestures>().TouchTap += this.SyncSceneLoad;
            this.asyncImagesLoadButton.FindComponent<TouchGestures>().TouchTap += this.AsyncImagesLoad;
            this.asyncSceneLoadButton.FindComponent<TouchGestures>().TouchTap += this.AsyncSceneLoad;
        }

        private void SyncSceneLoad(object sender, GestureEventArgs e)
        {
            ScreenContext screenContext = new ScreenContext(new HeavyLoadScene());
            WaveServices.ScreenContextManager.To(screenContext);
        }

        private async void AsyncImagesLoad(object sender, GestureEventArgs e)
        {
            await this.AnimateClick(this.asyncImagesLoadButton).AsTask().ConfigureWaveAwait(WaveTaskContinueOn.Foreground);
            ScreenContext screenContext = new ScreenContext(new LoadImagesScene());
            WaveServices.ScreenContextManager.To(screenContext);
        }

        private async void AsyncSceneLoad(object sender, GestureEventArgs e)
        {
            var sceneTask = WaveBackgroundTask.Run(() =>
            {
                var heavyScene = new HeavyLoadScene();
                heavyScene.Initialize(WaveServices.GraphicsDevice);
                return heavyScene;
            });

            await this.CreateLoopGameActionUntil(() => this.LoadingAnimation(this.asyncSceneLoadButton), () => sceneTask.IsCompleted).AsTask();

            var scene = await sceneTask.ConfigureWaveAwait(WaveTaskContinueOn.Foreground);
            ScreenContext screenContext = new ScreenContext(scene);
            WaveServices.ScreenContextManager.To(screenContext);
        }

        public IGameAction LoadingAnimation(Entity entity)
        {
            var transform = entity.FindComponent<Transform2D>();
            return new FloatAnimationGameAction(entity, 0f, 2 * MathHelper.Pi, TimeSpan.FromMilliseconds(800), EaseFunction.None, val =>
            {
                transform.LocalRotation = val;
            });
        }

        public IGameAction AnimateClick(Entity entity)
        {
            var transform = entity.FindComponent<Transform2D>();
            return this.CreateEmptyGameAction().ContinueWith(new FloatAnimationGameAction(entity, 1f, 0f, TimeSpan.FromMilliseconds(800), EaseFunction.BackInOutEase, v =>
            {
                transform.LocalScale = new Vector2(v);
            }));
        }
    }
}
