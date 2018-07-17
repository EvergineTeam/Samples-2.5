using Networking_P2P.Behaviors;
using Networking_P2P.Components;
using Networking_P2P.Extensions;
using Networking_P2P.Networking;
using Networking_P2P.Networking.Messages;
using System;
using System.Collections.Generic;
using System.Text;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
using WaveEngine.Networking.P2P;
using WaveEngine.Networking.P2P.Events;
using WaveEngine.Networking.P2P.Players;

namespace Networking_P2P.Scenes
{
    public class GameCustomPropertiesScene : Scene
    {
        private NetworkPeerService networkPeerService;
        private List<string> addedIps = new List<string>();

        protected override async void CreateScene()
        {
            this.Load(WaveContent.Scenes.GameScene);

            this.networkPeerService = WaveServices.GetService<NetworkPeerService>();

            networkPeerService.NetworkPlayerChange -= this.OnNetworkPeerServiceNetworkPlayerChange;
            networkPeerService.NetworkPlayerChange += this.OnNetworkPeerServiceNetworkPlayerChange;

            var playerId = await this.networkPeerService.GetIPAddress();
            var message = NetworkMessage.CreateMessage(P2PMessageType.NewPlayer, playerId.Sanitize());
            await this.networkPeerService.SendBroadcastAsync(message);
        }

        private async void OnNetworkPeerServiceNetworkPlayerChange(object sender, NetworkPlayerChangeEventArgs e)
        {
            var localIpAddress = await networkPeerService.GetIPAddress();
            var players = e.NetworkPlayers;

            foreach (var player in players)
            {
                if (!this.addedIps.Contains(player.IpAddress))
                {
                    if (player.IpAddress != localIpAddress)
                    {
                        this.AddPlayer(player, false);
                    }
                    else
                    {
                        this.AddPlayer(player, true);
                    }

                    this.addedIps.Add(player.IpAddress);
                }
            }
        }

        private void AddPlayer(NetworkPlayer nPlayer, bool isLocal)
        {

            var playerEntity = this.EntityManager.Instantiate(WaveContent.Prefabs.Player);
            playerEntity.Name = this.GetEntityName(nPlayer.IpAddress.Sanitize());

            var random = WaveServices.Random;
            var transform = playerEntity.FindComponent<Transform2D>();
            transform.Position = new Vector2(random.Next(10, 800), random.Next(10, 200));

            if (isLocal)
            {
                playerEntity.AddComponent(new MovementByCustomPropertyBehavior(nPlayer));
            }
            else
            {
                playerEntity.AddComponent(new NetworkMovementByCustomProperty(nPlayer));
            }

            if (!this.EntityManager.Contains(playerEntity))
            {
                this.EntityManager.Add(playerEntity);
            }
        }

        private string GetEntityName(string playerId)
        {
            return $"player_{playerId}";
        }
    }
}
