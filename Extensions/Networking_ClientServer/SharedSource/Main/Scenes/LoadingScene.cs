using System;
using System.Threading.Tasks;
using WaveEngine.Components.UI;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;
using WaveEngine.Framework.UI;
using WaveEngine.Networking;
using WaveEngine.Networking.Client;
using WaveEngine.Networking.Messages;

namespace Networking_ClientServer.Scenes
{
    public class LoadingScene : Scene
    {
        private TextBlock messageTextBlock;

        protected async override void CreateScene()
        {
            this.Load(WaveContent.Scenes.MyScene);

            this.messageTextBlock = new TextBlock()
            {
                Width = 600,
                Height = 50,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(10, 0, 10, 10)
            };
            this.EntityManager.Add(this.messageTextBlock);

            await this.JoinRoom();
        }

        private async Task JoinRoom()
        {
            this.messageTextBlock.Text = "Joining to room...";

            var roomOptions = new RoomOptions()
            {
                MaxPlayers = (byte)NetworkConfiguration.NumberOfPlayers
            };

            var matchmakingClientService = WaveServices.GetService<MatchmakingClientService>();
            var joinResult = await matchmakingClientService.JoinOrCreateRoomAsync(roomOptions);

            if (joinResult != EnterRoomResultCodes.Succeed)
            {
                this.messageTextBlock.Text = $"Cannot join room. Reason: {joinResult}. In 1 second you will return to room selection.";

                WaveServices.TimerFactory.CreateTimer(TimeSpan.FromSeconds(1), () =>
                {
                    WaveServices.ScreenContextManager.Pop(false);
                }, false, this);
            }
            else
            {
                //Wait 3 seconds and start game
                int remainingSeconds = 3;
                this.UpdateRemainingSeconds(remainingSeconds);

                Timer timer = null;
                timer = WaveServices.TimerFactory.CreateTimer(TimeSpan.FromSeconds(1), () =>
                {
                    remainingSeconds--;
                    if (remainingSeconds == 0)
                    {
                        WaveServices.TimerFactory.RemoveTimer(timer);

                        //Navigate to GameScene
                        WaveServices.ScreenContextManager.Push(new ScreenContext(new GameScene()));
                    }

                    this.UpdateRemainingSeconds(remainingSeconds);
                }, true, this);
            }
        }

        private void UpdateRemainingSeconds(int remainingSeconds)
        {
            messageTextBlock.Text = $"Joined to room. Starting in {remainingSeconds} second(s)";
        }
    }
}
