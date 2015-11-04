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
using WaveEngine.Framework.Services;

namespace TiledMap.Components
{
    [DataContract]
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

        [RequiredComponent(false)]
        private Transform3D transform = null;

        private Entity followEntity;
        private Transform2D followTransform;

        private Entity tiledMapEntity;
        private WaveEngine.TiledMap.TiledMap tiledMap;

        private bool needRefreshLimits;

        [DataMember]
        [RenderPropertyAsEntity(new string[] { "WaveEngine.Framework.Graphics.Transform2D" })]
        public string FollowEntityPath { get; set; }

        [DataMember]
        [RenderPropertyAsEntity(new string[] { "WaveEngine.TiledMap.TiledMap" })]
        public string TiledMapEntityPath { get; set; }

        protected override void Initialize()
        {
            base.Initialize();

            if (string.IsNullOrEmpty(this.FollowEntityPath)
                || string.IsNullOrEmpty(this.TiledMapEntityPath))
            {
                return;
            }

            this.followEntity = this.EntityManager.Find(this.FollowEntityPath);
            this.followTransform = this.followEntity.FindComponent<Transform2D>();

            this.tiledMapEntity = this.EntityManager.Find(this.TiledMapEntityPath);
            this.tiledMap = this.tiledMapEntity.FindComponent<WaveEngine.TiledMap.TiledMap>();            

            this.viewportManager = WaveServices.ViewportManager;
            this.platform = WaveServices.Platform;

            this.platform.OnScreenSizeChanged += OnScreenSizeChanged;
            this.needRefreshLimits = true;
        }

        protected override void Update(TimeSpan gameTime)
        {
            if (this.needRefreshLimits)
            {
                this.RefreshCameraLimits();
                this.needRefreshLimits = false;
            }

            Vector3 currentPosition = this.camera2D.Position;
            Vector3 desiredPosition = this.camera2D.Position;

            desiredPosition.X = Math.Max(this.minCameraX, Math.Min(this.followTransform.X, this.maxCameraX));
            desiredPosition.Y = Math.Max(this.minCameraY, Math.Min(this.followTransform.Y, this.maxCameraY)); ;

            this.transform.Position = currentPosition + (desiredPosition - currentPosition) * MathHelper.Min((float)(gameTime.TotalSeconds * CameraSpeed), 1);
        }

        public void Dispose()
        {
            if (this.platform != null)
            {
                this.platform.OnScreenSizeChanged -= OnScreenSizeChanged;
            }
        }

        private void OnScreenSizeChanged(object sender, WaveEngine.Common.Helpers.SizeEventArgs e)
        {
            this.RefreshCameraLimits();
        }

        private void RefreshCameraLimits()
        {
            this.limitRectangle = new RectangleF(0, 0, this.tiledMap.Width * this.tiledMap.TileWidth, this.tiledMap.Height * this.tiledMap.TileHeight);
            var vm = WaveServices.ViewportManager;

            var zoom = this.transform.LocalScale;
            float marginX = (vm.RightEdge - vm.LeftEdge) * zoom.X / 2;
            float marginY = (vm.BottomEdge - vm.TopEdge) * zoom.Y / 2;

            this.minCameraX = this.limitRectangle.X + marginX;
            this.maxCameraX = this.limitRectangle.X + this.limitRectangle.Width - marginX;
            this.minCameraY = this.limitRectangle.Y + marginY;
            this.maxCameraY = this.limitRectangle.Y + this.limitRectangle.Height - marginY;
        }
    }
}
