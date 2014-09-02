using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IsisTempleProject.Components
{
    #region Using Statements

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using WaveEngine.Common.Graphics;
    using WaveEngine.Common.Math;
    using WaveEngine.Components.Graphics3D;
    using WaveEngine.Framework;
    using WaveEngine.Framework.Graphics;
    using WaveEngine.Framework.Services;
    using WaveEngine.Materials;
    #endregion

    /// <summary>
    /// Behavior that simulate the torch light
    /// </summary>
    public class TorchLightBehaviour : Behavior
    {

        #region Variables
        [RequiredComponent]
        public PointLightProperties light;

        private bool initialized = false;
        private TimeSpan ellapsedGameTime;
        private TimeSpan startColorTime;
        private TimeSpan nextColorTime;
        private float period; 
        #endregion

        #region Properties
        private Vector3 RefColor { get; set; }
        private Color PrevColor { get; set; }
        private Color NextColor { get; set; }
        #endregion

        #region Initlalize
        /// <summary>
        /// Initializes a new instance of the <see cref="TorchLightBehaviour"/> class.
        /// </summary>
        public TorchLightBehaviour()
            : base("TorchLightBehaviour")
        {
        } 
        #endregion

        #region Public Methods
        /// <summary>
        /// Updates the light color like a torch
        /// </summary>
        /// <param name="gameTime">Time of the game </param>
        protected override void Update(TimeSpan gameTime)
        {
            if (!this.initialized)
            {
                this.RefColor = light.Color.ToVector3();
                this.initialized = true;
            }

            ellapsedGameTime += gameTime;

            if (this.ellapsedGameTime > this.nextColorTime)
            {
                light.Color = this.NextColor;

                float r = ((float)WaveServices.Random.NextDouble() * 0.15f + 0.85f);
                float att = ((float)WaveServices.Random.NextDouble() * 0.3f + 0.7f);
                this.period = ((float)WaveServices.Random.NextDouble() * 0.2f + 0.05f);

                this.PrevColor = this.NextColor;
                this.NextColor = new Color(
                    this.RefColor.X * ((float)WaveServices.Random.NextDouble() * 0.05f + 0.95f),
                    this.RefColor.Y * r,
                    this.RefColor.Z * r) * att;
                this.startColorTime = this.ellapsedGameTime;
                this.nextColorTime = this.ellapsedGameTime + TimeSpan.FromSeconds(this.period);
            }
            else
            {
                float lerp = (float)(this.ellapsedGameTime - this.startColorTime).TotalSeconds / this.period;

                this.light.Color = this.PrevColor * (1 - lerp) + this.NextColor * (lerp);
            }
        }
        #endregion
    }
}
