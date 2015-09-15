#region File Description
//-----------------------------------------------------------------------------
// PadController
//
// Copyright © 2015 Wave Engine S.L. All rights reserved.
// Use is subject to license terms.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using Kinect2DGameSampleProject.Behaviors;
using WaveEngine.Common.Math;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Framework.Physics2D;
using WaveEngine.Common.Graphics;
#endregion

namespace Kinect2DGameSampleProject.Components
{
    /// <summary>
    /// Pad Controller class.
    /// </summary>
    public class PadBody : BaseDecorator
    {
        /// <summary>
        /// Gets the pad behavior.
        /// </summary>
        /// <value>
        /// The pad behavior.
        /// </value>
        public PadBehavior PadBehavior
        {
            get
            {
                return this.entity.FindComponent<PadBehavior>();
            }
        }

        /// <summary>
        /// Gets the spring joint. Used to move the physic pad smoothly. Better than sets the position directly (tele-transportation causes non-Physic artifacts).
        /// </summary>
        /// <value>
        /// The spring joint.
        /// </value>
        public FixedMouseJoint2D SpringJoint { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PadController" /> class.
        /// </summary>
        /// <param name="padColor">Color of the pad.</param>
        public PadBody(Color padColor)
            : base()
        {
            this.entity = new Entity()
                .AddComponent(new Transform2D() { Origin = Vector2.Center, Scale = new Vector2(2, 1) })
                .AddComponent(new Sprite("Content/pad.wpk") { TintColor = padColor })
                .AddComponent(new RectangleCollider())
                .AddComponent(new PadBehavior())
                .AddComponent(new RigidBody2D() { CollisionCategories = Physic2DCategory.Cat2, CollidesWith = Physic2DCategory.Cat1, FixedRotation = true })
                .AddComponent(new JointMap2D())
                .AddComponent(new SpriteRenderer(DefaultLayers.Alpha));

            this.SpringJoint = new FixedMouseJoint2D();
            this.entity.FindComponent<JointMap2D>().AddJoint("spring", this.SpringJoint);

            // Controls the pad collisions
            this.entity.FindComponent<RigidBody2D>().OnPhysic2DCollision += this.OnOnPhysic2DCollision;
        }

        /// <summary>
        /// Called when [on physic2 d collision].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="Physic2DCollisionEventArgs" /> instance containing the event data.</param>
        private void OnOnPhysic2DCollision(object sender, Physic2DCollisionEventArgs args)
        {
            var controller = args.Body2DB.Owner.FindComponent<FallingBodyController>();
            if (controller == null)
            {
                return;
            }

            // Calculates the Damage by pad velocity
            var velocityMagnitude = args.Body2DB.LinearVelocity.Length();
            controller.Damage(velocityMagnitude / 100.0f);
        }
    }
}
