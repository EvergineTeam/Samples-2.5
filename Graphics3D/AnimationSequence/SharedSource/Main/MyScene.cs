#region Using Statements
using AnimationSequence.Animations;
using System;
using System.Collections.Generic;
using System.Linq;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.GameActions;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Resources;
using WaveEngine.Framework.Services;
using WaveEngine.Framework.Sound;
#endregion

namespace AnimationSequence
{
    public class MyScene : Scene
    {
        protected override void CreateScene()
        {
            this.Load(WaveContent.Scenes.MyScene);
        }

        protected override void Start()
        {
            base.Start();

            var robotSound = this.EntityManager.FindFirstComponentOfType<SoundEmitter3D>();
            var joints = this.EntityManager.FindComponentsOfType<AnimationJoint>();
            var hip = joints.FirstOrDefault(j => j.Joint == JointEnum.Hip);
            var box = joints.FirstOrDefault(j => j.Joint == JointEnum.Box);
            var elbow = joints.FirstOrDefault(j => j.Joint == JointEnum.Elbow);
            var handedBox = joints.FirstOrDefault(j => j.Joint == JointEnum.HandedBox);
            var shoulder = joints.FirstOrDefault(j => j.Joint == JointEnum.Shoulder);
            var wrist = joints.FirstOrDefault(j => j.Joint == JointEnum.Wrist);
            var pi2 = (float)Math.PI * 2f;
            box.Owner.IsVisible = false;

            var welcomeAnim = GameActionFactory.CreateGameAction(this,
                                new Vector3AnimationGameAction(hip.Owner, Vector3.Zero, (pi2 * 0.5f) * Vector3.UnitY, TimeSpan.FromSeconds(2), EaseFunction.BackInEase,
                                    (v) => { hip.Transform.LocalRotation = v; }))
                              .ContinueWith(new Vector3AnimationGameAction(wrist.Owner, Vector3.Zero, pi2 * Vector3.UnitY, TimeSpan.FromSeconds(2), EaseFunction.CubicInOutEase,
                                    (v) => { wrist.Transform.LocalRotation = v; }));

            var extendshoulder = GameActionFactory.CreateGameAction(this,
                                new Vector3AnimationGameAction(shoulder.Owner, Vector3.Zero, this.ToRad(-54.53f, 0, -20f), TimeSpan.FromSeconds(2), EaseFunction.CubicInOutEase,
                                    (v) => { shoulder.Transform.LocalRotation = v; }));
            var extendwrist = GameActionFactory.CreateGameAction(this,
                                new Vector3AnimationGameAction(wrist.Owner, Vector3.Zero, this.ToRad(18.8153f, 0, 0), TimeSpan.FromSeconds(2), EaseFunction.CubicInOutEase,
                                    (v) => { wrist.Transform.LocalRotation = v; }));
            var extendelbow = GameActionFactory.CreateGameAction(this,
                                new Vector3AnimationGameAction(elbow.Owner, Vector3.Zero, this.ToRad(35.98f, 0, 0), TimeSpan.FromSeconds(2), EaseFunction.CubicInOutEase,
                                    (v) => { elbow.Transform.LocalRotation = v; }));

            GameActionFactory.CreateDelayGameAction(this, TimeSpan.FromSeconds(1))
                .ContinueWith(welcomeAnim)
                .CreateParallelGameActions(new List<IGameAction>() { extendelbow, extendshoulder, extendwrist })
                .WaitAll()
                .ContinueWithAction(() =>
                {
                    box.Owner.IsVisible = true;
                    handedBox.Owner.IsVisible = false;
                    box.Transform.Position = handedBox.Transform.Position;

                })
                .CreateParallelGameActions(new List<IGameAction>() { this.DeferedAction(shoulder, 1, Vector3.Zero),
                                                                    this.DeferedAction(shoulder, 1, Vector3.Zero),
                                                                    this.DeferedAction(shoulder, 1, Vector3.Zero) })
                .WaitAll()
                .ContinueWith(this.DeferedAction(hip, 2, Vector3.Zero))
                .Run();
        }

        private Vector3 ToRad(float xGrad, float yGrad, float zGrad)
        {
            return new Vector3(MathHelper.ToRadians(xGrad), MathHelper.ToRadians(yGrad), MathHelper.ToRadians(zGrad));
        }

        private IGameAction DeferedAction(AnimationJoint joint, float time, Vector3 to)
        {
            return GameActionFactory.CreateGameAction(this, () =>
            {
                // This will execute in the moment this animations is executed, and not when created
                return GameActionFactory.CreateGameAction(this,
                            new Vector3AnimationGameAction(joint.Owner, joint.Transform.LocalRotation, to, TimeSpan.FromSeconds(time), EaseFunction.CubicInOutEase,
                                (v) => { joint.Transform.LocalRotation = v; }));
            });
        }
    }
}
