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
            var cube2 = this.EntityManager.Find("base.zone0.zone1.zone2.zone3.cube2");
            var joint0 = this.EntityManager.Find("base.zone0");
            var joint1 = this.EntityManager.Find("base.zone0.zone1");
            var joint2 = this.EntityManager.Find("base.zone0.zone1.zone2");
            var joint3 = this.EntityManager.Find("base.zone0.zone1.zone2.zone3");

            this.animationSequence = (new RotateTo3DGameAction(joint0, new Vector3(0, MathHelper.ToRadians(180), 0), TimeSpan.FromSeconds(1.5), EaseFunction.SineInOutEase, true)).ContinueWith(
                                                            this.CreateParallelGameActions(
                                                                new RotateTo3DGameAction(joint2, new Vector3(0, 0, MathHelper.ToRadians(1.7)), TimeSpan.FromSeconds(1.5), EaseFunction.SineInOutEase, true),
                                                                new RotateTo3DGameAction(joint1, new Vector3(0, 0, MathHelper.ToRadians(14)), TimeSpan.FromSeconds(1.5), EaseFunction.SineInOutEase, true),
                                                                new RotateTo3DGameAction(joint3, new Vector3(0, 0, MathHelper.ToRadians(-23)), TimeSpan.FromSeconds(1.5), EaseFunction.SineInOutEase, true)
                                                                ).WaitAll())
                                                            .ContinueWithAction(() =>
                                                            {
                                                                cube1.IsVisible = false;
                                                                cube2.IsVisible = true;
                                                            }).ContinueWith(
                                                            this.CreateParallelGameActions(
                                                                new RotateTo3DGameAction(joint0, new Vector3(0, MathHelper.ToRadians(53), 0), TimeSpan.FromSeconds(1.5), EaseFunction.SineInOutEase, true),
                                                                new RotateTo3DGameAction(joint1, new Vector3(0, 0, MathHelper.ToRadians(-75)), TimeSpan.FromSeconds(1.5), EaseFunction.SineInOutEase, true),
                                                                new RotateTo3DGameAction(joint2, new Vector3(0, 0, MathHelper.ToRadians(50)), TimeSpan.FromSeconds(1.5), EaseFunction.SineInOutEase, true),
                                                                new RotateTo3DGameAction(joint3, new Vector3(0, 0, MathHelper.ToRadians(-65)), TimeSpan.FromSeconds(1.5), EaseFunction.SineInOutEase, true)).WaitAll());
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
