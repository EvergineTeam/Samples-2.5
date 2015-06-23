#region File Description
//-----------------------------------------------------------------------------
// StartSceneBehavior
//
// Copyright © 2015 Wave Engine S.L. All rights reserved.
// Use is subject to license terms.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;
using WaveEngine.Kinect;
using Kinect2DGameSampleProject.Scenes;
using Microsoft.Kinect;
using WaveEngine.Common.Math;
using WaveEngine.Framework.Graphics;
#endregion

namespace Kinect2DGameSampleProject.Behaviors
{
    /// <summary>
    /// Start Scene Behavior
    /// </summary>
    public class StartSceneBehavior : SceneBehavior
    {
        /// <summary>
        /// The kinect service
        /// </summary>
        private KinectService kinectService;

        /// <summary>
        /// Resolves the dependencies needed for this instance to work.
        /// </summary>
        protected override void ResolveDependencies()
        {
            this.kinectService = WaveServices.GetService<KinectService>();
        }

        /// <summary>
        /// Allows this instance to execute custom logic during its
        /// <c>Update</c>.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        /// <remarks>
        /// This method will not be executed if it are not
        /// <c>Active</c>.
        /// </remarks>
        protected override void Update(TimeSpan gameTime)
        {
            if (this.kinectService.Bodies == null)
            {
                return;
            }

            for (var i = 0; i < this.kinectService.Bodies.Length; i++)
            {
                // Gets the body
                var body = this.kinectService.Bodies[i];
                if (body.IsTracked)
                {
                    var leftHandJoint = body.Joints[JointType.HandLeft];
                    if (leftHandJoint.TrackingState == TrackingState.Tracked
                        && body.HandLeftConfidence == TrackingConfidence.High
                        && body.HandLeftState == HandState.Closed)
                    {
                        // Converts the Kinect space position to screen position
                        var screenPosition = this.kinectService.Mapper.MapCameraPointToColorSpace(leftHandJoint.Position);

                        var startEntity = this.Scene.EntityManager.Find("StartEntity");
                        var startEntityTransform = startEntity.FindComponent<Transform2D>();

                        var vector2Position = Vector2.Zero;
                        vector2Position.X = screenPosition.X;
                        vector2Position.Y = screenPosition.Y;

                        // Checks if player has "catch" the message with left hand
                        if (startEntityTransform.Rectangle.Contains(vector2Position))
                        {
                            // Transition to Game Scene
                            WaveServices.ScreenContextManager.Pop();
                            WaveServices.ScreenContextManager.Push(new ScreenContext(new GameScene()));
                            this.IsActive = false;
                        }
                    }
                }
            }
        }
    }
}
