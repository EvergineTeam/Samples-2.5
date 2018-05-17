using Networking_ClientServer.Components;
using System.Linq;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
using WaveEngine.Networking.Client;
using WaveEngine.Networking.Client.Players;
using WaveEngine.Networking.Components;
using WaveEngine.Networking.Players;

namespace Networking_ClientServer.Scenes
{
    /// <summary>
    /// This scene contains the network entities that uses the SyncComponents.
    /// </summary>
    public class GameScene : Scene
    {
        private MatchmakingClientService matchmakingClientService;

        protected override void CreateScene()
        {
            this.Load(WaveContent.Scenes.GameScene);

            this.matchmakingClientService = WaveServices.GetService<MatchmakingClientService>();
            this.matchmakingClientService.StateChanged += this.MatchmakingClientService_StateChanged;
        }

        private void MatchmakingClientService_StateChanged(object sender, ClientStates state)
        {
            if(state != ClientStates.Joined)
            {
                ScreenContext screenContext = new ScreenContext(new MainScene());
                WaveServices.ScreenContextManager.To(screenContext);
            }
        }

        private void CurrentRoom_PlayerJoined(object sender, RemoteNetworkPlayer remotePlayer)
        {
            this.AddPlayer(remotePlayer);
        }

        private void CurrentRoom_PlayerLeaving(object sender, RemoteNetworkPlayer remotePlayer)
        {
            this.RemovePlayer(remotePlayer);
        }

        protected override void Start()
        {
            // Add local player entity
            this.AddPlayer(this.matchmakingClientService.LocalPlayer);

            // Add existing remote players entity
            var currentRoom = this.matchmakingClientService.CurrentRoom;
            currentRoom.PlayerJoined += this.CurrentRoom_PlayerJoined;
            currentRoom.PlayerLeaving += this.CurrentRoom_PlayerLeaving;

            foreach (var remotePlayer in currentRoom.RemotePlayers.ToList())
            {
                this.AddPlayer(remotePlayer);
            }
        }

        private void AddPlayer(INetworkPlayer player)
        {
            var playerEntity = this.EntityManager.Instantiate(WaveContent.Prefabs.Player);
            playerEntity.Name = this.GetEntityName(player);

            if (player is LocalNetworkPlayer)
            {
                var random = WaveServices.Random;

                var transform = playerEntity.FindComponent<Transform2D>();
                transform.Position = new Vector2(random.Next(10, 800), random.Next(10, 200));
                transform.DrawOrder = player.Id;

                playerEntity.AddComponent(new MovementBehavior());
            }
            else
            {
                var networkPlayerProvider = playerEntity.FindComponent<NetworkPlayerProvider>();
                networkPlayerProvider.PlayerId = player.Id;
                playerEntity.IsVisible = false;
            }

            this.EntityManager.Add(playerEntity);
        }

        private void RemovePlayer(INetworkPlayer player)
        {
            var entityName = this.GetEntityName(player);
            this.EntityManager.Remove(entityName);
        }

        private string GetEntityName(INetworkPlayer player)
        {
            return $"player_{player.Id}";
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (this.matchmakingClientService != null)
            {
                this.matchmakingClientService.StateChanged -= this.MatchmakingClientService_StateChanged;

                var currentRoom = this.matchmakingClientService.CurrentRoom;
                if (currentRoom != null)
                {
                    currentRoom.PlayerJoined -= this.CurrentRoom_PlayerJoined;
                    currentRoom.PlayerLeaving -= this.CurrentRoom_PlayerLeaving;
                }
            }
        }
    }
}
