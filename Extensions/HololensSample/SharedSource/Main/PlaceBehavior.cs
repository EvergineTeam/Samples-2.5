using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Common.Math;
using WaveEngine.Components.VR;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
using WaveEngine.Hololens;
using WaveEngine.Hololens.Interaction;

namespace HololensSample
{
    [DataContract]
    public class PlaceBehavior : Behavior
    {
        [RequiredComponent]
        public Transform3D transform;

        private SpatialState lastState;
        
        private HololensService hololensService;
        private SpatialInputService spatialInputManager;

        [DataMember]
        public float PlaceDistance { get; set; }

        protected override void DefaultValues()
        {
            base.DefaultValues();

            this.PlaceDistance = 1;
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.hololensService = WaveServices.GetService<HololensService>();
            this.spatialInputManager = WaveServices.GetService<SpatialInputService>();

            this.PlaceEntity();
        }

        protected override void Update(TimeSpan gameTime)
        {
            var gesture = this.spatialInputManager.SpatialState;
           
            if (gesture.IsSelected && !lastState.IsSelected)
            {
                this.PlaceEntity();
            }

            this.hololensService.SetStabilizationPlane(transform.Position);

            lastState = gesture;
        }

        private void PlaceEntity()
        {
            Camera3D camera = this.RenderManager.ActiveCamera3D;
            if (camera != null)
            {
                transform.LocalPosition = camera.Transform.Position + (camera.Transform.WorldTransform.Forward * this.PlaceDistance);
            }
        }
    }
}