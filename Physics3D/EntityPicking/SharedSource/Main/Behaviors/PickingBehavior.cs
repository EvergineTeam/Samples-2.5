using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Common.Input;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics3D;
using WaveEngine.Framework.Services;

namespace EntityPicking.Behaviors
{
    [DataContract]
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
        private BoxCollider3D entityBoxCollider;

        private SpringJoint3D mouseJoint;

        public PickingBehavior()
            : base("PickingCameraBehavior")
        {
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void DefaultValues()
        {
            base.DefaultValues();

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
                            this.mouseJoint = new SpringJoint3D(this.pickingPosition);
                            jointMap.AddJoint("mouseJoint", mouseJoint);
                        }
                    }
                }

                // Left Button Released
                if (this.mouseState.LeftButton == ButtonState.Released && continousPressed)
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
                this.entityBoxCollider = entity.FindComponent<BoxCollider3D>();
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
