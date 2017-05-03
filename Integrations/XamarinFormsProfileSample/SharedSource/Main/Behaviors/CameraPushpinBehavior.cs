using System;
using System.Linq;
using System.Runtime.Serialization;
using WaveEngine.Common.Input;
using WaveEngine.Common.Math;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics3D;
using WaveEngine.Framework.Services;
using XamarinFormsProfileSample.Events;

namespace XamarinFormsProfileSample.Behaviors
{
    [DataContract]
    public class CameraPushpinBehavior : Behavior
    {
        private MaterialsMap _materialsMap;
        private Ray _ray;
        private Vector3 _nearPosition;
        private Vector3 _farPosition;
        private Vector3 _rayDirection;
        private BoxCollider3D _entityBoxCollider;
        private float? _collisionResult;

        public event EventHandler<PushpinTappedEventArgs> PushpinTapped;

        protected override void Update(TimeSpan gameTime)
        {
            TouchPanelState state = WaveServices.Input.TouchPanelState;

            if (state.IsConnected && state.Any())
            {
                var activeCamera3D = this.RenderManager.ActiveCamera3D;

                var touchLocation = state.FirstOrDefault();

                Ray(activeCamera3D, touchLocation);
            }
        }

        private void Ray(Camera3D activeCamera, TouchLocation touchLocation)
        {
            // Creates Ray from Touch 2D position
            _nearPosition.X = touchLocation.Position.X;
            _nearPosition.Y = touchLocation.Position.Y;
            _nearPosition.Z = 0.0f;

            _farPosition.X = touchLocation.Position.X;
            _farPosition.Y = touchLocation.Position.Y;
            _farPosition.Z = 1.0f;

            // Unproject Mouse Position
            _nearPosition = activeCamera.Unproject(ref _nearPosition);
            _farPosition = activeCamera.Unproject(ref _farPosition);

            // Update ray. Ray launched from nearPosition in rayDirection direction.
            _rayDirection = _farPosition - _nearPosition;
            _rayDirection.Normalize();
            _ray.Direction = _rayDirection;
            _ray.Position = _nearPosition;

            foreach (Entity entity in this.Owner.Scene.EntityManager.FindAllByTag("touchable"))
            {
                _entityBoxCollider = entity.FindComponent<BoxCollider3D>();

                if (_entityBoxCollider != null)
                {
                    _materialsMap = entity.FindComponent<MaterialsMap>();

                    _collisionResult = _entityBoxCollider.Intersects(ref _ray);

                    if (_collisionResult.HasValue)
                    {
                        PushpinTapped?.Invoke(this, new PushpinTappedEventArgs(entity.Name));
                    }
                }
            }
        }
    }
}