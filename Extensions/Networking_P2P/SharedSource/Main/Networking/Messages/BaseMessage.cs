using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;
using WaveEngine.Common.Math;
using WaveEngine.Networking.Connection.Messages;

namespace Networking_P2P.Networking.Messages
{
    public class BaseMessage
    {
        public P2PMessageType MessageType { get; private set; } 

        public OutgoingMessage Message { get; private set; }

        public BaseMessage(P2PMessageType messageType)
        {
            this.MessageType = messageType;
        }

        protected void CreateMessage(string s)
        {
            Message = new OutgoingMessage(new NetBuffer());
            Message.Write((int)this.MessageType);
            Message.Write(s);
        }

        protected void CreateMessage(Vector2 vector)
        {
            Message = new OutgoingMessage(new NetBuffer());
            Message.Write((int)this.MessageType);
            Message.Write(vector);
        }
    }
}
