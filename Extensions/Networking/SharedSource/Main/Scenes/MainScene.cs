using System;
using System.Collections.Generic;
using WaveEngine.Common.Graphics;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.UI;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;
using WaveEngine.Framework.UI;
using WaveEngine.Networking;

namespace Networking.Scenes
{
    /// <summary>
    /// This scene handles the search and connection to host.
    /// </summary>
    public class MainScene : Scene
    {
        private readonly NetworkService networkService;
        private readonly List<Entity> discoveredHostButtons;

        private TextBlock errorMessage;

        public MainScene()
        {
            this.networkService = WaveServices.GetService<NetworkService>();
            this.discoveredHostButtons = new List<Entity>();
        }

        protected override void CreateScene()
        {
            this.Load(WaveContent.Scenes.MainScene);

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
            discoveryButton.Click += this.OnDiscoverHostsClicked;
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
                this.DisableHostDiscoveryAndCleanButtons();
                this.networkService.InitializeHost(NetworkConfiguration.GameIdentifier, NetworkConfiguration.Port);
                serverButton.IsVisible = false;

                this.DiscoverHosts();
            }
            catch (Exception exception)
            {
                this.ShowErrorMenssage(exception);
            }
        }

        private void OnDiscoverHostsClicked(object sender, EventArgs args)
        {
            this.DiscoverHosts();
        }

        private void DiscoverHosts()
        {
            this.CleanErrorMessage();
            this.DisableHostDiscoveryAndCleanButtons();
            this.networkService.HostDiscovered += this.OnHostDiscovered;
            this.networkService.DiscoveryHosts(NetworkConfiguration.GameIdentifier, NetworkConfiguration.Port);
        }

        private void DisableHostDiscoveryAndCleanButtons()
        {
            this.networkService.HostDiscovered -= this.OnHostDiscovered;
            foreach (var item in this.discoveredHostButtons)
            {
                this.EntityManager.Remove(item);
            }

            this.discoveredHostButtons.Clear();
        }

        private void OnHostDiscovered(object sender, Host host)
        {
            var clientButton = new Button
            {
                Text = string.Format("Connect to: {0}:{1}", host.Address, host.Port),
                Width = 400,
                Height = 50,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(10, 10 + (70 * (this.discoveredHostButtons.Count + 2)), 10, 10)
            };
            clientButton.Click += (s, e) => this.OnConnectToHostClicked(host);
            this.discoveredHostButtons.Add(clientButton.Entity);
            this.EntityManager.Add(clientButton);
            WaveServices.Layout.PerformLayout(this);
        }

        private async void OnConnectToHostClicked(Host host)
        {
            try
            {
                await this.networkService.ConnectAsync(host);
                WaveServices.ScreenContextManager.Push(new ScreenContext(new SelectPlayerScene()));
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
