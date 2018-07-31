#region Using Statements
using StickyProjectiles.Behaviors;
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
#endregion

namespace StickyProjectiles
{
    public class MyScene : Scene
    {
        protected override void CreateScene()
        {
            this.PhysicsManager.Simulation2D.DrawFlags |= WaveEngine.Common.Physics2D.DebugDrawFlags.CenterOfMassBit;
            this.Load(WaveContent.Scenes.MyScene);
        }
    }
}
