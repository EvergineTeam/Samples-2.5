using Networking_P2P.Extensions;
using Networking_P2P.Networking.Messages;
using System;
using System.Linq;
using System.Runtime.Serialization;
using WaveEngine.Common.Input;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Managers;
using WaveEngine.Framework.Services;
using WaveEngine.Networking.P2P;

namespace Networking_P2P.Behaviors
{
    [DataContract]
    public class MovementBehavior : Behavior
    {
        [RequiredComponent]
        protected Transform2D transform;

        private VirtualScreenManager virtualScreenManager;

        private NetworkPeerService networkPeerService;

        protected override void Initialize()
        {
            base.Initialize();

            this.virtualScreenManager = this.Owner.Scene.VirtualScreenManager;
            this.networkPeerService = this.networkPeerService = WaveServices.GetService<NetworkPeerService>();
        }

        protected override async void Update(TimeSpan gameTime)
        {
            var keyState = WaveServices.Input.KeyboardState;
            var touchState = WaveServices.Input.TouchPanelState;

            var screenXCenter = this.virtualScreenManager.VirtualWidth / 2;
            var screenYCenter = this.virtualScreenManager.VirtualHeight / 2;

            // Collect input
            var input = Vector2.Zero;

            if (touchState.Count > 0)
            {
                var touchPosition = touchState.First().Position;

                input.X = touchPosition.X > screenXCenter ? 1 : -1;
                input.Y = touchPosition.Y > screenYCenter ? 1 : -1;
            }
            else if (keyState.IsConnected)
            {
                // Use Arrows or WASD to move avatar
                if (keyState.IsKeyPressed(Keys.Left) ||
                    keyState.IsKeyPressed(Keys.A))
                {
                    input.X = -1;
                }
                else if (keyState.IsKeyPressed(Keys.Right) ||
                         keyState.IsKeyPressed(Keys.D))
                {
                    input.X = 1;
                }

                if (keyState.IsKeyPressed(Keys.Up) ||
                    keyState.IsKeyPressed(Keys.W))
                {
                    input.Y = -1;
                }
                else if (keyState.IsKeyPressed(Keys.Down) ||
                         keyState.IsKeyPressed(Keys.S))
                {
                    input.Y = 1;
                }
            }

            if (input != Vector2.Zero)
            {
                this.transform.Position += input;
            }

            var playerId = await networkPeerService.GetIPAddress();
            var message = NetworkMessage.CreateMessage(Networking.P2PMessageType.Move, playerId.Sanitize(), this.transform.Position);
            await networkPeerService.SendBroadcastAsync(message);
        }
    }
}
