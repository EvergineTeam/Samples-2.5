using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics3D;
using WaveEngine.Materials;
using WaveOculusDemoProject.Components;

namespace WaveOculusDemoProject.Entities
{
    /// <summary>
    /// Asteroid field entity decorator
    /// </summary>
    public class AsteroidFieldDecorator : BaseDecorator
    {
        /// <summary>
        /// Instantiate a new asteroid field decorator
        /// </summary>
        /// <param name="name">The asteroid name</param>
        /// <param name="sectorSize">The sector size</param>
        public AsteroidFieldDecorator(string name, Vector3 sectorSize)
        {
            this.entity = new Entity(name)
            .AddComponent(new AsteroidFieldController(sectorSize))
            ;
        }
    }
}
