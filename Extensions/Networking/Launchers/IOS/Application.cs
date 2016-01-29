using System;
using System.Collections.Generic;
using WaveEngine.Framework.Services;
using Foundation;
using UIKit;

namespace Networking
{
    [Register("AppDelegate")]
    public class Application : WaveEngine.Adapter.Application
    {
        private static void IncludeAssemblies()
        {
            // Include WaveEngine Components
            var a1 = typeof(WaveEngine.Components.Cameras.FreeCamera3D);
        }

        private Networking.Game game;

        public Application()
        {
        }

        public override void Initialize()
        {
            game = new Networking.Game();
            game.Initialize(this);
        }

        public override void Update(TimeSpan elapsedTime)
        {
            game.UpdateFrame(elapsedTime);
        }

        public override void Draw(TimeSpan elapsedTime)
        {
            game.DrawFrame(elapsedTime);
        }

        public override void OnResignActivation(UIApplication application)
        {
            base.OnResignActivation(application);
            game.OnDeactivated();
        }

        public override void OnActivated(UIApplication application)
        {
            if (game != null)
            {
                game.OnActivated();
            }
            base.OnActivated(application);
        }
    }
}

