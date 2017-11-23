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
        private SoundInfo coinSound;

        protected override void CreateScene()
        {
            this.Load(WaveContent.Scenes.Scene2);

            var robotSound = this.EntityManager.Find("robotic").FindComponent<SoundEmitter3D>();

            SoundBank bank = new SoundBank(Assets);
            WaveServices.SoundPlayer.RegisterSoundBank(bank);
            this.coinSound = new SoundInfo(WaveContent.Assets.coin_wav);
            bank.Add(this.coinSound);
            var camera3d = this.EntityManager.Find("defaultCamera3D").FindComponent<Transform3D>();
            var cube1 = this.EntityManager.Find("cube1");
            var cube1Transform = cube1.FindComponent<Transform3D>();
            var cube2 = this.EntityManager.Find("robotic.base.zone0.zone1.zone2.zone3.cube2");
            var cube2Transform = cube2.FindComponent<Transform3D>();
            var robot = this.EntityManager.Find("robotic.base");
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
                                    new RotateTo3DGameAction(joint2, new Vector3(MathHelper.ToRadians(14), 0, 0), TimeSpan.FromSeconds(1.2), EaseFunction.SineInOutEase, true).AndPlaySound(coinSound),
                                    new RotateTo3DGameAction(joint3, new Vector3(MathHelper.ToRadians(8.7f), 0, 0), TimeSpan.FromSeconds(1.5), EaseFunction.SineInOutEase, true))
                                .WaitAny())
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
                                    new RotateTo3DGameAction(joint2, new Vector3(0, 0, MathHelper.ToRadians(-30)), TimeSpan.FromSeconds(1), EaseFunction.SineInOutEase, true),
                                    new RotateTo3DGameAction(joint3, new Vector3(MathHelper.ToRadians(45), 0, 0), TimeSpan.FromSeconds(0.5f), EaseFunction.SineInOutEase, true))
                               .WaitAll())
                               .ContinueWithAction(() => robotSound.Play()).CreateParallelGameActions(new List<IGameAction>()
                               {
                                   new RotateTo3DGameAction(joint3, new Vector3(0, MathHelper.ToRadians(90), 0), TimeSpan.FromSeconds(1.5), EaseFunction.SineInOutEase, true),
                                   new Vector3AnimationGameAction(robot, camera3d.Position, new Vector3(0, 8, 11), TimeSpan.FromSeconds(1f), EaseFunction.CubicInEase, (f) =>
                                        {
                                            camera3d.Position = f;
                                        })
                               })
                               .WaitAll()
                                .ContinueWithAction(() =>
                                {
                                    cube1.IsVisible = true;
                                    cube2.IsVisible = false;
                                })
                               .ContinueWithAction(() =>
                               {
                                   new Vector3AnimationGameAction(robot, cube2Transform.Position, cube2Transform.Position * new Vector3(1, 0, 1) + new Vector3(0, 1, 0), TimeSpan.FromSeconds(0.5f), EaseFunction.ElasticOutEase, (f) =>
                                   {
                                       cube1Transform.Position = f;
                                   }).Run();
                               });
        }

        protected override void Start()
        {
            base.Start();

            GameActionFactory.CreateDelayGameAction(this, TimeSpan.FromSeconds(1))
                .ContinueWith(this.animationSequence)
                .AndPlaySound(this.coinSound)
                .Run();
        }
    }
}
