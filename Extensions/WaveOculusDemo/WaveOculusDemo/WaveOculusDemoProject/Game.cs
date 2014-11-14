#region Using Statements
using System;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;
using WaveOculusDemoProject.Sensor;
#endregion

namespace WaveOculusDemoProject
{
    public class Game : WaveEngine.Framework.Game
    {
        public override void Initialize(IApplication application)
        {
            base.Initialize(application);

#if WINDOWS
            //Register oculus rift service
            WaveServices.RegisterService<WaveEngine.OculusRift.OVRService>(new WaveEngine.OculusRift.OVRService(application)); 
#endif
            WaveServices.RegisterService<HeadTrackSensor>(new HeadTrackSensor()); 

            ScreenContext screenContext = new ScreenContext(new MyScene());
            WaveServices.ScreenContextManager.To(screenContext);
        }
    }
}
