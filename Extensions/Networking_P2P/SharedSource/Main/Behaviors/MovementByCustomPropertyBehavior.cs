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
        private NetworkPeerService networkPeerService;
        private Vector2 lastPosition = Vector2.Zero;

        public MovementByCustomPropertyBehavior(NetworkPlayer player)
        {
            this.networkPeerService = WaveServices.GetService<NetworkPeerService>();
        }

        protected override void CurrentNetworkBehavior()
        {
            base.CurrentNetworkBehavior();

            if (this.lastPosition != this.transform.Position)
            {
                if (this.networkPeerService.Player != null)
                {
                    this.networkPeerService.Player.CustomProperties.Set(0, this.transform.Position);
                }

                this.lastPosition = this.transform.Position;
            }
        }
    }
}
