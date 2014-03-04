#region Using Statements
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components;
using WaveEngine.Components.Cameras;
using WaveEngine.Framework;
#endregion

namespace LinesProject
{
    public class MyScene : Scene
    {
        protected override void CreateScene()
        {
            RenderManager.BackgroundColor = Color.Black;

            ViewCamera mainCamera = new ViewCamera("MainCamera", new Vector3(500, 500, 500), Vector3.Zero);
            EntityManager.Add(mainCamera.Entity);

            RenderManager.SetActiveCamera(mainCamera.Entity);

            Entity drawable = new Entity("Drawable")
                .AddComponent(new DrawableLines(1));

            EntityManager.Add(drawable);
        }
    }
}
