// Copyright (C) 2012-2013 Weekend Game Studio
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to
// deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
// sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.

#region File Description
//-----------------------------------------------------------------------------
// MainScene
//
// Copyright © 2013 Weekend Game Studio. All rights reserved.
// Use is subject to license terms.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics2D;
using WaveEngine.Framework.Services;
#endregion

namespace RevoluteJoint2DSampleProject
{
    public class MainScene : Scene
    {
        // Constants
        private const int BRIDGE_Y_POSITION = 200;
        private const int BRIDGE_LENGTH = 600;
        private const int BRIDGE_CHAIN_LINKS = 40;

        private const string LEFT_BRIDGE_FILENAME = "Content/PinAzureB.wpk";
        private const string RIGHT_BRIDGE_FILENAME = "Content/PinAzureA.wpk";

        private const string BRIDGE_LINK1_FILENAME = "Content/Wood20x5a.wpk";
        private const string BRIDGE_LINK2_FILENAME = "Content/Wood20x5b.wpk";
        private const string BRIDGE_LINK3_FILENAME = "Content/Wood20x5c.wpk";

        private const string CRATE1_FILENAME = "Content/CrateA.wpk";
        private const string CRATE2_FILENAME = "Content/CrateB.wpk";

        // Instance count. Use to create different entity names.
        private static long instances = 0;

        /// <summary>
        /// Scene
        /// </summary>
        protected override void CreateScene()
        {
            RenderManager.BackgroundColor = Color.CornflowerBlue;

            PhysicsManager.Physics2DPositionIterations = 15;

            // Left Bridge Anchor
            Entity Pin1 = this.CreateSquareSprite("Pin1", 50, BRIDGE_Y_POSITION, LEFT_BRIDGE_FILENAME, true);
            EntityManager.Add(Pin1);

            // Bridge links creation
            Entity lastLink = null;
            int spacing = BRIDGE_LENGTH / BRIDGE_CHAIN_LINKS;
            for (int i = 0; i < BRIDGE_CHAIN_LINKS; i++)
            {
                Entity currentLink = this.CreateSquareSprite("link" + i + 1, 70 + i * spacing, BRIDGE_Y_POSITION, this.GetRandomLinkFileName(), false);
                EntityManager.Add(currentLink);

                if (i == 0)
                {
                    // First Link Joins to Left Anchor
                    currentLink.AddComponent(new RevoluteJoint2D(Pin1, new Vector2(-6, 0), new Vector2(0, 24)));
                }
                else
                {
                    if (lastLink != null)
                    {
                        currentLink.AddComponent(new RevoluteJoint2D(lastLink, new Vector2(-8, 0), new Vector2(8, 0)));
                        // each link breaks if support a specific force
                        currentLink.FindComponent<RevoluteJoint2D>().BreakPoint = 12;
                    }
                }

                lastLink = currentLink;
            }

            if (lastLink != null)
            {
                // Right Bridge Anchor
                Entity Pin2 = this.CreateSquareSprite("Pin2", 750, BRIDGE_Y_POSITION, RIGHT_BRIDGE_FILENAME, true);
                EntityManager.Add(Pin2);

                // Last Bridge Link joins to Right Anchor
                Pin2.AddComponent(new RevoluteJoint2D(lastLink, new Vector2(-1, 23), new Vector2(6, 0)));
            }

            // Physic ground
            Entity ground = this.CreateSquareSprite("Ground", 400, 500, "Content/groundSprite.wpk", true);
            EntityManager.Add(ground);

            // Timer to create crates
            WaveServices.TimerFactory.CreateTimer("FallingCratesTimer", TimeSpan.FromSeconds(1.0f), () =>
                {
                    Entity box = this.CreateSquareSprite("Crate" + instances++, WaveServices.Random.Next(300, 400), -40, this.GetRandomCrateFileName(), false, 0.5f);
                    EntityManager.Add(box);

                    if (instances == 10)
                    {
                        WaveServices.TimerFactory.RemoveTimer("FallingCratesTimer");
                    }
                });
        }

        /// <summary>
        /// Creates a Sprite
        /// </summary>
        /// <param name="name">Sprite Name.</param>
        /// <param name="x">X position.</param>
        /// <param name="y">Y posisition.</param>
        /// <param name="fileName">Sprite filename.</param>
        /// <param name="isKinematic">Physic IsKinetic Sprite Parameter.</param>
        /// <param name="mass">Physic Mass Sprite Parameter.</param>
        /// <returns></returns>
        private Entity CreateSquareSprite(string name, int x, int y, string fileName, bool isKinematic, float mass = 1f)
        {
            Entity sprite = new Entity(name)
                .AddComponent(new Transform2D() { X = x, Y = y })
                .AddComponent(new RectangleCollider())
                .AddComponent(new Sprite(fileName))
                .AddComponent(new RigidBody2D() { IsKinematic = isKinematic, Mass = mass })
                .AddComponent(new SpriteRenderer(DefaultLayers.Alpha));
            return sprite;
        }

        /// <summary>
        /// Gets a random link sprite file name
        /// </summary>
        /// <returns></returns>
        private string GetRandomLinkFileName()
        {
            double random = WaveServices.Random.NextDouble();
            string result = BRIDGE_LINK1_FILENAME;

            if (random >= 0 && random < 1d / 3d)
            {
                result = BRIDGE_LINK1_FILENAME;
            }
            else if (random >= 1d / 3d && random < 2d / 3d)
            {
                result = BRIDGE_LINK2_FILENAME;
            }
            else
            {
                result = BRIDGE_LINK3_FILENAME;
            }

            return result;
        }

        /// <summary>
        /// Gets a random crate filename
        /// </summary>
        /// <returns></returns>
        private string GetRandomCrateFileName()
        {
            double random = WaveServices.Random.NextDouble();
            string result = CRATE1_FILENAME;

            if (random >= 0.5d)
            {
                result = CRATE2_FILENAME;
            }

            return result;
        }
    }
}
