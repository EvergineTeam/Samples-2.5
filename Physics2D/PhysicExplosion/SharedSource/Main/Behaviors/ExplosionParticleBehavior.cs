#region Using Statements
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Framework;
#endregion

namespace PhysicExplosion.Behaviors
{
    [DataContract]
    public class ExplosionParticleBehavior : Behavior
    {
        private int lifeSpan;
        private TimeSpan time;
        public bool Killed;

        #region Properties

        /// <summary>
        /// Gets or sets Life span
        /// </summary>
        [DataMember]
        public int LifeSpan
        {
            get { return this.lifeSpan; }
            set
            {
                this.lifeSpan = value;
                this.time = TimeSpan.FromMilliseconds(this.LifeSpan);
            }
        }

        #endregion

        /// <summary>
        /// Default values method
        /// </summary>
        protected override void DefaultValues()
        {
            base.DefaultValues();

            this.LifeSpan = 800;
            this.Killed = false;            
        }

        /// <summary>
        /// Update method
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Update(TimeSpan gameTime)
        {
            this.time -= gameTime;

            if (this.time <= TimeSpan.Zero)
            {
                this.Killed = true;
                return;
            }
        }
    }
}
