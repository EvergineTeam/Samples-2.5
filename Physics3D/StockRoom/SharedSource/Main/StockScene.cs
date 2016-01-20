#region Using Statements
using StockRoom.Behaviors;
using System;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Components.UI;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Resources;
using WaveEngine.Framework.Services;
using WaveEngine.Framework.UI;
#endregion

namespace StockRoom
{
    public class StockScene : Scene
    {
        public string ScenePath;
        public TextBlock helpText;

        public StockScene(string scenePath)
        { 
            this.ScenePath = scenePath;
        }

        protected override void CreateScene()
        {
            this.PhysicsManager.Gravity3D = new Vector3(0, -14, 0);
            this.Load(this.ScenePath); 
            this.CreateHelpText();

            this.AddSceneBehavior(new ModeBehavior(), SceneBehavior.Order.PostUpdate);
        }

        /// <summary>
        /// Create Help text
        /// </summary>
        private void CreateHelpText()
        {
            helpText = new TextBlock()
            {
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(20, 20, 0, 0),
                Text = "Key H show/hide help text \n" +
                          "Key F1 diagnostics mode \n" +
                          "Key F5 Emiter boxes scene \n" +
                          "Key F6 Wall boxes scene \n" +
                          "Key F7 Bridge scene \n" +
                          "Key R restart scene \n" +
                          "Key G change gravity direction"
            };
            EntityManager.Add(helpText.Entity);
        }
    }
}
