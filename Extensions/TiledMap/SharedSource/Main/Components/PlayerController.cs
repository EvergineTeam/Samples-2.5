﻿using System;
using System.Runtime.Serialization;
using TiledMap.Entities;
using WaveEngine.Common.Input;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Diagnostic;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Managers;
using WaveEngine.Framework.Physics2D;
using WaveEngine.Framework.Services;

namespace TiledMap.Components
{
    public enum ControllerType
    {
        JoystickController,
        KeyboardController,
        GamepadController
    }

    [DataContract]
    public class PlayerController : Behavior
    {
        private const float SideImpulse = 0.001f;
        private const float JumpImpulse = 0.08f;

        private Joystick joystick;
        private JumpButton jumpButton;

        private Input input;
        private bool isJump;
        private int collisionCounter = 0;
        private Vector2 initPosition;

        private ControllerType controller;

        private SoundManager soundManager;
        private VirtualScreenManager vm;

        private bool OnFloor
        {
            get
            {
                Labels.Add("CollisionCounter", collisionCounter);
                return this.collisionCounter > 0;
            }
        }

        [RequiredComponent]
        public RigidBody2D RigidBody;

        [RequiredComponent]
        public Transform2D Transform2D;

        [RequiredComponent(false)]
        public Collider2D Collider;

        protected override void Initialize()
        {
            base.Initialize();

            this.soundManager = this.EntityManager.Find("SoundManager").FindComponent<SoundManager>();
            this.vm = this.Owner.Scene.VirtualScreenManager;

            this.input = WaveServices.Input;
            this.initPosition = this.Transform2D.Position;

            this.Collider.BeginCollision += Collider_BeginCollision;
            this.Collider.EndCollision += Collider_EndCollision;
        }

        public void Reset()
        {
            this.RigidBody.ResetPosition(this.initPosition);
            this.RigidBody.Transform2D.Rotation = 0;
            this.collisionCounter = 0;
        }

        protected override void Update(TimeSpan gameTime)
        {
            this.HandleJoystick();
            this.HandleKeys();
            this.HandleGamepad();
        }

        private void HandleGamepad()
        {
            if (this.input.GamePadState.IsConnected)
            {
                var gamepadState = this.input.GamePadState;

                if (this.OnFloor && (gamepadState.ThumbSticks.Left.X != 0))
                {
                    this.RigidBody.ApplyLinearImpulse(Vector2.UnitX * SideImpulse * gamepadState.ThumbSticks.Left.X, RigidBody.Transform2D.Position);
                }

                if (gamepadState.Buttons.A == ButtonState.Pressed)
                {
                    this.Jump();
                    this.controller = ControllerType.GamepadController;
                    this.isJump = true;
                }
                else
                {
                    if (this.controller == ControllerType.GamepadController)
                    {
                        this.isJump = false;
                    }
                }
            }
        }

        private void HandleJoystick()
        {
            if (this.joystick == null)
            {
                var joystickScene = WaveServices.ScreenContextManager.CurrentContext.FindScene<JoystickScene>();
                if (joystickScene != null)
                {
                    this.joystick = joystickScene.Joystick;
                    this.jumpButton = joystickScene.JumpButton;
                }
            }
            else
            {
                if (this.OnFloor && (joystick.Direction.X != 0))
                {
                    this.RigidBody.ApplyLinearImpulse(Vector2.UnitX * SideImpulse * joystick.Direction.X, this.RigidBody.Transform2D.Position);
                }

                if (this.jumpButton.IsShooting)
                {
                    this.Jump();
                    this.controller = ControllerType.JoystickController;
                    this.isJump = true;
                }
                else
                {
                    if (this.controller == ControllerType.JoystickController)
                    {
                        this.isJump = false;
                    }
                }
            }
        }

        private void HandleKeys()
        {
            if (this.input.KeyboardState.IsConnected)
            {
                var keyState = this.input.KeyboardState;

                if (keyState.Right == WaveEngine.Common.Input.ButtonState.Pressed)
                {
                    this.MoveRight();
                }

                if (keyState.Left == WaveEngine.Common.Input.ButtonState.Pressed)
                {
                    this.MoveLeft();
                }

                if (keyState.Space == WaveEngine.Common.Input.ButtonState.Pressed)
                {
                    this.Jump();
                    this.controller = ControllerType.KeyboardController;
                    this.isJump = true;
                }
                else
                {
                    if (this.controller == ControllerType.KeyboardController)
                    {
                        this.isJump = false;
                    }
                }
            }
        }

        private void Jump()
        {
            if (!this.isJump && this.OnFloor)
            {
                this.RigidBody.ApplyLinearImpulse(Vector2.UnitY * -JumpImpulse, this.RigidBody.Transform2D.Position);
                this.soundManager.PlaySound(SoundType.Jump);
            }
        }

        private void MoveLeft()
        {
            if (this.OnFloor)
            {
                this.RigidBody.ApplyLinearImpulse(Vector2.UnitX * -SideImpulse, this.RigidBody.Transform2D.Position);
            }
        }

        private void MoveRight()
        {
            if (this.OnFloor)
            {
                this.RigidBody.ApplyLinearImpulse(Vector2.UnitX * SideImpulse, this.RigidBody.Transform2D.Position);
            }
        }

        private void Collider_BeginCollision(WaveEngine.Common.Physics2D.ICollisionInfo2D contact)
        {
            this.collisionCounter++;
            this.soundManager.PlaySound(SoundType.Contact);
        }

        private void Collider_EndCollision(WaveEngine.Common.Physics2D.ICollisionInfo2D contact)
        {
            this.collisionCounter--;
        }
    }
}
