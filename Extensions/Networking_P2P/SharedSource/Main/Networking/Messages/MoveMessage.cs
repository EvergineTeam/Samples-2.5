using Lidgren.Network;
using WaveEngine.Common.Math;
using WaveEngine.Networking.Connection.Messages;

namespace Networking_P2P.Networking.Messages
{
    public class MoveMessage : BaseMessage
    {
        private Vector2 position;

        public MoveMessage(Vector2 position) 
            : base(P2PMessageType.Move)
        {
            this.Position = position;
        }

        public Vector2 Position
        {
            get { return this.position; }
            set
            {
                this.position = value;
                CreateMessage(this.position);
            }
        }
    }
}
