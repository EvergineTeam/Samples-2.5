using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Materials;

namespace WaveOculusDemoProject.Components
{
    /// <summary>
    /// Shockwave behavior
    /// </summary>
    [DataContract]
    public class ShockwaveBehavior : Behavior
    {
        private const float MaxScale = 200;
        private const double DefaultTimeLeft = 1f;

        [RequiredComponent]
        private Transform3D transform;

        [RequiredComponent]
        private MaterialComponent materialComponent;

        private StandardMaterial material;    
        private double timeLeft;
        
        /// <summary>
        /// Instantiate the shockwave behavior
        /// </summary>
        public void StartShockWave()
        {
            this.timeLeft = DefaultTimeLeft;
            this.IsActive = true;
            this.Owner.IsVisible = true;
        }

        /// <summary>
        /// Initializes the shockwave behavior
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            this.material = this.materialComponent.Material as StandardMaterial;
        }

        /// <summary>
        /// Updates the shockwave
        /// </summary>
        /// <param name="gameTime">The current game time</param>
        protected override void Update(TimeSpan gameTime)
        {
            if(this.timeLeft <= 0)
            {
                this.IsActive = false;
                this.Owner.IsVisible = false;
                return;
            }

            this.timeLeft -= gameTime.TotalSeconds;
            
            float lerp = Math.Max((float)(this.timeLeft / DefaultTimeLeft), 0);

            this.material.DiffuseColor = Color.White * lerp;
            this.transform.LocalScale = Vector3.One * MaxScale * (1 - lerp);
        }
    }
}
