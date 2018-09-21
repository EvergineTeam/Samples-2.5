using System;
using System.Collections.Generic;
using System.Linq;
using WaveEngine.Common;
using WaveEngine.Framework.Services;
using WaveEngine.Networking.P2P;
using WaveEngine.Networking.P2P.Players;

namespace Networking_P2P
{
    class P2PNetworkServerService : Service, IDisposable
    {
        private Queue<int> availableAvatars;
        private P2PServerService matchmakingServerService;

        protected override void Initialize()
        {
            base.Initialize();

            this.availableAvatars = new Queue<int>(Enumerable.Range(1, NetworkConfiguration.NumberOfPlayers));

            this.matchmakingServerService = WaveServices.GetService<P2PServerService>();
            this.matchmakingServerService.PlayerConnected += this.P2PServerService_PlayerJoined;
            this.matchmakingServerService.PlayerDisconnected += this.P2PServerService_PlayerLeft;

            this.matchmakingServerService.Start(NetworkConfiguration.Port);
        }

        private void P2PServerService_PlayerJoined(object sender, P2PRemotePlayer player)
        {
            var avatarIndex = this.availableAvatars.Dequeue();
            player.CustomProperties.Set((byte)PlayerProperties.Avatar, avatarIndex);
        }

        private void P2PServerService_PlayerLeft(object sender, P2PRemotePlayer player)
        {
            var spriteIndex = player.CustomProperties.GetByte((byte)PlayerProperties.Avatar);
            this.availableAvatars.Enqueue(spriteIndex);
        }

        public void Dispose()
        {
            if (this.matchmakingServerService != null)
            {
                this.matchmakingServerService.PlayerConnected -= this.P2PServerService_PlayerJoined;
                this.matchmakingServerService.PlayerDisconnected -= this.P2PServerService_PlayerLeft;
            }
        }
    }
}
