#region File Description
//-----------------------------------------------------------------------------
// GameScene
//
// Copyright © 2015 Wave Engine S.L. All rights reserved.
// Use is subject to license terms.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using WaveEngine.Framework;
using Kinect2DGameSampleProject.Entities;
using WaveEngine.Common.Graphics;
using WaveEngine.Components.Cameras;
using WaveEngine.Framework.Services;
using Kinect2DGameSampleProject.Behaviors;
using WaveEngine.Kinect;
using WaveEngine.Common.Math;
#endregion

namespace Kinect2DGameSampleProject.Scenes
{
    /// <summary>
    /// Game Scene
    /// </summary>
    public class GameScene : Scene
    {
        /// <summary>
        /// The kinect service
        /// </summary>
        private KinectService kinectService;

        /// <summary>
        /// The margin for the falling bodies
        /// </summary>
        private int margin = 200;

        /// <summary>
        /// The falling bodies
        /// </summary>
        private List<FallingBody> fallingBodies;

        /// <summary>
        /// The explosion emitter collection
        /// </summary>
        private ExplosionEmitter[] explosionEmitterCollection;

        /// <summary>
        /// The explosion emitter pool index
        /// </summary>
        private int explosionEmitterIndex;

        /// <summary>
        /// Gets the player collection.
        /// </summary>
        /// <value>
        /// The players.
        /// </value>
        public Player[] Players
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets or sets the time counter control.
        /// </summary>
        /// <value>
        /// The time counter.
        /// </value>
        public TimeCounter TimeCounter { get; set; }

        /// <summary>
        /// Gets or sets the points counter control.
        /// </summary>
        /// <value>
        /// The points counter.
        /// </value>
        public PointsCounter PointsCounter { get; set; }

        /// <summary>
        /// Creates the scene.
        /// </summary>
        /// <remarks>
        /// This method is called before all 
        /// <see cref="T:WaveEngine.Framework.Entity" /> instances in this instance are initialized.
        /// </remarks>
        protected override void CreateScene()
        {
            // Services
            this.kinectService = WaveServices.GetService<KinectService>();

            // Scene Behaviors
            this.AddSceneBehavior(new GameSceneBehavior(), SceneBehavior.Order.PostUpdate);

            // Create a 2D camera
            var camera2D = new FixedCamera2D("Camera2D") { ClearFlags = ClearFlags.DepthAndStencil }; // Transparent background need this clearFlags.
            this.EntityManager.Add(camera2D);

            // Initializes Players
            this.Players = new Player[this.kinectService.BodyCount];
            for (var i = 0; i < this.Players.Length; i++)
            {
                this.Players[i] = new Player(this) { PlayerBody = this.kinectService.Bodies[i] };
            }

            // Initializes Smoke Emitters
            this.explosionEmitterCollection = new ExplosionEmitter[10];
            for (int i = 0; i < this.explosionEmitterCollection.Length; i++)
            {
                this.explosionEmitterCollection[i] = new ExplosionEmitter();
                this.EntityManager.Add(this.explosionEmitterCollection[i]);
            }

            // Timer
            this.TimeCounter = new TimeCounter();
            this.EntityManager.Add(this.TimeCounter);
            this.TimeCounter.CountDownBehavior.Start();

            // Points
            this.PointsCounter = new PointsCounter();
            this.EntityManager.Add(this.PointsCounter);
            this.PointsCounter.PointCounterBehavior.ResetPoints();

            // Falling Things Initializer!!!
            this.fallingBodies = new List<FallingBody>();
            int createdBodies = 0;

            Timer timer = null;
            timer = WaveServices.TimerFactory.CreateTimer(
                "CreateBallTimer",
                TimeSpan.FromSeconds(2f),
                () =>
                {
                    // Calculates the next interval to drop a falling body.
                    timer.Interval = TimeSpan.FromSeconds(WaveServices.Random.NextDouble());

                    // Calculates the X position
                    var fallingX = WaveServices.Random.Next((int)WaveServices.ViewportManager.LeftEdge + this.margin, (int)WaveServices.ViewportManager.RightEdge - this.margin);

                    // Gets the first free body in pool
                    var fallingBody = this.fallingBodies.FirstOrDefault(fb => !fb.IsInUse);

                    // If there are not any free body we'll create one
                    if (fallingBody == null)
                    {
                        fallingBody = new FallingBody(fallingX, 1, createdBodies++);
                        this.EntityManager.Add(fallingBody);
                        this.fallingBodies.Add(fallingBody);
                    }

                    // Fall!!! I Command!!!
                    fallingBody.Fall(fallingX);
                });
        }

        /// <summary>
        /// Explodes the falling body. Updates the particle emitter position, active it and raise the points.
        /// </summary>
        /// <param name="position">The position.</param>
        public void ExplodeIn(Vector2 position)
        {
            // Calculates the next index from 0 to array.lenght.
            this.explosionEmitterIndex = (this.explosionEmitterIndex + 1) % this.explosionEmitterCollection.Length;

            // Sets position and starts the emitter
            this.explosionEmitterCollection[this.explosionEmitterIndex].Transform.Position = position;
            this.explosionEmitterCollection[this.explosionEmitterIndex].ExplosionBehavior.Explode();

            // Send points to point counter
            this.PointsCounter.PointCounterBehavior.HitPoint(50);
        }
    }
}
