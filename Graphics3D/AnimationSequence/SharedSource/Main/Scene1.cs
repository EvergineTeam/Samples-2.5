#region Using Statements
using AnimationSequence.Animations;
using System;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;
using WaveEngine.Components.GameActions;
#endregion

namespace AnimationSequence
{
    public class Scene1 : Scene
    {
        IGameAction animationSequence;

        protected override void CreateScene()
        {
            this.Load(WaveContent.Scenes.Scene1);

            var cube = this.EntityManager.Find("cube");
            this.animationSequence = new MoveTo3DGameAction(cube, new Vector3(0, 2, 0), TimeSpan.FromSeconds(1.5), EaseFunction.SineInOutEase)
                .ContinueWith(new RotateTo3DGameAction(cube, new Vector3(0, (float)Math.PI * 2, 0), TimeSpan.FromSeconds(1.5), EaseFunction.SineInOutEase));
        }

        protected override void Start()
        {
            base.Start();

            this.animationSequence.Run();
        }
    }
}
