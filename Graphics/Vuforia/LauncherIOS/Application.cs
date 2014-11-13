using System;
using System.Collections.Generic;
using WaveEngine.Framework.Services;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System;

namespace LauncherIOS
{
    [Register("AppDelegate")]
    public class Application : WaveEngine.Adapter.Application
    {
        private VuforiaProject.Game game;

        public Application()
        {
        }

        public override void Initialize()
        {
            game = new VuforiaProject.Game();
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

