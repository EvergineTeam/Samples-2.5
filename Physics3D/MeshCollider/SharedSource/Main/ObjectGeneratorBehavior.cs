using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
using WaveRandom = WaveEngine.Framework.Services.Random;

namespace MeshCollider
{
    [DataContract]
    public class ObjectGeneratorBehavior : Behavior
    {
        [RequiredService]
        protected WaveRandom randomService;

        private int objectCount;

        private TimeSpan elapsed;

        [DataMember]
        public int MaxObjectsCount { get; set; }

        [DataMember]
        public TimeSpan Interval { get; set; }

        [DataMember]
        public float GenerationRadius { get; set; }

        public float GenerationAltitude { get; set; }

        protected override void DefaultValues()
        {
            base.DefaultValues();

            this.MaxObjectsCount = 50;
            this.Interval = TimeSpan.FromSeconds(1);
            this.GenerationRadius = 8;
            this.GenerationAltitude = 5;
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.objectCount = 0;
            this.elapsed = TimeSpan.Zero;
        }

        protected override void Update(TimeSpan gameTime)
        {
            this.elapsed += gameTime;

            if (this.elapsed >= this.Interval)
            {
                this.elapsed = TimeSpan.Zero;

                this.GenerateObject();

                if (this.objectCount >= this.MaxObjectsCount)
                {
                    this.IsActive = false;
                }
            }
        }

        private void GenerateObject()
        {
            Entity newEntity;

            switch (this.objectCount % 3)
            {
                case 0:
                    newEntity = this.EntityManager.Instantiate(WaveContent.Assets.cube);
                    break;
                case 1:
                    newEntity = this.EntityManager.Instantiate(WaveContent.Assets.sphere);
                    break;
                case 2:
                    newEntity = this.EntityManager.Instantiate(WaveContent.Assets.capsule);
                    break;
                default:
                    throw new InvalidOperationException("Invalid type.");
            }

            newEntity.Name += this.objectCount;

            var transform = newEntity.FindComponent<Transform3D>();
            var coordinate = this.randomService.InsideUnitCircle() * this.GenerationRadius;
            transform.Position = new Vector3(coordinate.X, this.GenerationAltitude, coordinate.Y);

            this.EntityManager.Add(newEntity);

            this.objectCount++;
        }
    }
}
