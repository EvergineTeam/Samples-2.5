using Networking_P2P.Behaviors;
using Networking_P2P.Components;
using Networking_P2P.Extensions;
using Networking_P2P.Networking;
using Networking_P2P.Networking.Messages;
using System.Collections.Generic;
using System.Diagnostics;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
using WaveEngine.Networking.Components;
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

            networkPeerService.MessageReceivedFromPlayer += this.NetworkPeerService_MessageReceivedFromPlayer;

            var playerId = await this.networkPeerService.GetIPAddress();
            var message = NetworkMessage.CreateMessage(P2PMessageType.NewPlayer, playerId.Sanitize());
            await this.networkPeerService.SendBroadcastAsync(message);
        }

        private void NetworkPeerService_MessageReceivedFromPlayer(object sender, WaveEngine.Networking.PeerMessageFromPlayerEventArgs e)
        {
            var rawMessage = e.ReceivedMessage;

            var playerFieldFlag = (PlayerFieldFlag)rawMessage.ReadInt32();

            if(playerFieldFlag == PlayerFieldFlag.CustomProperties)
            {
                var playerId = e.FromPlayer.IpAddress.Sanitize();
                var position = this.networkPeerService.Player.CustomProperties.GetVector2((byte)P2PMessageType.Move);

                Debug.WriteLine(playerId);
                Debug.WriteLine(position);

            }
        }

        private async void OnNetworkPeerServiceNetworkPlayerChange(object sender, NetworkPlayerChangeEventArgs e)
        {
            var localIpAddress = await networkPeerService.GetIPAddress();
            var players = e.NetworkPlayers;

            foreach (var player in players)
            {
                if (!this.addedIps.Contains(player.IpAddress))
                {
                    if (player.IsLocalPlayer)
                    {
                        this.AddPlayer(player, true);
                    }
                    else
                    {
                        this.AddPlayer(player, false);
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
                playerEntity.AddComponent(new WaveEngine.Networking.P2P.Providers.NetworkPlayerProvider());
                playerEntity.AddComponent(new NetworkMovementByCustomProperty());
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
