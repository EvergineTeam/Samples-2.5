using System;
using System.Collections.Generic;
using System.Linq;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Components.UI;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
using WaveEngine.Framework.UI;
using WaveEngine.Networking;
using WaveEngine.Networking.Messages;

namespace Networking.Scenes
{
    public class SelectPlayerScene : Scene
    {
        private const int MinSpriteIndex = 1;
        private const int MaxSpriteIndex = 5;

        private readonly NetworkService networkService;

        private readonly Dictionary<int, string> assignedPlayerSprites;

        private TextBlock messageTextBlock;
        private Entity playerEntity;

        public SelectPlayerScene()
        {
            this.networkService = WaveServices.GetService<NetworkService>();
            this.networkService.HostMessageReceived += this.HostMessageReceived;
            this.networkService.ClientMessageReceived += this.ClientMessageReceived;

            this.assignedPlayerSprites = new Dictionary<int, string>();
        }

        #region Host methods

        /// <summary>
        /// Handles the messages received from the clients. Only when this player is the host.
        /// </summary>
        private void HostMessageReceived(object sender, IncomingMessage receivedMessage)
        {
            var playerIdentifier = receivedMessage.ReadString();
            var playerSpriteIndex = receivedMessage.ReadInt32();

            var resultPlayerSpriteIndex = this.AssignPlayerSpriteIndex(playerIdentifier, playerSpriteIndex);

            var responseMessage = this.networkService.CreateServerMessage();
            responseMessage.Write(playerIdentifier);
            responseMessage.Write(resultPlayerSpriteIndex);

            this.networkService.SendToClients(responseMessage, DeliveryMethod.ReliableUnordered);
        }

        /// <summary>
        /// Assigns the index of the player sprite. Ensure that ech player has a different player sprite.
        /// </summary>
        /// <param name="playerIdentifier">The player identifier.</param>
        /// <param name="playerSpriteIndex">Index of the player sprite.</param>
        /// <returns>The selected player sprite index. If no sprites is available return -1, and the player must leave the server.</returns>
        private int AssignPlayerSpriteIndex(string playerIdentifier, int playerSpriteIndex)
        {
            lock (this)
            {
                if (this.assignedPlayerSprites.ContainsKey(playerSpriteIndex)
                    && this.assignedPlayerSprites[playerSpriteIndex] != playerIdentifier)
                {
                    playerSpriteIndex = this.GetNextFreePlayerSpriteIndex();
                }

                this.assignedPlayerSprites[playerSpriteIndex] = playerIdentifier;
            }

            return playerSpriteIndex;
        }

        private int GetNextFreePlayerSpriteIndex()
        {
            for (int i = MinSpriteIndex; i < MaxSpriteIndex; i++)
            {
                if (!this.assignedPlayerSprites.ContainsKey(i))
                {
                    return i;
                }
            }

            return -1;
        }

        #endregion

        protected override void CreateScene()
        {
            this.Load(WaveContent.Scenes.SelectPlayerScene);

            this.messageTextBlock = new TextBlock()
            {
                Width = 600,
                Height = 50,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(10, 0, 10, 10)
            };
            this.EntityManager.Add(this.messageTextBlock);

            this.SendHelloToServer();
        }

        private void SendHelloToServer()
        {
            this.messageTextBlock.Text = "Waiting player assignment...";

            //The player send a random sprite index to the host. The host return the assigned player.
            this.SelectPlayer(WaveServices.Random.Next(MinSpriteIndex, MaxSpriteIndex));
        }

        private void SelectPlayer(int spriteIndex)
        {
            var message = this.networkService.CreateClientMessage();
            message.Write(this.networkService.ClientIdentifier);
            message.Write(spriteIndex);

            this.networkService.SendToServer(message, DeliveryMethod.ReliableUnordered);
        }

        /// <summary>
        /// Handles the messages received from the host.
        /// </summary>
        private void ClientMessageReceived(object sender, IncomingMessage receivedMessage)
        {
            var playerIdentifier = receivedMessage.ReadString();
            var playerSpriteIndex = receivedMessage.ReadInt32();

            if (this.networkService.ClientIdentifier == playerIdentifier)
            {
                this.HandlePlayerSelectionResponse(playerSpriteIndex);
            }
        }

        /// <summary>
        /// Handles the player selection response.
        /// </summary>
        /// <param name="playerSpriteIndex">Index of the player sprite.</param>
        private void HandlePlayerSelectionResponse(int playerSpriteIndex)
        {
            if (playerSpriteIndex < 0)
            {
                this.ServerCompleted();
            }
            else
            {
                this.PlayerSelected(playerSpriteIndex);
            }
        }

        private void ServerCompleted()
        {
            //Sprite selection not allowed, server completed, disconnect, and navigate back.
            messageTextBlock.Text = "This room is completed. In 3 second you will return to room selection.";
            this.networkService.Disconnect();
            WaveServices.TimerFactory.CreateTimer(TimeSpan.FromSeconds(1), () =>
            {
                WaveServices.ScreenContextManager.Pop(false);
            }, false, this);
        }

        private void PlayerSelected(int playerSpriteIndex)
        {
            // Set player entity texture
            this.playerEntity = this.EntityManager.Find("PlayerEntity");
            var spriteAtlas = this.playerEntity.FindComponent<SpriteAtlas>();
            spriteAtlas.TextureName = string.Format("c{0}", playerSpriteIndex);
            this.playerEntity.IsVisible = true;

            //Wait 3 seconds and start game.
            int remainingSeconds = 3;
            var timerName = "PlayerSelectedTimer";
            this.UpdateRemainingSeconds(remainingSeconds);
            WaveServices.TimerFactory.CreateTimer(timerName, TimeSpan.FromSeconds(1), () =>
            {
                remainingSeconds--;
                if (remainingSeconds == 0)
                {
                    WaveServices.TimerFactory.RemoveTimer(timerName);

                    //Navigate to GameScene and created player with selected sprite.
                    WaveServices.ScreenContextManager.Push(new ScreenContext(new GameScene(playerSpriteIndex)));
                }

                this.UpdateRemainingSeconds(remainingSeconds);
            }, true, this);
        }

        private void UpdateRemainingSeconds(int remainingSeconds)
        {
            messageTextBlock.Text = string.Format("Player assigned. Starting in {0} second(s)", remainingSeconds);
        }
    }
}
