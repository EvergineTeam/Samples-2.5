using System;
using System.Runtime.Serialization;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Networking.Components;

namespace Networking_ClientServer.Components
{
    [DataContract]
    public class SyncPositionComponent : NetworkVector3PropertySync<PlayerProperties>
    {
        [RequiredComponent(false)]
        protected Transform3D transform;

        protected override void DefaultValues()
        {
            base.DefaultValues();
            this.ProviderFilter = NetworkPropertyProviderFilter.Player;
            this.PropertyKey = PlayerProperties.Position;
        }

        protected override void Initialize()
        {
            base.Initialize();

            var playerProvider = this.propertiesTableProvider as NetworkPlayerProvider;
            if (playerProvider?.Player?.IsLocalPlayer == true)
            {
                this.transform.PositionChanged += this.Transform_PositionChanged;
                this.Transform_PositionChanged(this, EventArgs.Empty);
            }
        }

        protected override void OnPropertyAddedOrChanged()
        {
            this.transform.Position = this.PropertyValue;
        }

        protected override void OnPropertyRemoved()
        {
        }

        private void Transform_PositionChanged(object sender, EventArgs e)
        {
            this.PropertyValue = this.transform.Position;
        }
    }
}