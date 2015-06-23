#region File Description
//-----------------------------------------------------------------------------
// Player
//
// Copyright © 2015 Wave Engine S.L. All rights reserved.
// Use is subject to license terms.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Kinect2DGameSampleProject.Components;
using Microsoft.Kinect;
using WaveEngine.Common.Graphics;
using WaveEngine.Framework;
using Kinect2DGameSampleProject.Behaviors;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Framework.Graphics;
using WaveEngine.Materials;
#endregion

namespace Kinect2DGameSampleProject.Entities
{
    /// <summary>
    /// Player Element
    /// </summary>
    public class Player
    {
        /// <summary>
        /// The player Kinect Body
        /// </summary>
        private Body playerBody;

        /// <summary>
        /// The left pad (hand)
        /// </summary>
        private PadBody leftPad;

        /// <summary>
        /// The right pad (hand)
        /// </summary>
        private PadBody rightPad;

        /// <summary>
        /// The left particle system
        /// </summary>
        private Entity leftParticleEntity;

        /// <summary>
        /// The right particle system
        /// </summary>
        private Entity rightParticleEntity;

        /// <summary>
        /// The current scene
        /// </summary>
        private Scene scene;

        /// <summary>
        /// Gets or sets the player body.
        /// </summary>
        /// <value>
        /// The player body.
        /// </value>
        public Body PlayerBody
        {
            get
            {
                return this.playerBody;
            }

            set
            {
                // Configures the Player using the Kinect Body
                this.playerBody = value;
                this.leftPad.PadBehavior.KinectBody = this.playerBody;
                this.rightPad.PadBehavior.KinectBody = this.playerBody;

                var leftParticles = this.leftParticleEntity.FindComponent<FollowPadParticlesBehavior>();
                leftParticles.KinectBody = this.playerBody;
                leftParticles.Joint = JointType.HandLeft;

                var rightParticles = this.rightParticleEntity.FindComponent<FollowPadParticlesBehavior>();
                rightParticles.KinectBody = this.playerBody;
                rightParticles.Joint = JointType.HandRight;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Player" /> class.
        /// </summary>
        /// <param name="playerScene">The player scene.</param>
        public Player(Scene playerScene)
        {
            this.scene = playerScene;

            // Left Pad
            this.leftPad = new PadBody(Color.Transparent);
            this.leftPad.PadBehavior.Joint = JointType.HandLeft;
            this.scene.EntityManager.Add(this.leftPad);

            // Right Pad
            this.rightPad = new PadBody(Color.Transparent);
            this.rightPad.PadBehavior.Joint = JointType.HandRight;
            this.scene.EntityManager.Add(this.rightPad);

            // Burning effects. Not neccesary but cool!
            this.leftParticleEntity = new Entity()
                .AddComponent(new Transform2D())
                .AddComponent(ParticleSystemFactory.CreateFireParticle())
                .AddComponent(new FollowPadParticlesBehavior())
                .AddComponent(new Material2D(new BasicMaterial2D("Content/particleFire.wpk", DefaultLayers.Additive)))
                .AddComponent(new ParticleSystemRenderer2D("particleRenderer"));
            this.scene.EntityManager.Add(this.leftParticleEntity);

            this.rightParticleEntity = new Entity()
                .AddComponent(new Transform2D())
                .AddComponent(ParticleSystemFactory.CreateFireParticle())
                .AddComponent(new FollowPadParticlesBehavior())
                .AddComponent(new Material2D(new BasicMaterial2D("Content/particleFire.wpk", DefaultLayers.Additive)))
                .AddComponent(new ParticleSystemRenderer2D("particleRenderer"));
            this.scene.EntityManager.Add(this.rightParticleEntity);
        }

        /// <summary>
        /// Removes the player.
        /// </summary>
        private void RemovePlayer()
        {
            this.scene.EntityManager.Remove(this.leftPad);
            this.scene.EntityManager.Remove(this.rightPad);

            this.leftPad = null;
            this.rightPad = null;
        }
    }
}
