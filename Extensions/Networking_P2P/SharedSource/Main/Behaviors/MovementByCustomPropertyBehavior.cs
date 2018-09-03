using Networking_P2P.Networking;
using System.Diagnostics;
using System.Runtime.Serialization;
using WaveEngine.Common.Math;
using WaveEngine.Framework.Services;
using WaveEngine.Networking.P2P;
using WaveEngine.Networking.P2P.Players;

namespace Networking_P2P.Behaviors
{
    [DataContract]
    public class MovementByCustomPropertyBehavior : MovementBaseBehavior
    {
        private NetworkPlayer networkPlayer;
        private NetworkPeerService networkPeerService;
        private Vector2 lastPosition = Vector2.Zero;

        public MovementByCustomPropertyBehavior(NetworkPlayer player)
        {
            this.networkPlayer = player;
            this.networkPeerService = WaveServices.GetService<NetworkPeerService>();
        }

        protected override void CurrentNetworkBehavior()
        {
            base.CurrentNetworkBehavior();

            if (this.lastPosition != this.transform.Position)
            {
                if (this.networkPlayer != null)
                {
                    this.networkPlayer.CustomProperties.Set((byte)P2PMessageType.Position, this.transform.Position);

                    //Debug.WriteLine("SetCustomProperties:" + this.transform.Position);
                }

                this.lastPosition = this.transform.Position;
            }
        }
    }
}
