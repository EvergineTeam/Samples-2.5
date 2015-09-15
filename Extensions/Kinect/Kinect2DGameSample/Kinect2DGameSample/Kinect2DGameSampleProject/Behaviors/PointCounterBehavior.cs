#region File Description
//-----------------------------------------------------------------------------
// PointCounterBehavior
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
    /// Point Counter Behavior
    /// </summary>
    public class PointCounterBehavior : Behavior
    {
        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        /// <value>
        /// The parent.
        /// </value>
        public PointsCounter Parent { get; set; }

        /// <summary>
        /// The desired points. Used to smooth counter
        /// </summary>
        private int desiredPoints;

        /// <summary>
        /// Gets the current points.
        /// </summary>
        /// <value>
        /// The points.
        /// </value>
        public int Points { get; internal set; }

        /// <summary>
        /// Hit! Point counts.
        /// </summary>
        /// <param name="points">The points.</param>
        public void HitPoint(int points)
        {
            this.desiredPoints += points;
        }

        /// <summary>
        /// Resets the points.
        /// </summary>
        public void ResetPoints()
        {
            this.desiredPoints = 0;
            this.Points = 0;
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
            var updateTextbox = false;

            // Move Points property to desiredPoints value.
            if (this.desiredPoints != this.Points)
            {
                if (this.desiredPoints > this.Points)
                {
                    this.Points++;
                    updateTextbox = true;
                }
                else
                {
                    this.Points--;
                    updateTextbox = true;
                }
            }

            // only updates if needs
            if (updateTextbox)
            {
                this.Parent.Textblock.Text = this.Points.ToString();
            }
        }
    }
}
