using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveEngine.Common.Graphics;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveOculusDemoProject.Components;
using WaveEngine.Framework.Services;
using WaveEngine.Common.Math;

namespace WaveOculusDemoProject.Entities
{
    /// <summary>
    /// The stereoscopic camera entity decorator
    /// </summary>
    public class StereoscopicCameraDecorator : BaseDecorator
    {
        public Transform3D Transform3D
        {
            get
            {
                return this.entity.FindComponent<Transform3D>();
            }
        }

        public float FarPlane
        {
            get
            {
                return this.entity.FindComponent<StereoscopicCameraController>().FarPlane;
            }
            set
            {
                this.entity.FindComponent<StereoscopicCameraController>().FarPlane = value;
            }
        }

        public float FieldOfView
        {
            get
            {
                return this.entity.FindComponent<StereoscopicCameraController>().FieldOfView;
            }
            set
            {
                this.entity.FindComponent<StereoscopicCameraController>().FieldOfView = value;
            }
        }

        /// <summary>
        /// Instantiates the stereoscopic camera decorator
        /// </summary>
        /// <param name="name">The entity name</param>
        public StereoscopicCameraDecorator(string name)
        {
            this.entity = new Entity(name)
            .AddComponent(new Transform3D())
            .AddComponent(new StereoscopicCameraController())
            ;

            Entity leftLookAtEntity = new Entity("leftLookAt")
            .AddComponent(new Transform3D())
            ;

            Entity rightLookAtEntity = new Entity("rightLookAt")
            .AddComponent(new Transform3D())
            ;

            Entity leftEye = new Entity("leftEye")
            .AddComponent(new Transform3D())
            .AddComponent(new Camera3D() { ClearFlags = ClearFlags.DepthAndStencil })
            .AddComponent(new Skybox("Content/Textures/Skybox.cubemap"))
            ;

            Entity rightEye = new Entity("rightEye")
            .AddComponent(new Transform3D())
            .AddComponent(new Camera3D() { ClearFlags = ClearFlags.DepthAndStencil })
            .AddComponent(new Skybox("Content/Textures/Skybox.cubemap"))
            ;

            this.entity.AddChild(rightEye);
            this.entity.AddChild(leftEye);
            this.entity.AddChild(leftLookAtEntity);
            this.entity.AddChild(rightLookAtEntity);
        }

        /// <summary>
        /// Set camera rendertargtet and projection
        /// </summary>
        /// <param name="renderTarget">Camera rendertarget</param>
        /// <param name="leftEyeProjection">Left eye custom projection</param>
        /// <param name="rightEyeProjection">Right eye custom projection</param>
        public void SetRenderTarget(RenderTarget renderTarget, Matrix leftEyeProjection, Matrix rightEyeProjection)
        {
            var leftCamera = this.entity.FindChild("leftEye").FindComponent<Camera3D>();
            leftCamera.SetCustomProjection(leftEyeProjection);
            leftCamera.RenderTarget = renderTarget;

            var rightCamera = this.entity.FindChild("rightEye").FindComponent<Camera3D>();
            rightCamera.SetCustomProjection(rightEyeProjection);
            rightCamera.RenderTarget = renderTarget;
        }
    }
}
