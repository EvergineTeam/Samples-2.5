using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Networking.P2P.Players;

namespace Networking_P2P.Components
{
    [DataContract]
    public class NetworkMovementByCustomProperty : Component
    {
        private readonly NetworkPlayer networkPlayer;

        private Vector2 amount = Vector2.Zero;

        [RequiredComponent]
        private Transform2D transform = null;

        public NetworkMovementByCustomProperty(NetworkPlayer nPlayer)
        {
            this.networkPlayer = nPlayer;
            this.networkPlayer.CustomProperties.PropertyChanged += CustomProperties_PropertyChanged;
        }

        private void CustomProperties_PropertyChanged(object sender, byte e)
        {
            if (e == 0)
            {
                amount = this.networkPlayer.CustomProperties.GetVector2(0);
            }
        }

        private void NetworkPlayer_OnCustomPropertiesChanged(object sender, EventArgs e)
        {
            if (amount != Vector2.Zero)
            {
                this.transform.Position = amount;
                amount = Vector2.Zero;
            }
        }
    }
}
