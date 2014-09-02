using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveEngine.Common.Input;
using WaveEngine.Components.Cameras;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;

namespace FrustumCullingProject
{
    public class SelectCamera : SceneBehavior
    {
        KeyboardState lastState, currentState;
        FreeCamera freeCamera;
        FixedCamera fixedCamera;

        public SelectCamera(FreeCamera freeCamera, FixedCamera fixedCamera)
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
                //Scene.RenderManager.SetActiveCamera(freeCamera.Entity);
            }

            if (currentState.D2 == ButtonState.Pressed && lastState.D2 == ButtonState.Release)
            {
                freeCamera.IsActive = false;
                fixedCamera.IsActive = true;
                //Scene.RenderManager.SetActiveCamera(fixedCamera.Entity);
            }

            lastState = currentState;
        }
    }
}
