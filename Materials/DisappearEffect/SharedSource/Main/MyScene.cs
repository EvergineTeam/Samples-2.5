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
using WaveEngine.Components.GameActions;
#endregion

namespace DisappearEffect
{
    public class MyScene : Scene
    {
        private Entity teapot;
        private DisappearMaterial disappearMaterial;

        protected override void CreateScene()
        {
            this.Load(WaveContent.Scenes.MyScene);

            this.teapot = this.EntityManager.Find("teapot");
            this.disappearMaterial = new DisappearMaterial(WaveContent.Assets.Textures.tile1_png,
                                                                        WaveContent.Assets.Textures.Noise_png,
                                                                        WaveContent.Assets.Textures.Burn_png);
            teapot.FindComponent<MaterialsMap>().DefaultMaterial = disappearMaterial;
        }

        protected override void Start()
        {
            base.Start();

            IGameAction action = this.CreateDelayGameAction(TimeSpan.FromSeconds(1.0f))
                                 .ContinueWith(new FloatAnimationGameAction(this.teapot, 0.0f, 255.0f, TimeSpan.FromSeconds(1.5f), EaseFunction.None, (v) =>
                                 {
                                     this.disappearMaterial.Threshold = v;
                                 }))
                                 .ContinueWith(new FloatAnimationGameAction(this.teapot, 255.0f, 0.0f, TimeSpan.FromSeconds(1.5f), EaseFunction.None, (v) =>
                                 {
                                     this.disappearMaterial.Threshold = v;
                                 }));
            
            action.Run();                                 
        }
    }
}
