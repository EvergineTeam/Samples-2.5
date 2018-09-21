#region Using Statements
using Networking_P2P.Scenes;
using System;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;
using WaveEngine.Networking.P2P;
#endregion

namespace Networking_P2P
{
    public class Game : WaveEngine.Framework.Game
    {
        public override void Initialize(IApplication application)
        {
            base.Initialize(application);

            SerializerFactory.DefaultSerializationType = SerializationType.DATACONTRACT;

            this.Load(WaveContent.GameInfo);

            WaveServices.RegisterService(new P2PServerService());
            WaveServices.RegisterService(new P2PClientService());


            ScreenContext screenContext = new ScreenContext(new MainScene());	
			WaveServices.ScreenContextManager.To(screenContext);
        }
    }
}
