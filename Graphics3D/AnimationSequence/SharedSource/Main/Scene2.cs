using AnimationSequence.Animations;
using System;
using System.Collections.Generic;
using System.Text;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;
using WaveEngine.Components.GameActions;

namespace AnimationSequence
{
    public class Scene2 : Scene
    {
        IGameAction animationSequence;

        protected override void CreateScene()
        {
            this.Load(WaveContent.Scenes.Scene2);

            var cube1 = this.EntityManager.Find("cube1");
            var cube2 = this.EntityManager.Find("robotic.base.zone0.zone1.zone2.zone3.cube2");
            var joint0 = this.EntityManager.Find("robotic.base.zone0");
            var joint1 = this.EntityManager.Find("robotic.base.zone0.zone1");
            var joint2 = this.EntityManager.Find("robotic.base.zone0.zone1.zone2");
            var joint3 = this.EntityManager.Find("robotic.base.zone0.zone1.zone2.zone3");

            this.animationSequence = (new RotateTo3DGameAction(joint0, new Vector3(0,0, MathHelper.ToRadians(180)), TimeSpan.FromSeconds(1.5), EaseFunction.SineInOutEase, true)).ContinueWith(
                                                            this.CreateParallelGameActions(
                                                                new RotateTo3DGameAction(joint1, new Vector3(MathHelper.ToRadians(-40), 0, MathHelper.ToRadians(-22.6f)), TimeSpan.FromSeconds(1.5), EaseFunction.SineInOutEase, true),
                                                                new RotateTo3DGameAction(joint2, new Vector3(MathHelper.ToRadians(14), 0, 0), TimeSpan.FromSeconds(1.5), EaseFunction.SineInOutEase, true),                                                                
                                                                new RotateTo3DGameAction(joint3, new Vector3(MathHelper.ToRadians(8.7f), 0, 0), TimeSpan.FromSeconds(1.5), EaseFunction.SineInOutEase, true)
                                                                ).WaitAll())
                                                            .ContinueWithAction(() =>
                                                            {
                                                                cube1.IsVisible = false;
                                                                cube2.IsVisible = true;
                                                            }).ContinueWith(
                                                            this.CreateParallelGameActions(
                                                                new RotateTo3DGameAction(joint0, new Vector3(0, 0, MathHelper.ToRadians(53)), TimeSpan.FromSeconds(1.5), EaseFunction.SineInOutEase, true),
                                                                new RotateTo3DGameAction(joint1, new Vector3(MathHelper.ToRadians(20), 0, MathHelper.ToRadians(-22.6f)), TimeSpan.FromSeconds(1.5), EaseFunction.SineInOutEase, true),
                                                                new RotateTo3DGameAction(joint2, new Vector3(MathHelper.ToRadians(-14), 0, 0), TimeSpan.FromSeconds(1.5), EaseFunction.SineInOutEase, true),
                                                                new RotateTo3DGameAction(joint3, new Vector3(MathHelper.ToRadians(45), 0, 0), TimeSpan.FromSeconds(1.5), EaseFunction.SineInOutEase, true)).WaitAll());
        }

        protected override void Start()
        {
            base.Start();

            WaveServices.TimerFactory.CreateTimer(TimeSpan.FromSeconds(1), () =>
            {
                this.animationSequence.Run();
            }, false);
        }
    }
}
