using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using WaveEngine.Common.Attributes;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Diagnostic;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Managers;
using WaveEngine.Framework.Services;

namespace ParallaxCamera2D.Behaviors
{
    [DataContract]
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

        [DataMember]
        private string followEntityPath;

        private Transform2D followTransform;

        [RenderPropertyAsEntity(new string[] { "WaveEngine.Framework.Transform3D" })]
        public string FollowEntityPath
        {
            get
            {
                return this.followEntityPath;
            }
            set
            {
                this.followEntityPath = value;

                if (this.isInitialized)
                {
                    this.RefreshFollowEntity();
                }
            }
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.virtualScreenManager = this.Owner.Scene.VirtualScreenManager;
            this.platform = WaveServices.Platform;

            this.RefreshFollowEntity();

            this.platform.OnScreenSizeChanged += this.OnScreenSizeChanged;
            this.RefreshCameraLimits();
        }

        protected override void Update(TimeSpan gameTime)
        {
            if (this.followTransform == null)
            {
                return;
            }

            Vector3 currentPosition = this.camera2D.Position;
            Vector3 desiredPosition = this.camera2D.Position;
            desiredPosition.X = Math.Max(this.limitMinX, Math.Min(this.limitMaxX, followTransform.X));

            var elapsed = MathHelper.Min((float)(gameTime.TotalSeconds * CameraSpeed), 1);
            this.camera2D.Transform.Transform3D.Position = currentPosition + (desiredPosition - currentPosition) * elapsed;
        }

        public void Dispose()
        {
            this.platform.OnScreenSizeChanged -= this.OnScreenSizeChanged;
        }

        private void RefreshFollowEntity()
        {
            this.followTransform = null;

            if (!string.IsNullOrEmpty(this.followEntityPath))
            {
                var followEntity = this.EntityManager.Find(this.followEntityPath, this.Owner);
                this.followTransform = followEntity?.FindComponent<Transform2D>();
            }
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
