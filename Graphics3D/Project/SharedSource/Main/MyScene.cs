#region Using Statements
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

namespace Project
{
    public class MyScene : Scene
    {
        TextBlock instructions;


        protected override void CreateScene()
        {
            this.Load(WaveContent.Scenes.MyScene);

            
            // Add the textbox to show the picked entity
            instructions = new TextBlock()
            {
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Left,
                Text = "Selected Entity",
            };
            EntityManager.Add(instructions.Entity);
        }

        /// <summary>
        /// Show in the screen the name
        /// </summary>
        /// <param name="entityName">the entity name</param>
        public void ShowPickedEntity(string entityName)
        {
            instructions.Text = string.Format("Selected Entity: {0}", entityName);
        }
    }
}
