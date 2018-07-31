﻿using System;
using System.Runtime.Serialization;
using WaveEngine.Common.Attributes;
using WaveEngine.Common.Input;
using WaveEngine.Common.Math;
using WaveEngine.Components.Animation;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
using WaveEngine.Spine;

namespace ParallaxCamera2D.Behaviors
{
    [DataContract]
    public class YureiBehavior : Behavior
    {
        private enum YureiState
        {
            Idle = 0,   // Yurei is idle
            RunRight,   // Is Running toward right direction
            RunLeft     // Is Running toward left direction
        }

        private enum YureiControl
        {
            None = 0,
            Touch,
            Keyboard,
            Gamepad
        }

        private const string IdleAnimation = "idle";
        private const string MoveAnimation = "move";

        private const float MinX = 0;
        private const float MaxX = 4400;

        private const float Speed = 500;
        private const float MixDuration = 0.075f;

        [RequiredService]
        private Input input;

        [RequiredComponent]
        private Transform2D transform = null;

        [RequiredComponent]
        private SkeletalAnimation skeletalAnimation = null;

        [DataMember]
        private string dustEntityPath;

        private Entity dustEntity;
        private Animation2D dustAnimation;
        private Transform2D dustTransform;

        private YureiState state;
        private YureiControl yureiControl;

        [RenderPropertyAsEntity(new string[] { "WaveEngine.Framework.Transform3D" })]
        public string DustEntityPath
        {
            get
            {
                return this.dustEntityPath;
            }
            set
            {
                this.dustEntityPath = value;

                if (this.isInitialized)
                {
                    this.RefreshDustEntity();
                }
            }
        }

        private YureiState State
        {
            get
            {
                return this.state;
            }

            set
            {
                YureiState oldState = this.state;
                this.state = value;

                if (this.state != oldState)
                {
                    this.ChangeState();
                }
            }
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.RefreshDustEntity();

            // Set animation mix
            this.skeletalAnimation.SetAnimationMixDuration(IdleAnimation, MoveAnimation, MixDuration);
            this.skeletalAnimation.SetAnimationMixDuration(MoveAnimation, IdleAnimation, MixDuration);

            this.ChangeState();
        }

        protected override void Update(TimeSpan gameTime)
        {
            this.CheckTouch();
            this.CheckKeyboard();
            this.CheckGamePad();

            if (dustAnimation.State == WaveEngine.Framework.Animation.AnimationState.Stopped)
            {
                this.dustEntity.Enabled = false;
            }

            switch (this.State)
            {
                case YureiState.Idle:
                    break;
                case YureiState.RunRight:
                    this.transform.X += (float)(Speed * gameTime.TotalSeconds);
                    break;
                case YureiState.RunLeft:
                    this.transform.X -= (float)(Speed * gameTime.TotalSeconds);
                    break;
                default:
                    break;
            }

            this.transform.X = Math.Max(MinX, Math.Min(MaxX, this.transform.X));
        }

        private void RefreshDustEntity()
        {
            this.dustEntity = null;
            this.dustAnimation = null;
            this.dustTransform = null;

            if (!string.IsNullOrEmpty(this.dustEntityPath))
            {
                this.dustEntity = this.EntityManager.Find(this.dustEntityPath, this.Owner);

                this.dustAnimation = this.dustEntity.FindComponent<Animation2D>();
                this.dustTransform = this.dustEntity.FindComponent<Transform2D>();
                this.dustEntity.Enabled = false;
            }
        }

        private void ChangeState()
        {
            switch (this.State)
            {
                case YureiState.Idle:
                    {
                        this.skeletalAnimation.CurrentAnimation = IdleAnimation;
                        this.skeletalAnimation.Play(true);
                        break;
                    }
                case YureiState.RunRight:
                    {
                        this.transform.Transform3D.Rotation = Vector3.Zero;
                        this.skeletalAnimation.CurrentAnimation = MoveAnimation;
                        this.skeletalAnimation.Play(true);

                        var dustPos = this.dustTransform.Position;
                        dustPos.X = this.transform.Position.X;
                        this.dustTransform.Position = dustPos;
                        this.dustTransform.Transform3D.Rotation = Vector3.Zero;

                        this.dustEntity.Enabled = true;
                        this.dustAnimation.PlayAnimation("start", false);
                        break;
                    }
                case YureiState.RunLeft:
                    {
                        this.transform.Transform3D.Rotation = Vector3.UnitY * MathHelper.Pi;
                        this.skeletalAnimation.CurrentAnimation = MoveAnimation;
                        this.skeletalAnimation.Play(true);

                        var dustPos = this.dustTransform.Position;
                        dustPos.X = this.transform.Position.X;
                        this.dustTransform.Position = dustPos;
                        //this.dustTransform.Transform3D.Rotation = Vector3.UnitY * MathHelper.Pi;

                        this.dustEntity.Enabled = true;
                        this.dustAnimation.PlayAnimation("start", false);
                        break;
                    }
                default:
                    break;
            }

        }

        private void CheckTouch()
        {
            // touch panel
            var touches = this.input.TouchPanelState;
            if (touches.Count > 0)
            {
                this.yureiControl = YureiControl.Touch;

                var firstTouch = touches[0];
                if (firstTouch.Position.X > WaveServices.Platform.ScreenWidth / 2)
                {
                    this.State = YureiState.RunRight;
                }
                else
                {
                    this.State = YureiState.RunLeft;
                }
            }
            else if (this.yureiControl == YureiControl.Touch)
            {
                this.State = YureiState.Idle;
            }
        }

        private void CheckKeyboard()
        {
            var keyboard = this.input.KeyboardState;
            if (keyboard.IsConnected)
            {
                if (keyboard.Right == ButtonState.Pressed)
                {
                    this.yureiControl = YureiControl.Keyboard;
                    this.State = YureiState.RunRight;
                }
                else if (keyboard.Left == ButtonState.Pressed)
                {
                    this.yureiControl = YureiControl.Keyboard;
                    this.State = YureiState.RunLeft;
                }
                else if (this.yureiControl == YureiControl.Keyboard)
                {
                    this.State = YureiState.Idle;
                }
            }
        }

        private void CheckGamePad()
        {
            var gamepad = this.input.GamePadState;
            if (gamepad.IsConnected)
            {
                if (gamepad.ThumbSticks.Left.X > 0)
                {
                    this.yureiControl = YureiControl.Gamepad;
                    this.State = YureiState.RunRight;
                }
                else if (gamepad.ThumbSticks.Left.X < 0)
                {
                    this.yureiControl = YureiControl.Gamepad;
                    this.State = YureiState.RunLeft;
                }
                else if (gamepad.DPad.Right == ButtonState.Pressed)
                {
                    this.yureiControl = YureiControl.Gamepad;
                    this.State = YureiState.RunRight;
                }
                else if (gamepad.DPad.Left == ButtonState.Pressed)
                {
                    this.yureiControl = YureiControl.Gamepad;
                    this.State = YureiState.RunLeft;
                }
                else if (this.yureiControl == YureiControl.Gamepad)
                {
                    this.State = YureiState.Idle;
                }
            }
        }
    }
}
