using Networking_P2P.Behaviors;
using Networking_P2P.Components;
using Networking_P2P.Extensions;
using Networking_P2P.Networking;
using Networking_P2P.Networking.Messages;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

            foreach (var player in players.Distinct())
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

        private async void AddPlayer(NetworkPlayer nPlayer, bool isLocal)
        {

            var playerEntity = this.EntityManager.Instantiate(WaveContent.Prefabs.Player);
            playerEntity.Name = this.GetEntityName(nPlayer.IpAddress.Sanitize());

            var random = WaveServices.Random;
            //var transform = playerEntity.FindComponent<Transform2D>();

            //transform.PositionChanged += Transform_PositionChanged;

            //transform.Position = new Vector2(random.Next(10, 800), random.Next(10, 200));

            if (isLocal)
            {
                playerEntity.AddComponent(new MovementByCustomPropertyBehavior());
                this.networkPeerService.Player = nPlayer;
         
                Debug.WriteLine("LOCAL: " + playerEntity.Name);
            }
            else
            {
                //playerEntity.AddComponent(new WaveEngine.Networking.P2P.Providers.NetworkPlayerProvider());
                //playerEntity.AddComponent(new NetworkMovementByCustomProperty());

                Debug.WriteLine("REMOTE: " + playerEntity.Name);
            }

            var localIpAddress = await networkPeerService.GetIPAddress();

            playerEntity.AddComponent(new WaveEngine.Networking.P2P.Providers.NetworkPlayerProvider
            {
                PlayerId = System.Convert.ToInt32(nPlayer.IpAddress.GetHashCode())
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
