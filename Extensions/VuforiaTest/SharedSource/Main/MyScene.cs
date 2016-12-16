#region Using Statements
using System;
using System.Diagnostics;
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
using WaveEngine.Vuforia;
#endregion

namespace VuforiaTest
{
    public class MyScene : Scene
    {
        protected override void CreateScene()
        {
            this.Load(WaveContent.Scenes.MyScene);
        }

        protected override void Start()
        {
            base.Start();

            var vuforiaService = WaveServices.GetService<VuforiaService>();
            if (vuforiaService != null && vuforiaService.IsSupported)
            {
                vuforiaService.StartTrack(true);
                vuforiaService.TrackNameChanged += VuforiaService_TrackNameChanged;
            }
        }

        private void VuforiaService_TrackNameChanged(object sender, string newTrackName)
        {
            Debug.WriteLine("TRACK: " + newTrackName);
        }
    }
}
