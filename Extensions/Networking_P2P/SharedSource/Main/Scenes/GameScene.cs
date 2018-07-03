using Networking_P2P.Behaviors;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
using WaveEngine.Networking;
using WaveEngine.Networking.P2P;
using WaveEngine.Networking.P2P.Events;
using Networking_P2P.Networking;
using Networking_P2P.Networking.Messages;
using System;
using Networking_P2P.Extensions;

namespace Networking_P2P.Scenes
{
    public class GameScene : Scene
    {
        private NetworkPeerService networkPeerService;

        protected override async void CreateScene()
        {
            this.Load(WaveContent.Scenes.GameScene);

            this.networkPeerService = WaveServices.GetService<NetworkPeerService>();
          
            networkPeerService.MessageReceivedFromPlayer -= this.OnNetworkPeerServiceMessageReceivedFromPlayer;
            networkPeerService.NetworkPlayerChange -= this.OnNetworkPeerServiceNetworkPlayerChange;
            networkPeerService.MessageReceivedFromPlayer += this.OnNetworkPeerServiceMessageReceivedFromPlayer;
            networkPeerService.NetworkPlayerChange += this.OnNetworkPeerServiceNetworkPlayerChange;

            var playerId = await this.networkPeerService.GetIPAddress();
            var message = NetworkMessage.CreateMessage(P2PMessageType.NewPlayer, playerId.Sanitize());
            await this.networkPeerService.SendBroadcastAsync(message);
        }

        private void AddPlayer(string playerId, bool isLocal)
        {
            var playerEntity = this.EntityManager.Instantiate(WaveContent.Prefabs.Player);
            playerEntity.Name = this.GetEntityName(playerId);

            var random = WaveServices.Random;
            var transform = playerEntity.FindComponent<Transform2D>();
            transform.Position = new Vector2(random.Next(10, 800), random.Next(10, 200));

            if (isLocal)
            {
                playerEntity.AddComponent(new MovementBehavior());
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

        private void OnNetworkPeerServiceMessageReceivedFromPlayer(object sender, PeerMessageFromPlayerEventArgs e)
        {
            var rawMessage = e.ReceivedMessage;

            var messagetType = (P2PMessageType) rawMessage.ReadInt32();
            var playerId = rawMessage.ReadString();

            switch (messagetType)
            {
                case P2PMessageType.NewPlayer:
                    this.CreateNetworkPlayer(playerId);
                    break;
                case P2PMessageType.Move:
                    var position = rawMessage.ReadVector2();
                    this.MoveNetworkPlayer(playerId, position);
                    break;
                default:
                    break;
            }
        }

        private void CreateNetworkPlayer(string playerId)
        {
            AddPlayer(playerId, false);
        }

        private void MoveNetworkPlayer(string playerId, Vector2 position)
        {
            var playerTransform = EntityManager.FindComponentFromEntityPath<Transform2D>(GetEntityName(playerId), true);

			if (playerTransform != null)
			{
				playerTransform.Position = position;
			}
        }

        private async void OnNetworkPeerServiceNetworkPlayerChange(object sender, NetworkPlayerChangeEventArgs e)
        {
            var localIpAddress = await networkPeerService.GetIPAddress();
            var players = e.NetworkPlayers;

            foreach (var player in players)
            {
                if (player.IpAddress != localIpAddress)
                {
                    AddPlayer(player.Id.ToString(), false);
                }
                else
                {
                    AddPlayer(player.Id.ToString(), true);
                }
            }
        }
    }
}
