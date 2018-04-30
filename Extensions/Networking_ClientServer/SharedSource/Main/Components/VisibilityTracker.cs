using System;
using System.Runtime.Serialization;
using WaveEngine.Framework;

namespace Networking_ClientServer.Components
{
    [DataContract]
    public class VisibilityTracker : Behavior
    {
        private bool lastVisibility;

        public event EventHandler VisibilityChanged;

        protected override void Initialize()
        {
            base.Initialize();
            this.lastVisibility = this.Owner.IsVisible;
        }

        protected override void Update(TimeSpan gameTime)
        {
            var currentVisibility = this.Owner.IsVisible;

            if(currentVisibility != this.lastVisibility)
            {
                this.lastVisibility = currentVisibility;
                this.VisibilityChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
