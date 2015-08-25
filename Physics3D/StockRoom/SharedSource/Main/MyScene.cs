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
    public class MyScene : Scene
    {
        public TextBlock helpText;

        protected override void CreateScene()
        {
            this.Load(WaveContent.Scenes.MyScene);
            this.CreateHelpText();
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
                          "Key G change gravity direction \n" +
                          "Key W, A, S, D move camera \n" +
                          "Key 1 apply impulse mode \n" +
                          "Key 2 launch ball mode"
            };
            EntityManager.Add(helpText.Entity);
        }
    }
}
