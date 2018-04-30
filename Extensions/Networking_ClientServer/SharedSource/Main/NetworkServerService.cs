using System;
using System.Collections.Generic;
using System.Linq;
using WaveEngine.Common;
using WaveEngine.Framework.Services;
using WaveEngine.Networking.Server;
using WaveEngine.Networking.Server.Players;

namespace Networking_ClientServer
{
    public class NetworkServerService : Service, IDisposable
    {
        private Queue<int> availableAvatars;
        private MatchmakingServerService matchmakingServerService;

        protected override void Initialize()
        {
            base.Initialize();

            this.availableAvatars = new Queue<int>(Enumerable.Range(1, NetworkConfiguration.NumberOfPlayers));

            this.matchmakingServerService = WaveServices.GetService<MatchmakingServerService>();
            this.matchmakingServerService.PlayerJoined += this.MatchmakingServerService_PlayerJoined;
            this.matchmakingServerService.PlayerLeft += this.MatchmakingServerService_PlayerLeft;

            this.matchmakingServerService.Start(NetworkConfiguration.Port);
        }

        private void MatchmakingServerService_PlayerJoined(object sender, ServerPlayer player)
        {
            var avatarIndex = this.availableAvatars.Dequeue();
            player.CustomProperties.Set((byte)PlayerProperties.Avatar, avatarIndex);
        }

        private void MatchmakingServerService_PlayerLeft(object sender, ServerPlayer player)
        {
            var spriteIndex = player.CustomProperties.GetByte((byte)PlayerProperties.Avatar);
            this.availableAvatars.Enqueue(spriteIndex);
        }

        public void Dispose()
        {
            if (this.matchmakingServerService != null)
            {
                this.matchmakingServerService.PlayerJoined -= this.MatchmakingServerService_PlayerJoined;
                this.matchmakingServerService.PlayerLeft -= this.MatchmakingServerService_PlayerLeft;
            }
        }
    }
}
