#region Using Statements
using WaveEngine.Common;
using WaveEngine.Framework.Services;
using WaveEngine.Kinect;
using WaveEngine.Kinect.Enums;
#endregion

namespace KinectSample
{
    public class Game : WaveEngine.Framework.Game
    {
        public override void Initialize(IApplication application)
        {
            base.Initialize(application);           

            // Register KinectServices
            var kinectService = new KinectService();
            WaveServices.RegisterService(kinectService);
            kinectService.StartSensor(KinectSources.Color | KinectSources.Body);//| KinectSources.Face);

            ScreenContext screenContext = new ScreenContext(new MyScene());	
			WaveServices.ScreenContextManager.To(screenContext);
        }
    }
}
