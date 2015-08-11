#region Using Statemetns
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Common.Input;
using WaveEngine.Common.Math;
using WaveEngine.Components.Animation;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
#endregion

namespace IsisTemple.Components
{
    public enum EAnimState
    {
        Idle,
        Walk,
        Run
    }

    [DataContract]
    public class IsisControllerBehavior : Behavior
    {
        #region Constants
        // Walk speed (m/s)
        private const float WALK_SPEED = 1.26f;
        // Jog speed (m/s)
        private const float JOG_SPEED = 2.177f;
        #endregion

        #region Variables
        [RequiredComponent]
        protected Transform3D transform3D;

        [RequiredComponent]
        protected Animation3D animation3D;

        private EAnimState state = EAnimState.Idle;
        #endregion

        #region Properties
        private EAnimState State
        {
            get { return this.state; }
            set
            {
                if (this.state != value)
                {
                    this.animation3D.PlayAnimation(value.ToString(), true);
                }
                this.state = value;
            }
        }

        public bool GoUp { get; set; }

        public bool GoDown { get; set; }

        public bool GoRight { get; set; }

        public bool GoLeft { get; set; }

        public bool Run { get; set; }
        #endregion

        #region Initialize
        public IsisControllerBehavior()
            : base("IsisControllerBehavior")
        {
        }
        #endregion

        #region Public Methods
        protected override void Update(TimeSpan gameTime)
        {
            var inputService = WaveServices.Input;

            Vector3 advance = Vector3.Zero;
            Vector3 rotation = transform3D.Rotation;
            bool isMoving = false;

            var keyboardState = inputService.KeyboardState;
            var gamePadState = inputService.GamePadState;

            // Key up for advance!
            if (this.GoUp
             || keyboardState.Up == ButtonState.Pressed
             || keyboardState.W == ButtonState.Pressed
             || gamePadState.DPad.Up == ButtonState.Pressed)
            {
                isMoving = true;
                rotation = new Vector3(0, MathHelper.Pi, 0);
                advance = -Vector3.UnitZ;
            }

            // Key down for advance!
            if (this.GoDown
             || keyboardState.Down == ButtonState.Pressed
             || keyboardState.S == ButtonState.Pressed
             || gamePadState.DPad.Down == ButtonState.Pressed)
            {
                isMoving = true;
                rotation = Vector3.Zero;
                advance = Vector3.UnitZ;
            }

            // Key up for advance!
            if (this.GoRight
             || keyboardState.Right == ButtonState.Pressed
             || keyboardState.D == ButtonState.Pressed
             || gamePadState.DPad.Right == ButtonState.Pressed)
            {
                isMoving = true;
                rotation = new Vector3(0, MathHelper.PiOver2, 0);
                advance = Vector3.UnitX;
            }

            // Key left for advance!
            if (this.GoLeft
             || keyboardState.Left == ButtonState.Pressed
             || keyboardState.A == ButtonState.Pressed
             || gamePadState.DPad.Left == ButtonState.Pressed)
            {
                isMoving = true;
                rotation = new Vector3(0, -MathHelper.PiOver2, 0);
                advance = -Vector3.UnitX;
            }

            if (!isMoving)
            {
                // If no action, return to idle state :)
                this.State = EAnimState.Idle;
                advance = Vector3.Zero;
            }
            else if (this.Run
             || keyboardState.RightShift == ButtonState.Pressed 
             || keyboardState.LeftShift == ButtonState.Pressed 
             || gamePadState.Buttons.A == ButtonState.Pressed)
            {
                // Run you fools!
                this.State = EAnimState.Run;
                advance *= JOG_SPEED;
            }
            else
            {
                // Walk for a while
                this.State = EAnimState.Walk;
                advance *= WALK_SPEED;
            }

            this.transform3D.Position += (advance * (float)gameTime.TotalSeconds);
            this.transform3D.Rotation = rotation;
        }
    }
        #endregion
}
