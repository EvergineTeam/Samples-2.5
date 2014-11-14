using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Diagnostic;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
using WaveOculusDemoProject.Sensor;

namespace WaveOculusDemoProject.Components
{
    /// <summary>
    /// Stereoscopic camera controller
    /// </summary>
    public class StereoscopicCameraController : Behavior
    {
        public const float DefaultHeadScale = 1.926f;
        public readonly float DefaultFieldOfView = MathHelper.ToRadians(110);
        public readonly float DefaultEyeSeparation = 0.065f * DefaultHeadScale;
        public readonly float DefaultEyeFocalDistance = 16;
        public readonly Vector2 DefaultEyeCenterAngleOffset = Vector2.Zero;

        private readonly float EyeSeparationSpeed = 0.15f;
        private readonly float CameraMoveSpeed = 0.2f;
        private readonly float CameraSpeed = 1.2f;
        private readonly float FocalSpeed = 1f;
        private readonly float OffsetSpeed = 0.04f;
        private readonly float DefaultNearPlane = WaveServices.Platform.AdapterType == WaveEngine.Common.AdapterType.DirectX ? 0.3f : 1f;
        private readonly float DefaultFarPlane = 6000;
        private readonly Vector3 NeckDistance = new Vector3(0, 0.3826f, 0.5076f);


        [RequiredComponent]
        private Transform3D transform = null;

        private Input inputService = WaveServices.Input;

        private HeadTrackSensor headTrack;

        private Entity leftLookAtEntity;
        private Entity rightLookAtEntity;
        private Entity leftEyeEntity;
        private Entity rightEyeEntity;

        private Camera3D leftEye;
        private Camera3D rightEye;

        private Transform3D leftLookAtTransform;
        private Transform3D rightLookAtTransform;
        private Transform3D leftEyeTransform;
        private Transform3D rightEyeTransform;

        private float fov;
        private float focalDistance;
        private float eyeSeparation;
        private Vector2 eyeCenterAngleOffset;
        private float nearPlane, farPlane;
        
        public float FieldOfView
        {
            get
            {
                return this.fov;
            }
            set
            {
                this.fov = value;

                if (this.isInitialized)
                {
                    this.leftEye.FieldOfView = this.fov * this.leftEye.AspectRatio;
                    this.rightEye.FieldOfView = this.fov * this.leftEye.AspectRatio;
                }
            }
        }

        public float EyeSeparation
        {
            get
            {
                return this.eyeSeparation;
            }
            set
            {
                this.eyeSeparation = value;
                this.RefreshCameraCalibration();
            }
        }


        public float FocalDistance
        {
            get
            {
                return this.focalDistance;
            }

            set
            {
                this.focalDistance = value;
                this.RefreshCameraCalibration();
            }
        }

        public Vector2 EyeCenterAngleOffset
        {
            get
            {
                return this.eyeCenterAngleOffset;
            }

            set
            {
                this.eyeCenterAngleOffset = value;
                this.RefreshCameraCalibration();
            }
        }

        public float FarPlane
        {
            get
            {
                return this.farPlane;
            }
            set
            {
                this.farPlane = value;

                if (this.isInitialized)
                {
                    this.leftEye.FarPlane = value;
                    this.rightEye.FarPlane = value;
                }
            }
        }

        public float NearPlane
        {
            get
            {
                return this.nearPlane;
            }
            set
            {
                this.nearPlane = value;

                if (this.isInitialized)
                {
                    this.leftEye.NearPlane = value;
                    this.rightEye.NearPlane = value;
                }
            }
        }

        /// <summary>
        /// Instantiate a new stereoscopic camera controller
        /// </summary>
        public StereoscopicCameraController()
        {
            this.EyeSeparation = DefaultEyeSeparation;
            this.FocalDistance = DefaultEyeFocalDistance;
            this.FieldOfView = DefaultFieldOfView;
            this.nearPlane = DefaultNearPlane;

            Platform platform = WaveServices.Platform;
            if (platform.PlatformType == PlatformType.Windows
                || platform.PlatformType == PlatformType.Linux
                || platform.PlatformType == PlatformType.MacOS)
            {
                this.farPlane = 10000;
            }
            else
            {
                this.farPlane = DefaultFarPlane;
            }

            this.UpdateOrder = 0.9f;
        }

        /// <summary>
        /// Initialize stereoscopic camera controller
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            this.headTrack = WaveServices.GetService<HeadTrackSensor>();

            this.leftLookAtEntity = this.Owner.FindChild("leftLookAt");
            this.rightLookAtEntity = this.Owner.FindChild("rightLookAt");
            this.leftEyeEntity = this.Owner.FindChild("leftEye");
            this.rightEyeEntity = this.Owner.FindChild("rightEye");

            this.leftEye = this.leftEyeEntity.FindComponent<Camera3D>();
            this.rightEye = this.rightEyeEntity.FindComponent<Camera3D>();

            this.leftLookAtTransform = this.leftLookAtEntity.FindComponent<Transform3D>();
            this.rightLookAtTransform = this.rightLookAtEntity.FindComponent<Transform3D>();
            this.leftEyeTransform = this.leftEyeEntity.FindComponent<Transform3D>();
            this.rightEyeTransform = this.rightEyeEntity.FindComponent<Transform3D>();

            // Left eye
            this.leftEye.Viewport = new Viewport(0, 0, 0.5f, 1);
            this.leftEye.FieldOfView = this.FieldOfView;
            this.leftEye.NearPlane = this.NearPlane;
            this.leftEye.FarPlane = this.FarPlane;

            // Right eye
            this.rightEye.Viewport = new Viewport(0.5f, 0, 0.5f, 1);
            this.rightEye.FieldOfView = this.FieldOfView;
            this.rightEye.NearPlane = this.NearPlane;
            this.rightEye.FarPlane = this.FarPlane;

            this.isInitialized = true;
            this.RefreshCameraCalibration();
        }

        /// <summary>
        /// Update the stereoscopic camera to position each eye camera
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Update(TimeSpan gameTime)
        {
            if (this.headTrack.IsSupported)
            {
                this.transform.LocalOrientation = this.headTrack.GetHeadOrientation();
                this.transform.LocalPosition = (this.headTrack.GetHeadPosition() * DefaultHeadScale * 3) - NeckDistance;
            }

            this.HandleKeys(gameTime);

            this.leftEye.Position = this.leftEyeTransform.Position;
            this.rightEye.Position = this.rightEyeTransform.Position;

            this.leftEye.LookAt = this.leftLookAtTransform.Position;
            this.rightEye.LookAt = this.rightLookAtTransform.Position;

            var upVector = this.transform.WorldTransform.Up;
            this.leftEye.UpVector = upVector;
            this.rightEye.UpVector = upVector;
        }

        /// <summary>
        /// Handle keys to adjust camera properties
        /// </summary>
        /// <param name="gameTime">The gametime</param>
        private void HandleKeys(TimeSpan gameTime)
        {
            if (this.inputService.KeyboardState.IsConnected)
            {
                var orientation = Quaternion.Identity;
                var keyState = this.inputService.KeyboardState;

                if (keyState.F == WaveEngine.Common.Input.ButtonState.Pressed)
                {
                    this.FocalDistance -= (float)(FocalSpeed * gameTime.TotalSeconds);
                }

                if (keyState.R == WaveEngine.Common.Input.ButtonState.Pressed)
                {
                    this.FocalDistance += (float)(FocalSpeed * gameTime.TotalSeconds);
                }

                if (keyState.Q == WaveEngine.Common.Input.ButtonState.Pressed)
                {
                    Vector2 offset = this.EyeCenterAngleOffset;
                    offset.X += (float)(OffsetSpeed * gameTime.TotalSeconds);

                    this.EyeCenterAngleOffset = offset;
                }

                if (keyState.E == WaveEngine.Common.Input.ButtonState.Pressed)
                {
                    Vector2 offset = this.EyeCenterAngleOffset;
                    offset.X -= (float)(OffsetSpeed * gameTime.TotalSeconds);

                    this.EyeCenterAngleOffset = offset;
                }

                if (keyState.W == WaveEngine.Common.Input.ButtonState.Pressed)
                {
                    var dir = this.transform.LocalTransform.Forward;
                    this.transform.LocalPosition -= dir * CameraMoveSpeed * (float)gameTime.TotalSeconds;
                }

                if (keyState.S == WaveEngine.Common.Input.ButtonState.Pressed)
                {
                    var dir = this.transform.LocalTransform.Backward;
                    this.transform.LocalPosition -= dir * CameraMoveSpeed * (float)gameTime.TotalSeconds;
                }

                if (keyState.A == WaveEngine.Common.Input.ButtonState.Pressed)
                {
                    var dir = this.transform.LocalTransform.Left;
                    this.transform.LocalPosition -= dir * CameraMoveSpeed * (float)gameTime.TotalSeconds;
                }

                if (keyState.D == WaveEngine.Common.Input.ButtonState.Pressed)
                {
                    var dir = this.transform.LocalTransform.Right;
                    this.transform.LocalPosition -= dir * CameraMoveSpeed * (float)gameTime.TotalSeconds;
                }

                if (keyState.Z == WaveEngine.Common.Input.ButtonState.Pressed)
                {
                    this.EyeSeparation -= EyeSeparationSpeed * (float)gameTime.TotalSeconds;
                }

                if (keyState.X == WaveEngine.Common.Input.ButtonState.Pressed)
                {
                    this.EyeSeparation += EyeSeparationSpeed * (float)gameTime.TotalSeconds;
                }


                if (keyState.Up == WaveEngine.Common.Input.ButtonState.Pressed)
                {
                    Quaternion q = Quaternion.CreateFromAxisAngle(Vector3.UnitX, (float)(-CameraSpeed * gameTime.TotalSeconds));

                    orientation *= q;
                }

                if (keyState.Down == WaveEngine.Common.Input.ButtonState.Pressed)
                {
                    Quaternion q = Quaternion.CreateFromAxisAngle(Vector3.UnitX, (float)(CameraSpeed * gameTime.TotalSeconds));
                    orientation *= q;
                }

                if (keyState.Right == WaveEngine.Common.Input.ButtonState.Pressed)
                {
                    Quaternion q = Quaternion.CreateFromAxisAngle(Vector3.UnitY, (float)(-CameraSpeed * gameTime.TotalSeconds));

                    orientation *= q;
                }

                if (keyState.Left == WaveEngine.Common.Input.ButtonState.Pressed)
                {
                    Quaternion q = Quaternion.CreateFromAxisAngle(Vector3.UnitY, (float)(CameraSpeed * gameTime.TotalSeconds));
                    orientation *= q;
                }

                this.transform.LocalOrientation = this.transform.LocalOrientation * orientation;
            }
        }

        /// <summary>
        /// Refresh camera calibration. Called when stereo camera properties are changed.
        /// </summary>
        private void RefreshCameraCalibration()
        {
            if (!this.isInitialized)
            {
                return;
            }

            this.leftEyeTransform.LocalPosition = Vector3.UnitX * this.EyeSeparation * 0.5f;
            this.rightEyeTransform.LocalPosition = Vector3.UnitX * -this.EyeSeparation * 0.5f;


            Vector3 lookAtVector;
            Quaternion q;


            lookAtVector = Vector3.UnitZ * this.focalDistance - this.leftEyeTransform.LocalPosition;
            q = Quaternion.CreateFromAxisAngle(Vector3.UnitX, this.eyeCenterAngleOffset.Y)
                * Quaternion.CreateFromAxisAngle(Vector3.UnitY, this.eyeCenterAngleOffset.X);
            this.leftLookAtTransform.LocalPosition = Vector3.Transform(lookAtVector, q);

            lookAtVector = Vector3.UnitZ * this.focalDistance - this.rightEyeTransform.LocalPosition;
            q = Quaternion.CreateFromAxisAngle(Vector3.UnitX, this.eyeCenterAngleOffset.Y)
                * Quaternion.CreateFromAxisAngle(Vector3.UnitY, -this.eyeCenterAngleOffset.X);
            this.rightLookAtTransform.LocalPosition = Vector3.Transform(lookAtVector, q);
        }
    }
}


