#region File Description
//-----------------------------------------------------------------------------
// FallingBody
//
// Copyright © 2015 Wave Engine S.L. All rights reserved.
// Use is subject to license terms.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using WaveEngine.Framework;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Framework.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Framework.Physics2D;
using Kinect2DGameSampleProject.Behaviors;
using Kinect2DGameSampleProject.Components;
#endregion

namespace Kinect2DGameSampleProject.Entities
{
    /// <summary>
    /// Falling body class
    /// </summary>
    public class FallingBody : BaseDecorator
    {
        /// <summary>
        /// The stand position. Position create the bodies (out of the screen)
        /// </summary>
        protected Vector2 standPosition = new Vector2(0, -100);

        /// <summary>
        /// The assets collection. Bidi array of assets randomly selected.
        /// </summary>
        private readonly string[][] assets = new string[][]
                                        {
                                            new string[] { "Content/Fruit/apple.wpk", "Content/Fruit/lemon.wpk", "Content/Fruit/orange.wpk", "Content/Fruit/strawberry.wpk"},
                                            new string[] { "Content/Robot/robot1.wpk", "Content/Robot/robot2.wpk", "Content/Robot/robot3.wpk", "Content/Robot/robot4.wpk", "Content/Robot/robot5.wpk", "Content/Robot/robot6.wpk" }
                                        };

        /// <summary>
        /// Gets the transform 2D.
        /// </summary>
        /// <value>
        /// The transform.
        /// </value>
        public Transform2D Transform
        {
            get
            {
                return this.entity.FindComponent<Transform2D>();
            }
        }

        /// <summary>
        /// Gets the physic rigid body.
        /// </summary>
        /// <value>
        /// The rigid body.
        /// </value>
        public RigidBody2D RigidBody
        {
            get
            {
                return this.entity.FindComponent<RigidBody2D>();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is in use.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is in use; otherwise, <c>false</c>.
        /// </value>
        public bool IsInUse
        {
            get
            {
                return this.entity.FindComponent<FallingBodyController>().IsInUse;
            }

            set
            {
                this.entity.FindComponent<FallingBodyController>().IsInUse = value;
            }
        }

        /// <summary>
        /// Gets the current energy.
        /// </summary>
        /// <value>
        /// The energy.
        /// </value>
        public float Energy 
        {
            get
            {
                return this.entity.FindComponent<FallingBodyController>().Energy;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FallingBody" /> class.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="assetCollection">The asset collection index.</param>
        /// <param name="assetIndex">Index of the asset in the collection.</param>
        public FallingBody(int x, int assetCollection, int assetIndex)
            : base()
        {
            this.standPosition.X = x;

            this.entity = new Entity()
                .AddComponent(new Transform2D() { Origin = Vector2.Center, Position = this.standPosition })
                .AddComponent(new Sprite(this.assets[assetCollection][assetIndex % this.assets.Length]))
                .AddComponent(new CircleCollider())
                //// This Physic bodies collides ONLY with Category2 (pads)
                .AddComponent(new RigidBody2D() { CollisionCategories = Physic2DCategory.Cat1, CollidesWith = Physic2DCategory.Cat2, Restitution = 1.0f })
                .AddComponent(new FallingBodyController() { IsInUse = true })
                .AddComponent(new FallingBehavior() { UpdateOrder = 0 })
                .AddComponent(new SpriteRenderer(DefaultLayers.Alpha));
        }

        /// <summary>
        /// Damages the specified amount.
        /// </summary>
        /// <param name="damage">The damage.</param>
        public void Damage(float damage)
        {
            this.entity.FindComponent<FallingBodyController>().Damage(damage);
        }

        /// <summary>
        /// Falls the specified x position.
        /// </summary>
        /// <param name="x">The x.</param>
        public void Fall(int x)
        {
            this.entity.FindComponent<FallingBodyController>().Fall(x);
        }
    }
}
