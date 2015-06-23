#region File Description
//-----------------------------------------------------------------------------
// PadBehavior
//
// Copyright © 2015 Wave Engine S.L. All rights reserved.
// Use is subject to license terms.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using WaveEngine.Framework;
using Microsoft.Kinect;
using WaveEngine.Common.Math;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics2D;
using WaveEngine.Framework.Services;
using WaveEngine.Kinect;
#endregion

namespace Kinect2DGameSampleProject.Behaviors
{
    /// <summary>
    /// Pad Behavior
    /// </summary>
    public class PadBehavior : Behavior
    {
        /// <summary>
        /// The last tracked value
        /// </summary>
        private bool lastTrackedValue;

        /// <summary>
        /// The out position
        /// </summary>
        private Vector2 outPosition;

        /// <summary>
        /// The transform
        /// </summary>
        [RequiredComponent]
        private Transform2D transform;

        /// <summary>
        /// The rigid body
        /// </summary>
        [RequiredComponent]
        private RigidBody2D rigidBody;

        /// <summary>
        /// Gets or sets the kinect body.
        /// </summary>
        /// <value>
        /// The kinect body.
        /// </value>
        public Body KinectBody { get; set; }

        /// <summary>
        /// Gets or sets the joint.
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
        /// The rotation
        /// </summary>
        private float rotation = 0;

        /// <summary>
        /// The kinect service
        /// </summary>
        private KinectService kinectService;

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
            this.outPosition = new Vector2(-500, -500);
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
            if (this.KinectBody == null)
            {
                return;
            }

            if (this.KinectBody.IsTracked)
            {
                var joint = this.KinectBody.Joints[this.Joint];
                if (joint.TrackingState != TrackingState.NotTracked)
                {
                    // It's need to update position AND angle
                    // Calculates 2D euler angles from Joint Quaternions
                    var kinectQuaternion = this.KinectBody.JointOrientations[this.Joint].Orientation;

                    var kinectPosition = this.kinectService.Mapper.MapCameraPointToColorSpace(joint.Position);

                    var q = new Quaternion(kinectQuaternion.X, kinectQuaternion.Y, kinectQuaternion.Z, kinectQuaternion.W);
                    Vector3 eulerAngles;
                    Quaternion.ToEuler(ref q, out eulerAngles);

                    this.position.X = kinectPosition.X;
                    this.position.Y = kinectPosition.Y;

                    if (eulerAngles.Y < 0)
                    {
                        this.rotation = eulerAngles.X;
                    }
                    else
                    {
                        this.rotation = -eulerAngles.X;
                    }

                    // If player is new, move to new position.
                    if (this.lastTrackedValue == false)
                    {
                        this.rigidBody.ResetPosition(this.position);
                    }
                    else
                    {
                        var springJoint = (FixedMouseJoint2D)this.Owner.FindComponent<JointMap2D>().Joints["spring"];
                        springJoint.WorldAnchor = this.position;
                    }

                    this.rigidBody.Rotation = -this.rotation;
                }
            }
            else
            {
                this.rigidBody.ResetPosition(this.outPosition);
                var springJoint = (FixedMouseJoint2D)this.Owner.FindComponent<JointMap2D>().Joints["spring"];

                // Updates the pad position (moves the phsyic rigidBody by the SpringJoint)
                springJoint.WorldAnchor = this.outPosition;
            }

            this.lastTrackedValue = this.KinectBody.IsTracked;
        }
    }
}
