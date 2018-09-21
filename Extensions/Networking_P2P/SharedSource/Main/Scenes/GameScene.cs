using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
using WaveEngine.Networking.P2P;
using Networking_P2P.Extensions;
using WaveEngine.Networking.P2P.Players;

namespace Networking_P2P.Scenes
{
    public class GameScene : Scene
    {
        private P2PServerService p2PServerService;

        protected override void CreateScene()
        {
            this.Load(WaveContent.Scenes.GameScene);

            this.p2PServerService = WaveServices.GetService<P2PServerService>();

            this.p2PServerService.PlayerConnected += this.NetworkPeerService_PlayerConnected;
            this.p2PServerService.PlayerDisconnected += NetworkPeerService_PlayerDisconnected; 
        }

        private void NetworkPeerService_PlayerDisconnected(object sender, P2PRemotePlayer e)
        {
            this.RemovePlayer(e.Endpoint.ToString().Sanitize());
        }

        private void NetworkPeerService_PlayerConnected(object sender, P2PRemotePlayer e)
        {
            this.AddPlayer(e.Endpoint.ToString().Sanitize(), e.IsLocalPlayer);
        }

        private void AddPlayer(string playerId, bool isLocal)
        {
            var playerEntity = this.EntityManager.Instantiate(WaveContent.Prefabs.Player);
            playerEntity.Name = this.GetEntityName(playerId);

            var random = WaveServices.Random;
            var transform = playerEntity.FindComponent<Transform2D>();
            transform.Position = new Vector2(random.Next(10, 800), random.Next(10, 200));
            
            if (!this.EntityManager.Contains(playerEntity))
            {
                this.EntityManager.Add(playerEntity);
            }
        }

        private void RemovePlayer(string playerName)
        {
            this.EntityManager.Remove(playerName);
        }

        private string GetEntityName(string playerId)
        {
            return $"player_{playerId}";
        }
    }
}