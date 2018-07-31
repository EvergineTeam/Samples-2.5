using System;
using System.Linq;
using WaveEngine.Common.Input;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
using WaveEngine.Framework.Physics3D;
using System.Runtime.Serialization;
using WaveEngine.Framework.Managers;

namespace Project
{
    [DataContract(Namespace="Project")]
    public class PickingBehavior : Behavior
    {
        // A camera required component to calculate the projections
        [RequiredComponent()]
        public Camera3D Camera;

        // Cached variables
        private TouchPanelState touchPanelState;
        private TouchLocation currentLocation;
        private Vector3 nearPoint;
        private Vector3 farPoint;        
        private Vector3 direction;
        private Ray ray;
        private Entity currentEntity;
        private Collider3D entityCollider;
        private float bestValue;
        private float? collisionResult;
        private MyScene myScene;

        public PickingBehavior()
            : base("PickingBehavior")
        { }

        /// <summary>
        ///  Resolve dependencies method
        /// </summary>
        protected override void ResolveDependencies()
        {
            base.ResolveDependencies();

            this.myScene = WaveServices.ScreenContextManager.CurrentContext[0] as MyScene;
        }

        /// <summary>
        /// Update method
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Update(TimeSpan gameTime)
        {            
            touchPanelState = WaveServices.Input.TouchPanelState;
           // bestValue = float.MaxValue;
            if (touchPanelState.IsConnected && touchPanelState.Count > 0)
            {
                // Calculate the ray
                CalculateRay();

                var hitResult = this.myScene.PhysicsManager.Simulation3D.RayCast(ref this.ray, 1000);
                if (hitResult.Succeeded)
                {
                    var userData = (Collider3D)hitResult.Collider.UserData;
                    this.myScene.ShowPickedEntity(userData.Owner.Name);
                }
            }
            else
            {                                
                if (this.myScene != null)
                {
                    this.myScene.ShowPickedEntity("None");
                }
            }
        }

        /// <summary>
        /// Initialize method
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            nearPoint = new Vector3();
            farPoint = new Vector3(0f, 0f, 1f);
            ray = new Ray();
        }

        /// <summary>
        /// Calculate a ray between the camer and the selected point
        /// </summary>
        private void CalculateRay()
        {
            // Get the current touch location
            currentLocation = touchPanelState.First();

            // Apply the near point:
            nearPoint.X = currentLocation.Position.X;
            nearPoint.Y = currentLocation.Position.Y;
            nearPoint.Z = 0f;
            // And the far point
            farPoint.X = currentLocation.Position.X;
            farPoint.Y = currentLocation.Position.Y;
            farPoint.Z = 1f;

            // Unproject both to get the 2d point in a 3d screen projections. 
            
            nearPoint = Camera.Unproject(ref nearPoint);
            farPoint = Camera.Unproject(ref farPoint);

            // Now, we have a 3d point. We calculate the point direction
            direction = farPoint - nearPoint;
            // And normalized it.
            direction.Normalize();

            // Set the ray to the cached variable
            ray.Direction = direction;
            ray.Position = nearPoint;
        }
    }
}
