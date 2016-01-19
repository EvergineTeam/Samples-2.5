#region Using Statements
using System;
using System.Linq;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Components.UI;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics3D;
using WaveEngine.Framework.Resources;
using WaveEngine.Framework.Services;
using WaveEngine.Framework.UI;
#endregion

namespace Rope
{
    public class MyScene : Scene
    {
        protected override void CreateScene()
        {
            this.Load(WaveContent.Scenes.MyScene);

            var instructions = new TextBlock
            {
                Text = "Left-button click or tap to throw a ball",
                Margin = new Thickness(10)
            };
            this.EntityManager.Add(instructions);
        }
    }
}
