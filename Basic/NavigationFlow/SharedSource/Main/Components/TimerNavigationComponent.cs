#region Using Statements
using System;
using System.Runtime.Serialization;
using WaveEngine.Components.GameActions;
using WaveEngine.Framework;
#endregion

namespace NavigationFlow.Components
{
    [DataContract]
    public class TimerNavigationComponent : Component
    {
        [RequiredComponent]
        protected NavigationComponent navComponent;

        [DataMember]
        public TimeSpan Duration { get; set; }

        [DataMember]
        public bool StartAutomatically { get; set; }
        
        private bool timerStarted;

        protected override void DefaultValues()
        {
            base.DefaultValues();

            this.timerStarted = false;
            this.Duration = TimeSpan.FromSeconds(1);
            this.StartAutomatically = true;
        }

        protected override void Initialize()
        {
            base.Initialize();

            if (this.StartAutomatically)
            {
                StartTimer();
            }
        }

        public bool StartTimer()
        {
            bool result = false;

            if (result = !this.timerStarted)
            {
                this.timerStarted = true;
                this.Owner.Scene.CreateDelayGameAction(this.Duration)
                          .ContinueWithAction(() => this.navComponent.DoNavigation())
                          .Run();
            }

            return result;
        }
    }
}
