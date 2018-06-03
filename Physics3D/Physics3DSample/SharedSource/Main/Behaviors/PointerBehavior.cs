using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Common.Attributes;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;

namespace Physics3DSample.Behaviors
{
    [DataContract]
    public class PointerBehavior : Behavior
    {
        [RequiredComponent]
        private Transform3D transform;

        private Transform3D gazeTransform;

        [RenderPropertyAsEntity(new string[] { "WaveEngine.Framework.Graphics.Transform3D" })]
        [DataMember]
        public string GazeEntity;

        protected override void Initialize()
        {
            base.Initialize();

            this.gazeTransform = this.Owner.Find(this.GazeEntity)?.FindComponent<Transform3D>();
        }

        protected override void Update(TimeSpan gameTime)
        {
            if((this.gazeTransform != null) && (this.Owner.Scene.PhysicsManager.Simulation3D.InternalWorld != null))
            {
                // Create ray
                Ray ray = new Ray(this.transform.Position, this.transform.WorldTransform.Forward);

                var result = this.Owner.Scene.PhysicsManager.Simulation3D.RayCast(ref ray, 1000);

                if(result.Succeeded)
                {
                    this.gazeTransform.Position = result.Point;
                    this.gazeTransform.LookAt(result.Point + result.Normal);
                }
            }
        }
    }
}
