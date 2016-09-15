#region Using Statements
using System;
using System.Linq;
using TiledMap;
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
using WaveEngine.TiledMap;
#endregion

namespace TiledMap
{
    public class MyScene : Scene
    {
        private WaveEngine.TiledMap.TiledMap tiledMap;
        private static int CrateTileID = 190;

        protected override void CreateScene()
        {
            //WaveServices.ScreenContextManager.SetDiagnosticsActive(true);
            this.Load(WaveContent.Scenes.MyScene);
        }

        protected override void Start()
        {
            base.Start();

            this.tiledMap = this.EntityManager.Find("Map").FindComponent<WaveEngine.TiledMap.TiledMap>();

            this.AddCoins();
            this.AddCrates();
            this.AddTraps();
            this.AddSceneColliders();
            this.AddEnd();
            this.StartMusic();
            this.AddSceneBehavior(new GameplaySceneBehavior(), SceneBehavior.Order.PostUpdate);
        }

        private void AddEnd()
        {
            var endLayer = this.tiledMap.ObjectLayers["End"];
            var endObj = endLayer.Objects[0];
            var endEntity = TiledMapUtils.CollisionEntityFromObject("End", endObj);

            this.EntityManager.Add(endEntity);
        }

        /// <summary>
        /// Add coins
        /// </summary> 
        private void AddCoins()
        {
            var cratesLayer = this.tiledMap.ObjectLayers["Coins"];

            int i = 0;
            foreach (var obj in cratesLayer.Objects)
            {
                Entity crateEntity = new Entity("coin_" + (i++)) { Tag = "coin" }
                .AddComponent(new Transform2D() { LocalPosition = new Vector2(obj.X, obj.Y), Origin = Vector2.Center, DrawOrder = -9 })
                .AddComponent(new SpriteAtlas(WaveContent.Assets.coin_spritesheet))
                .AddComponent(new Animation2D() { PlayAutomatically = true, CurrentAnimation = "Flip" })
                .AddComponent(new SpriteAtlasRenderer(DefaultLayers.Alpha, AddressMode.PointClamp))
                .AddComponent(new CircleCollider2D())
                ;

                this.EntityManager.Add(crateEntity);
            }
        }

        /// <summary>
        /// Add physics crates
        /// </summary>
        private void AddCrates()
        {
            var cratesLayer = this.tiledMap.ObjectLayers["Crates"];

            var sceneTiles = this.tiledMap.Tilesets.Where(t => t.Name == "sceneTiles").First();
            var createRectangle = TiledMapUtils.GetRectangleTileByID(sceneTiles, CrateTileID);

            int i = 0;
            foreach (var obj in cratesLayer.Objects)
            {
                Entity crateEntity = new Entity("crate_" + (i++)) { Tag = "crate" }
                .AddComponent(new Transform2D() { LocalPosition = new Vector2(obj.X, obj.Y), Rotation = (float)obj.Rotation, DrawOrder = -9 })
                .AddComponent(new Sprite(WaveContent.Assets.tilesets.sceneTiles_png) { SourceRectangle = createRectangle })
                .AddComponent(new SpriteRenderer(DefaultLayers.Alpha, AddressMode.PointWrap))
                .AddComponent(new RectangleCollider2D()
                {
                    Density = 0.3f,
                })
                .AddComponent(new RigidBody2D());

                this.EntityManager.Add(crateEntity);
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

                colliderEntity.AddComponent(new RigidBody2D() { PhysicBodyType  = WaveEngine.Common.Physics2D.RigidBodyType2D.Static });

                this.EntityManager.Add(colliderEntity);
            }
        }

        private void StartMusic()
        {
            MusicInfo musicInfo = new MusicInfo(WaveContent.Assets.Music.Ozzed___A_Well_Worked_Analogy_mp3);
            WaveServices.MusicPlayer.IsRepeat = true;
            WaveServices.MusicPlayer.Volume = 0.6f;
            WaveServices.MusicPlayer.Play(musicInfo);
        }
    }
}
