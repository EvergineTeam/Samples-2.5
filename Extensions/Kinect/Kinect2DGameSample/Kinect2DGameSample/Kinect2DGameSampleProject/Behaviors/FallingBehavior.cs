#region File Description
//-----------------------------------------------------------------------------
// FallingBehavior
//
// Copyright © 2015 Wave Engine S.L. All rights reserved.
// Use is subject to license terms.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
using Kinect2DGameSampleProject.Components;
using WaveEngine.Framework.Physics2D;
#endregion

namespace Kinect2DGameSampleProject.Behaviors
{
    /// <summary>
    /// Falling Behavior Class
    /// </summary>
    public class FallingBehavior : Behavior
    {
        /// <summary>
        /// The margin to remove the falling bodies. Creates the Out of Screen margins
        /// </summary>
        private const int MARGIN = 200;

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
        /// Falling Body Controller
        /// </summary>
        [RequiredComponent]
        private FallingBodyController fallingBodyController;

        /// <summary>
        /// Allows this instance to execute custom logic during its
        /// <c>Update</c>.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        /// <remarks>
        /// This method will not be executed if the
        /// <see cref="T:WaveEngine.Framework.Component" />, or the
        /// <see cref="T:WaveEngine.Framework.Entity" />
        /// owning it are not
        /// <c>Active</c>.
        /// </remarks>
        protected override void Update(TimeSpan gameTime)
        {
            if (!this.fallingBodyController.IsInUse)
            {
                return;
            }

            // Checks if position is out of screen or not valid transform position. Kill it then (no points, no particles this time).
            if (float.IsNaN(this.transform.X)
                || float.IsNaN(this.transform.Y)
                || this.transform.Y > WaveServices.ViewportManager.BottomEdge + MARGIN
                || this.transform.X < WaveServices.ViewportManager.LeftEdge - MARGIN
                || this.transform.X > WaveServices.ViewportManager.RightEdge + MARGIN)
            {
                this.fallingBodyController.Kill(false);
            }
        }
    }
}
