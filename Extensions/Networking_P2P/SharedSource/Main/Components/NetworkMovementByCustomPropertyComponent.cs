using Networking_P2P.Networking;
using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Networking.P2P.Providers;

namespace Networking_P2P.Components
{
    [DataContract]
    public class NetworkMovementByCustomProperty : WaveEngine.Networking.P2P.Synchronization.NetworkVector2PropertySync<P2PMessageType>
    {
        [RequiredComponent(false)]
        protected Transform2D transform;

        protected override void DefaultValues()
        {
            base.DefaultValues();
            this.ProviderFilter = WaveEngine.Networking.Components.NetworkPropertyProviderFilter.Player;
            this.PropertyKey = P2PMessageType.Move;
        }

        protected override void Initialize()
        {
            base.Initialize();

            var playerProvider = this.propertiesTableProvider as NetworkPlayerProvider;

            if (playerProvider?.Player?.IsLocalPlayer == true)
            {
                this.transform.PositionChanged -= this.OnTransformPositionChanged;
                this.transform.PositionChanged += this.OnTransformPositionChanged;
                this.OnTransformPositionChanged(this, EventArgs.Empty);
            }
        }

        protected override void OnPropertyAddedOrChanged()
        {
            var playerProvider = this.propertiesTableProvider as NetworkPlayerProvider;

            if (playerProvider?.Player?.IsLocalPlayer != true)
            {
                this.transform.Position = this.PropertyValue;

                Debug.WriteLine(string.Format("OnPropertyAddedOrChanged: {0},{1}", this.PropertyValue.X, this.PropertyValue.Y));
            }
        }

        protected override void OnPropertyRemoved()
        {
        }

        private void OnTransformPositionChanged(object sender, EventArgs e)
        {
            this.PropertyValue = this.transform.Position;
        }
    }
}
