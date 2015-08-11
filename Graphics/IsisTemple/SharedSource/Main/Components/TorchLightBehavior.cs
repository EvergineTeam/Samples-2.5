#region Using Statements
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
using WaveEngine.Materials;
#endregion

namespace IsisTemple.Components
{
    /// <summary>
    /// Behavior that simulate the torch light
    /// </summary>
    [DataContract]
    public class TorchLightBehaviour : Behavior
    {

        #region Variables
        [RequiredComponent]
        protected PointLightProperties light;

        private bool initialized = false;
        private TimeSpan ellapsedGameTime;
        private TimeSpan startColorTime;
        private TimeSpan nextColorTime;
        private float period;

        private Vector3 refColor;
        private Color prevColor;
        private Color nextColor;
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
                this.refColor = light.Color.ToVector3();
                this.initialized = true;
            }

            ellapsedGameTime += gameTime;

            if (this.ellapsedGameTime > this.nextColorTime)
            {
                light.Color = this.nextColor;

                float r = ((float)WaveServices.Random.NextDouble() * 0.15f + 0.85f);
                float att = ((float)WaveServices.Random.NextDouble() * 0.3f + 0.7f);
                this.period = ((float)WaveServices.Random.NextDouble() * 0.2f + 0.05f);

                this.prevColor = this.nextColor;
                this.nextColor = new Color(
                    this.refColor.X * ((float)WaveServices.Random.NextDouble() * 0.05f + 0.95f),
                    this.refColor.Y * r,
                    this.refColor.Z * r) * att;
                this.startColorTime = this.ellapsedGameTime;
                this.nextColorTime = this.ellapsedGameTime + TimeSpan.FromSeconds(this.period);
            }
            else
            {
                float lerp = (float)(this.ellapsedGameTime - this.startColorTime).TotalSeconds / this.period;

                this.light.Color = this.prevColor * (1 - lerp) + this.nextColor * (lerp);
            }
        }
        #endregion
    }
}
