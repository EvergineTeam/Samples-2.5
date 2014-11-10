using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Diagnostic;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;

namespace TiledMapProject.Components
{
    public class CameraBehavior : Behavior, IDisposable
    {
        private RectangleF limitRectangle;
        private float minCameraX, maxCameraX;
        private float minCameraY, maxCameraY;

        private const float CameraSpeed = 4;

        private ViewportManager viewportManager;
        private Platform platform;

        [RequiredComponent]
        private Camera2D camera2D = null;

        private Entity followEntity;
        private Transform2D followTransform;

        public CameraBehavior(Entity followEntity, RectangleF limitRectangle)
        {
            this.followEntity = followEntity;
            this.limitRectangle = limitRectangle;
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.followTransform = this.followEntity.FindComponent<Transform2D>();

            this.viewportManager = WaveServices.ViewportManager;
            this.platform = WaveServices.Platform;

            this.platform.OnScreenSizeChanged += OnScreenSizeChanged;
            this.RefreshCameraLimits();
        }

        protected override void Update(TimeSpan gameTime)
        {
            Vector3 currentPosition = this.camera2D.Position;
            Vector3 desiredPosition = this.camera2D.Position;

            desiredPosition.X = Math.Max(this.minCameraX, Math.Min(this.followTransform.X, this.maxCameraX));
            desiredPosition.Y = Math.Max(this.minCameraY, Math.Min(this.followTransform.Y, this.maxCameraY)); ;

            this.camera2D.Position = currentPosition + (desiredPosition - currentPosition) * MathHelper.Min((float)(gameTime.TotalSeconds * CameraSpeed), 1);
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
            var vm = WaveServices.ViewportManager;

            float marginX = (vm.RightEdge - vm.LeftEdge) * this.camera2D.Zoom.X / 2;
            float marginY = (vm.BottomEdge - vm.TopEdge) * this.camera2D.Zoom.Y / 2;            

            this.minCameraX = this.limitRectangle.X + marginX;
            this.maxCameraX = this.limitRectangle.X + this.limitRectangle.Width - marginX;
            this.minCameraY = this.limitRectangle.Y + marginY;
            this.maxCameraY = this.limitRectangle.Y + this.limitRectangle.Height - marginY;

            ////float halfScreenSize = (this.viewportManager.RightEdge - this.viewportManager.LeftEdge) / 2;

            ////this.limitMinX = minX + halfScreenSize;
            ////this.limitMaxX = maxX - halfScreenSize;
        }
    }
}
