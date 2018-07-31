#region Using Statements
using System;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;
using WaveEngine.LeapMotion;
#endregion

namespace LeapMotionSample
{
    public class Game : WaveEngine.Framework.Game
    {
        public override void Initialize(IApplication application)
        {
            base.Initialize(application);

            this.Load(WaveContent.GameInfo);
            LeapMotionService leapMotionService = new LeapMotionService();
            WaveServices.RegisterService(leapMotionService);
            leapMotionService.StartSensor(LeapFeatures.Hands | LeapFeatures.CameraImages);

            ScreenContext screenContext = new ScreenContext(new MyScene());	
			WaveServices.ScreenContextManager.To(screenContext);
        }
    }
}
