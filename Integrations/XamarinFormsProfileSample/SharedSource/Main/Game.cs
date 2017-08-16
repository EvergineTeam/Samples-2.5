#region Using Statements
using System;
using WaveEngine.Common;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;
#endregion

namespace XamarinFormsProfileSample
{
    public class Game : WaveEngine.Framework.Game
    {
        private MyScene _scene;
        public override void Initialize(IApplication application)
        {
            SerializerFactory.DefaultSerializationType = SerializationType.DATACONTRACT;

            base.Initialize(application);

            _scene = new MyScene();
            _scene.Started += OnStarted;

            ScreenContext screenContext = new ScreenContext(_scene);
            WaveServices.ScreenContextManager.To(screenContext);
        }

        private void OnStarted(object sender, EventArgs e)
        {
#if WINDOWS_UWP || ANDROID
            Helpers.WaveEngineFacade.Initialize();
#endif
        }
    }
}
