using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveEngine.Framework;
using WaveOculusDemoProject.Components;

namespace WaveOculusDemoProject.Entities
{
    /// <summary>
    /// Screenplay entity decorator
    /// </summary>
    public class ScreenplayManagerDecorator : BaseDecorator
    {
        public double CurrentFrameTime
        {
            get { return this.entity.FindComponent<ScreenplayManager>().CurrentFrameTime; }
            set { this.entity.FindComponent<ScreenplayManager>().CurrentFrameTime = value; }
        }

        public float Fps
        {
            get { return this.entity.FindComponent<ScreenplayManager>().Fps; }
            set { this.entity.FindComponent<ScreenplayManager>().Fps = value; }
        }

        /// <summary>
        /// Instantiates the new screenplay decorator
        /// </summary>
        /// <param name="name">The entity name</param>
        public ScreenplayManagerDecorator(string name)
        {
            this.entity = new Entity(name)
            .AddComponent(new ScreenplayManager());
        }
    }
}
