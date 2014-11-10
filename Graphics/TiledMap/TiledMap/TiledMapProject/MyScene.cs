#region Using Statements
using System;
using TiledMapProject.Components;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Common.Media;
using WaveEngine.Components.Animation;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics2D;
using WaveEngine.Framework.Resources;
using WaveEngine.Framework.Services;
using WaveEngine.ImageEffects;
using WaveEngine.TiledMap;
#endregion

namespace TiledMapProject
{
    public class MyScene : Scene
    {
        private TiledMap tiledMap;
        private SoundManager soundManager;

        protected override void CreateScene()
        {
            this.CreateCamera();
            this.CreateTiledMap();
            this.CreateSounds();
        }

        protected override void Start()
        {
            base.Start();

            this.AddCoins();
            this.AddCrates();
            this.AddTraps();
            this.AddSceneColliders();
            this.AddEnd();
            this.CreateCharacter();
            this.StartMusic();

            this.AddSceneBehavior(new GameplaySceneBehavior(), SceneBehavior.Order.PostUpdate);
        }

        private void CreateSounds()
        {
            Entity sound = new Entity("soundManager")
            .AddComponent(this.soundManager = new SoundManager());

            this.EntityManager.Add(sound);
        }

        private void CreateCamera()
        {
            // Create a 2D camera
            var camera = new FixedCamera2D("Camera2D")
            {
                FieldOfView = MathHelper.ToRadians(70),
                BackgroundColor = new Color("#5a93e0"),
            };

            var camera2DComponent = camera.Entity.FindComponent<Camera2D>();
            camera2DComponent.Zoom = Vector2.One / 2.5f;

            //if (WaveServices.Platform.PlatformType == PlatformType.Windows ||
            //    WaveServices.Platform.PlatformType == PlatformType.Linux || 
            //    WaveServices.Platform.PlatformType == PlatformType.MacOS)
            //{
            //    camera.Entity.AddComponent(ImageEffects.FishEye());
            //    camera.Entity.AddComponent(new ChromaticAberrationLens() { AberrationStrength = 5.5f });
            //    camera.Entity.AddComponent(new RadialBlurLens() { Center = new Vector2(0.5f, 0.75f), BlurWidth = 0.02f, Nsamples = 5 }); 
            //    camera.Entity.AddComponent(ImageEffects.Vignette());
            //    camera.Entity.AddComponent(new FilmGrainLens() { GrainIntensityMin = 0.075f, GrainIntensityMax = 0.15f });
            //} 

            EntityManager.Add(camera);
        }

        private void CreateTiledMap()
        {
            var map = new Entity("map")
            .AddComponent(new Transform2D())
            .AddComponent(this.tiledMap = new TiledMap("Content/map.tmx")
            {
                MinLayerDrawOrder = -10,
                MaxLayerDrawOrder = -0
            });

            this.EntityManager.Add(map);
        }

        private void StartMusic()
        {
            MusicInfo musicInfo = new MusicInfo("Content/Music/Ozzed_-_A_Well_Worked_Analogy.mp3");
            WaveServices.MusicPlayer.IsRepeat = true;
            WaveServices.MusicPlayer.Volume = 0.6f;
            WaveServices.MusicPlayer.Play(musicInfo);
        }

        private void AddEnd()
        {
            var endLayer = this.tiledMap.ObjectLayers["End"];
            var endObj = endLayer.Objects[0];
            var endEntity = TiledMapUtils.CollisionEntityFromObject("End", endObj);

            this.EntityManager.Add(endEntity);
        }


        /// <summary>
        /// Create character
        /// </summary>
        private void CreateCharacter()
        {
            var cratesLayer = this.tiledMap.ObjectLayers["Start"];
            var startObj = cratesLayer.Objects[0];

            Entity character = new Entity("Player")
            .AddComponent(new Transform2D() { LocalPosition = new Vector2(startObj.X, startObj.Y), Origin = Vector2.Center, DrawOrder = -9 })
            .AddComponent(new Sprite("Content/ball.png"))
            .AddComponent(new SpriteRenderer(DefaultLayers.Alpha, AddressMode.PointWrap))
            .AddComponent(new CircleCollider())
            .AddComponent(new RigidBody2D())
            .AddComponent(new PlayerController())
            ;

            this.EntityManager.Add(character);

            this.EntityManager.Find("Camera2D").AddComponent(new CameraBehavior(
                character,
                new RectangleF(0, 0, this.tiledMap.Width * this.tiledMap.TileWidth, this.tiledMap.Height * this.tiledMap.TileHeight)));
        }

        /// <summary>
        /// Add coins
        /// </summary> 
        private void AddCoins()
        {
            var cratesLayer = this.tiledMap.ObjectLayers["Coins"];
            Animation2D anim;

            int i = 0;
            foreach (var obj in cratesLayer.Objects)
            {
                Entity crateEntity = new Entity("coin_" + (i++)) { Tag = "coin" }
                .AddComponent(new Transform2D() { LocalPosition = new Vector2(obj.X, obj.Y), Origin = Vector2.Center, DrawOrder = -9 })
                .AddComponent(new Sprite("Content/coin.png") { SourceRectangle = new Rectangle(0, 128, 16, 16) })
                .AddComponent(anim = Animation2D.Create<TexturePackerGenericXml>("Content/coin.xml").Add("flip", new SpriteSheetAnimationSequence() { First = 1, Length = 8, FramesPerSecond = 12 }))
                .AddComponent(new AnimatedSpriteRenderer(DefaultLayers.Alpha, AddressMode.PointWrap))
                .AddComponent(new CircleCollider())
                ;

                this.EntityManager.Add(crateEntity);

                anim.CurrentAnimation = "flip";
                anim.Play(true);
            }
        }

        /// <summary>
        /// Add physics crates
        /// </summary>
        private void AddCrates()
        {
            var cratesLayer = this.tiledMap.ObjectLayers["Crates"];

            int i = 0;
            foreach (var obj in cratesLayer.Objects)
            {
                RigidBody2D rigidBody;

                Entity crateEntity = new Entity("crate_" + (i++)) { Tag = "crate" }
                .AddComponent(new Transform2D() { LocalPosition = new Vector2(obj.X, obj.Y), Rotation = (float)obj.Rotation, DrawOrder = -9 })
                .AddComponent(new Sprite("Content/tilesets/scene-tiles.png") { SourceRectangle = new Rectangle(0, 128, 16, 16) })
                .AddComponent(new SpriteRenderer(DefaultLayers.Alpha, AddressMode.PointWrap))
                .AddComponent(new RectangleCollider())
                .AddComponent(rigidBody = new RigidBody2D()
                {
                    Mass = 0.003f
                });

                this.EntityManager.Add(crateEntity);

                rigidBody.OnPhysic2DCollision +=(s,e) =>
                    {
                        this.soundManager.PlaySound(SoundType.CrateDrop);
                    };
            }
        }

        /// <summary>
        /// Add traps
        /// </summary>
        private void AddTraps()
        {
            var collisionLayer = this.tiledMap.ObjectLayers["Traps"];

            int i = 0;
            foreach (var obj in collisionLayer.Objects)
            {
                var colliderEntity = TiledMapUtils.CollisionEntityFromObject("trap_" + (i++), obj);
                colliderEntity.Tag = "trap";

                this.EntityManager.Add(colliderEntity);
            }
        }

        /// <summary>
        /// Add Collision entities from TMX object layer
        /// </summary>
        private void AddSceneColliders()
        {
            var collisionLayer = this.tiledMap.ObjectLayers["Collisions"];

            int i = 0;
            foreach (var obj in collisionLayer.Objects)
            {
                var colliderEntity = TiledMapUtils.CollisionEntityFromObject("collider_" + (i++), obj);
                colliderEntity.Tag = "collider";

                colliderEntity.AddComponent(new RigidBody2D() { PhysicBodyType = PhysicBodyType.Static });

                this.EntityManager.Add(colliderEntity);
            }
        }
    }
}
