#region Using Statements
using AnimationSequence.Animations;
using System;
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

            var welcomeAnim = GameActionFactory.CreateGameAction(this,
                                new Vector3AnimationGameAction(hip.Owner, Vector3.Zero, (pi2 * 0.5f) * Vector3.UnitY, TimeSpan.FromSeconds(2), EaseFunction.BackInEase,
                                    (v) => { hip.Transform.LocalRotation = v; }))
                              .ContinueWith(new Vector3AnimationGameAction(wrist.Owner, Vector3.Zero, pi2 * Vector3.UnitY, TimeSpan.FromSeconds(2), EaseFunction.CubicInOutEase,
                                    (v) => { wrist.Transform.LocalRotation = v; }));

            GameActionFactory.CreateDelayGameAction(this, TimeSpan.FromSeconds(1))
                .ContinueWith(welcomeAnim)
                .Run();
        }
    }
}
