using Networking_P2P.Extensions;
using Networking_P2P.Networking.Messages;
using System.Runtime.Serialization;
using WaveEngine.Common.Math;
using WaveEngine.Framework.Services;
using WaveEngine.Networking.P2P;

namespace Networking_P2P.Behaviors
{
    [DataContract]
    public class MovementBehavior : MovementBaseBehavior
    {
        private NetworkPeerService networkPeerService;

        protected override void CurrentInitialize()
        {
            base.CurrentInitialize();

            this.networkPeerService = this.networkPeerService = WaveServices.GetService<NetworkPeerService>();
        }

        protected override async void CurrentNetworkBehavior(Vector2 input)
        {
            this.transform.Position += input;
            var playerId = await this.networkPeerService.GetIPAddress();
            var message = NetworkMessage.CreateMessage(Networking.P2PMessageType.Position, playerId.Sanitize(), this.transform.Position);
            await this.networkPeerService.SendBroadcastAsync(message);
        }
    }
}
