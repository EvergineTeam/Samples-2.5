using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;

namespace DolbySampleProject.Behaviors
{
    public class BlinkBehavior : Behavior
    {
        private const int Min = 50;
        private const int Max = 200;

        private TimeSpan time;
        private Transform2D ownerTransform;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlinkBehavior"/> class.
        /// </summary>
        public BlinkBehavior()
        {
            time = TimeSpan.FromMilliseconds(WaveServices.Random.Next(Min, Max));
        }

        /// <summary>
        /// Resolves the dependencies needed for this instance to work.
        /// </summary>
        protected override void ResolveDependencies()
        {
            base.ResolveDependencies();
            this.ownerTransform = this.Owner.Parent.FindComponent<Transform2D>();
        }

        /// <summary>
        /// Allows this instance to execute custom logic during its 
        /// <c>Update</c>.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        /// <remarks>
        /// This method will not be executed if the 
        /// <see cref="T:WaveEngine.Framework.Component" />, or the 
        /// <see cref="T:WaveEngine.Framework.Entity" />
        ///             owning it are not 
        /// <c>Active</c>.
        /// </remarks>
        protected override void Update(TimeSpan gameTime)
        {
            // Only works with dolby
            if (WaveServices.MusicPlayer.IsDolbyEnabled)
            {
                time -= gameTime;

                if (time < TimeSpan.Zero)
                {
                    time = TimeSpan.FromMilliseconds(WaveServices.Random.Next(Min, Max));
                    
                    this.Owner.IsVisible = !this.Owner.IsVisible;

                    // more speaker FX!!
                    if (this.Owner.IsVisible)
                    {
                        this.ownerTransform.XScale = 1.005f;
                        this.ownerTransform.YScale = 1.005f;
                    }
                    else
                    {
                        this.ownerTransform.XScale = 1;
                        this.ownerTransform.YScale = 1;
                    }
                }
            }
        }
    }
}
