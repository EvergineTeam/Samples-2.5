using Lidgren.Network;
using Networking_P2P.Behaviors;
using System.Text;
using System.Threading.Tasks;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
using WaveEngine.Framework.UI;
using WaveEngine.Components.UI;
using WaveEngine.Networking;
using WaveEngine.Networking.Connection.Messages;
using WaveEngine.Networking.P2P;
using WaveEngine.Networking.P2P.Events;
using WaveEngine.Networking.P2P.Players;
using System;
using Networking_P2P.Networking;

namespace Networking_P2P.Scenes
{
    public class GameScene : Scene
    {
        private NetworkPeerService networkPeerService;

        protected override async void CreateScene()
        {
            this.Load(WaveContent.Scenes.GameScene);

            var button = new Button
            {
                Text = "Test",
                Width = 200,
                Height = 50,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(10, 10 + 70, 10, 10)
            };
            button.Click += this.OnButtonClicked;
            this.EntityManager.Add(button);

            this.networkPeerService = WaveServices.GetService<NetworkPeerService>();

            networkPeerService.PortNum = 21809;

            networkPeerService.MessageReceivedFromPlayer += OnNetworkPeerServiceMessageReceivedFromPlayer;
            networkPeerService.NetworkPlayerChange += OnNetworkPeerServiceNetworkPlayerChange;

            await networkPeerService.StartAsync();
        }

        private async void OnButtonClicked(object sender, EventArgs e)
        {
            await this.networkPeerService.SendBroadcastAsync(new OutgoingMessage(new NetBuffer() { Data = new byte[] { 1, 2, 3, 4 } }));
        }

        private void AddPlayer(INetworkPlayer player, bool isLocal)
        {
            var playerEntity = this.EntityManager.Instantiate(WaveContent.Prefabs.Player);
            playerEntity.Name = this.GetEntityName(player);

            var random = WaveServices.Random;
            var transform = playerEntity.FindComponent<Transform2D>();
            transform.Position = new Vector2(random.Next(10, 800), random.Next(10, 200));
            transform.DrawOrder = player.Id;

            if (isLocal)
            {
                playerEntity.AddComponent(new MovementBehavior());
            }

            this.EntityManager.Add(playerEntity);
        }

        private string GetEntityName(INetworkPlayer player)
        {
            return $"player_{player.Id}";
        }

        private void OnNetworkPeerServiceMessageReceivedFromPlayer(object sender, PeerMessageFromPlayerEventArgs e)
        {
            var rawMessage = e.ReceivedMessage;
            var messagetType = (P2PMessageType) rawMessage.ReadInt32();

            switch (messagetType)
            {
                case P2PMessageType.NewPlayer:
                    var playerId = rawMessage.ReadString();
                    break;
                case P2PMessageType.Move:
                    var position = rawMessage.ReadVector2();
                    break;
                default:
                    break;
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
                    AddPlayer(player, true);
                }

                await SendPlayerInfoRequestAsync();
            }
        }

        private async Task SendPlayerInfoRequestAsync()
        {
            NetBuffer netBuffer = new NetBuffer
            {
                Data = Encoding.ASCII.GetBytes("New Player")
            };

            OutgoingMessage message = new OutgoingMessage(netBuffer);
            await this.networkPeerService.SendBroadcastAsync(message);
        }
    }
}
