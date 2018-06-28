using Lidgren.Network;
using WaveEngine.Common.Math;
using WaveEngine.Framework.Services;
using WaveEngine.Networking.Messages;
using WaveEngine.Networking.P2P;

namespace Networking_P2P.Networking.Messages
{
    public static class NetworkMessage
    {
        public static P2PMessageType MessageType { get; private set; }

        public static string PlayerId
        {
            get;
            private set;
        }

        public static OutgoingMessage CreateMessage(P2PMessageType messageType, string playerId, string s = "")
        {
            var service = WaveServices.GetService<NetworkPeerService>();
            var message = service.CreateMessage();
            message.Write((int)messageType);
            message.Write(playerId);

            if (!string.IsNullOrEmpty(s))
            {
                message.Write(s);
            }

            return message;
        }

        public static OutgoingMessage CreateMessage(P2PMessageType messageType, string playerId, Vector2 vector)
        {
            var service = WaveServices.GetService<NetworkPeerService>();
            var message = service.CreateMessage();
            message.Write((int)messageType);
            message.Write(playerId);
            message.Write(vector);

            return message;
        }
    }
}
