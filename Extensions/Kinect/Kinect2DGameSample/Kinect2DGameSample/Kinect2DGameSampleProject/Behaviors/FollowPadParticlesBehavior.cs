#region File Description
//-----------------------------------------------------------------------------
// FollowPadParticlesBehavior
//
// Copyright © 2015 Wave Engine S.L. All rights reserved.
// Use is subject to license terms.
//-----------------------------------------------------------------------------
#endregion

using System;
using Microsoft.Kinect;
using WaveEngine.Common.Math;
using WaveEngine.Components.Particles;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
using WaveEngine.Kinect;
using WaveEngine.Framework;

namespace Kinect2DGameSampleProject.Behaviors
{
    /// <summary>
    /// Follow Pad Particles Behavior
    /// </summary>
    public class FollowPadParticlesBehavior : Behavior
    {
        /// <summary>
        /// The particle system
        /// </summary>
        [RequiredComponent]
        private ParticleSystem2D particleSystem;

        /// <summary>
        /// The transform
        /// </summary>
        [RequiredComponent]
        private Transform2D transform;

        /// <summary>
        /// The kinect service
        /// </summary>
        private KinectService kinectService;

        /// <summary>
        /// Gets or sets the kinect body.
        /// </summary>
        /// <value>
        /// The kinect body.
        /// </value>
        public Body KinectBody { get; set; }

        /// <summary>
        /// Gets or sets the joint to follow.
        /// </summary>
        /// <value>
        /// The joint.
        /// </value>
        public JointType Joint { get; set; }

        /// <summary>
        /// The position
        /// </summary>
        private Vector2 position = Vector2.Zero;

        /// <summary>
        /// Performs further custom initialization for this instance.
        /// </summary>
        /// <remarks>
        /// By default this method does nothing.
        /// </remarks>
        protected override void Initialize()
        {
            base.Initialize();

            this.kinectService = WaveServices.GetService<KinectService>();
        }

        /// <summary>
        /// Allows this instance to execute custom logic during its 
        /// <c>Update</c>.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        /// <remarks>
        /// This method will not be executed if the 
        /// <see cref="T:WaveEngine.Framework.Component" />, or the 
        /// <see cref="T:WaveEngine.Framework.Entity" />
        ///             owning it are not 
        /// <c>Active</c>.
        /// </remarks>
        protected override void Update(TimeSpan gameTime)
        {
            if (this.KinectBody != null)
            {
                if (this.KinectBody.IsTracked)
                {
                    // Updates the pad particles to joint position
                    var joint = this.KinectBody.Joints[this.Joint];
                    var kinectPosition = this.kinectService.Mapper.MapCameraPointToColorSpace(joint.Position);
                    this.position.X = kinectPosition.X;
                    this.position.Y = kinectPosition.Y;
                    this.transform.Position = this.position;

                    // It's only tracked HandRight and HandLeft.
                    switch (this.Joint)
                    {
                        case JointType.HandRight:
                            this.particleSystem.Emit = this.KinectBody.HandRightState != HandState.Closed;
                            break;
                        case JointType.HandLeft:
                            this.particleSystem.Emit = this.KinectBody.HandLeftState != HandState.Closed;
                            break;
                    }
                }
                else
                {
                    this.particleSystem.Emit = false;
                }
            }
            else
            {
                this.particleSystem.Emit = false;
            }
        }
    }
}
