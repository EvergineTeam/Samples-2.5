using System.Runtime.Serialization;
using WaveEngine.Components.Gestures;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;

namespace AsyncAwait
{
    [DataContract]
    public class BackComponent : Component
    {
        [RequiredComponent]
        protected TouchGestures touchGestures;

        protected override void Initialize()
        {
            base.Initialize();

            this.touchGestures.TouchTap += this.TouchTap;
        }

        private void TouchTap(object sender, GestureEventArgs e)
        {
            WaveServices.ScreenContextManager.To(new ScreenContext(new MyScene()));
        }
    }
}
