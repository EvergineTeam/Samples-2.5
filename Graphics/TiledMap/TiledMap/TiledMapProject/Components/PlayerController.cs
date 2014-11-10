using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Diagnostic;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics2D;
using WaveEngine.Framework.Services;

namespace TiledMapProject.Components
{
    public class PlayerController : Behavior
    {
        private const float SideImpulse = 0.001f;
        private const float JumpImpulse = 0.08f;


        private Input input;
        private bool isJump;
        private int collisionCounter = 0;
        private Vector2 initPosition;

        private SoundManager soundManager;


        private bool OnFloor
        {
            get { return this.collisionCounter > 0; }
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

            this.soundManager = this.EntityManager.Find("soundManager").FindComponent<SoundManager>();

            this.input = WaveServices.Input;
            this.initPosition = this.Transform2D.Position;

            this.RigidBody.OnPhysic2DCollision += OnPhysic2DCollision;
            this.RigidBody.OnPhysic2DSeparation += OnPhysic2DSeparation;
        }

        public void Reset()
        {
            this.RigidBody.ResetPosition(this.initPosition);
            this.RigidBody.Rotation = 0;
            this.collisionCounter = 0;
        }

        protected override void Update(TimeSpan gameTime)
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
                    this.isJump = true;
                }
                else
                {
                    this.isJump = false;
                }
            }
        }

        private void Jump()
        {
            if (!this.isJump && this.OnFloor)
            {
                this.RigidBody.ApplyLinearImpulse(Vector2.UnitY * -JumpImpulse);
                this.soundManager.PlaySound(SoundType.Jump);
            }
        }

        private void MoveLeft()
        {
            if (this.OnFloor)
            {
                this.RigidBody.ApplyLinearImpulse(Vector2.UnitX * -SideImpulse);
            }
        }

        private void MoveRight()
        {
            if (this.OnFloor)
            {
                this.RigidBody.ApplyLinearImpulse(Vector2.UnitX * SideImpulse);
            }
        }

        private void OnPhysic2DCollision(object sender, Physic2DCollisionEventArgs args)
        {
            this.collisionCounter++;
            Labels.Add("#Counter", this.collisionCounter);

            this.soundManager.PlaySound(SoundType.Contact);
        }

        private void OnPhysic2DSeparation(object sender, Physic2DSeparationEventArgs args)
        {
            this.collisionCounter--;
            Labels.Add("#Counter", this.collisionCounter);
        }
    }
}
