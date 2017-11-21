using AnimationSequence.Animations;
using System;
using System.Collections.Generic;
using System.Text;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;
using WaveEngine.Components.GameActions;
using WaveEngine.Framework.Sound;
using WaveEngine.Framework.Animation;
using WaveEngine.Framework.Graphics;

namespace AnimationSequence
{
    public class Scene2 : Scene
    {
        IGameAction animationSequence;

        protected override void CreateScene()
        {
            this.Load(WaveContent.Scenes.Scene2);

            var robotSound = this.EntityManager.Find("robotic").FindComponent<SoundEmitter3D>();

            SoundBank bank = new SoundBank(Assets);
            WaveServices.SoundPlayer.RegisterSoundBank(bank);
            var coinSound = new SoundInfo(WaveContent.Assets.coin_wav);
            bank.Add(coinSound);

            var cube1 = this.EntityManager.Find("cube1");
            var cube2 = this.EntityManager.Find("robotic.base.zone0.zone1.zone2.zone3.cube2");
            var robot = this.EntityManager.Find("robotic.base");
            var robotTransform = robot.FindComponent<Transform3D>();
            var joint0 = this.EntityManager.Find("robotic.base.zone0");            
            var joint1 = this.EntityManager.Find("robotic.base.zone0.zone1");
            var joint2 = this.EntityManager.Find("robotic.base.zone0.zone1.zone2");
            var joint3 = this.EntityManager.Find("robotic.base.zone0.zone1.zone2.zone3");

            this.animationSequence = this.CreateParallelGameActions(
                                    new RotateTo3DGameAction(joint0, new Vector3(0, 0, MathHelper.ToRadians(180)), TimeSpan.FromSeconds(1.5), EaseFunction.SineInOutEase, true),
                                    GameActionFactory.CreateGameActionFromAction(this, () => robotSound.Play()))
                                .WaitAll()
                                .ContinueWith(this.CreateParallelGameActions(
                                    new RotateTo3DGameAction(joint1, new Vector3(MathHelper.ToRadians(-40), 0, MathHelper.ToRadians(-22.6f)), TimeSpan.FromSeconds(1.5), EaseFunction.SineInOutEase, true),
                                    new RotateTo3DGameAction(joint2, new Vector3(MathHelper.ToRadians(14), 0, 0), TimeSpan.FromSeconds(1.5), EaseFunction.SineInOutEase, true),
                                    new RotateTo3DGameAction(joint3, new Vector3(MathHelper.ToRadians(8.7f), 0, 0), TimeSpan.FromSeconds(1.5), EaseFunction.SineInOutEase, true)
                                            .AndPlaySound(coinSound))
                                .WaitAll())
                                .ContinueWithAction(() =>
                                {
                                    cube1.IsVisible = false;
                                    cube2.IsVisible = true;
                                })
                                .Delay(TimeSpan.FromSeconds(0.5f))
                                .ContinueWithAction(() => robotSound.Play())
                                .ContinueWith(this.CreateParallelGameActions(
                                    new RotateTo3DGameAction(joint0, new Vector3(0, 0, MathHelper.ToRadians(53)), TimeSpan.FromSeconds(1.5f), EaseFunction.SineInOutEase, true),
                                    new RotateTo3DGameAction(joint1, new Vector3(MathHelper.ToRadians(20), 0, MathHelper.ToRadians(-22.6f)), TimeSpan.FromSeconds(1.5), EaseFunction.SineInOutEase, true),
                                    new RotateTo3DGameAction(joint2, new Vector3(MathHelper.ToRadians(-14), 0, 0), TimeSpan.FromSeconds(1), EaseFunction.SineInOutEase, true),
                                    new RotateTo3DGameAction(joint3, new Vector3(MathHelper.ToRadians(45), 0, 0), TimeSpan.FromSeconds(0.5f), EaseFunction.SineInOutEase, true))
                               .WaitAny())
                               .ContinueWithAction(() => robotSound.Play())
                               .ContinueWith(new Vector3AnimationGameAction(robot, Vector3.Zero, Vector3.UnitY,TimeSpan.FromSeconds(0.3f),  EaseFunction.ElasticInEase, (f) =>
                               {
                                   robotTransform.Position = f;
                               }))
                               .ContinueWith(new Vector3AnimationGameAction(robot, Vector3.UnitY, Vector3.Zero,  TimeSpan.FromSeconds(0.5f), EaseFunction.CubicInOutEase, (f) =>
                               {
                                   robotTransform.Position = f;
                               }));
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
