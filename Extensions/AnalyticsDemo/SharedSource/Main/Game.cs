#region Using Statements
using System;
using WaveEngine.Analytics;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;
#endregion

namespace AnalyticsDemo
{
    public class Game : WaveEngine.Framework.Game
    {
        protected AnalyticsManager analyticsManager;

        public override void Initialize(IApplication application)
        {
            base.Initialize(application);

            ViewportManager vm = WaveServices.GetService<ViewportManager>();
            vm.Activate(800, 480, ViewportManager.StretchMode.Uniform);

            this.analyticsManager = new AnalyticsManager(application.Adapter);
            WaveServices.RegisterService<AnalyticsManager>(analyticsManager);

            this.analyticsManager.SetAnalyticsSystem(new LocalyticsInfo("Insert your Localytics API KEY code here"));
            this.analyticsManager.Open();
            this.analyticsManager.Upload();

            ScreenContext screenContext = new ScreenContext(new MyScene());
            WaveServices.ScreenContextManager.To(screenContext);
        }

        /// <summary>
        /// Called when [activated].
        /// </summary>
        public override void OnActivated()
        {
            base.OnActivated();

            if (!this.analyticsManager.IsOpen)
            {
                this.analyticsManager.Open();
                this.analyticsManager.Upload();
            }
        }

        /// <summary>
        /// Called when [deactivated].
        /// </summary>
        public override void OnDeactivated()
        {
            base.OnDeactivated();

            analyticsManager.Close();
        }
    }
}
