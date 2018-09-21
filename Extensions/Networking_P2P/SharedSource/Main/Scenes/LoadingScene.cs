using System;
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

        protected override void CreateScene()
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

            this.ConnectPeers();
        }

        private void ConnectPeers()
        {
            this.messageTextBlock.Text = "Connecting...";

            try
            {
                var networkPeerService = WaveServices.GetService<P2PServerService>();
                networkPeerService.Start(21000);

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
                        WaveServices.ScreenContextManager.Push(new ScreenContext(new GameCustomPropertiesScene()));
                    }

                    this.UpdateRemainingSeconds(remainingSeconds);
                }, true, this);
            }
            catch (Exception)
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
