
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics3D;
using WaveEngine.Materials;
using WaveOculusDemo;

namespace WaveOculusDemoProject.Entities
{
    /// <summary>
    /// Asteroid sector entity decorator
    /// </summary>
    public class AsteroidSectorDecorator : BaseDecorator
    {
        private Transform3D transform;

        public Transform3D Transform
        {
            get
            {
                return this.transform;
            }
        }

        /// <summary>
        /// Instantiates a new asteroid sector
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="sectorType">Sector type</param>
        /// <param name="lod">The LOD level</param>
        public AsteroidSectorDecorator(string name, int sectorType, int lod)
        {
            this.entity = new Entity(name)            
            .AddComponent(this.transform = new Transform3D())
            .AddComponent(new Model(string.Format("Content/Assets/Models/Asteroids/Asteroids_{0}_{1}.FBX", sectorType, lod)))
            .AddComponent(new ModelRenderer())
            .AddComponent(new BoxCollider3D())
            .AddComponent(new MaterialsMap()
            {
                DefaultMaterialPath = WaveContent.Assets.Materials.AsteroidMat
            })
            ;
        }
    }
}
