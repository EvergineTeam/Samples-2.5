using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Materials;
using WaveOculusDemoProject.Layers;

namespace WaveOculusDemoProject.Components
{
    /// <summary>
    /// This behavior moves each radar ticks in the HUD radar.
    /// </summary>
    public class RadarBehavior : Behavior
    {
        private const float radarSize = 0.113f;
        private const float decay = 0.9f;
        private const float decayInv = 1 - decay;
        private const float radarRange = 750;
        private const float radarRangeSqr = radarRange * radarRange;

        private ScreenplayManager screenplay;
        private Color[] colors = new Color[] { Color.Transparent, new Color("398792"), new Color("ff5b32") };

        [RequiredComponent]
        private Transform3D transform;

        private List<Transform3D> radarTicks = new List<Transform3D>();
        private List<BasicMaterial> radarTickMaterials = new List<BasicMaterial>();

        /// <summary>
        /// Initializes the radar behavior
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            this.screenplay = this.EntityManager.Find("ScreenplayManager").FindComponent<ScreenplayManager>();

            foreach (var fighter in this.screenplay.FighterList)
            {
                if (fighter.State == FighterState.Player)
                {
                    continue;
                }

                Transform3D radarTickTransform;
                BasicMaterial radarTickMaterial;
                Entity radarTick = new Entity()
                .AddComponent(radarTickTransform = new Transform3D())
                .AddComponent(new Model("Content/Models/Hud/HudRadarTick.FBX"))
                .AddComponent(new ModelRenderer())
                .AddComponent(new MaterialsMap(radarTickMaterial = new BasicMaterial("Content/Textures/Hud/RadarTick.png", typeof(CockpitAdditiveLayer))
                    {
                        DiffuseColor = colors[(int)fighter.State]
                    }))
                ;

                radarTick.Enabled = false;

                this.Owner.AddChild(radarTick);
                this.radarTicks.Add(radarTickTransform);
                this.radarTickMaterials.Add(radarTickMaterial);
            }
        }

        /// <summary>
        /// Updates the behavior. Place each radar ticks
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Update(TimeSpan gameTime)
        {
            for (int i = 0; i < this.radarTicks.Count; i++)
            {
                var fighter = this.screenplay.FighterList[i];
                Transform3D fighterTransform = fighter.Transform;
                Transform3D radarTransform = this.radarTicks[i];

                Vector3 distance = fighterTransform.Position - this.transform.Position;
                float distanceLength = distance.Length();

                if (distanceLength < radarRange)
                {
                    BasicMaterial material = radarTickMaterials[i];
                    if (!radarTransform.Owner.Enabled)
                    {
                        radarTransform.Owner.Enabled = true;
                    }

                    float lerp = distanceLength / radarRange;

                    float atenuationColor = (lerp < decay) ? 1 : 1 - ((lerp - decay) / decayInv);

                    Color color = this.colors[(int)fighter.State] * atenuationColor;

                    radarTransform.LocalPosition = (distance / radarRange) * radarSize;
                    material.DiffuseColor = color;
                }
                else if (radarTransform.Owner.Enabled)
                {
                    radarTransform.Owner.Enabled = false;
                }

            }
        }
    }
}
