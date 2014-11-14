using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveEngine.Framework;
using WaveOculusDemoProject.Audio;

namespace WaveOculusDemoProject.Entities
{
    /// <summary>
    /// Sound manager entity decorator
    /// </summary>
    public class SoundManagerDecorator : BaseDecorator
    {
        /// <summary>
        /// Instantiate the sound manager
        /// </summary>
        /// <param name="name">The entity name</param>
        public SoundManagerDecorator(string name)
        {
            this.entity = new Entity(name)
            .AddComponent(new SoundManager());
        }
    }
}
