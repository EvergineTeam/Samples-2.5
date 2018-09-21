using Networking_P2P.Behaviors;
using Networking_P2P.Components;
using Networking_P2P.Extensions;
using System.Collections.Generic;
using System.Diagnostics;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
using WaveEngine.Networking.P2P;
using WaveEngine.Networking.P2P.Players;

namespace Networking_P2P.Scenes
{
    public class GameCustomPropertiesScene : Scene
    {
        private P2PServerService networkPeerService;
        private P2PClientService clientPeerService;
        private List<string> addedIps = new List<string>();

        public int OnPlayerSynchronized { get; private set; }

        protected override void CreateScene()
        {
            this.Load(WaveContent.Scenes.GameScene);

            this.networkPeerService = WaveServices.GetService<P2PServerService>();
            this.networkPeerService.PlayerConnected += this.NetworkPeerService_PlayerConnected;
            this.networkPeerService.PlayerDisconnected += NetworkPeerService_PlayerDisconnected; ;

            this.clientPeerService = WaveServices.GetService<P2PClientService>();
            this.clientPeerService.ConnectAsync(new WaveEngine.Networking.NetworkEndpoint("10.4.1.37", 21000));
            this.clientPeerService.SendBroadcast();
            
            // var playerId = await this.networkPeerService.GetIPAddress();
            // var message = NetworkMessage.CreateMessage(P2PMessageType.NewPlayer, playerId.Sanitize());
            // await this.networkPeerService.SendBroadcastAsync(message);
        }

        private void NetworkPeerService_PlayerDisconnected(object sender, P2PRemotePlayer e)
        {
            // this.RemovePlayer(e.Endpoint.ToString().Sanitize());
        }

        private void NetworkPeerService_PlayerConnected(object sender, P2PRemotePlayer e)
        {
            this.AddPlayer(e);
        }


        private void AddPlayer(P2PRemotePlayer player)
        {

            var playerEntity = this.EntityManager.Instantiate(WaveContent.Prefabs.Player);
            playerEntity.Name = this.GetEntityName(player.Endpoint.ToString().Sanitize());

            var random = WaveServices.Random;
            //var transform = playerEntity.FindComponent<Transform2D>();

            //transform.PositionChanged += Transform_PositionChanged;

            //transform.Position = new Vector2(random.Next(10, 800), random.Next(10, 200));

            if (player.IsLocalPlayer)
            {
                playerEntity.AddComponent(new MovementByCustomPropertyBehavior());
                Debug.WriteLine("LOCAL: " + playerEntity.Name);
            }
            else
            {
                //playerEntity.AddComponent(new WaveEngine.Networking.P2P.Providers.NetworkPlayerProvider());
                //playerEntity.AddComponent(new NetworkMovementByCustomProperty());

                Debug.WriteLine("REMOTE: " + playerEntity.Name);
            }

            playerEntity.AddComponent(new WaveEngine.Networking.P2P.Providers.NetworkPlayerProvider
            {
                PlayerId = System.Convert.ToInt32(player.Endpoint.ToString().GetHashCode())
            });

            playerEntity.AddComponent(new NetworkMovementByCustomProperty());

            if (!this.EntityManager.Contains(playerEntity))
            {
                this.EntityManager.Add(playerEntity);
            }
        }

        private void Transform_PositionChanged(object sender, System.EventArgs e)
        {
            Debug.WriteLine(((Transform2D)sender).Owner.Name);
        }

        private string GetEntityName(string playerId)
        {
            return $"player_{playerId}";
        }
    }
}
