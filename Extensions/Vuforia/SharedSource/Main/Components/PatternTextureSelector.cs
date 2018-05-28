using System;
using System.Runtime.Serialization;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;
using WaveEngine.Materials;
using WaveEngine.Vuforia;

namespace Vuforia.Components
{
    [DataContract]
    public class PatternTextureSelector : Behavior
    {
        [RequiredService]
        protected VuforiaService vuforiaService;

        [RequiredComponent]
        protected MaterialComponent materialComponent;

        private int lastWorldCenterId;

        protected override void DefaultValues()
        {
            base.DefaultValues();
            this.lastWorldCenterId = -1;
        }

        protected override void Update(TimeSpan gameTime)
        {
            var worldCenterResult = this.vuforiaService.WorldCenterResult;

            if (worldCenterResult != null &&
                worldCenterResult.Id != this.lastWorldCenterId)
            {
                var material = this.materialComponent.Material as StandardMaterial;
                this.lastWorldCenterId = worldCenterResult.Id;

                switch (worldCenterResult.Trackable.Name)
                {
                    case "chips":
                        material.Diffuse1Path = WaveContent.Assets.Textures.chips_scaled_jpg;
                        break;
                    case "stones":
                        material.Diffuse1Path = WaveContent.Assets.Textures.stones_scaled_jpg;
                        break;
                    default:
                        material.Diffuse1 = null;
                        break;
                }
            }
        }
    }
}
