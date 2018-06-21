#region Using Statements
using System;
using System.Collections.Generic;
using WaveEngine.Components.UI;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;
using WaveEngine.Framework.UI;
using WaveEngine.Networking.P2P;
#endregion

namespace Networking_P2P.Scenes
{
    public class MainScene : Scene
    {
        private NetworkPeerService networkPeerService;
        private readonly List<Entity> discoveredHostButtons;

        private TextBlock errorMessage;

        public MainScene()
        {
            
            this.discoveredHostButtons = new List<Entity>();
        }

        protected override void CreateScene()
        {
            this.Load(WaveContent.Scenes.MainScene);

            this.networkPeerService = new NetworkPeerService(); 

            WaveServices.RegisterService(this.networkPeerService);

            this.CreateUi();
        }

        private void CreateUi()
        {
            var startButton = new Button
            {
                Text = "Start",
                Width = 200,
                Height = 50,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(10, 10 + 70, 10, 10)
            };
            startButton.Click += this.OnStartClicked;
            this.EntityManager.Add(startButton);

            this.errorMessage = new TextBlock
            {
                Width = 600,
                Height = 50,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(10, 0, 10, 10),
                IsVisible = false
            };
            this.EntityManager.Add(this.errorMessage);
        }

        private void OnStartClicked(object sender, EventArgs args)
        {
            var startButton = (Button)sender;
            try
            {
                this.CleanErrorMessage();
                this.DisableHostDiscoveryAndCleanButtons();
                //this.networkPeerService.StartAsync();

                WaveServices.ScreenContextManager.Push(new ScreenContext(new LoadingScene()));
            }
            catch (Exception exception)
            {
                this.ShowErrorMenssage(exception);
            }
        }

        private void DiscoverHosts()
        {
            this.CleanErrorMessage();
            this.DisableHostDiscoveryAndCleanButtons();
        }

        private void DisableHostDiscoveryAndCleanButtons()
        {
            foreach (var item in this.discoveredHostButtons)
            {
                this.EntityManager.Remove(item);
            }

            this.discoveredHostButtons.Clear();
        }

        private void ShowErrorMenssage(Exception exception)
        {
            this.errorMessage.Text = string.Format("Error: {0}", exception.Message);
            this.errorMessage.IsVisible = true;
        }

        private void CleanErrorMessage()
        {
            this.errorMessage.IsVisible = false;
        }
    }
}
