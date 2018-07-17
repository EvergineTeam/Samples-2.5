using Networking_P2P.Extensions;
using Networking_P2P.Networking.Messages;
using System;
using System.Linq;
using System.Runtime.Serialization;
using WaveEngine.Common.Input;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Managers;
using WaveEngine.Framework.Services;
using WaveEngine.Networking.P2P;
using WaveEngine.Networking.P2P.Players;

namespace Networking_P2P.Behaviors
{
    [DataContract]
    public class MovementByCustomPropertyBehavior : MovementBaseBehavior
    {
        private NetworkPlayer networkPlayer;

        public MovementByCustomPropertyBehavior(NetworkPlayer player)
        {
            this.networkPlayer = player;
        }

        protected override void CurrentNetworkBehavior()
        {
            base.CurrentNetworkBehavior();

            this.networkPlayer.CustomProperties.Set(0, this.transform.Position);
        }
    }
}
