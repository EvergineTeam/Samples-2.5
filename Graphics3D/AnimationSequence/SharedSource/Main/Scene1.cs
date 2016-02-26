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

            AnimationSlot animationSlot1 = new AnimationSlot()
            {
                TransformationType = AnimationSlot.TransformationTypes.Position,
                TotalTime = TimeSpan.FromSeconds(1.5f),
                StartPosition = new Vector3(0, 0, 0),
                EndPosition = new Vector3(0, 2, 0),
            };

            AnimationSlot animationSlot2 = new AnimationSlot()
            {
                TransformationType = AnimationSlot.TransformationTypes.Rotation,
                TotalTime = TimeSpan.FromSeconds(1.5f),
                StartRotation = new Vector3(0, 0, 0),
                EndRotation = new Vector3(0, (float)Math.PI * 2, 0),
            };

            Animation3DBehavior cubeAnimationBehavior = this.EntityManager.Find("cube").FindComponent<Animation3DBehavior>();

            this.animationSequence = this.CreateGameAction(this.CreateGameAction(new Animation3DGameAction(animationSlot1, cubeAnimationBehavior)))
                                                    .ContinueWith(new Animation3DGameAction(animationSlot2, cubeAnimationBehavior));
        }

        protected override void Start()
        {
            base.Start();

            this.animationSequence.Run();
        }
    }
}
