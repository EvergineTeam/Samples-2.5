using System;
using System.Threading.Tasks;
using WaveEngine.Components.UI;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;
using WaveEngine.Framework.UI;
using WaveEngine.Networking.P2P;

namespace Networking_P2P.Scenes
{
    public class LoadingScene : Scene
    {
        private TextBlock messageTextBlock;

        protected async override void CreateScene()
        {
            this.Load(WaveContent.Scenes.LoadingScene);

            this.messageTextBlock = new TextBlock()
            {
                Width = 600,
                Height = 50,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(10, 0, 10, 10)
            };
            this.EntityManager.Add(this.messageTextBlock);

            await this.ConnectPeers();
        }

        private async Task ConnectPeers()
        {
            this.messageTextBlock.Text = "Connecting...";

            try
            {
                var networkPeerService = WaveServices.GetService<NetworkPeerService>();
                networkPeerService.PortNum = 21000;
                await networkPeerService.StartAsync();

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
                        //WaveServices.ScreenContextManager.Push(new ScreenContext(new GameScene()));
                        WaveServices.ScreenContextManager.Push(new ScreenContext(new GameCustomPropertiesScene()));
                    }

                    this.UpdateRemainingSeconds(remainingSeconds);
                }, true, this);
            }
            catch(Exception)
            {
                this.messageTextBlock.Text = $"Cannot connect. In 1 second you will back screen.";
                WaveServices.TimerFactory.CreateTimer(TimeSpan.FromSeconds(1), () =>
                {
                    WaveServices.ScreenContextManager.Pop(false);
                }, false, this);
            }
        }

        private void UpdateRemainingSeconds(int remainingSeconds)
        {
            messageTextBlock.Text = $"Connected. Starting in {remainingSeconds} second(s)";
        }
    }
}
