#region File Description
//-----------------------------------------------------------------------------
// TimeCounter
//
// Copyright © 2015 Wave Engine S.L. All rights reserved.
// Use is subject to license terms.
//-----------------------------------------------------------------------------
#endregion

using WaveEngine.Framework;
using System;
using Kinect2DGameSampleProject.Behaviors;
using WaveEngine.Components.UI;
using WaveEngine.Common.Graphics;
using WaveEngine.Framework.UI;

namespace Kinect2DGameSampleProject.Entities
{
    /// <summary>
    /// Timer Counter
    /// </summary>
    public class TimeCounter : BaseDecorator
    {
        /// <summary>
        /// The textblock to draw the Time
        /// </summary>
        public TextBlock Textblock { get; set; }

        /// <summary>
        /// Gets the count down behavior.
        /// </summary>
        /// <value>
        /// The count down behavior.
        /// </value>
        public CountDownBehavior CountDownBehavior
        {
            get
            {
                return this.entity.FindComponent<CountDownBehavior>();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="TimeCounter"/> is finished.
        /// </summary>
        /// <value>
        ///   <c>true</c> if finished; otherwise, <c>false</c>.
        /// </value>
        public bool Finished { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeCounter"/> class.
        /// </summary>
        public TimeCounter()
            : base()
        {
            this.Textblock = new TextBlock()
                                 {
                                     Width = 400,
                                     HorizontalAlignment = HorizontalAlignment.Right,
                                     TextAlignment = TextAlignment.Left,
                                     FontPath = "Content/PFDarkBigFont",
                                     Text = "Catch this to START",
                                     Foreground = Color.Red,
                                 };
            this.entity = this.Textblock.Entity
                .AddComponent(new CountDownBehavior() { Duration = TimeSpan.FromMinutes(1.0f), Parent = this });
        }
    }
}
