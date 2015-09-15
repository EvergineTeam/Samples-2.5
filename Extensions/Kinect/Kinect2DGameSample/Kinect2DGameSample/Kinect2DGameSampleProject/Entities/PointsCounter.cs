#region File Description
//-----------------------------------------------------------------------------
// PointsCounter
//
// Copyright © 2015 Wave Engine S.L. All rights reserved.
// Use is subject to license terms.
//-----------------------------------------------------------------------------
#endregion

using WaveEngine.Framework;
using Kinect2DGameSampleProject.Behaviors;
using WaveEngine.Common.Graphics;
using WaveEngine.Components.UI;
using WaveEngine.Framework.UI;

namespace Kinect2DGameSampleProject.Entities
{
    /// <summary>
    /// Points Counter Base Decorator
    /// </summary>
    public class PointsCounter : BaseDecorator
    {
        /// <summary>
        /// The textblock
        /// </summary>
        public TextBlock Textblock { get; set; }

        /// <summary>
        /// Gets the point counter behavior.
        /// </summary>
        /// <value>
        /// The point counter behavior.
        /// </value>
        public PointCounterBehavior PointCounterBehavior
        {
            get
            {
                return this.entity.FindComponent<PointCounterBehavior>();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PointsCounter"/> class.
        /// </summary>
        public PointsCounter()
            : base()
        {
            this.Textblock = new TextBlock()
            {
                Width = 400,
                BorderColor = Color.Black,
                Height = 80,
                HorizontalAlignment = HorizontalAlignment.Left,
                TextAlignment = TextAlignment.Right,
                FontPath = "Content/PFDarkBigFont",
                Text = "0",
                Foreground = new Color(255, 0, 0, 128),
            };

            this.entity = this.Textblock.Entity
                .AddComponent(new PointCounterBehavior() { Points = 0, Parent = this });
        }
    }
}
