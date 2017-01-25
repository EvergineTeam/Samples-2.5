#region Using Statements
using NavigationFlow.Navigation;
using System.Runtime.Serialization;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;
#endregion

namespace NavigationFlow.Components
{
    [DataContract]
    public class NavigationComponent : Component
    {
        [DataMember]
        public NavigateCommands NavigationCommand { get; set; }

        protected override void DefaultValues()
        {
            base.DefaultValues();

            this.NavigationCommand = NavigateCommands.Back;
        }

        public void DoNavigation()
        {
            var navService = WaveServices.GetService<NavigationService>();

            if (navService != null)
            {
                navService.Navigate(this.NavigationCommand);
            }
        }
    }
}
