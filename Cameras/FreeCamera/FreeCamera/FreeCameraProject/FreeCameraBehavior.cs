using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Framework.Services;
using WaveEngine.Common.Input;
using System.Diagnostics;

namespace FreeCameraProject
{
    public class FreeCameraBehavior : Behavior
    {
        #region "Properties"

        /// <summary>
        /// Get or set the speed of the camera movement
        /// </summary>
        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        /// <summary>
        /// Get or set the speed of the mouse movement
        /// </summary>
        public float RotationSpeed
        {
            get
            {
                return rotationSpeed;
            }
            set { rotationSpeed = value; }
        }
        #endregion

        // Camera component
        [RequiredComponent]
        private Camera3D camera;

        // Speed of the movement
        private float speed = 14f;

        // Mouse speed movement
        private float rotationSpeed = .16f;

        // The mouse movement
        private float yaw, pitch;

        // Camera rotation calculation
        private Matrix cameraMatrixRotation;

        // Cached input
        private Input input;

        // Cached camera position
        private Vector3 position;

        // Current keyboard state
        private KeyboardState keyboardState;

        // Current touch panel state
        private TouchPanelState currentTouchPanelState;

        // Current touch panel location
        private TouchLocation currentTouchLocation;

        // Last touch panel location
        private TouchLocation lastTouchLocation;

        // Current mouse state
        private MouseState currentMouseState;

        // Las mouse state
        private MouseState lastMouseState;

        // Is the user currently dragging on this arcball?
        private bool isDragging = false;

        // Normalized forward vector
        private Vector3 forwardNormalizedVector;

        // Normalized right vector
        private Vector3 rightNormalizedVector;

        private Vector3 upNormalizedVector;

        private Matrix tempRotationMatrix;

        // Cached direction movements
        private Vector3 forward;
        private Vector3 right;
        private float xDifference;
        private float yDifference;
        private float timeDifference;
        private bool isMouseConnected;
        private bool isTouchPanelConnected;

        #region Initialize

        public FreeCameraBehavior()
            : base("FreeCameraBehavior")
        { }

        protected override void Initialize()
        {
            yaw = 0.0f;
            pitch = 0.0f;
            xDifference = 0f;
            yDifference = 0f;
            isDragging = false;

            // Preview of the view matrix
            cameraMatrixRotation = Matrix.Identity;

            var initialMatrix = Matrix.Invert(Matrix.CreateLookAt(camera.Position, camera.LookAt, camera.UpVector));
            right = initialMatrix.Right;
            forward = initialMatrix.Forward;
            var up = initialMatrix.Up;
            up.Normalize();
            upNormalizedVector = up;
            position = camera.Position;
            cameraMatrixRotation = initialMatrix;
        }

        #endregion

        #region Public methods

        #endregion

        #region Private methods

        protected override void Update(TimeSpan gameTime)
        {
            timeDifference = (float)gameTime.Milliseconds / 1000.0f;
            HandleInput(timeDifference);
        }

        private void HandleInput(float amount)
        {
            input = WaveServices.Input;
            isMouseConnected = input.MouseState.IsConnected;
            isTouchPanelConnected = input.MouseState.IsConnected;

            if (isMouseConnected)
            {
                isTouchPanelConnected = false;
            }

            if (input.KeyboardState.IsConnected)
            {
                keyboardState = input.KeyboardState;

                if (keyboardState.W == ButtonState.Pressed)
                {
                    // Manual inline: position += speed * forward;
                    position.X = position.X + (amount * speed * forward.X);
                    position.Y = position.Y + (amount * speed * forward.Y);
                    position.Z = position.Z + (amount * speed * forward.Z);
                    UpdateCameraPosition();
                }
                else if (keyboardState.S == ButtonState.Pressed)
                {
                    // Manual inline: position -= speed * forward;
                    position.X = position.X - (amount * speed * forward.X);
                    position.Y = position.Y - (amount * speed * forward.Y);
                    position.Z = position.Z - (amount * speed * forward.Z);
                    UpdateCameraPosition();
                }
                if (keyboardState.A == ButtonState.Pressed)
                {
                    // Manual inline: position -= speed * right;
                    position.X = position.X - (amount * speed * right.X);
                    position.Y = position.Y - (amount * speed * right.Y);
                    position.Z = position.Z - (amount * speed * right.Z);
                    UpdateCameraPosition();
                }
                else if (keyboardState.D == ButtonState.Pressed)
                {
                    // Manual inline: position += speed * right;
                    position.X = position.X + (amount * speed * right.X);
                    position.Y = position.Y + (amount * speed * right.Y);
                    position.Z = position.Z + (amount * speed * right.Z);
                    UpdateCameraPosition();
                }
            }

            if (isTouchPanelConnected || isMouseConnected)
            {
                if (isTouchPanelConnected)
                {
                    currentTouchPanelState = input.TouchPanelState;
                }
                if (isMouseConnected)
                {
                    currentMouseState = input.MouseState;
                }

                if ((isTouchPanelConnected && currentTouchPanelState.Count == 1)
                    || (isMouseConnected && currentMouseState.RightButton == ButtonState.Pressed))
                {
                    if (isTouchPanelConnected && currentTouchPanelState.Count == 1)
                    {
                        currentTouchLocation = currentTouchPanelState.First();
                    }

                    if ((isTouchPanelConnected && currentTouchLocation.State == TouchLocationState.Pressed)
                        ||
                        (isMouseConnected && currentMouseState.RightButton == ButtonState.Pressed))
                    {
                        if (isDragging == false)
                        {
                            isDragging = true;
                        }
                        else
                        {
                            if (currentTouchPanelState.IsConnected)
                            {
                                xDifference = (currentTouchLocation.Position.X - lastTouchLocation.Position.X);
                                yDifference = (currentTouchLocation.Position.Y - lastTouchLocation.Position.Y);
                            }
                            if (isMouseConnected)
                            {
                                xDifference = (currentMouseState.X - lastMouseState.X);
                                yDifference = (currentMouseState.Y - lastMouseState.Y);
                            }

                            yaw = yaw - (xDifference * amount * rotationSpeed);
                            pitch = pitch - (yDifference * amount * rotationSpeed);

                            // Manual inline: forwardNormalizedVector = cameraRotation.Forward;
                            forwardNormalizedVector.X = cameraMatrixRotation.Forward.X;
                            forwardNormalizedVector.Y = cameraMatrixRotation.Forward.Y;
                            forwardNormalizedVector.Z = cameraMatrixRotation.Forward.Z;
                            forwardNormalizedVector.Normalize();

                            // Manual inline: rightNormalizedVector = cameraRotation.Right;
                            rightNormalizedVector.X = cameraMatrixRotation.Right.X;
                            rightNormalizedVector.Y = cameraMatrixRotation.Right.Y;
                            rightNormalizedVector.Z = cameraMatrixRotation.Right.Z;
                            rightNormalizedVector.Normalize();

                            // Manual inline: upNormalizedVector = cameraMatrixRotation.Up;
                            upNormalizedVector.X = cameraMatrixRotation.Up.X;
                            upNormalizedVector.Y = cameraMatrixRotation.Up.Y;
                            upNormalizedVector.Z = cameraMatrixRotation.Up.Z;
                            upNormalizedVector.Normalize();

                            Matrix.CreateFromAxisAngle(ref rightNormalizedVector, pitch, out tempRotationMatrix);
                            Matrix.Multiply(ref cameraMatrixRotation, ref tempRotationMatrix, out cameraMatrixRotation);

                            Matrix.CreateFromAxisAngle(ref upNormalizedVector, yaw, out tempRotationMatrix);
                            Matrix.Multiply(ref cameraMatrixRotation, ref tempRotationMatrix, out cameraMatrixRotation);

                            Matrix.CreateFromAxisAngle(ref forwardNormalizedVector, 0f, out tempRotationMatrix);
                            Matrix.Multiply(ref cameraMatrixRotation, ref tempRotationMatrix, out cameraMatrixRotation);

                            // Original code
                            /*
                            cameraMatrixRotation.Forward.Normalize();
                            cameraMatrixRotation.Right.Normalize();
                            cameraMatrixRotation.Up.Normalize();
                            
                            cameraMatrixRotation *= Matrix.CreateFromAxisAngle(cameraMatrixRotation.Right, pitch);
                            cameraMatrixRotation *= Matrix.CreateFromAxisAngle(cameraMatrixRotation.Up, yaw);
                            cameraMatrixRotation *= Matrix.CreateFromAxisAngle(cameraMatrixRotation.Forward, 0f);
                            */

                            yaw = 0.0f;
                            pitch = 0.0f;

                            // Manual inline: forward = cameraRotation.Forward;
                            forward.X = cameraMatrixRotation.Forward.X;
                            forward.Y = cameraMatrixRotation.Forward.Y;
                            forward.Z = cameraMatrixRotation.Forward.Z;

                            //// Manual inline: right = cameraRotation.Right;
                            right.X = cameraMatrixRotation.Right.X;
                            right.Y = cameraMatrixRotation.Right.Y;
                            right.Z = cameraMatrixRotation.Right.Z;

                            UpdateLookAt();

                            //Restore the current matrix rotation
                            cameraMatrixRotation = Matrix.Invert(Matrix.CreateLookAt(camera.Position, camera.LookAt, camera.UpVector));
                        }
                    }
                    lastTouchLocation = currentTouchLocation;
                    lastMouseState = currentMouseState;
                }
                else
                {
                    isDragging = false;
                }
            }
        }

        private void UpdateLookAt()
        {
            // Manual inline: camera.LookAt = target;

            var cameraLookAt = camera.LookAt;
            cameraLookAt.X = position.X;
            cameraLookAt.Y = position.Y;
            cameraLookAt.Z = position.Z;
            camera.LookAt = cameraLookAt;

            camera.UpVector = Vector3.Up;
        }

        private void UpdateCameraPosition()
        {
            UpdateLookAt();

            // Manual inline: camera.Position = position;

            var cameraPosition = camera.Position;
            cameraPosition.X = position.X;
            cameraPosition.Y = position.Y;
            cameraPosition.Z = position.Z;
            camera.Position = cameraPosition;
        }

        #endregion
    }
}


