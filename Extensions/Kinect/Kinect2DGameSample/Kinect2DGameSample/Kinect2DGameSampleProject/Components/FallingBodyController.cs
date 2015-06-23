#region File Description
//-----------------------------------------------------------------------------
// FallingBodyController
//
// Copyright © 2015 Wave Engine S.L. All rights reserved.
// Use is subject to license terms.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Physics2D;
#endregion

namespace Kinect2DGameSampleProject.Components
{
    using Kinect2DGameSampleProject.Scenes;

    using WaveEngine.Common.Graphics;
    using WaveEngine.Components.Graphics2D;
    using WaveEngine.Framework.Graphics;

    /// <summary>
    /// Falling Body Controller
    /// </summary>
    public class FallingBodyController : Component
    {
        /// <summary>
        /// The rigid body
        /// </summary>
        [RequiredComponent]
        private RigidBody2D rigidBody;

        /// <summary>
        /// The transform 2D
        /// </summary>
        [RequiredComponent]
        private Transform2D transform2D;

        /// <summary>
        /// The sprite
        /// </summary>
        [RequiredComponent]
        private Sprite sprite;

        /// <summary>
        /// Gets the current energy.
        /// </summary>
        /// <value>
        /// The energy.
        /// </value>
        public float Energy { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether this instance is in use.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is in use; otherwise, <c>false</c>.
        /// </value>
        public bool IsInUse { get; internal set; }

        /// <summary>
        /// The current tint color
        /// </summary>
        private Color currentTintColor = new Color(1.0f);

        /// <summary>
        /// Falls the specified x position.
        /// </summary>
        /// <param name="x">The x.</param>
        public void Fall(int x)
        {
            this.IsInUse = true;

            // Reset visualization
            this.Energy = 1.0f;
            this.Damage(0);

            this.transform2D.LocalXScale = 1.0f;
            this.transform2D.LocalYScale = 1.0f;

            var newPosition = new Vector2(x, -200);
            this.transform2D.Position = newPosition;
            this.rigidBody.ResetPosition(newPosition);
        }

        /// <summary>
        /// Kills this instance.
        /// </summary>
        /// <param name="explode">if set to <c>true</c> [explode].</param>
        public void Kill(bool explode)
        {
            this.IsInUse = false;

            if (explode)
            {
                var scene = this.Owner.Scene as GameScene;
                if (scene != null)
                {
                    scene.ExplodeIn(this.transform2D.Position);
                }
            }
        }

        /// <summary>
        /// Damages the specified damage.
        /// </summary>
        /// <param name="damage">The damage.</param>
        public void Damage(float damage)
        {
            if (!this.IsInUse)
            {
                return;
            }

            this.Energy -= damage;
            if (this.Energy < 0)
            {
                this.Energy = 0;
                this.Kill(true);
            }
            else
            {
                // this.currentTintColor.R = (byte)(byte.MaxValue * this.Energy);
                this.currentTintColor.G = (byte)(byte.MaxValue * this.Energy);
                this.currentTintColor.B = (byte)(byte.MaxValue * this.Energy);

                this.sprite.TintColor = this.currentTintColor;

                this.transform2D.XScale = 1 + ((1 - this.Energy) / 3.0f);
                this.transform2D.YScale = 1 + ((1 - this.Energy) / 3.0f);
            }
        }
    }
}
