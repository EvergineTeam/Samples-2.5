#region Using Statements
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
using MixedRealitySample.Materials;
using MixedRealitySample.Drawables;
using WaveEngine.Common.Attributes;
#endregion

namespace MixedRealitySample.Behaviors
{
    public enum ControllerType
    {
        Right,
        Left,
    }

    [DataContract]
    public abstract class BaseControllerBehavior : Behavior
    {
        // Entities
        protected Entity ring;
        protected Entity trigger;
        protected Entity grab;
        protected Entity thumbstick;
        protected Entity menu;
        protected Entity ray;
        protected Entity dot;
        protected Entity gaze;

        // Components
        protected Transform3D triggerTransform;
        protected Transform3D thumbstickTransform;
        protected Transform3D grabTransform;
        protected Transform3D menuTransform;
        protected Transform3D rayTransform;
        protected GPULineRenderer lineRenderer;
        protected LineMaterial lineMaterial;
        protected Transform3D dotTransform;
        protected Transform3D gazeTransform;
        protected Transform3D cameraTransform;

        // Cached
        protected Vector3 initialThumbStickRotation;
        protected Vector3 initialGrabRotation;
        protected Vector3 initialMenuPosition;
        protected List<LineVertexInfo> rayPoints;
        protected float lastTriggerValue;
        protected float rayThin;
        protected ControllerType type;

        #region Properties
        /// <summary>
        /// Trigger value [0-1]
        /// </summary>
        [DontRenderProperty]
        public float TriggerValue { get; set; }

        /// <summary>
        ///([-1,1], [-1,1])         
        /// </summary>
        [DontRenderProperty]
        public Vector2 ThumbstickValue { get; set; }

        [DontRenderProperty]
        public bool GrabValue { get; set; }

        [DontRenderProperty]
        public bool MenuValue { get; set; }

        [DontRenderProperty]
        public Vector2 TouchpadValue { get; set; }

        [DataMember]
        public ControllerType Type
        {
            get { return this.type; }
            set
            {
                this.type = value;
            }
        }

        public float ScaledRayThin
        {
            get
            {
                return this.rayThin * this.rayTransform.Scale.Length();
            }
        }
        #endregion

        protected override void DefaultValues()
        {
            base.DefaultValues();

            this.rayThin = 0.43f;
        }

        /// <summary>
        /// Resolve dependencies method
        /// </summary>
        protected override void ResolveDependencies()
        {
            base.ResolveDependencies();

            var current = this.Owner;

            this.ring = this.Owner.FindChild("Ring");

            this.ray = this.Owner.FindChild("Ray");
            this.rayTransform = this.ray?.FindComponent<Transform3D>();
            this.lineRenderer = new GPULineRenderer()
            {
                WorldSpace = true,
            };
            this.ray.AddComponent(this.lineRenderer);


            this.menu = this.Owner.FindChild("Menu");
            this.menuTransform = this.menu.FindComponent<Transform3D>();
            this.initialMenuPosition = this.menuTransform.LocalPosition;

            this.dot = this.Owner.Find("[this].Touchpad.Dot");
            this.dotTransform = this.dot?.FindComponent<Transform3D>();

            this.thumbstick = this.Owner.FindChild("Thumbstick");
            this.thumbstickTransform = this.thumbstick?.FindComponent<Transform3D>();
            this.initialThumbStickRotation = this.thumbstickTransform.LocalRotation;

            this.grab = this.Owner.FindChild("Grab");
            this.grabTransform = this.grab?.FindComponent<Transform3D>();
            this.initialGrabRotation = this.grabTransform.LocalRotation;

            this.trigger = this.Owner.FindChild("Trigger");
            this.triggerTransform = this.trigger?.FindComponent<Transform3D>();

            this.gaze = this.ray.FindChild("Gaze");
            this.gazeTransform = this.gaze?.FindComponent<Transform3D>();

            var centerEyeAnchorEntity = this.EntityManager.Find("CameraRig.TrackingSpace.CenterEyeAnchor");
            this.cameraTransform = centerEyeAnchorEntity.FindComponent<Transform3D>();
        }

        /// <summary>
        /// Initialize method
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            // Points    
            var pointInfo1 = new LineVertexInfo()
            {
                Position = Vector3.Zero,
                Size = -this.ScaledRayThin,
                Color = Color.White,
            };

            var pointInfo2 = new LineVertexInfo()
            {
                Position = Vector3.UnitZ * 15,
                Size = -this.ScaledRayThin,
                Color = Color.White,
            };
            this.rayPoints = new List<LineVertexInfo>();
            this.rayPoints.Add(pointInfo1);
            this.rayPoints.Add(pointInfo2);
            this.lineRenderer.SetLineVertices(this.rayPoints);

            // Material
            if (!WaveServices.Platform.IsEditor)
            {
                this.lineMaterial = new LineMaterial(DefaultLayers.Additive)
                {
                    TexturePath = WaveContent.Assets.MotionControllers.Textures.RayMat2_png,
                    //Texture = tex,
                    //Bias = -0.0005f,                
                };
                this.lineMaterial.Initialize(this.Assets);

                this.lineRenderer.LineMaterial = this.lineMaterial;
            }
        }

        /// <summary>
        /// Update method
        /// </summary>
        /// <param name="gameTime">game time</param>
        protected override void Update(TimeSpan gameTime)
        {
            this.UpdateState();

            this.UpdateGaze();

            this.AnimateController();
        }

        protected abstract void UpdateState();
        private void UpdateGaze()
        {
            Vector3 rayPosition = this.rayTransform.Position;
            Ray ray;
            ray.Position = rayPosition;
            ray.Direction = -this.rayTransform.WorldTransform.Forward;

            Vector3 gazePosition;
            Vector3 gazeNormal;

            if (this.ray.IsActive)
            {
                this.CalculateGazeProperties(ref ray, out gazePosition, out gazeNormal);
            }
            else
            {
                gazePosition = rayPosition;
                gazeNormal = -ray.Direction;
            }

            LineVertexInfo startRay = this.rayPoints[0];
            startRay.Position = ray.Position;
            this.rayPoints[0] = startRay;

            LineVertexInfo endRay = this.rayPoints[1];
            endRay.Position = gazePosition;
            this.rayPoints[1] = endRay;

            for (int i = 0; i < this.rayPoints.Count; i++)
            {
                LineVertexInfo vertex = this.rayPoints[i];
                vertex.Size = this.ScaledRayThin;
                this.rayPoints[i] = vertex;
            }

            float distance;
            Vector3 cameraPosition = this.cameraTransform.Position;
            Vector3.Distance(ref cameraPosition, ref gazePosition, out distance);
            this.gazeTransform.Position = gazePosition;
            this.gazeTransform.LocalScale = Vector3.One * (distance / 2.5f);

            Vector3 gazeNormalPosition = gazePosition + gazeNormal;
            this.gazeTransform.LookAt(gazeNormalPosition, Vector3.Up);

            this.lineRenderer.SetLineVertices(this.rayPoints);
        }

        protected abstract void CalculateGazeProperties(ref Ray ray, out Vector3 gazePosition, out Vector3 gazeNormalPosition);
        
        private void AnimateController()
        {
            // Trigger
            if (this.lastTriggerValue != this.TriggerValue)
            {
                var triggerRotation = this.triggerTransform.LocalRotation;
                triggerRotation.X = MathHelper.ToRadians(TriggerValue * 14);
                this.triggerTransform.LocalRotation = triggerRotation;

                float raySize = -this.ScaledRayThin - (0.03f * TriggerValue);
                for (int i = 0; i < this.rayPoints.Count; i++)
                {
                    LineVertexInfo vertex = this.rayPoints[i];
                    vertex.Size = raySize;
                    this.rayPoints[i] = vertex;
                }
                this.lineRenderer.SetLineVertices(this.rayPoints);

                this.lastTriggerValue = this.TriggerValue;
            }

            // Thumbstick
            //X->Z -15+15 , Y->X 15
            var thumbstickRotation = initialThumbStickRotation;
            thumbstickRotation.Z += MathHelper.ToRadians(this.ThumbstickValue.X * 15);
            thumbstickRotation.X += MathHelper.ToRadians(this.ThumbstickValue.Y * 15);
            this.thumbstickTransform.LocalRotation = thumbstickRotation;

            // Grab
            var grabRotation = this.grabTransform.LocalRotation;
            if (Type == ControllerType.Right)
            {
                grabRotation.Z = this.GrabValue ? MathHelper.ToRadians(8) : this.initialGrabRotation.Z;
            }
            else
            {
                grabRotation.Z = this.GrabValue ? MathHelper.ToRadians(-8) : this.initialGrabRotation.Z;
            }
            this.grabTransform.LocalRotation = grabRotation;

            // Menu
            //var menuPosition = this.menuTransform.LocalPosition;
            //menuPosition.Y = this.MenuValue ? this.initialMenuPosition.Y - 0.03f : initialMenuPosition.Y;
            //this.menuTransform.LocalPosition = menuPosition;

            // Touchpad           
            this.dot.IsVisible = (TouchpadValue == Vector2.Zero) ? false : true;
            var touchpadPosition = this.dotTransform.LocalPosition;
            float radius = this.Type == ControllerType.Right ? 0.42f : 0.042f;
            touchpadPosition.X = -radius * this.TouchpadValue.X;
            touchpadPosition.Z = radius * this.TouchpadValue.Y;
            this.dotTransform.LocalPosition = touchpadPosition;
        }
    }
}
