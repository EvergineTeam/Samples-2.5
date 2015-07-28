#region Using Statements
using System;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics2D;
using WaveEngine.Framework.Resources;
using WaveEngine.Framework.Services;
#endregion

namespace RevoluteJoint2DSample
{
    public class MyScene : Scene
    {
        private const int BRIDGE_CHAIN_LINKS = 30;

        private const string BRIDGE_LINK1_FILENAME = "Content/Assets/Wood20x5a.wpk";
        private const string BRIDGE_LINK2_FILENAME = "Content/Assets/Wood20x5b.wpk";
        private const string BRIDGE_LINK3_FILENAME = "Content/Assets/Wood20x5c.wpk";
        private const string CRATE1_FILENAME = "Content/Assets/CrateA.wpk";
        private const string CRATE2_FILENAME = "Content/Assets/CrateB.wpk";

        // Instance count. Use to create different entity names.
        private static long instances = 0;

        protected override void CreateScene()
        {
            this.Load(@"Content/Scenes/MyScene.wscene");

            Entity pin01 = this.EntityManager.Find("pin01");
            Transform2D pin01Transform = pin01.FindComponent<Transform2D>();

            Entity pin02 = this.EntityManager.Find("pin02");
            Transform2D pin02Transform = pin02.FindComponent<Transform2D>();

            // Bridge links creation
            Entity lastLink = null;
            int spacing = (int)((pin02Transform.Position.X - pin01Transform.Position.X) / BRIDGE_CHAIN_LINKS);

            for (int i = 0; i < BRIDGE_CHAIN_LINKS; i++)
            {
                Entity currentLink = this.CreateSquareSprite((int)pin01Transform.Position.X + i * spacing, (int)pin01Transform.Position.Y, this.GetRandomLinkFileName(), 0.005f);
                this.EntityManager.Add(currentLink);

                // First Link Joins to Left Anchor
                if (i == 0)
                {
                    currentLink.AddComponent(new JointMap2D().AddJoint("joint", new RevoluteJoint2D(pin01, new Vector2(-8, 0), new Vector2(0, 26))));
                }
                else
                {
                    if (lastLink != null)
                    {
                        currentLink.AddComponent(new JointMap2D()
                                                          .AddJoint("joint", new RevoluteJoint2D(lastLink, new Vector2(-8, 0), new Vector2(8, 0))
                                                          {
                                                              BreakPoint = 10,
                                                          }));
                    }
                }

                lastLink = currentLink;
            }

            if (lastLink != null)
            {
                // Last Bridge Link joins to Right Anchor
                pin02.AddComponent(new JointMap2D()
                                        .AddJoint("joint", new RevoluteJoint2D(lastLink, new Vector2(-1, 23), new Vector2(6, 0))));
            }

            // Timer to create crates
            WaveServices.TimerFactory.CreateTimer("FallingCratesTimer", TimeSpan.FromSeconds(1.0f), () =>
            {
                instances++;
                Entity box = this.CreateSquareSprite(WaveServices.Random.Next(500, 800), -40, this.GetRandomCrateFileName(), 0.03f);
                this.EntityManager.Add(box);

                if (instances == 10)
                {
                    Entity bigBox = this.CreateSquareSprite(WaveServices.Random.Next(500, 800), -100, this.GetRandomCrateFileName(), 2f);
                    this.EntityManager.Add(bigBox);
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
        private Entity CreateSquareSprite(int x, int y, string fileName, float mass)
        {
            Entity sprite = new Entity()
                .AddComponent(new Transform2D() { X = x, Y = y, Origin = Vector2.Center })
                .AddComponent(new RectangleCollider2D())
                .AddComponent(new Sprite(fileName))
                .AddComponent(new RigidBody2D() { Mass = mass })
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
