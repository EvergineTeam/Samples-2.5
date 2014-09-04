#region Using Statements
using ParallaxCamera2DProject.Behaviors;
using ParallaxCamera2DProject.Entities;
using ParallaxCamera2DProject.Layers;
using System;
using System.Collections.Generic;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Animation;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Components.Particles;
using WaveEngine.Components.UI;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
using WaveEngine.ImageEffects;
using WaveEngine.Materials;
#endregion

namespace ParallaxCamera2DProject
{
    public class MyScene : Scene
    {
        private YureiDecorator yurei;

        protected override void CreateScene()
        {
            this.RegisterCustomLayers();
            this.CreateGrass();
            this.CreateForest();
            this.CreateHauntedHouse();
            this.CreateSun();

            this.CreateYurei();
            this.CreateCamera();
        }

        /// <summary>
        /// Register custom layers
        /// </summary>
        private void RegisterCustomLayers()
        {
            this.RenderManager.RegisterLayerAfter(new ForegroundLayer(this.RenderManager), DefaultLayers.Additive);
            this.RenderManager.RegisterLayerAfter(new SunLayer(this.RenderManager), typeof(ForegroundLayer));
            this.RenderManager.RegisterLayerBefore(new BackgroundLayer(this.RenderManager), DefaultLayers.Alpha);
        }

        /// <summary>
        /// Create the camera
        /// </summary>
        private void CreateCamera()
        {
            FixedCamera2D camera = new FixedCamera2D("camera")
                {
                    NearPlane = -1000,
                    FarPlane = 30000,
                    FieldOfView = MathHelper.ToRadians(45),
                    VanishingPoint = new Vector2(0.5f, 0.95f),
                    BackgroundColor = SceneResources.BackgroundColor,
                };

            // Lens effects
            camera.Entity.AddComponent(new CameraBehavior(this.yurei.Entity));


            if (WaveServices.Platform.PlatformType == PlatformType.Windows ||
                WaveServices.Platform.PlatformType == PlatformType.Linux ||
                WaveServices.Platform.PlatformType == PlatformType.MacOS)
            {
                camera.Entity.AddComponent(ImageEffects.FishEye());
                camera.Entity.AddComponent(new ChromaticAberrationLens() { AberrationStrength = 5.5f });
                camera.Entity.AddComponent(new RadialBlurLens() { Center = new Vector2(0.5f, 0.75f), BlurWidth = 0.02f, Nsamples = 5 });
                camera.Entity.AddComponent(ImageEffects.Vignette());
                camera.Entity.AddComponent(new FilmGrainLens() { GrainIntensityMin = 0.075f, GrainIntensityMax = 0.15f });
            }

            this.EntityManager.Add(camera);
        }

        /// <summary>
        /// Create the Haunted house
        /// </summary>
        private void CreateHauntedHouse()
        {
            float initOffset = -500;

            // Foreground graveyard
            this.CreateSprite(typeof(ForegroundLayer), "Content/GraveyardForeground", new Vector3(initOffset + 3120, WaveServices.ViewportManager.BottomEdge + 15, -200), new Vector3(1.5f, 1.5f, 1), Color.Black);
            this.CreateSprite(typeof(ForegroundLayer), "Content/GraveyardForeground", new Vector3(initOffset + 4169, WaveServices.ViewportManager.BottomEdge + 17, -150), new Vector3(1.5f, 1.5f, 1), Vector3.Zero, Color.Black, new Vector2(0.5f, 1), SpriteEffects.FlipHorizontally);
            this.CreateSprite(typeof(ForegroundLayer), "Content/GraveyardForeground", new Vector3(initOffset + 4403, WaveServices.ViewportManager.BottomEdge - 15, -10), new Vector3(1.5f, 1.5f, 1), Vector3.Zero, Color.Black, new Vector2(0.5f, 1), SpriteEffects.FlipHorizontally);
            this.CreateSprite(typeof(ForegroundLayer), "Content/GraveyardForeground", new Vector3(initOffset + 4605, WaveServices.ViewportManager.BottomEdge + 30, -200), new Vector3(1.5f, 1.5f, 1), Color.Black);

            //Background graveyard
            this.CreateSprite(typeof(ForegroundLayer), "Content/GraveyardBackground", new Vector3(initOffset + 3615, WaveServices.ViewportManager.BottomEdge - 20, 300), new Vector3(1.8f, 1.8f, 1), Vector3.Zero, SceneResources.GraveyardColor, new Vector2(0.5f, 1), SpriteEffects.FlipHorizontally);
            this.CreateSprite(typeof(ForegroundLayer), "Content/GraveyardBackground", new Vector3(initOffset + 3684, WaveServices.ViewportManager.BottomEdge - 30, 260), new Vector3(1.8f, 1.8f, 1), Vector3.Zero, SceneResources.GraveyardColor, new Vector2(0.5f, 1), SpriteEffects.None);

            // Fence
            this.CreateSpriteStripLayer(
                typeof(ForegroundLayer),
                SceneResources.FenceContent,
                new Vector3(
                    initOffset + 3752,
                    WaveServices.ViewportManager.BottomEdge - 20,
                    204),
                Vector3.One,
                4,
                -30,
                SceneResources.FenceColor,
                false);

            // Haunted House
            this.CreateSprite(DefaultLayers.Alpha, "Content/HouseHill", new Vector3(initOffset + 5200, WaveServices.ViewportManager.BottomEdge - 30, 400), new Vector3(2, 2, 1), Vector3.Zero, SceneResources.HouseHillColor, Vector2.One, SpriteEffects.None);
            this.CreateSprite(DefaultLayers.Alpha, "Content/HillMist", new Vector3(initOffset + 5300, WaveServices.ViewportManager.BottomEdge, 500), new Vector3(12, 12, 1), Vector3.Zero, SceneResources.HouseHillMist, Vector2.One, SpriteEffects.None);
            this.CreateSprite(DefaultLayers.Alpha, "Content/HountedHouse", new Vector3(initOffset + 5400, WaveServices.ViewportManager.BottomEdge - 130, 600), new Vector3(3, 3, 1), Vector3.Zero, SceneResources.HauntedHouseColor, Vector2.One, SpriteEffects.None);
            this.CreateSprite(DefaultLayers.Alpha, "Content/HouseTower", new Vector3(initOffset + 5120, WaveServices.ViewportManager.BottomEdge - 500, 700), new Vector3(3, 3, 1), Vector3.Zero, SceneResources.HauntedHouseColor, new Vector2(0.5f, 1), SpriteEffects.None);

            // Crow
            this.CreateSprite(typeof(ForegroundLayer), "Content/Crow", new Vector3(initOffset + 3802, WaveServices.ViewportManager.BottomEdge - 123, 205), new Vector3(1.1f, 1.1f, 1), SceneResources.CrowColor, new Vector2(0.5f, 1));

            // Scarecrow
            this.CreateSprite(DefaultLayers.Alpha, "Content/scarecrow", new Vector3(initOffset + 4120, WaveServices.ViewportManager.BottomEdge, 400), new Vector3(1.3f, 1.3f, 1), SceneResources.ScarecrowColor);
            this.CreateSprite(DefaultLayers.Alpha, "Content/ghost", new Vector3(initOffset + 4030, WaveServices.ViewportManager.BottomEdge - 79, 300), new Vector3(1.2f, 1.2f, 1), SceneResources.GhostColor);

            this.CreateSprite(DefaultLayers.Alpha, "Content/Flare", new Vector3(initOffset + 3945, WaveServices.ViewportManager.BottomEdge - 195, 290), new Vector3(3, 3, 1), Color.White, Vector2.Center);

        }

        /// <summary>
        /// Create the forest
        /// </summary>
        private void CreateForest()
        {
            float initOffset = -500;

            // Darkness
            this.CreateSprite(typeof(ForegroundLayer), "Content/Darkness", new Vector3(0, WaveServices.ViewportManager.TopEdge, 0), new Vector3(10, 6.5f, 1), SceneResources.DarknessColor, Vector2.Zero);

            // Mist
            this.CreateSprite(typeof(ForegroundLayer), "Content/Mist", new Vector3(-160, WaveServices.ViewportManager.BottomEdge + 300, 210), Vector3.One * 6.5f, SceneResources.TreeMistColor, Vector2.UnitY);

            // Mushrooms
            this.CreateSprite(typeof(ForegroundLayer), "Content/Mushrooms/mushroom_0", new Vector3(394, WaveServices.ViewportManager.BottomEdge - 20, 205), Vector3.One, SceneResources.Mushroom1Color);
            this.CreateSprite(typeof(ForegroundLayer), "Content/Mushrooms/mushroom_1", new Vector3(1350, WaveServices.ViewportManager.BottomEdge - 20, 165), Vector3.One, SceneResources.Mushroom1Color);
            this.CreateSprite(typeof(ForegroundLayer), "Content/Mushrooms/mushroom_2", new Vector3(1940, WaveServices.ViewportManager.BottomEdge - 20, 195), Vector3.One, SceneResources.Mushroom2Color);
            this.CreateSprite(typeof(ForegroundLayer), "Content/Mushrooms/mushroom_3", new Vector3(1618, WaveServices.ViewportManager.BottomEdge + 50, -100), Vector3.One * 2.5f, SceneResources.Mushroom3Color);

            //LightBeams
            this.CreateSprite(DefaultLayers.Alpha, "Content/LightBeam", new Vector3(0, WaveServices.ViewportManager.TopEdge - 400, 600), new Vector3(10, 6, 1), new Vector3(0, 0, MathHelper.ToRadians(30)), Color.White, new Vector2(0, 0.5f));
            this.CreateSprite(DefaultLayers.Alpha, "Content/LightBeam", new Vector3(100, WaveServices.ViewportManager.TopEdge - 600, 1000), new Vector3(13, 7, 1), new Vector3(0, 0, MathHelper.ToRadians(25)), Color.White, new Vector2(0, 0.5f));
            this.CreateSprite(DefaultLayers.Alpha, "Content/LightBeam", new Vector3(100, WaveServices.ViewportManager.TopEdge - 700, 910), new Vector3(13, 5, 1), new Vector3(0, 0, MathHelper.ToRadians(18)), Color.White, new Vector2(0, 0.5f));

            // 1st tree layer
            float scale = 1.95f;
            float depth = 400;
            this.CreateSprite(typeof(ForegroundLayer), "Content/Trees/tree1st_0", new Vector3(978 + initOffset, WaveServices.ViewportManager.BottomEdge, depth - 50), Vector3.One * scale, SceneResources.Trees1stLayerColor);
            this.CreateSprite(typeof(ForegroundLayer), "Content/Trees/tree1st_1", new Vector3(1592 + initOffset, WaveServices.ViewportManager.BottomEdge, depth), Vector3.One * scale, SceneResources.Trees1stLayerColor);
            this.CreateSprite(typeof(ForegroundLayer), "Content/Trees/tree1st_2", new Vector3(2330 + initOffset, WaveServices.ViewportManager.BottomEdge, depth + 20), Vector3.One * scale, SceneResources.Trees1stLayerColor);
            this.CreateSprite(typeof(ForegroundLayer), "Content/Trees/tree1st_3", new Vector3(2968 + initOffset, WaveServices.ViewportManager.BottomEdge, depth), Vector3.One * scale, SceneResources.Trees1stLayerColor);

            // 2st tree layer
            scale = 5.4f;
            depth = 900;
            this.CreateSprite(DefaultLayers.Alpha, "Content/Trees/tree2nd_0", new Vector3(158 + initOffset, WaveServices.ViewportManager.BottomEdge, depth - 50), Vector3.One * scale, SceneResources.Trees2ndLayerColor);
            this.CreateSprite(DefaultLayers.Alpha, "Content/Trees/tree2nd_1", new Vector3(578 + initOffset, WaveServices.ViewportManager.BottomEdge, depth), Vector3.One * scale, SceneResources.Trees2ndLayerColor);
            this.CreateSprite(DefaultLayers.Alpha, "Content/Trees/tree2nd_2", new Vector3(1161 + initOffset, WaveServices.ViewportManager.BottomEdge, depth + 20), Vector3.One * scale, SceneResources.Trees2ndLayerColor);
            this.CreateSprite(DefaultLayers.Alpha, "Content/Trees/tree2nd_3", new Vector3(1799 + initOffset, WaveServices.ViewportManager.BottomEdge, depth - 30), Vector3.One * scale, SceneResources.Trees2ndLayerColor);
            this.CreateSprite(DefaultLayers.Alpha, "Content/Trees/tree2nd_4", new Vector3(2283 + initOffset, WaveServices.ViewportManager.BottomEdge, depth + 40), Vector3.One * scale, SceneResources.Trees2ndLayerColor);

            // 3rd tree layer
            scale = 6.8f;
            depth = 1300;
            this.CreateSprite(DefaultLayers.Alpha, "Content/Trees/tree3rd_0", new Vector3(516 + initOffset, WaveServices.ViewportManager.BottomEdge, depth), Vector3.One * scale, SceneResources.Trees3rdLayerColor);
            this.CreateSprite(DefaultLayers.Alpha, "Content/Trees/tree3rd_1", new Vector3(1227 + initOffset, WaveServices.ViewportManager.BottomEdge, depth + 150), Vector3.One * (scale + 0.3f), SceneResources.Trees3rdLayerColor);
            this.CreateSprite(DefaultLayers.Alpha, "Content/Trees/tree3rd_2", new Vector3(1983 + initOffset, WaveServices.ViewportManager.BottomEdge, depth - 20), Vector3.One * scale, SceneResources.Trees3rdLayerColor);
            this.CreateSprite(DefaultLayers.Alpha, "Content/Trees/tree3rd_3", new Vector3(2682 + initOffset, WaveServices.ViewportManager.BottomEdge, depth - 100), Vector3.One * scale, SceneResources.Trees3rdLayerColor);

            // Dust particles            
            Entity dust = new Entity("smoke")
                .AddComponent(new Transform2D() { Position = new Vector2(1500, 300), DrawOrder = 700 })
                .AddComponent(new ParticleSystem2D()
                {
                    NumParticles = 200,
                    EmitRate = 50,
                    MinColor = Color.White,
                    MaxColor = Color.White,
                    MinLife = TimeSpan.FromSeconds(4),
                    MaxLife = TimeSpan.FromSeconds(6),
                    LocalVelocity = Vector2.Zero,
                    RandomVelocity = Vector2.One * 0.2f,
                    MinSize = 4,
                    MaxSize = 8,
                    EmitterSize = new Vector3(1400, 700, 400),
                    EmitterShape = ParticleSystem2D.Shape.FillBox,
                    InterpolationColors = new List<Color>() { Color.Transparent, SceneResources.DustParticleColor, Color.Transparent },
                    LinearColorEnabled = true,
                })
                .AddComponent(new Material2D(new BasicMaterial2D("Content/DustParticle.wpk", DefaultLayers.Additive)))
                .AddComponent(new ParticleSystemRenderer2D());

            EntityManager.Add(dust);
        }

        /// <summary>
        /// Create the Sun
        /// </summary>
        private void CreateSun()
        {
            this.CreateSprite(typeof(SunLayer), "Content/Sun", new Vector3(2000, 1600, 10500), Vector3.One * 120, SceneResources.SunColor * 0.35f);
            this.CreateSprite(typeof(BackgroundLayer), "Content/Sun", new Vector3(2000, 1600, 10500), Vector3.One * 120, Color.White * 0.7f);
        }

        /// <summary>
        /// Create Yurei, the main character
        /// </summary>
        private void CreateYurei()
        {
            Entity startDust = new Entity("dust") { Enabled = false }
               .AddComponent(new Transform2D()
               {
                   Position = new Vector2(0, WaveServices.ViewportManager.BottomEdge - 27),
                   DrawOrder = 140,
                   Origin = new Vector2(0.75f, 1)
               })
               .AddComponent(new Sprite("Content/Dust/Dust"))
               .AddComponent(Animation2D.Create<TexturePackerGenericXml>("Content/Dust/Dust.xml")
                       .Add("start", new SpriteSheetAnimationSequence() { First = 0, Length = 13, FramesPerSecond = 30 }))
               .AddComponent(new AnimatedSpriteRenderer(typeof(ForegroundLayer)));

            EntityManager.Add(startDust);

            this.yurei = new YureiDecorator(
                "yurei",
                new Vector3(
                    500,
                    WaveServices.ViewportManager.BottomEdge - 82,
                    150),
                Vector3.One * 0.85f,
                startDust,
                typeof(ForegroundLayer));

            EntityManager.Add(yurei);
        }

        /// <summary>
        /// Create grass
        /// </summary>
        private void CreateGrass()
        {
            this.CreateSpriteStripLayer(
                typeof(ForegroundLayer),
                SceneResources.GrassContent,
                new Vector3(
                    WaveServices.ViewportManager.LeftEdge,
                    WaveServices.ViewportManager.BottomEdge,
                    0),
                Vector3.One,
                18,
                0,
                Color.Black);

            this.CreateSpriteStripLayer(
                typeof(ForegroundLayer),
                SceneResources.GrassContent,
                new Vector3(
                    WaveServices.ViewportManager.LeftEdge - 100,
                    WaveServices.ViewportManager.BottomEdge,
                    200),
                Vector3.One,
                18,
                0,
                SceneResources.Grass2ndPlaneColor);
        }

        private void CreateSprite(Type layer, string texture, Vector3 position, Vector3 scale, Color color)
        {
            this.CreateSprite(layer, texture, position, scale, color, new Vector2(0.5f, 1));
        }

        private void CreateSprite(Type layer, string texture, Vector3 position, Vector3 scale, Color color, Vector2 origin)
        {
            this.CreateSprite(layer, texture, position, scale, Vector3.Zero, color, origin);
        }

        private void CreateSprite(Type layer, string texture, Vector3 position, Vector3 scale, Vector3 rotation, Color color, Vector2 origin, SpriteEffects effect = SpriteEffects.None)
        {
            Transform2D transform = new Transform2D() { Origin = origin, Effect = effect };
            transform.Transform3D.Position = position;
            transform.Transform3D.Scale = scale;
            transform.Transform3D.Rotation = rotation;

            Entity sprite = new Entity()
               .AddComponent(transform)
               .AddComponent(new Sprite(texture) { TintColor = color })
                //.AddComponent(new RectangleCollider())
               .AddComponent(new SpriteRenderer(layer));

            this.EntityManager.Add(sprite);
        }

        private void CreateSpriteStripLayer(Type layer, string[] sprites, Vector3 start, Vector3 scale, int nSections, float overlaping, Color color, bool flip = true)
        {
            Texture2D grassTexture = this.Assets.LoadAsset<Texture2D>(sprites[0]);
            float width = grassTexture.Width;

            Vector3 position = start;
            for (int i = 0; i < nSections; i++)
            {
                int spriteIndex = WaveServices.Random.NextInt() % sprites.Length;
                SpriteEffects effect = flip ? (SpriteEffects)(WaveServices.Random.NextInt() % 2) : SpriteEffects.None;

                Transform2D transform = new Transform2D() { Origin = Vector2.UnitY, Effect = effect };
                transform.Transform3D.Position = position;
                transform.Transform3D.Scale = scale;

                Entity grass = new Entity()
                .AddComponent(transform)
                .AddComponent(new Sprite(sprites[spriteIndex]) { TintColor = color })
                .AddComponent(new SpriteRenderer(layer));

                this.EntityManager.Add(grass);

                position += Vector3.UnitX * (width + overlaping);
            }

            this.Assets.UnloadAsset(sprites[0]);
        }
    }
}
