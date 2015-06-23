#region File Description
//-----------------------------------------------------------------------------
// BlinkBehavior
//
// Copyright © 2015 Wave Engine S.L. All rights reserved.
// Use is subject to license terms.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using WaveEngine.Framework;
using WaveEngine.Framework.Animation;
using WaveEngine.Framework.Graphics;
#endregion

namespace Kinect2DGameSampleProject.Behaviors
{
    /// <summary>
    /// Opacity Blink Behavior
    /// </summary>
    public class BlinkBehavior : Behavior
    {
        /// <summary>
        /// The transform
        /// </summary>
        [RequiredComponent]
        private Transform2D transform = null;

        /// <summary>
        /// The timer
        /// </summary>
        private TimeSpan timer;

        /// <summary>
        /// The is fade in
        /// </summary>
        private bool isFadeIn;

        /// <summary>
        /// Gets or sets the duration.
        /// </summary>
        /// <value>
        /// The duration.
        /// </value>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Gets or sets the easing function.
        /// </summary>
        /// <value>
        /// The easing function.
        /// </value>
        public IEasingFunction EasingFunction { get; set; }

        /// <summary>
        /// Gets or sets the maximum opacity.
        /// </summary>
        /// <value>
        /// The maximum opacity.
        /// </value>
        public float MaxOpacity { get; set; }

        /// <summary>
        /// Gets or sets the minimum opacity.
        /// </summary>
        /// <value>
        /// The minimum opacity.
        /// </value>
        public float MinOpacity { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlinkBehavior" /> class.
        /// </summary>
        /// <param name="duration">The duration.</param>
        public BlinkBehavior(TimeSpan duration)
        {
            this.Duration = duration;
            this.MaxOpacity = 1;
            this.MinOpacity = 0;
        }

        /// <summary>
        /// Performs further custom initialization for this instance.
        /// </summary>
        /// <remarks>
        /// By default this method does nothing.
        /// </remarks>
        protected override void Initialize()
        {
            base.Initialize();
            this.timer = TimeSpan.Zero;
            this.isFadeIn = true;
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
            // Creates a Ping-pong opacity effect between maxopacity and minopacity
            if (this.timer >= this.Duration)
            {
                this.timer = TimeSpan.Zero;
                this.isFadeIn = !this.isFadeIn;
            }

            float elapsedTime = (float)(this.timer.TotalSeconds / this.Duration.TotalSeconds);

            if (this.EasingFunction != null)
            {
                elapsedTime = (float)this.EasingFunction.Ease(elapsedTime);
            }

            if (this.isFadeIn)
            {
                this.transform.Opacity = this.MinOpacity + (this.MaxOpacity - this.MinOpacity) * elapsedTime;
            }
            else
            {
                this.transform.Opacity = this.MinOpacity + (this.MaxOpacity - this.MinOpacity) * (1 - elapsedTime);
            }

            this.timer += gameTime;
        }
    }
}
