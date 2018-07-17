using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Common.Input;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Managers;
using WaveEngine.Framework.Services;
using WaveEngine.Networking.P2P;

namespace Networking_P2P.Behaviors
{
    [DataContract]
    public class MovementBaseBehavior : Behavior
    {
        [RequiredComponent]
        protected Transform2D transform;


        protected override void Initialize()
        {
            base.Initialize();

            this.CurrentInitialize();
        }

        protected override void Update(TimeSpan gameTime)
        {
            this.CurrentNetworkBehavior();
        }

        protected virtual void CurrentNetworkBehavior()
        {
        }

        protected virtual void CurrentInitialize()
        {
        }
    }
}
