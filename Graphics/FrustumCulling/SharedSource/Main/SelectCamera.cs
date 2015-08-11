using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveEngine.Common.Input;
using WaveEngine.Components.Cameras;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;

namespace FrustumCulling
{
    public class SelectCamera4 : SceneBehavior
    {
        KeyboardState lastState, currentState;
        FreeCamera3D freeCamera;
        FixedCamera3D fixedCamera;

        public SelectCamera4(FreeCamera3D freeCamera, FixedCamera3D fixedCamera)
            : base("SelectCameraBehavior")
        {
            this.freeCamera = freeCamera;
            this.fixedCamera = fixedCamera;
        }

        protected override void ResolveDependencies()
        {
        }

        protected override void Update(TimeSpan gameTime)
        {
            currentState = WaveServices.Input.KeyboardState;
            if (currentState.D1 == ButtonState.Pressed && lastState.D1 == ButtonState.Release)
            {
                freeCamera.IsActive = true;
                fixedCamera.IsActive = false;                
            }

            if (currentState.D2 == ButtonState.Pressed && lastState.D2 == ButtonState.Release)
            {
                freeCamera.IsActive = false;
                fixedCamera.IsActive = true;                
            }

            lastState = currentState;
        }
    }
}
