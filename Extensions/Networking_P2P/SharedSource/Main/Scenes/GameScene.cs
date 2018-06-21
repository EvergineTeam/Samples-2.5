using Lidgren.Network;
using Networking_P2P.Behaviors;
using System.Text;
using System.Threading.Tasks;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
using WaveEngine.Networking;
using WaveEngine.Networking.Connection.Messages;
using WaveEngine.Networking.P2P;
using WaveEngine.Networking.P2P.Events;
using WaveEngine.Networking.P2P.Players;

namespace Networking_P2P.Scenes
{
    public class GameScene : Scene
    {
        private NetworkPeerService networkPeerService;

        protected override async void CreateScene()
        {
            this.Load(WaveContent.Scenes.GameScene);

            this.networkPeerService = WaveServices.GetService<NetworkPeerService>();

            networkPeerService.PortNum = 21809;
  
            networkPeerService.MessageReceivedFromPlayer += OnNetworkPeerServiceMessageReceivedFromPlayer;
            networkPeerService.NetworkPlayerChange += OnNetworkPeerServiceNetworkPlayerChange;

            await networkPeerService.StartAsync();
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

        private void OnNetworkPeerServiceMessageReceivedFromPlayer(object sender, MessageFromPlayerEventArgs e)
        {
            var messageReceived = Encoding.ASCII.GetString(e.ReceivedMessage.ReadBytes());

            if(messageReceived.Equals("New Player"))
            {
                AddPlayer(null, false);
            }
     
        }

        private async void OnNetworkPeerServiceNetworkPlayerChange(object sender, NetworkPlayerChangeEventArgs e)
        {
            var localIpAddress = await networkPeerService.GetIPAddress();
            var players = e.NetworkPlayers;

            foreach(var player in players)
            {
                if(player.IpAddress == localIpAddress)
                {
                    AddPlayer(player, true);
                    await SendPlayerInfoRequestAsync();
                }
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
