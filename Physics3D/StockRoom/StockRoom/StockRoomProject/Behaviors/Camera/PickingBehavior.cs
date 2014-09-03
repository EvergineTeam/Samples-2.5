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

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Input;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Diagnostic;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics3D;
using WaveEngine.Framework.Services;
#endregion

namespace StockRoomProject.Behaviors
{
    public class PickingBehavior : Behavior
    {
        private const int FORCE = 2;

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
        private Line line, line2;
        private Entity currentEntity;
        private BoxCollider entityCollider;
        private float bestValue;
        private float? collisionResult;

        #region Properties
        public Entity CurrentEntity
        {
            get { return currentEntity; }
        }
        #endregion

        public PickingBehavior()
            : base("PickingBehavior")
        { }

        protected override void Initialize()
        {
            base.Initialize();

            nearPoint = new Vector3();
            farPoint = new Vector3(0f, 0f, 1f);
            ray = new Ray();
            line = new Line(Vector2.Zero, Vector2.Zero, Color.Red);
            line2 = new Line(Vector2.Zero, Vector2.Zero, Color.Blue);
        }

        protected override void Update(TimeSpan gameTime)
        {
            touchPanelState = WaveServices.Input.TouchPanelState;

            if (touchPanelState.IsConnected && touchPanelState.Count > 0)
            {
                // Calculate the ray
                CalculateRay();

                // Look for all entities in the game...
                Entity auxEntity = currentEntity = null;
                bestValue = float.MaxValue;
                for (int i = 0; i < EntityManager.Count; i++)
                {
                    auxEntity = EntityManager.EntityGraph.ElementAt(i);
                    entityCollider = auxEntity.FindComponent<BoxCollider>();
                    // ... but only a collidable entities ( entities which have a boxCollider component)
                    if (entityCollider != null && ( auxEntity.Name.Contains("box") ||
                                                  auxEntity.Name.Contains("anchor") ||
                                                  auxEntity.Name.Contains("BigBall")) )
                    {
                        // Intersect our calculated ray with the entity's boxCollider
                        collisionResult = entityCollider.Intersects(ref ray);
                        // If any collision
                        if (collisionResult.HasValue && collisionResult.Value > 0.001f)
                        {
                            //Labels.Add("CollisionResult", collisionResult.ToString());
                            //Labels.Add("CollisionValue", collisionResult.Value.ToString());
                            // Check the distance. We want to have the closer to the screen entity, so we want to get the low collisionResult value
                            if (collisionResult.Value < bestValue)
                            {
                                this.currentEntity = auxEntity;
                                bestValue = collisionResult.Value;
                            }
                        }
                    }
                }

                if (this.currentEntity != null)
                {
                    Vector3 entityPosition = this.currentEntity.FindComponent<Transform3D>().Position;
                    Vector3 impulse = entityPosition - this.Camera.Position;                    
                    this.currentEntity.FindComponent<RigidBody3D>().ApplyLinearImpulse(impulse*FORCE);

                    this.line.StartPoint = ray.Position;
                    this.line.EndPoint = entityPosition;

                    Labels.Add("Entity", this.currentEntity.Name);
                    //Labels.Add("Impulse", impulse.ToString());
                    //Labels.Add("IsActive", this.currentEntity.FindComponent<RigidBody3D>().IsActive.ToString());
                }
                else
                {
                    Labels.Add("Entity", "None");
                    //Labels.Add("Impulse", "0,0,0");
                }
            }

            //RenderManager.LineBatch3D.DrawLine(ref line);
            //RenderManager.LineBatch3D.DrawLine(ref line2);
        }

        /// <summary>
        /// Calculate a ray between the camer and the selected point
        /// </summary>
        private void CalculateRay()
        {
            // Get the current touch location
            currentLocation = touchPanelState.First();

            //Labels.Add("MouseX", currentLocation.Position.X.ToString());
            //Labels.Add("MouseY", currentLocation.Position.Y.ToString());

            // Apply the near point:
            nearPoint = Vector3.Zero;
            farPoint = Vector3.Zero;
            nearPoint.X = currentLocation.Position.X;
            nearPoint.Y = currentLocation.Position.Y;
            nearPoint.Z = 0f;
            // And the far point
            farPoint.X = currentLocation.Position.X;
            farPoint.Y = currentLocation.Position.Y;
            farPoint.Z = 1f;
            //Labels.Add("Nearpoint", nearPoint.ToString());
            //Labels.Add("Farpoint", farPoint.ToString());

            Matrix world = Matrix.CreateTranslation(0, 0, 0);

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

            this.line2.StartPoint = nearPoint;
            this.line2.EndPoint = farPoint;
        }
    }
}
