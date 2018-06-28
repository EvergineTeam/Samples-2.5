using Lidgren.Network;
using WaveEngine.Networking.Connection.Messages;

namespace Networking_P2P.Networking.Messages
{
    public class NewPlayerMessage : BaseMessage
    {
        private string playerId;

        public NewPlayerMessage(string playerId)
            : base(P2PMessageType.NewPlayer)
        {
            this.PlayerId = playerId;
        }

        public string PlayerId
        {
            get { return this.playerId; }
            set
            {
                this.playerId = value;
                CreateMessage(this.playerId);
            }
        }
    }
}
