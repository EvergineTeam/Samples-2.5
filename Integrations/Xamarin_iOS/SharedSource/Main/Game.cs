#region Using Statements
using System;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;
#endregion

namespace Xamarin_iOS
{
    public class Game : WaveEngine.Framework.Game
    {
        private MyScene scene;

        public override void Initialize(IApplication application)
        {
            base.Initialize(application);

            this.scene = new MyScene();
            ScreenContext screenContext = new ScreenContext(this.scene);	
			WaveServices.ScreenContextManager.To(screenContext);
        }

        public void UpdateAutoRotation(bool enabled)
        {
            this.scene.UpdateAutoRotation(enabled);
        }
    }
}
