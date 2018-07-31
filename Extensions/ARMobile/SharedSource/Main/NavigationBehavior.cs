using System;
using System.Linq;
using System.Runtime.Serialization;
using WaveEngine.ARMobile;
using WaveEngine.ARMobile.Components;
using WaveEngine.Common.Attributes;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
using WaveEngine.Framework.Sound;

namespace ARMobile
{
    [DataContract]
    public class NavigationBehavior : Behavior
    {
        private enum AnimationStates
        {
            Idle,
            Turning,
            Moving
        }

        [RequiredService]
        protected Input input;

        [RequiredComponent]
        protected ARMobileProvider arProvider;

        [RequiredComponent]
        protected SoundEmitter3D soundEmitter;

        private Guid lastPlaneId;

        private Vector3 destinationPosition;

        private float smoothPositionTime;

        private Vector3 smoothPositionVelocity;

        private Quaternion destinationOrientation;

        private float smoothOrientationTime;

        private Quaternion smoothDeriv;

        private AnimationStates currentAnimationState;

        private bool dirtyTarget;

        private bool dirtyIndicator;

        private Transform3D targetTransform;

        private Transform3D indicatorTransform;

        [DataMember]
        private string targetEntity;

        [DataMember]
        private string indicatorEntity;

        /// <summary>
        /// Gets or sets the target entity path
        /// </summary>
        [RenderPropertyAsEntity(
            new string[] { "WaveEngine.Framework.Graphics.Transform3D" },
            CustomPropertyName = "Target Entity",
            Tooltip = "The target entity to be placed")]
        public string TargetEntity
        {
            get
            {
                return this.targetEntity;
            }

            set
            {
                this.targetEntity = value;
                this.dirtyTarget = true;
            }
        }

        /// <summary>
        /// Gets or sets the indicator entity path
        /// </summary>
        [RenderPropertyAsEntity(
            new string[] { "WaveEngine.Framework.Graphics.Transform3D" },
            CustomPropertyName = "Indicator Entity",
            Tooltip = "The indicator entity to be placed")]
        public string IndicatorEntity
        {
            get
            {
                return this.indicatorEntity;
            }

            set
            {
                this.indicatorEntity = value;
                this.dirtyIndicator = true;
            }
        }

        [DataMember]
        [RenderProperty]
        public ARMobileHitType HitType { get; set; }

        protected override void DefaultValues()
        {
            base.DefaultValues();

            this.currentAnimationState = AnimationStates.Idle;
            this.HitType = ARMobileHitType.ExistingPlane;
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.soundEmitter.PlayAutomatically = false;
            this.soundEmitter.Loop = false;

            this.UpdateTransform(this.targetEntity, out this.targetTransform);
            this.UpdateTransform(this.indicatorEntity, out this.indicatorTransform);
        }

        /// <inheritdoc />
        protected override void Update(TimeSpan gameTime)
        {
            if (this.dirtyTarget)
            {
                this.UpdateTransform(this.targetEntity, out this.targetTransform);
                this.dirtyTarget = false;
            }

            if (this.dirtyIndicator)
            {
                this.UpdateTransform(this.indicatorEntity, out this.indicatorTransform);
                this.dirtyIndicator = false;
            }

            if (this.targetTransform == null)
            {
                return;
            }

            var newTouches = this.input.TouchPanelState
                                       .Where(t => t.IsNew);

            foreach (var touch in newTouches)
            {
                ARMobileHitTestResult[] results;
                if (this.arProvider.HitTest(touch.Position, this.HitType, out results))
                {
                    var firstResult = results.First();

                    if (firstResult.Anchor == null)
                    {
                        continue;
                    }

                    var resultAnchorId = firstResult.Anchor.Id;
                    var worldTransform = firstResult.WorldTransform;

                    this.destinationPosition = worldTransform.Translation;

                    var up = Vector3.Up;
                    var dest = this.targetTransform.Position - this.destinationPosition;
                    Quaternion.CreateFromLookAt(ref dest, ref up, out this.destinationOrientation);

                    if (this.lastPlaneId != resultAnchorId)
                    {
                        this.PlaceItem(worldTransform);
                    }
                    else
                    {
                        this.smoothPositionTime = (this.targetTransform.Position - this.destinationPosition).Length() / 1.4f;
                        this.smoothOrientationTime = (this.targetTransform.Orientation - this.destinationOrientation).Length() / 10f;

                        this.currentAnimationState = AnimationStates.Turning;
                        this.MoveIndicator(this.destinationPosition);
                    }

                    this.PlaySound();

                    this.lastPlaneId = resultAnchorId;
                    break;
                }
            }

            if (this.currentAnimationState == AnimationStates.Turning)
            {
                var orientation = Quaternion.SmoothDamp(this.targetTransform.Orientation, this.destinationOrientation, ref this.smoothDeriv, this.smoothOrientationTime, (float)gameTime.TotalSeconds);
                this.targetTransform.Orientation = orientation;

                if (this.QuaternionAreEquals(ref orientation, ref this.destinationOrientation))
                {
                    this.currentAnimationState = AnimationStates.Moving;
                }
            }
            else if (this.currentAnimationState == AnimationStates.Moving)
            {
                var position = Vector3.SmoothDamp(this.targetTransform.Position, this.destinationPosition, ref this.smoothPositionVelocity, this.smoothPositionTime, (float)gameTime.TotalSeconds);
                this.targetTransform.Position = position;

                if (this.indicatorTransform != null &&
                    Vector3.DistanceSquared(position, this.destinationPosition) < 0.0225f)
                {
                    this.indicatorTransform.Owner.IsVisible = false;
                    this.currentAnimationState = AnimationStates.Idle;
                }
            }
        }

        private void MoveIndicator(Vector3 destinationPosition)
        {
            if (this.indicatorTransform != null)
            {
                this.indicatorTransform.Position = destinationPosition;
                this.indicatorTransform.Owner.IsVisible = true;
            }
        }

        private void PlaySound()
        {
            var random = WaveServices.Random.Next(1, 5);
            this.soundEmitter.SoundPath = WaveContent.Assets.Sounds.droid1_wav.Replace("droid1", $"droid{random}");
            this.soundEmitter.Play();
        }

        private void UpdateTransform(string entityPath, out Transform3D entityTransform)
        {
            if (entityPath != null)
            {
                var target = this.EntityManager.Find(entityPath, this.Owner);
                if (target != null)
                {
                    if (!WaveServices.Platform.IsEditor)
                    {
                        target.IsVisible = false;
                    }

                    entityTransform = target.FindComponent<Transform3D>();
                }
                else
                {
                    entityTransform = null;
                }
            }
            else
            {
                entityTransform = null;
            }
        }

        private void PlaceItem(Matrix t)
        {
            this.targetTransform.Position = t.Translation;
            this.targetTransform.Orientation = t.Orientation;
            this.targetTransform.Owner.IsVisible = true;

            var soundEmitter = this.targetTransform.Owner.FindComponentsInChildren<SoundEmitter3D>().First();

            if (soundEmitter.IsMuted)
            {
                soundEmitter.Play();
                soundEmitter.IsMuted = false;
            }

            this.currentAnimationState = AnimationStates.Idle;
        }

        private bool QuaternionAreEquals(ref Quaternion q1, ref Quaternion q2, float maxRelativeError = 0.1f)
        {
            return (q1.X.Equal(q2.X, maxRelativeError) && q1.Y.Equal(q2.Y, maxRelativeError) && q1.Z.Equal(q2.Z, maxRelativeError) && q1.W.Equal(q2.W, maxRelativeError)) ||
                   (q1.X.Equal(-q2.X, maxRelativeError) && q1.Y.Equal(-q2.Y, maxRelativeError) && q1.Z.Equal(-q2.Z, maxRelativeError) && q1.W.Equal(-q2.W, maxRelativeError));
        }
    }
}
