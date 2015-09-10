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

namespace RenderTarget
{
    /// <summary>
    /// This scene will be rendered on a RenderTarget texture.
    /// </summary>
    public class RenderTargetScene : Scene
    {
        public WaveEngine.Common.Graphics.RenderTarget SmallTarget
        {
            get;
            private set;
        }

        protected override void CreateScene()
        {
            this.Load(WaveContent.Scenes.RenderTargetScene);

            // First, we creates the RenderTarget where the scene will be rendered
            this.SmallTarget = WaveServices.GraphicsDevice.RenderTargets.CreateRenderTarget(256, 256);

            // Then, retrieve the camera from this scene and set the RenderTarget property in the Camera3D component.
            var defaultCamera3D = this.EntityManager.Find("defaultCamera3D");
            var camera3DComponent = defaultCamera3D.FindComponent<Camera3D>(isExactType: false);
            camera3DComponent.RenderTarget = this.SmallTarget;
        }
    }
}
