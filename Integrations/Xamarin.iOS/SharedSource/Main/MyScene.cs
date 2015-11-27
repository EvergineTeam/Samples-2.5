#region Using Statements
using System;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Resources;
using WaveEngine.Framework.Services;
using WaveEngine.Materials;
#endregion

namespace Sample
{
    public class MyScene : Scene
    {
        private Spinner spinner;

        protected override void CreateScene()
        {
            this.Load(WaveContent.Scenes.MyScene);

            this.spinner = new Spinner { IncreaseX = 1, IncreaseY = 1, IncreaseZ = 1 };
            var green = new Color(76, 217, 100);
            var cube = new Entity().AddComponent(Model.CreateCube())
                                   .AddComponent(new ModelRenderer())
                                   .AddComponent(new MaterialsMap(new StandardMaterial() { DiffuseColor = green, LightingEnabled = true }))
                                   .AddComponent(new Transform3D())
                                   .AddComponent(this.spinner);
            this.EntityManager.Add(cube);
        }

        internal void UpdateAutoRotation(bool enabled)
        {
            this.spinner.IsActive = enabled;
        }
    }
}
