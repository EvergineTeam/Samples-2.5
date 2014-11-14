using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveEngine.Common.Graphics;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Materials;
using WaveOculusDemoProject.Components;
using WaveOculusDemoProject.Layers;

namespace WaveOculusDemoProject.Entities
{
    /// <summary>
    /// Starfield entity decorator
    /// </summary>
    public class StarfieldDecorator : BaseDecorator
    {
        /// <summary>
        /// Instantiate Starfield
        /// </summary>
        /// <param name="name">The entity name</param>
        public StarfieldDecorator(string name)
        {
            this.entity = new Entity(name)
            .AddComponent(new Transform3D())
            .AddComponent(new FollowCameraBehavior())
            .AddComponent(new MaterialsMap(new BasicMaterial("Content/Textures/StarField.wpk", typeof(StarfieldLayer))
            {
                VertexColorEnabled = true,
                SamplerMode = AddressMode.LinearClamp 
            }))

            .AddComponent(new StarfieldRenderer(10000))
            ;
        }
    }
}
