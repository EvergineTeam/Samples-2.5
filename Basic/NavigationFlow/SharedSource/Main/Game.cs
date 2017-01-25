#region Using Statements
using NavigationFlow.Navigation;
using System;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;
#endregion

namespace NavigationFlow
{
    public class Game : WaveEngine.Framework.Game
    {
        public override void Initialize(IApplication application)
        {
            base.Initialize(application);

            var navigationService = new NavigationService();
            WaveServices.RegisterService(navigationService);

            WaveServices.ScreenContextManager.SetDiagnosticsActive(true);

            navigationService.StartNavigation();
        }
    }
}
