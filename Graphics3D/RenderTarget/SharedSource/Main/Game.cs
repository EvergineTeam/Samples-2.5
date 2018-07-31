#region Using Statements
using WaveEngine.Common;
using WaveEngine.Framework.Services;
#endregion

namespace RenderTarget
{
    public class Game : WaveEngine.Framework.Game
    {
        public override void Initialize(IApplication application)
        {
            base.Initialize(application);

            this.Load(WaveContent.GameInfo);

            // Creates a ScreenContext with two scenes.
            //  + The first one will be rendered in a RenderTarget.
            //  + The second one is the primary scene.
            // The RenderTargetScene must be the added before the primary scene.
            ScreenContext screenContext = new ScreenContext(new RenderTargetScene(), new MyScene());
            WaveServices.ScreenContextManager.To(screenContext);
        }
    }
}
