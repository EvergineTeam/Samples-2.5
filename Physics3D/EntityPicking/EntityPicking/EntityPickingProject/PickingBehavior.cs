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

#region File Description
//-----------------------------------------------------------------------------
// PickingBehavior
//
// Copyright © 2013 Weekend Game Studio. All rights reserved.
// Use is subject to license terms.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using WaveEngine.Common.Input;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics3D;
using WaveEngine.Framework.Services;
#endregion

namespace EntityPickingProject
{
    public class PickingBehavior : Behavior
    {
        private const float MINDISTANCE = 2.5f;

        [RequiredComponent()]
        public Camera3D camera;

        private Input input;
        private MouseState mouseState;
        private KeyboardState keyboardState;
        private bool continousPressed = false;

        private Ray ray;
        private Vector2 mousePosition;

        private Vector3 nearPosition;
        private Vector3 farPosition;
        private Vector3 pickingPosition;
        private Vector3 rayDirection;

        private Matrix identity = Matrix.Identity;

        private float distance = float.MaxValue;
        private float? collisionResult;
        private Entity selectedEntity;
        private BoxCollider entityBoxCollider;

        private MotorizedGrabSpring3D mouseJoint;

        public PickingBehavior()
            : base("PickingCameraBehavior")
        {
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.ray = new Ray();
            this.mousePosition = Vector2.Zero;
            this.nearPosition = new Vector3(0f, 0f, 0f);
            this.farPosition = new Vector3(0f, 0f, 1f);
            this.pickingPosition = farPosition;
        }

        protected override void Update(TimeSpan gameTime)
        {
            this.input = WaveServices.Input;
            if (this.input.MouseState.IsConnected)
            {
                this.mouseState = this.input.MouseState;

                // Update Mouse Position
                this.mousePosition.X = this.mouseState.X;
                this.mousePosition.Y = this.mouseState.Y;

                // Left Button Presseded (just one time while pressed)
                if (this.mouseState.LeftButton == ButtonState.Pressed && !continousPressed)
                {
                    this.continousPressed = true;
                    this.SelectPickedEntity();

                    if (this.selectedEntity != null)
                    {
                        this.distance = MathHelper.Max(this.distance, MINDISTANCE);

                        // Mouse point at entity collide point distance
                        this.pickingPosition.X = this.mousePosition.X;
                        this.pickingPosition.Y = this.mousePosition.Y;
                        this.pickingPosition.Z = this.distance;
                        this.pickingPosition = this.camera.Unproject(ref this.pickingPosition);

                        // obtains CameraPosition-PickingPosition vector
                        this.pickingPosition = this.camera.Position - this.pickingPosition;
                        this.pickingPosition.Normalize();

                        // Calculate 
                        this.pickingPosition = this.camera.Position + this.pickingPosition * this.distance;

                        var jointMap = this.selectedEntity.FindComponent<JointMap3D>();
                        if (jointMap != null)
                        {
                            this.mouseJoint = new MotorizedGrabSpring3D(this.pickingPosition);
                            jointMap.AddJoint("mouseJoint", mouseJoint);
                        }
                    }
                }

                // Left Button Released
                if (this.mouseState.LeftButton == ButtonState.Release && continousPressed)
                {
                    this.RemoveSelected();
                }

                if (this.selectedEntity != null && this.distance < float.MaxValue && this.mouseJoint != null)
                {
                    // Mouse point at entity collide point distance
                    this.pickingPosition.X = this.mousePosition.X;
                    this.pickingPosition.Y = this.mousePosition.Y;
                    this.pickingPosition.Z = this.distance;
                    this.pickingPosition = this.camera.Unproject(ref this.pickingPosition);

                    // obtains CameraPosition-PickingPosition vector
                    this.pickingPosition = this.camera.Position - this.pickingPosition;
                    this.pickingPosition.Normalize();

                    // Calculate 
                    this.pickingPosition = this.camera.Position + this.pickingPosition * this.distance;

                    this.mouseJoint.WorldAnchor = this.pickingPosition;
                }
            }

            // Keyboard Controls
            if (this.input.KeyboardState.IsConnected)
            {
                this.keyboardState = this.input.KeyboardState;

                // (Up) adds distance
                if (this.keyboardState.Up == ButtonState.Pressed)
                {
                    if (this.selectedEntity != null & this.distance < float.MaxValue)
                    {
                        this.distance += 0.3f;
                    }
                } // (Down) Substracts distance
                else if (this.keyboardState.Down == ButtonState.Pressed)
                {
                    if (this.selectedEntity != null && this.distance > MINDISTANCE)
                    {
                        this.distance -= 0.3f;
                    }
                }
            }
        }

        private void RemoveSelected()
        {
            this.continousPressed = false;

            this.distance = float.MaxValue;
            if (this.selectedEntity != null)
            {
                var jointMap = this.selectedEntity.FindComponent<JointMap3D>();
                if (jointMap != null)
                {
                    jointMap.RemoveJoint("mouseJoint");
                }
                this.selectedEntity = null;

            }
            this.mouseJoint = null;
        }

        private void SelectPickedEntity()
        {
            // Creates Ray from Mouse 2d position
            this.nearPosition.X = this.mousePosition.X;
            this.nearPosition.Y = this.mousePosition.Y;
            this.nearPosition.Z = 0.0f;

            this.farPosition.X = this.mousePosition.X;
            this.farPosition.Y = this.mousePosition.Y;
            this.farPosition.Z = 1.0f;

            // Unproject Mouse Position
            this.nearPosition = this.camera.Unproject(ref this.nearPosition);
            this.farPosition = this.camera.Unproject(ref this.farPosition);

            // Update ray. Ray launched from nearPosition in rayDirection direction.
            this.rayDirection = this.farPosition - this.nearPosition;
            this.rayDirection.Normalize();
            this.ray.Direction = this.rayDirection;
            this.ray.Position = this.nearPosition;

            foreach (Entity entity in this.Owner.Scene.EntityManager.EntityGraph)
            {
                // It takes Box Shaped Physic Entities
                this.entityBoxCollider = entity.FindComponent<BoxCollider>();
                if (this.entityBoxCollider != null)
                {
                    this.collisionResult = this.entityBoxCollider.Intersects(ref ray);

                    if (this.collisionResult.HasValue)
                    {
                        if (this.collisionResult.Value < this.distance)
                        {
                            this.distance = this.collisionResult.Value;
                            this.selectedEntity = entity;
                        }
                    }
                }
            }
        }
    }
}
