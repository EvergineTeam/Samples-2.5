#region Using Statements
using System;
using System.Linq;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Components.Shared.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Resources;
using WaveEngine.Framework.Services;
#endregion

namespace Billboard
{
    public class MyScene : Scene
    {
        protected override void CreateScene()
        {
            this.Load(@"Content/Scenes/MyScene.wscene");

            this.CustomizeTree();
            this.CustomizeFlares();
        }

        private void CustomizeFlares()
        {
            var flares = this.EntityManager.FindAllByTag("flares");

            if (flares == null || flares.Count() == 0)
            {
                return;
            }

            var index = 0;

            foreach (var flare in flares)
            {
                var flareEntity = flare as Entity;
                var angle = ((float)index++ / 6) * MathHelper.TwoPi;

                if (flareEntity != null)
                {
                    // TODO: Editor crashes when adding this behavior from the it-self
                    flareEntity.AddComponent(new FlareBehavior { Angle = angle, Speed = 1 });
                }
            }
        }

        private void CustomizeTree()
        {
            var treeBillboard = this.EntityManager.Find("treeBillboard");

            if (treeBillboard == null)
            {
                return;
            }

            // TODO: We need to recreate the renderer since cannot access to sampler mode 
            //       within the Editor
            var billboardRenderer = new BillboardRenderer(DefaultLayers.Alpha,
                AddressMode.PointClamp);

            treeBillboard.RemoveComponent<BillboardRenderer>();
            treeBillboard.AddComponent(billboardRenderer);
            treeBillboard.RefreshDependencies();
        }
    }
}
