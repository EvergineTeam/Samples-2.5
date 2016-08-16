using System;
using System.Runtime.Serialization;
using WaveEngine.Common.Attributes;
using WaveEngine.Framework;
using WaveEngine.Framework.Models.Assets;
using WaveEngine.Framework.Services;

namespace Tumbler
{
    [DataContract]
    public class EmitterBehavior : Behavior
    {
        /// <summary>
        /// The interval in seconds of the emitter
        /// </summary>
        [DataMember]
        public float Interval { get; set; }

        /// <summary>
        /// Maximum element number
        /// </summary>
        [DataMember]
        public int MaxElements { get; set; }

        /// <summary>
        /// The first item type
        /// </summary>
        [DataMember]
        [RenderPropertyAsAsset(AssetType.Prefab)]
        public string Item1 { get; set; }

        /// <summary>
        /// The second item type
        /// </summary>
        [DataMember]
        [RenderPropertyAsAsset(AssetType.Prefab)]
        public string Item2 { get; set; }

        /// <summary>
        /// The third item type
        /// </summary>
        [DataMember]
        [RenderPropertyAsAsset(AssetType.Prefab)]
        public string Item3 { get; set; }

        private double counter;

        private int instances;

        /// <summary>
        /// The update method
        /// </summary>
        /// <param name="gameTime">The ellapsed game time</param>
        protected override void Update(TimeSpan gameTime)
        {
            this.counter += gameTime.TotalSeconds;

            if (this.counter > this.Interval)
            {
                if (this.instances < this.MaxElements)
                {
                    this.counter -= this.Interval;

                    var next = WaveServices.Random.Next(0, 4);

                    Entity entity;

                    switch (next)
                    {
                        case 0:
                            entity = this.EntityManager.Instantiate(this.Item1);
                            break;
                        case 1:
                            entity = this.EntityManager.Instantiate(this.Item2);
                            break;
                        case 2:
                        default:
                            entity = this.EntityManager.Instantiate(this.Item3);
                            break;
                    }

                    entity.Name = "instance_" + this.instances++;

                    this.EntityManager.Add(entity);
                }
                else
                {
                    this.IsActive = false;
                }
            }
        }
    }
}
