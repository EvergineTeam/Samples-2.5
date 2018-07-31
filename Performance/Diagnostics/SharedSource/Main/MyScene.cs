#region Using Statements
using WaveEngine.Framework;
using WaveEngine.Framework.Services;
#endregion

namespace Diagnostics
{
    public class MyScene : Scene
    {
        protected override void CreateScene()
        {
            WaveServices.ScreenContextManager.SetDiagnosticsActive(true);

            this.Load(WaveContent.Scenes.MyScene);           
        }
    }
}
