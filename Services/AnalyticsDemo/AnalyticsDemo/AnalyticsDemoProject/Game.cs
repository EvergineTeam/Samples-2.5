#region Using Statements
using System;
using WaveEngine.Analytics;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;
#endregion

namespace AnalyticsDemoProject
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
                                   
            this.analyticsManager.SetAnalyticsSystem(new LocalyticsInfo("492098e861d94597fd5f0cf-e9b821a6-7d17-11e3-9836-009c5fda0a25"));
            this.analyticsManager.Open();
            this.analyticsManager.Upload();

            ScreenLayers screenLayers = WaveServices.ScreenLayers;
            screenLayers.AddScene<MyScene>();
            screenLayers.Apply();
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
