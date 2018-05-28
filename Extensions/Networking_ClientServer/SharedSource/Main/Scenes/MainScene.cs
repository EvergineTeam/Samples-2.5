using System;
using System.Collections.Generic;
using WaveEngine.Components.UI;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;
using WaveEngine.Framework.UI;
using WaveEngine.Networking;
using WaveEngine.Networking.Client;
using WaveEngine.Networking.Connection;

namespace Networking_ClientServer.Scenes
{
    /// <summary>
    /// This scene handles the search and connection to host.
    /// </summary>
    public class MainScene : Scene
    {
        private MatchmakingClientService matchmakingClientService;
        private readonly List<Entity> discoveredServersButtons;

        private TextBlock errorMessage;

        public MainScene()
        {
            this.discoveredServersButtons = new List<Entity>();
        }

        protected override void CreateScene()
        {
            this.Load(WaveContent.Scenes.MyScene);

            this.matchmakingClientService = WaveServices.GetService<MatchmakingClientService>();

            this.CreateUi();
        }

        private void CreateUi()
        {
            var serverButton = new Button
            {
                Text = "Create Server",
                Width = 200,
                Height = 50,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(10)
            };
            serverButton.Click += this.OnCreateServerClicked;
            this.EntityManager.Add(serverButton);

            var discoveryButton = new Button
            {
                Text = "Find Servers",
                Width = 200,
                Height = 50,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(10, 10 + 70, 10, 10)
            };
            discoveryButton.Click += this.OnDiscoverServersClicked;
            this.EntityManager.Add(discoveryButton);

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

        private void OnCreateServerClicked(object sender, EventArgs args)
        {
            var serverButton = (Button)sender;
            try
            {
                this.CleanErrorMessage();
                this.DisableServersDiscoveryAndCleanButtons();

                WaveServices.RegisterService(new NetworkServerService());
                serverButton.IsVisible = false;

                this.DiscoverServers();
            }
            catch (Exception exception)
            {
                this.ShowErrorMenssage(exception);
            }
        }

        private void OnDiscoverServersClicked(object sender, EventArgs args)
        {
            this.DiscoverServers();
        }

        private void DiscoverServers()
        {
            this.CleanErrorMessage();
            this.DisableServersDiscoveryAndCleanButtons();
            this.matchmakingClientService.ServerDiscovered += this.OnServerDiscovered;
            this.matchmakingClientService.DiscoverServers(NetworkConfiguration.Port);
        }

        private void DisableServersDiscoveryAndCleanButtons()
        {
            this.matchmakingClientService.ServerDiscovered -= this.OnServerDiscovered;
            foreach (var item in this.discoveredServersButtons)
            {
                this.EntityManager.Remove(item);
            }

            this.discoveredServersButtons.Clear();
        }

        private void OnServerDiscovered(object sender, HostDiscoveredEventArgs e)
        {
            var host = e.Host;

            var clientButton = new Button
            {
                Text = string.Format("Connect to: {0}:{1}", host.Address, host.Port),
                Width = 400,
                Height = 50,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(10, 10 + (70 * (this.discoveredServersButtons.Count + 2)), 10, 10)
            };
            clientButton.Click += (s, args) => this.OnConnectToServerClicked(host);
            this.discoveredServersButtons.Add(clientButton.Entity);
            this.EntityManager.Add(clientButton);
            WaveServices.Layout.PerformLayout(this);
        }

        private async void OnConnectToServerClicked(NetworkEndpoint host)
        {
            try
            {
                await this.matchmakingClientService.ConnectAsync(host);
                WaveServices.ScreenContextManager.Push(new ScreenContext(new LoadingScene()));
            }
            catch (Exception exception)
            {
                this.ShowErrorMenssage(exception);
            }
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
