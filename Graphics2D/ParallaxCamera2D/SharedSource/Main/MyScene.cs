#region Using Statements
using ParallaxCamera2D.Behaviors;
using ParallaxCamera2D.Layers;
using System;
using System.Collections.Generic;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Components.Particles;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Resources;
using WaveEngine.Framework.Services;
using WaveEngine.ImageEffects;
using WaveEngine.Materials;
using WaveEngine.Spine;
#endregion

namespace ParallaxCamera2D
{
    public class MyScene : Scene
    {
        private Entity yurei;
        protected override void CreateScene()
        {
            this.Load(WaveContent.Scenes.MyScene);

            //RenderManager.DebugLines = true;
            this.RegisterCustomLayers();
            this.CreateGrass();
            this.CreateForest();
            this.CreateHauntedHouse();
            this.CreateSun();

            this.CreateYurei();
            this.CreateCamera();
        }

        private void CreateSun()
        {
            this.CreateSprite(typeof(SunLayer), WaveContent.Assets.Textures.Sun_png, new Vector3(2000, 1600, 10500), Vector3.One * 120, SceneResources.SunColor * 0.35f);
            this.CreateSprite(typeof(BackgroundLayer), WaveContent.Assets.Textures.Sun_png, new Vector3(2000, 1600, 10500), Vector3.One * 120, Color.White * 0.7f);
        }

        private void CreateCamera()
        {
            var cameraEntity = EntityManager.Find("camera");

            Camera2D camera = cameraEntity.FindComponent<Camera2D>();
            camera.FieldOfView = MathHelper.ToRadians(45);
            camera.BackgroundColor = SceneResources.BackgroundColor;

            if (WaveServices.Platform.PlatformType == PlatformType.Windows ||
                WaveServices.Platform.PlatformType == PlatformType.Linux ||
                WaveServices.Platform.PlatformType == PlatformType.MacOS)
            {
                cameraEntity.AddComponent(ImageEffects.FishEye());
                cameraEntity.AddComponent(new ChromaticAberrationLens() { AberrationStrength = 5.5f });
                cameraEntity.AddComponent(new RadialBlurLens() { Center = new Vector2(0.5f, 0.75f), BlurWidth = 0.02f, Nsamples = 5 });
                cameraEntity.AddComponent(ImageEffects.Vignette());
                //cameraEntity.AddComponent(new FilmGrainLens() { GrainIntensityMin = 0.075f, GrainIntensityMax = 0.15f });
            }

            cameraEntity.AddComponent(new CameraBehavior(this.yurei));
        }

        /// <summary>
        /// Create Yurei, the main character
        /// </summary>
        private void CreateYurei()
        {
            this.yurei = EntityManager.Find("yurei");
            Entity starDust = yurei.FindChild("dust");

            starDust.FindComponent<SpriteAtlasRenderer>().LayerType = typeof(ForegroundLayer);

            yurei.AddComponent(new YureiBehavior(starDust));

            Transform3D transform = yurei.FindComponent<Transform2D>().Transform3D;
            transform.Position = new Vector3(500, this.VirtualScreenManager.BottomEdge - 152, 150);
            transform.Scale = Vector3.One * 0.85f;

            yurei.FindComponent<SkeletalRenderer>().LayerType = typeof(ForegroundLayer);

        }

        private void CreateHauntedHouse()
        {
            float initOffset = 0;

            // Foreground graveyard
            this.CreateSprite(typeof(ForegroundLayer), WaveContent.Assets.Textures.GraveyardForeground_png, new Vector3(initOffset + 3120, this.VirtualScreenManager.BottomEdge + 15, -200), new Vector3(1.5f, 1.5f, 1), Color.Black);
            this.CreateSprite(typeof(ForegroundLayer), WaveContent.Assets.Textures.GraveyardForeground_png, new Vector3(initOffset + 4169, this.VirtualScreenManager.BottomEdge + 17, -150), new Vector3(1.5f, 1.5f, 1), Vector3.Zero, Color.Black, new Vector2(0.5f, 1), SpriteEffects.FlipHorizontally);
            this.CreateSprite(typeof(ForegroundLayer), WaveContent.Assets.Textures.GraveyardForeground_png, new Vector3(initOffset + 4403, this.VirtualScreenManager.BottomEdge - 15, -10), new Vector3(1.5f, 1.5f, 1), Vector3.Zero, Color.Black, new Vector2(0.5f, 1), SpriteEffects.FlipHorizontally);
            this.CreateSprite(typeof(ForegroundLayer), WaveContent.Assets.Textures.GraveyardForeground_png, new Vector3(initOffset + 4605, this.VirtualScreenManager.BottomEdge + 30, -200), new Vector3(1.5f, 1.5f, 1), Color.Black);

            //Background graveyard
            this.CreateSprite(typeof(ForegroundLayer), WaveContent.Assets.Textures.GraveyardBackground_png, new Vector3(initOffset + 3615, this.VirtualScreenManager.BottomEdge - 20, 300), new Vector3(1.8f, 1.8f, 1), Vector3.Zero, SceneResources.GraveyardColor, new Vector2(0.5f, 1), SpriteEffects.FlipHorizontally);
            this.CreateSprite(typeof(ForegroundLayer), WaveContent.Assets.Textures.GraveyardBackground_png, new Vector3(initOffset + 3684, this.VirtualScreenManager.BottomEdge - 30, 260), new Vector3(1.8f, 1.8f, 1), Vector3.Zero, SceneResources.GraveyardColor, new Vector2(0.5f, 1), SpriteEffects.None);

            // Fence
            this.CreateSpriteStripLayer(
                typeof(ForegroundLayer),
                 SceneResources.FenceContent,
                new Vector3(
                    initOffset + 3752,
                    this.VirtualScreenManager.BottomEdge - 20,
                    204),
                Vector3.One,
                4,
                -30,
                SceneResources.FenceColor,
                false);

            // Haunted House
            this.CreateSprite(DefaultLayers.Alpha, WaveContent.Assets.Textures.HouseHill_png, new Vector3(initOffset + 5200, this.VirtualScreenManager.BottomEdge - 30, 400), new Vector3(2, 2, 1), Vector3.Zero, SceneResources.HouseHillColor, Vector2.One, SpriteEffects.None);
            this.CreateSprite(DefaultLayers.Alpha, WaveContent.Assets.Textures.HillMist_png, new Vector3(initOffset + 5300, this.VirtualScreenManager.BottomEdge, 500), new Vector3(12, 12, 1), Vector3.Zero, SceneResources.HouseHillMist, Vector2.One, SpriteEffects.None);
            this.CreateSprite(DefaultLayers.Alpha, WaveContent.Assets.Textures.HountedHouse_png, new Vector3(initOffset + 5400, this.VirtualScreenManager.BottomEdge - 130, 600), new Vector3(3, 3, 1), Vector3.Zero, SceneResources.HauntedHouseColor, Vector2.One, SpriteEffects.None);
            this.CreateSprite(DefaultLayers.Alpha, WaveContent.Assets.Textures.HouseTower_png, new Vector3(initOffset + 5120, this.VirtualScreenManager.BottomEdge - 500, 700), new Vector3(3, 3, 1), Vector3.Zero, SceneResources.HauntedHouseColor, new Vector2(0.5f, 1), SpriteEffects.None);

            // Crow
            this.CreateSprite(typeof(ForegroundLayer), WaveContent.Assets.Textures.Crow_png, new Vector3(initOffset + 3802, this.VirtualScreenManager.BottomEdge - 123, 205), new Vector3(1.1f, 1.1f, 1), SceneResources.CrowColor, new Vector2(0.5f, 1));

            // Scarecrow
            this.CreateSprite(DefaultLayers.Alpha, WaveContent.Assets.Textures.scarecrow_png, new Vector3(initOffset + 4120, this.VirtualScreenManager.BottomEdge, 400), new Vector3(1.3f, 1.3f, 1), SceneResources.ScarecrowColor);
            this.CreateSprite(DefaultLayers.Alpha, WaveContent.Assets.Textures.ghost_png, new Vector3(initOffset + 4030, this.VirtualScreenManager.BottomEdge - 79, 300), new Vector3(1.2f, 1.2f, 1), SceneResources.GhostColor);

            this.CreateSprite(DefaultLayers.Alpha, WaveContent.Assets.Textures.Flare_png, new Vector3(initOffset + 3945, this.VirtualScreenManager.BottomEdge - 195, 290), new Vector3(3, 3, 1), Color.White, Vector2.Center);

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
        /// Create grass
        /// </summary>
        private void CreateGrass()
        {
            this.CreateSpriteStripLayer(
               typeof(ForegroundLayer),
               SceneResources.GrassContent,
               new Vector3(
                   this.VirtualScreenManager.LeftEdge,
                   this.VirtualScreenManager.BottomEdge,
                   0),
               Vector3.One,
               20,
               0,
               Color.Black);

            this.CreateSpriteStripLayer(
                typeof(ForegroundLayer),
                SceneResources.GrassContent,
                new Vector3(
                    this.VirtualScreenManager.LeftEdge - 100,
                    this.VirtualScreenManager.BottomEdge,
                    200),
                Vector3.One,
                20,
                0,
                SceneResources.Grass2ndPlaneColor);
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

        private void CreateForest()
        {
            float initOffset = 0;

            // Darkness
            this.CreateSprite(typeof(ForegroundLayer), WaveContent.Assets.Textures.Darkness_png, new Vector3(0, this.VirtualScreenManager.TopEdge, 0), new Vector3(10, 6.5f, 1), SceneResources.DarknessColor, Vector2.Zero);

            // Mist
            this.CreateSprite(typeof(ForegroundLayer), WaveContent.Assets.Textures.Mist_png, new Vector3(-160, this.VirtualScreenManager.BottomEdge + 300, 210), Vector3.One * 6.5f, SceneResources.TreeMistColor, Vector2.UnitY);

            // Mushrooms
            this.CreateSprite(typeof(ForegroundLayer), WaveContent.Assets.Textures.Mushrooms.mushroom_0_png, new Vector3(394, this.VirtualScreenManager.BottomEdge - 20, 205), Vector3.One, SceneResources.Mushroom1Color);
            this.CreateSprite(typeof(ForegroundLayer), WaveContent.Assets.Textures.Mushrooms.mushroom_1_png, new Vector3(1350, this.VirtualScreenManager.BottomEdge - 20, 165), Vector3.One, SceneResources.Mushroom1Color);
            this.CreateSprite(typeof(ForegroundLayer), WaveContent.Assets.Textures.Mushrooms.mushroom_2_png, new Vector3(1940, this.VirtualScreenManager.BottomEdge - 20, 195), Vector3.One, SceneResources.Mushroom2Color);
            this.CreateSprite(typeof(ForegroundLayer), WaveContent.Assets.Textures.Mushrooms.mushroom_3_png, new Vector3(1618, this.VirtualScreenManager.BottomEdge + 50, -100), Vector3.One * 2.5f, SceneResources.Mushroom3Color);

            //LightBeams
            this.CreateSprite(DefaultLayers.Alpha, WaveContent.Assets.Textures.LightBeam_png, new Vector3(0, this.VirtualScreenManager.TopEdge - 400, 600), new Vector3(10, 6, 1), new Vector3(0, 0, MathHelper.ToRadians(30)), Color.White, new Vector2(0, 0.5f));
            this.CreateSprite(DefaultLayers.Alpha, WaveContent.Assets.Textures.LightBeam_png, new Vector3(100, this.VirtualScreenManager.TopEdge - 600, 1000), new Vector3(13, 7, 1), new Vector3(0, 0, MathHelper.ToRadians(25)), Color.White, new Vector2(0, 0.5f));
            this.CreateSprite(DefaultLayers.Alpha, WaveContent.Assets.Textures.LightBeam_png, new Vector3(100, this.VirtualScreenManager.TopEdge - 700, 910), new Vector3(13, 5, 1), new Vector3(0, 0, MathHelper.ToRadians(18)), Color.White, new Vector2(0, 0.5f));

            // 1st tree layer
            float scale = 1.95f;
            float depth = 400;
            this.CreateSprite(typeof(ForegroundLayer), WaveContent.Assets.Textures.Trees.tree1st_0_png, new Vector3(978 + initOffset, this.VirtualScreenManager.BottomEdge, depth - 50), Vector3.One * scale, SceneResources.Trees1stLayerColor);
            this.CreateSprite(typeof(ForegroundLayer), WaveContent.Assets.Textures.Trees.tree1st_1_png, new Vector3(1592 + initOffset, this.VirtualScreenManager.BottomEdge, depth), Vector3.One * scale, SceneResources.Trees1stLayerColor);
            this.CreateSprite(typeof(ForegroundLayer), WaveContent.Assets.Textures.Trees.tree1st_2_png, new Vector3(2330 + initOffset, this.VirtualScreenManager.BottomEdge, depth + 20), Vector3.One * scale, SceneResources.Trees1stLayerColor);
            this.CreateSprite(typeof(ForegroundLayer), WaveContent.Assets.Textures.Trees.tree1st_3_png, new Vector3(2968 + initOffset, this.VirtualScreenManager.BottomEdge, depth), Vector3.One * scale, SceneResources.Trees1stLayerColor);

            // 2st tree layer
            scale = 5.4f;
            depth = 900;
            this.CreateSprite(DefaultLayers.Alpha, WaveContent.Assets.Textures.Trees.tree2nd_0_png, new Vector3(158 + initOffset, this.VirtualScreenManager.BottomEdge, depth - 50), Vector3.One * scale, SceneResources.Trees2ndLayerColor);
            this.CreateSprite(DefaultLayers.Alpha, WaveContent.Assets.Textures.Trees.tree2nd_1_png, new Vector3(578 + initOffset, this.VirtualScreenManager.BottomEdge, depth), Vector3.One * scale, SceneResources.Trees2ndLayerColor);
            this.CreateSprite(DefaultLayers.Alpha, WaveContent.Assets.Textures.Trees.tree2nd_2_png, new Vector3(1161 + initOffset, this.VirtualScreenManager.BottomEdge, depth + 20), Vector3.One * scale, SceneResources.Trees2ndLayerColor);
            this.CreateSprite(DefaultLayers.Alpha, WaveContent.Assets.Textures.Trees.tree2nd_3_png, new Vector3(1799 + initOffset, this.VirtualScreenManager.BottomEdge, depth - 30), Vector3.One * scale, SceneResources.Trees2ndLayerColor);
            this.CreateSprite(DefaultLayers.Alpha, WaveContent.Assets.Textures.Trees.tree2nd_4_png, new Vector3(2283 + initOffset, this.VirtualScreenManager.BottomEdge, depth + 40), Vector3.One * scale, SceneResources.Trees2ndLayerColor);

            // 3rd tree layer
            scale = 6.8f;
            depth = 1300;
            this.CreateSprite(DefaultLayers.Alpha, WaveContent.Assets.Textures.Trees.tree3rd_0_png, new Vector3(516 + initOffset, this.VirtualScreenManager.BottomEdge, depth), Vector3.One * scale, SceneResources.Trees3rdLayerColor);
            this.CreateSprite(DefaultLayers.Alpha, WaveContent.Assets.Textures.Trees.tree3rd_1_png, new Vector3(1227 + initOffset, this.VirtualScreenManager.BottomEdge, depth + 150), Vector3.One * (scale + 0.3f), SceneResources.Trees3rdLayerColor);
            this.CreateSprite(DefaultLayers.Alpha, WaveContent.Assets.Textures.Trees.tree3rd_2_png, new Vector3(1983 + initOffset, this.VirtualScreenManager.BottomEdge, depth - 20), Vector3.One * scale, SceneResources.Trees3rdLayerColor);
            this.CreateSprite(DefaultLayers.Alpha, WaveContent.Assets.Textures.Trees.tree3rd_3_png, new Vector3(2682 + initOffset, this.VirtualScreenManager.BottomEdge, depth - 100), Vector3.One * scale, SceneResources.Trees3rdLayerColor);

            // Dust particles            
            Entity dust = new Entity("smoke")
                .AddComponent(new Transform2D() { Position = new Vector2(1500, 300), DrawOrder = 700 })
                .AddComponent(new ParticleSystem2D()
                {
                    NumParticles = 200,
                    EmitRate = 50,
                    MinColor = Color.White,
                    MaxColor = Color.White,
                    MinLife = 4,
                    MaxLife = 6,
                    LocalVelocity = Vector2.Zero,
                    RandomVelocity = Vector2.One * 0.2f,
                    MinSize = 4,
                    MaxSize = 8,
                    EmitterSize = new Vector3(1400, 700, 400),
                    EmitterShape = ParticleSystem2D.Shape.FillBox,
                    InterpolationColors = new List<Color>() { Color.Transparent, SceneResources.DustParticleColor, Color.Transparent },
                    LinearColorEnabled = true,
                })
                .AddComponent(new MaterialsMap(new StandardMaterial()
                                                    {
                                                        LightingEnabled = false,
                                                        LayerType = DefaultLayers.Additive,
                                                        DiffusePath = WaveContent.Assets.Textures.DustParticle_png,
                                                        VertexColorEnable = true
                                                    }))
                .AddComponent(new ParticleSystemRenderer2D());

            EntityManager.Add(dust);
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
    }
}
