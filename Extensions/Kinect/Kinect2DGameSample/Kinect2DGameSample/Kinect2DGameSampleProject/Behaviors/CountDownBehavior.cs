#region File Description
//-----------------------------------------------------------------------------
// CountDownBehavior
//
// Copyright © 2015 Wave Engine S.L. All rights reserved.
// Use is subject to license terms.
//-----------------------------------------------------------------------------
#endregion

using System;
using WaveEngine.Framework;
using Kinect2DGameSampleProject.Entities;

namespace Kinect2DGameSampleProject.Behaviors
{
    /// <summary>
    /// CountDown Behavior
    /// </summary>
    public class CountDownBehavior : Behavior
    {
        /// <summary>
        /// Gets or sets the duration.
        /// </summary>
        /// <value>
        /// The duration.
        /// </value>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Gets the current time.
        /// </summary>
        /// <value>
        /// The current time.
        /// </value>
        public TimeSpan CurrentTime { get; internal set; }

        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        /// <value>
        /// The parent.
        /// </value>
        public TimeCounter Parent { get; set; }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            this.CurrentTime = this.Duration;
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
            var update = false;

            if (this.CurrentTime > TimeSpan.Zero)
            {
                this.CurrentTime -= gameTime;
                update = true;
            }

            if (this.CurrentTime < TimeSpan.Zero)
            {
                this.CurrentTime = TimeSpan.Zero;
                this.Parent.Finished = true;
                update = true;
            }

            // Updates the time only if needs
            if (update)
            {
                this.Parent.Textblock.Text = this.CurrentTime.ToString(@"mm'.'ss'.'ff");
            }
        }
    }
}
