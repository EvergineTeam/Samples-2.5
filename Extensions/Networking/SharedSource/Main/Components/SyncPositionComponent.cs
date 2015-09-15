using System.Runtime.Serialization;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Networking;
using WaveEngine.Networking.Messages;

namespace Networking.Components
{
    [DataContract(Namespace = "Networking.Components")]
    public class SyncPositionComponent : NetworkSyncComponent
    {
        private Vector2 lastPosition;

        [RequiredComponent]
        protected Transform2D transform;

        public override void ReadSyncData(IncomingMessage reader)
        {
            var x = reader.ReadSingle();
            var y = reader.ReadSingle();
            this.transform.Position = new Vector2(x, y);
        }

        public override bool NeedSendSyncData()
        {
            //Only data need to be synced if have changed
            return this.lastPosition != this.transform.Position;
        }

        public override void WriteSyncData(OutgoingMessage writer)
        {
            this.lastPosition = this.transform.Position;
            writer.Write(this.lastPosition.X);
            writer.Write(this.lastPosition.Y);
        }
    }
}