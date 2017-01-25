using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Framework;
using WaveEngine.Networking;
using WaveEngine.Networking.Messages;

namespace Networking.Components
{
    [DataContract(Namespace = "Networking.Components")]
    public class SyncAvatarComponent : NetworkSyncComponent
    {
        [RequiredComponent]
        protected SpriteAtlas spriteAtlas;

        private int lastAvatarIndex;

        public override bool NeedSendSyncData()
        {
            return this.lastAvatarIndex != this.spriteAtlas.TextureIndex;
        }

        public override void ReadSyncData(IncomingMessage reader)
        {
            var newIndex = reader.ReadInt32();
            this.spriteAtlas.TextureIndex = newIndex;
        }

        public override void WriteSyncData(OutgoingMessage writer)
        {
            this.lastAvatarIndex = this.spriteAtlas.TextureIndex;
            writer.Write(this.spriteAtlas.TextureIndex);
        }
    }
}
