using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Diagnostic;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Managers;
using WaveEngine.Framework.Services;

namespace ParallaxCamera2D.Behaviors
{
    public class CameraBehavior : Behavior, IDisposable
    {
        private const float minX = 0;
        private const float maxX = 5500;

        private float limitMinX;
        private float limitMaxX;

        private const float CameraSpeed = 4;

        private VirtualScreenManager virtualScreenManager;
        private Platform platform;

        [RequiredComponent]
        private Camera2D camera2D = null;
                
        private Entity followEntity;
        private Transform2D followTransform;

        public CameraBehavior(Entity followEntity)
        {
            this.followEntity = followEntity;
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.followTransform = this.followEntity.FindComponent<Transform2D>();
            
            this.virtualScreenManager = this.Owner.Scene.VirtualScreenManager;
            this.platform = WaveServices.Platform;

            this.platform.OnScreenSizeChanged += OnScreenSizeChanged;
            this.RefreshCameraLimits();
        }        

        protected override void Update(TimeSpan gameTime)
        {
            Vector3 currentPosition = this.camera2D.Position;
            Vector3 desiredPosition = this.camera2D.Position;
            desiredPosition.X = Math.Max(this.limitMinX, Math.Min(this.limitMaxX, followTransform.X));

            this.camera2D.Transform3D.Position = currentPosition + (desiredPosition - currentPosition) * MathHelper.Min((float)(gameTime.TotalSeconds * CameraSpeed), 1);
           
        }

        public void Dispose()
        {
            this.platform.OnScreenSizeChanged -= OnScreenSizeChanged;
        }

        private void OnScreenSizeChanged(object sender, WaveEngine.Common.Helpers.SizeEventArgs e)
        {
            this.RefreshCameraLimits();
        }

        private void RefreshCameraLimits()
        {
            float halfScreenSize = (this.virtualScreenManager.RightEdge - this.virtualScreenManager.LeftEdge) / 2;

            this.limitMinX = minX + halfScreenSize;
            this.limitMaxX = maxX - halfScreenSize;
        }
    }
}
