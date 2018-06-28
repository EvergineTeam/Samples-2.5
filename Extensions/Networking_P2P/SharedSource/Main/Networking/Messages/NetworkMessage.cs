using Lidgren.Network;
using WaveEngine.Common.Math;
using WaveEngine.Networking.Connection.Messages;

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

        public static OutgoingMessage CreateMessage(P2PMessageType messageType, string playerId, string s)
        {
            var message = new OutgoingMessage(new NetBuffer());
            message.Write((int)messageType);
            message.Write(playerId);
            message.Write(s);

            return message;
        }

        public static OutgoingMessage CreateMessage(P2PMessageType messageType, string playerId, Vector2 vector)
        {
            var message = new OutgoingMessage(new NetBuffer());
            message.Write((int)messageType);
            message.Write(playerId);
            message.Write(vector);

            return message;
        }
    }
}
