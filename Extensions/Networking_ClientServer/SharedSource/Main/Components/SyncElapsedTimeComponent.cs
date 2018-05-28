using System;
using System.Runtime.Serialization;
using WaveEngine.Components.Toolkit;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;
using WaveEngine.Networking.Client;
using WaveEngine.Networking.Components;

namespace Networking_ClientServer.Components
{
    [DataContract]
    public class SyncElapsedTimeComponent : NetworkTimeSpanPropertySync<RoomProperties>, IDisposable
    {
        [RequiredService]
        protected MatchmakingClientService matchmakingClientService;

        [RequiredService]
        protected TimerFactory timerFactory;

        [RequiredComponent(false)]
        protected TextComponent textComponent;

        private Timer timer;

        protected override void DefaultValues()
        {
            base.DefaultValues();
            this.ProviderFilter = NetworkPropertyProviderFilter.Room;
            this.PropertyKey = RoomProperties.ElapsedTime;
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.timer = this.timerFactory.CreateTimer(TimeSpan.FromSeconds(1), this.TimerTick, true, this.Owner.Scene);

            if (!WaveServices.Platform.IsEditor)
            {
                this.UpdateTimerText();
            }
        }

        protected override void OnPropertyAddedOrChanged()
        {
            this.UpdateTimerText();
        }

        protected override void OnPropertyRemoved()
        {
        }

        private void TimerTick()
        {
            if (this.matchmakingClientService.LocalPlayer.IsMasterClient)
            {
                this.PropertyValue += this.timer.InitialInterval;
                this.UpdateTimerText();
            }
        }

        private void UpdateTimerText()
        {
            this.textComponent.Text = $"Elapsed: {this.PropertyValue}";
        }

        public override void Dispose()
        {
            base.Dispose();

            if (this.timer != null)
            {
                this.timerFactory.RemoveTimer(this.timer);
                this.timer = null;
            }
        }
    }
}