#region Using Statements
using System;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Resources;
using WaveEngine.Framework.Services;
#endregion

namespace Xamarin_iOS
{
    public class MyScene : Scene
    {
        private Spinner spinner;

        protected override void CreateScene()
        {
            this.Load(WaveContent.Scenes.MyScene);

            this.spinner = this.EntityManager.Find("cube")?.FindComponent<Spinner>();
        }

        internal void UpdateAutoRotation(bool enabled)
        {
            if (spinner != null)
            {
                this.spinner.IsActive = enabled;
            }
        }
    }
}
