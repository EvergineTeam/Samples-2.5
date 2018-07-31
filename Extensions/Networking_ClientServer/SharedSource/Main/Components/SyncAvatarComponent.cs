using System.Runtime.Serialization;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;
using WaveEngine.Networking.Components;

namespace Networking_ClientServer.Components
{
    [DataContract]
    public class SyncAvatarComponent : NetworkBytePropertySync<PlayerProperties>
    {
        [RequiredComponent]
        protected SpriteAtlas spriteAtlas;

        protected override void DefaultValues()
        {
            base.DefaultValues();
            this.ProviderFilter = NetworkPropertyProviderFilter.Player;
            this.PropertyKey = PlayerProperties.Avatar;
        }

        protected override void Initialize()
        {
            base.Initialize();

            if (!WaveServices.Platform.IsEditor)
            {
                this.UpdateAvatarIndex();
            }
        }

        protected override void OnPropertyAddedOrChanged()
        {
            this.UpdateAvatarIndex();
        }

        protected override void OnPropertyRemoved()
        {
        }

        private void UpdateAvatarIndex()
        {
            this.spriteAtlas.TextureIndex = this.PropertyValue;
        }
    }
}
