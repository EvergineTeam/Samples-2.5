using Networking_P2P.Networking;
using System.Diagnostics;
using System.Runtime.Serialization;
using WaveEngine.Common.Math;
using WaveEngine.Framework.Services;
using WaveEngine.Networking.P2P;
using WaveEngine.Networking.P2P.Players;

namespace Networking_P2P.Behaviors
{
    [DataContract]
    public class MovementByCustomPropertyBehavior : MovementBaseBehavior
    {
        private Vector2 lastPosition = Vector2.Zero;

        public MovementByCustomPropertyBehavior()
        {
        }

        protected override void CurrentNetworkBehavior(Vector2 input)
        {
            base.CurrentNetworkBehavior(input);

            var newPos = this.transform.Position + input;

            if (this.lastPosition != newPos)
            {
                this.transform.Position = newPos;

                this.lastPosition = this.transform.Position;
            }
        }
    }
}
