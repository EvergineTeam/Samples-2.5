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
#endregion

namespace LoadingProject
{
    public class GameScene : Scene
    {
        protected override void CreateScene()
        {                      
            // Create a 2D camera
            var camera2D = new FixedCamera2D("Camera2D");
            EntityManager.Add(camera2D);

            Image image = new Image("Content/Start.png");
            EntityManager.Add(image);
            
        }      
    }
}
