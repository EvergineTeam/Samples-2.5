#region Using Statements
using System;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;
using WaveEngine.Vuforia;
#endregion

namespace VuforiaTest
{
    public class Game : WaveEngine.Framework.Game
    {
        public override void Initialize(IApplication application)
        {
            base.Initialize(application);

            ScreenContext screenContext = new ScreenContext(new MyScene());
            WaveServices.ScreenContextManager.To(screenContext);

            string liscenseKey = "Afv3YtH/////AAAAAd6O5swHskO5iA9kEPTrLhUfL0+AXF0Kmbc37H865djR8OyOWZxNgHLluSS1NML85StoInIgPw/UPEmcMfjV6KNwSn/enTSseboJxd8zrYfPY8v4iuhuZlyJ7K918/xnwDUS17Qx3o/6Fx2eVqU4puzoZNsLoMhpFTZbBgrpbZ5aCsLhz0jxsVZO729c6EXPZvMgLDaseU3DRgbSbk4cnn8LiQNhpeYVo5sxl3eqZKnu4HyDLhdCuGdtKEOChFKQShd7QcXnlxijV7+q6j1yI8GP8H75YFg5kYXf5nDquvbOGX7tXU17E/x62mRW+U0rJJ9ZYCQ857eo+PPKL4ZsEpAvGURGYpeSxtFgH0L8uyQ0";
            WaveServices.RegisterService(new VuforiaService(WaveContent.Assets.AR.TestVuforia_xml, liscenseKey));
        }
    }
}
