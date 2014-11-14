using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveEngine.Common.Math;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveOculusDemoProject.Audio;

namespace WaveOculusDemoProject.Components
{
    /// <summary>
    /// This class control a space fighter
    /// </summary>
    public class FighterController : Behavior
    {
        private List<Vector3> trailPositions;
        private List<TrailSetting> trails;
        private List<ProjectileEmitter> guns;
        private SoundType engineAudio;
        private SoundType shootAudio;

        [RequiredComponent]
        private Transform3D transform3D;

        [RequiredComponent]
        private TrailManager trailManager;

        [RequiredComponent]
        private MaterialsMap materialsMap;

        [RequiredComponent]
        private AnimatedParamBehavior laserTrigger;

        private Sound3DEmitter engineSoundEmitter;
        private Sound3DEmitter gunsSoundEmitter;

        private bool lastVisible;
        private Sound3DInstance engineInstance;
        private int nShoots = 0;

        private FighterState state;


        /// <summary>
        /// Instantiate a new fighter controller
        /// </summary>
        /// <param name="state">The fighter state (enemy, player, wingman...)</param>
        /// <param name="trailPositions">List of trails position</param>
        /// <param name="engineAudio">Engine audio type</param>
        /// <param name="shootAudio">Gun shoot audio type</param>
        public FighterController(FighterState state, List<Vector3> trailPositions, SoundType engineAudio, SoundType shootAudio)
        {
            this.state = state;
            this.trailPositions = trailPositions;
            this.engineAudio = engineAudio;
            this.shootAudio = shootAudio;
        }

        /// <summary>
        /// Initializes the fighter controller
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            // Init engine trails
            this.trails = new List<TrailSetting>();
            for (int i = 0; i < this.trailPositions.Count; i++)
            {
                var trailPosition = this.trailPositions[i];
                Transform3D trailTransform;

                Entity trailEntity = new Entity("trail_" + i)
                    .AddComponent(trailTransform = new Transform3D() { LocalPosition = trailPosition });

                this.Owner.AddChild(trailEntity);

                TrailSetting trail = this.trailManager.GetFreeTrail(trailTransform);

                trail.Thickness = 1;
                trail.ExpirationTime = 1f;
                trail.TimeStep = 0.02;
            }

            // Register fighter in screenplay manager
            var screenplay = this.EntityManager.Find("ScreenplayManager").FindComponent<ScreenplayManager>();
            screenplay.RegisterFighter(new FighterSetting()
                {
                    State = this.state,
                    Transform = transform3D
                });


            // Gets all fighter guns 
            this.guns = new List<ProjectileEmitter>();
            var gunEntities = this.Owner.ChildEntities.Where(e => e.Tag == "Gun");
            foreach (var gunEntity in gunEntities)
            {
                var projectileEmitter = gunEntity.FindComponent<ProjectileEmitter>();
                this.guns.Add(projectileEmitter);

                projectileEmitter.OnShoot += projectileEmitter_OnShoot;
            }

            this.laserTrigger.OnActionChange += laserTrigger_OnActionChange;


            // Load sound emitters
            var engineSoundEntity = this.Owner.FindChild("engineSoundEmitter");
            if (engineSoundEntity == null)
            {
                engineSoundEntity = this.Owner;
            }

            this.engineSoundEmitter = engineSoundEntity.FindComponent<Sound3DEmitter>();

            var gunsSoundEntity = this.Owner.FindChild("gunsSoundEmitter");
            if (gunsSoundEntity == null)
            {
                gunsSoundEntity = this.Owner;
            }

            this.gunsSoundEmitter = gunsSoundEntity.FindComponent<Sound3DEmitter>();

        }

        /// <summary>
        /// A projectile has been emitted
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void projectileEmitter_OnShoot(object sender, EventArgs e)
        {
            if (nShoots % 2 == 0)
            {
                // Play shoot sound 
                this.gunsSoundEmitter.Play(this.shootAudio, 1, false);
            }

            nShoots++;
        }

        /// <summary>
        /// The laser trigger value has been changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void laserTrigger_OnActionChange(object sender, bool e)
        {
            if (e)
            {
                foreach (var emitter in this.guns)
                {
                    emitter.StarFiring();
                }
            }
            else
            {
                foreach (var emitter in this.guns)
                {
                    emitter.StopFiring();
                }
            }
        }

        /// <summary>
        /// Update visibility of the fighter
        /// </summary>
        /// <param name="gameTime">The current game time</param>
        protected override void Update(TimeSpan gameTime)
        {
            if (this.lastVisible != this.Owner.IsVisible)
            {
                if (this.Owner.IsVisible)
                {
                    this.engineInstance = this.engineSoundEmitter.Play(this.engineAudio, 1, true);
                }
                else if (this.engineInstance != null)
                {
                    this.engineInstance.Stop();
                }

                this.lastVisible = this.Owner.IsVisible;
            }
        }
    }
}
