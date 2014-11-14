using System;
using System.Windows.Controls;
using WaveEngine.Adapter;
using WaveEngine.Common;
using Windows.Foundation;

namespace LauncherWP
{
    public class GameRenderer : Application
    {
        public int Width { get; set; }
        public int Height { get; set; }

        private WaveOculusDemoProject.Game game;

        public GameRenderer(Size windowBounds, int scaleFactor, MediaElement element)
            : base(windowBounds, scaleFactor, element)
        {
        }

        public override void Update(TimeSpan gameTime)
        {
            game.UpdateFrame(gameTime);
        }

        public override void Draw(TimeSpan gameTime)//TargetBase render)
        {
            //Render
            game.DrawFrame(gameTime);
        }

        public override void Initialize()
        {
            base.Initialize();

            // Initialize
            game = new WaveOculusDemoProject.Game();
            game.Initialize(this);
        }

        public override void OnResuming()
        {
            game.OnActivated();
        }

        public override void OnSuspending()
        {
            game.OnDeactivated();
        }      
    }
}
