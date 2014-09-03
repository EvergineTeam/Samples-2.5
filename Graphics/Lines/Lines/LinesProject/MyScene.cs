#region Using Statements
using System;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Resources;
using WaveEngine.Framework.Services;
#endregion

namespace LinesProject
{
    public class MyScene : Scene
    {
        protected override void CreateScene()
        {            
            ViewCamera mainCamera = new ViewCamera("MainCamera", new Vector3(500, 500, 500), Vector3.Zero);
            mainCamera.BackgroundColor = Color.Black;
            EntityManager.Add(mainCamera.Entity);            

            Entity drawable = new Entity("Drawable")
                .AddComponent(new DrawableLines(1));

            EntityManager.Add(drawable);
        }
    }
}
