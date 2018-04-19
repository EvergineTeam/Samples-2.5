using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMap.Components;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Diagnostic;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics2D;
using WaveEngine.Framework.Services;

namespace TiledMap
{
    public class GameplaySceneBehavior : SceneBehavior
    {
        private List<Entity> crates;
        private List<Entity> coins;
        private Entity player;

        private Vector2 initPlayerPosition;
        private List<Vector2> initCratePositions;
        private List<Vector2> initCoinPositions;

        private List<Collider2D> coinColliders;
        private List<Collider2D> trapColliders;
        private Collider2D endCollider;

        private SoundManager soundManager;


        private PlayerController playerController;

        bool isInitialized = false;

        protected override void ResolveDependencies()
        {

        }

        private void Initialize()
        {
            this.soundManager = this.soundManager = this.Scene.EntityManager.Find("SoundManager").FindComponent<SoundManager>();

            this.InitPlayer();
            this.InitCoins();
            this.InitTraps();
            this.InitCrates();
            this.InitEnd();
        }

        private void InitPlayer()
        {
            this.player = this.Scene.EntityManager.Find("Player");
            this.playerController = this.player.FindComponent<PlayerController>();
            this.initPlayerPosition = this.player.FindComponent<Transform2D>().Position;
        }

        private void InitEnd()
        {
            var endEntity = this.Scene.EntityManager.Find("End");
            if (endEntity != null)
            {
                this.endCollider = endEntity.FindComponent<Collider2D>(false);
            }
        }

        private void InitCoins()
        {
            this.coins = new List<Entity>();
            this.coinColliders = new List<Collider2D>();
            this.initCoinPositions = new List<Vector2>();
            var result = this.Scene.EntityManager.FindAllByTag("coin");
            foreach (var coin in result)
            {
                Entity coinEntity = coin as Entity;

                this.coins.Add(coinEntity);
                this.coinColliders.Add(coinEntity.FindComponent<Collider2D>(false));
                this.initCoinPositions.Add(coinEntity.FindComponent<Transform2D>().Position);
            }
        }

        private void InitCrates()
        {
            this.crates = new List<Entity>();
            this.initCratePositions = new List<Vector2>();
            var result = this.Scene.EntityManager.FindAllByTag("crate");
            foreach (var crate in result)
            {
                Entity crateEntity = crate as Entity;
                this.crates.Add(crateEntity);
                this.initCratePositions.Add(crateEntity.FindComponent<Transform2D>().Position);
            }

            WaveServices.TimerFactory.CreateTimer("cratesounds", TimeSpan.FromSeconds(2), () =>
            {
                foreach (var crate in result)
                {
                    Entity crateEntity = crate as Entity;
                    var collider = crateEntity.FindComponent<Collider2D>(false);
                    collider.BeginCollision += Collider_BeginCollision;
                }

            }, false);
        }

        private void InitTraps()
        {
            this.trapColliders = new List<Collider2D>();
            var traps = this.Scene.EntityManager.FindAllByTag("trap");
            foreach (var trap in traps)
            {
                Entity trapEntity = trap as Entity;
                this.trapColliders.Add(trapEntity.FindComponent<Collider2D>(false));
            }
        }

        protected override void Update(TimeSpan gameTime)
        {
            if (!this.isInitialized)
            {
                this.Initialize();
                this.isInitialized = true;
            }

            this.CheckCoins();
            this.CheckTraps();
            this.CheckEnd();
        }

        private void CheckEnd()
        {
            if (this.playerController.Collider.Intersects(this.endCollider))
            {
                this.Win();
            }

        }

        private void CheckCoins()
        {
            for (int i = coinColliders.Count - 1; i >= 0; i--)
            {
                Collider2D coinCollider = this.coinColliders[i];
                if (coinCollider.Owner.Enabled && this.playerController.Collider.Intersects(coinCollider))
                {
                    coinCollider.Owner.Enabled = false;
                    this.soundManager.PlaySound(SoundType.Coin);
                }
            }
        }

        private void CheckTraps()
        {
            for (int i = trapColliders.Count - 1; i >= 0; i--)
            {
                Collider2D trapCollider = this.trapColliders[i];
                if (this.playerController.Collider.Intersects(trapCollider))
                {
                    this.Defeat();
                }
            }
        }

        private void Win()
        {
            this.soundManager.PlaySound(SoundType.Victory);
            this.ResetGame();
        }

        private void Defeat()
        {
            this.soundManager.PlaySound(SoundType.Crash);
            this.ResetGame();
        }

        private void Collider_BeginCollision(WaveEngine.Common.Physics2D.ICollisionInfo2D contact)
        {
            var velocity = contact.ColliderB.RigidBody.LinearVelocity;
            float length = velocity.Length();
            float volume = Math.Min(1, length / 5);

            var instance = this.soundManager.PlaySound(SoundType.CrateDrop, volume);
        }

        private void ResetGame()
        {
            this.playerController.Reset();

            foreach (var coin in this.coins)
            {
                coin.Enabled = true;
            }

            for (int i = 0; i < this.crates.Count; i++)
            {
                var crate = this.crates[i];

                var crateBody = crate.FindComponent<RigidBody2D>();
                crateBody.ResetPosition(this.initCratePositions[i]);
                crateBody.Transform2D.Rotation = 0;
            }
        }
    }
}
