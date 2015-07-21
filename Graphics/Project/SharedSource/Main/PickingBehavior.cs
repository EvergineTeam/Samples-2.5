// Copyright (C) 2012-2013 Weekend Game Studio
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to
// deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
// sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.

using System;
using System.Linq;
using WaveEngine.Common.Input;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
using WaveEngine.Framework.Physics3D;
using System.Runtime.Serialization;

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
        private Matrix identity = Matrix.Identity;
        private Vector3 direction;
        private Ray ray;
        private Entity currentEntity;
        private BoxCollider3D entityCollider;
        private float bestValue;
        private float? collisionResult;

        public PickingBehavior()
            : base("PickingBehavior")
        { }

        protected override void Update(TimeSpan gameTime)
        {
            touchPanelState = WaveServices.Input.TouchPanelState;
            bestValue = float.MaxValue;
            if (touchPanelState.IsConnected && touchPanelState.Count > 0)
            {
                // Calculate the ray
                CalculateRay();

                // Look for all entities in the game...
                for (int i = 0; i < EntityManager.Count; i++)
                {
                    currentEntity = EntityManager.EntityGraph.ElementAt(i); ;

                    entityCollider = currentEntity.FindComponent<BoxCollider3D>();
                    // ... but only a collidable entities ( entities which have a boxCollider component)
                    if (entityCollider != null)
                    {
                        // Intersect our calculated ray with the entity's boxCollider
                        collisionResult = entityCollider.Intersects(ref ray);
                        // If any collision
                        if (collisionResult.HasValue)
                        {
                            // Check the distance. We want to have the closer to the screen entity, so we want to get the low collisionResult value
                            if (collisionResult.Value < bestValue)
                            {
                                // Send to the scene the new entity picked name
                                (WaveServices.ScreenContextManager.CurrentContext[0] as MyScene).ShowPickedEntity(currentEntity.Name);
                                bestValue = collisionResult.Value;
                            }
                        }
                    }
                }
            }
            else
            {
                (WaveServices.ScreenContextManager.CurrentContext[0] as MyScene).ShowPickedEntity("None");
            }
        }

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
