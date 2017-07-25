using NoesisWPFLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Common.Attributes;
using WaveEngine.Common.Input;
using WaveEngine.Components.GameActions;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;

namespace NoesisSample.Behaviors
{
    [DataContract]
    public class CameraManager : Component
    {
        [RequiredComponent]
        protected Transform3D transform;

        private Transform3D curiosityCamera;
        private Transform3D spiritCamera;
        private Transform3D globalSurveyorCamera;
        private Transform3D atmosphereCamera;

        ////private Dictionary<CameraEnum, Transform3D> cameraTransforms;

        private MissionEnum currentState;

        [DontRenderProperty]
        public MissionEnum CurrentState
        {
            get
            {
                return this.currentState;
            }

            set
            {
                if (value != this.currentState)
                {
                    this.UpdateState(value);
                }
            }
        }

        [RenderPropertyAsEntity(new string[] { "WaveEngine.Framework.Graphics.Transform3D" })]
        [DataMember]
        public string CuriosityPath { get; set; }

        [RenderPropertyAsEntity(new string[] { "WaveEngine.Framework.Graphics.Transform3D" })]
        [DataMember]
        public string SpiritPath { get; set; }

        [RenderPropertyAsEntity(new string[] { "WaveEngine.Framework.Graphics.Transform3D" })]
        [DataMember]
        public string GlobalSurveyorPath { get; set; }

        [RenderPropertyAsEntity(new string[] { "WaveEngine.Framework.Graphics.Transform3D" })]
        [DataMember]
        public string AtmosphereCameraPath { get; set; }

        private bool validCameras;

        protected override void Initialize()
        {
            base.Initialize();

            // Gets the path camera transforms

            if (!string.IsNullOrEmpty(this.CuriosityPath)
                && !string.IsNullOrEmpty(this.CuriosityPath)
                && !string.IsNullOrEmpty(this.CuriosityPath)
                && !string.IsNullOrEmpty(this.AtmosphereCameraPath))
            {
                this.curiosityCamera = this.Owner.Find(this.CuriosityPath)?.FindComponent<Transform3D>();
                this.spiritCamera = this.Owner.Find(this.SpiritPath)?.FindComponent<Transform3D>();
                this.globalSurveyorCamera = this.Owner.Find(this.GlobalSurveyorPath)?.FindComponent<Transform3D>();
                this.atmosphereCamera = this.Owner.Find(this.AtmosphereCameraPath)?.FindComponent<Transform3D>();

                this.validCameras = (this.curiosityCamera != null) && (this.spiritCamera != null) && (this.globalSurveyorCamera != null) && (this.atmosphereCamera != null);
            }

            StateManager.Instance.StateChanging += (e, state) =>
            {
                this.CurrentState = state;
            };
        }

        private void UpdateState(MissionEnum state)
        {
            // Updates the state
            var lastState = this.currentState;
            this.currentState = state;

            if (!this.validCameras)
            {
                return;
            }

            Transform3D firstTarget = null;
            Transform3D secondTarget = null;
            TimeSpan firstDuration = TimeSpan.FromSeconds(5);
            TimeSpan secondDuration = TimeSpan.FromSeconds(5);

            // Animates the camera towards the new state target
            switch (this.currentState)
            {
                case MissionEnum.Curiosity:

                    if (lastState != MissionEnum.MarsGlobarSurveyor)
                    {
                        firstTarget = this.curiosityCamera;
                    }
                    else
                    {
                        firstTarget = this.atmosphereCamera;
                        secondTarget = this.curiosityCamera;
                    }

                    break;
                case MissionEnum.Spirit:

                    if (lastState != MissionEnum.MarsGlobarSurveyor)
                    {
                        firstTarget = this.spiritCamera;
                    }
                    else
                    {
                        firstTarget = this.atmosphereCamera;
                        secondTarget = this.spiritCamera;
                    }

                    break;
                case MissionEnum.MarsGlobarSurveyor:
                    firstTarget = this.atmosphereCamera;
                    secondTarget = this.globalSurveyorCamera;
                    break;
                default:
                    break;
            }

            if (secondTarget != null)
            {
                this.Owner.Scene.CreateParallelGameActions(
                    new MoveTo3DGameAction(this.Owner, firstTarget.Position, firstDuration, EaseFunction.QuinticInOutEase),
                    new RotateTo3DGameAction(this.Owner, firstTarget.Rotation, firstDuration, EaseFunction.CubicInOutEase))
                .WaitAll().ContinueWith(
                    this.Owner.Scene.CreateParallelGameActions(
                        new MoveTo3DGameAction(this.Owner, secondTarget.Position, secondDuration, EaseFunction.QuinticInOutEase),
                        new RotateTo3DGameAction(this.Owner, secondTarget.Rotation, secondDuration, EaseFunction.SineInOutEase)
                    ).WaitAll()
                )
                .ContinueWithAction(() =>
                {
                    StateManager.Instance.StateInitialized(state);
                }).Run();
            }
            else
            {
                this.Owner.Scene.CreateParallelGameActions(
                    new MoveTo3DGameAction(this.Owner, firstTarget.Position, firstDuration, EaseFunction.QuinticInOutEase),
                    new RotateTo3DGameAction(this.Owner, firstTarget.Rotation, firstDuration, EaseFunction.CubicInOutEase))
                .WaitAll()
                .ContinueWithAction(() =>
                {
                    StateManager.Instance.StateInitialized(state);
                }).Run();
            }
        }
    }
}
