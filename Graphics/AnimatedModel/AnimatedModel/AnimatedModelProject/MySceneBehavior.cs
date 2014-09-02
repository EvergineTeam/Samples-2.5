using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveEngine.Common.Math;
using WaveEngine.Framework;

namespace AnimatedModelProject
{
    public class MySceneBehavior : SceneBehavior
    {
        private MyScene myScene;

        protected override void ResolveDependencies()
        {
            this.myScene = this.Scene as MyScene;
        }

        protected override void Update(TimeSpan gameTime)
        {
            Matrix footMatrix;
            bool changed = this.myScene.skinned.TryGetBoneWorldTransform("Bip01_L_Foot", out footMatrix);
            footMatrix.Translation = new Vector3(10, 0, 0);
        }
    }
}
