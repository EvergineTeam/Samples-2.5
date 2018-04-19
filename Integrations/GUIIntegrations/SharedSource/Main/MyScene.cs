#region Using Statements
using WaveEngine.Framework;
#endregion

namespace GUIIntegrations
{
    public class MyScene : Scene
    {
        protected override void CreateScene()
        {
            this.Load(WaveContent.Scenes.MyScene);
        }
    }
}
