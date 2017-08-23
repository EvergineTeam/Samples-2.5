using System;
using System.Runtime.Serialization;
using WaveEngine.Components.GameActions;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;

namespace XamarinFormsProfileSample.Components
{
    [DataContract]
    public class AnimationCameraComponent : Component
    {
        [DataMember]
        public float TranslationDuration { get; set; }

        [DataMember]
        public float RotationDuration { get; set; }

        protected override void Initialize()
        {
            base.Initialize();

            TranslationDuration = 1.5f;
            RotationDuration = 1.0f;
        }

        public void MoveTo(Transform3D transform)
        {
            var scene = this.Owner.Scene;

            IGameAction action =
                scene.CreateParallelGameActions(
                    new MoveTo3DGameAction(
                        this.Owner,
                        transform.Position,
                        TimeSpan.FromSeconds(TranslationDuration),
                        EaseFunction.SineInOutEase),
                    new RotateTo3DGameAction(
                        this.Owner,
                        transform.Rotation,
                        TimeSpan.FromSeconds(RotationDuration),
                        EaseFunction.SineInOutEase))
                    .WaitAll();

            action.Run();
        }
    }
}