using System;
using System.Runtime.Serialization;
using WaveEngine.Framework;
using WaveEngine.Networking.Components;

namespace Networking_ClientServer.Components
{
    [DataContract]
    public class SyncVisibilityComponent : NetworkBooleanPropertySync<PlayerProperties>
    {
        [RequiredComponent]
        protected VisibilityTracker visibilityTracker;

        protected override void DefaultValues()
        {
            base.DefaultValues();
            this.ProviderFilter = NetworkPropertyProviderFilter.Player;
            this.PropertyKey = PlayerProperties.IsVisible;
        }

        protected override void Initialize()
        {
            base.Initialize();

            var playerProvider = this.propertiesTableProvider as NetworkPlayerProvider;
            if (playerProvider?.Player?.IsLocalPlayer == true)
            {
                this.visibilityTracker.VisibilityChanged += this.VisibilityTracker_VisibilityChanged;
                this.VisibilityTracker_VisibilityChanged(this, EventArgs.Empty);
            }
        }

        protected override void OnPropertyAddedOrChanged()
        {
            this.Owner.IsVisible = this.PropertyValue;
        }

        protected override void OnPropertyRemoved()
        {
            this.Owner.IsVisible = false;
        }

        private void VisibilityTracker_VisibilityChanged(object sender, EventArgs e)
        {
            this.PropertyValue = this.Owner.IsVisible;
        }
    }
}