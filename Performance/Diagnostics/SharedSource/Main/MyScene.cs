#region Using Statements
using Diagnostic;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;
#endregion

namespace Diagnostic
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
