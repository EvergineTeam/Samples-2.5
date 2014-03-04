using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaveEngine.Common.Math;
using WaveEngine.Components.Animation;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;

namespace IsisTempleProject.Components
{
    public enum EAnimState
    {
        Idle,
        Walk,
        Jog
    }

    public class IsisBehavior : Behavior
    {
        #region Constants
        // Walk speed (m/s)
        private const float WALK_SPEED = 1.26f;
        // Jog speed (m/s)
        private const float JOG_SPEED = 2.177f;
        #endregion

        #region Variables
        [RequiredComponent]
        public Transform3D transform3D;

        [RequiredComponent]
        public Animation3D animation3D;

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
        public IsisBehavior()
            : base("IsisBehavior")
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


            // Key up for advance!
            if (this.GoUp || inputService.KeyboardState.Up == WaveEngine.Common.Input.ButtonState.Pressed || inputService.GamePadState.DPad.Up == WaveEngine.Common.Input.ButtonState.Pressed)
            {
                isMoving = true;
                rotation = Vector3.Zero;

                // Key left shift for RUN!
                if (this.Run || inputService.KeyboardState.LeftShift == WaveEngine.Common.Input.ButtonState.Pressed || inputService.GamePadState.Buttons.A == WaveEngine.Common.Input.ButtonState.Pressed)
                {
                    // Run you fools!
                    this.State = EAnimState.Jog;
                    advance = Vector3.UnitZ * JOG_SPEED;
                }
                else
                {
                    //Walk for a while
                    this.State = EAnimState.Walk;
                    advance = Vector3.UnitZ * WALK_SPEED;
                }

            }

            // Key down for advance!
            if (this.GoDown || inputService.KeyboardState.Down == WaveEngine.Common.Input.ButtonState.Pressed || inputService.GamePadState.DPad.Down == WaveEngine.Common.Input.ButtonState.Pressed)
            {
                isMoving = true;
                rotation = new Vector3(0, MathHelper.Pi, 0);

                // Key left shift for RUN!
                if (this.Run || inputService.KeyboardState.LeftShift == WaveEngine.Common.Input.ButtonState.Pressed || inputService.GamePadState.Buttons.A == WaveEngine.Common.Input.ButtonState.Pressed)
                {
                    // Run you fools!
                    this.State = EAnimState.Jog;
                    advance = -Vector3.UnitZ * JOG_SPEED;
                }
                else
                {
                    //Walk for a while
                    this.State = EAnimState.Walk;
                    advance = -Vector3.UnitZ * WALK_SPEED;
                }
            }

            // Key up for advance!
            if (this.GoRight || inputService.KeyboardState.Right == WaveEngine.Common.Input.ButtonState.Pressed || inputService.GamePadState.DPad.Right == WaveEngine.Common.Input.ButtonState.Pressed)
            {
                isMoving = true;
                rotation = new Vector3(0, -MathHelper.PiOver2, 0); ;

                // Key left shift for RUN!
                if (this.Run || inputService.KeyboardState.LeftShift == WaveEngine.Common.Input.ButtonState.Pressed || inputService.GamePadState.Buttons.A == WaveEngine.Common.Input.ButtonState.Pressed)
                {
                    // Run you fools!
                    this.State = EAnimState.Jog;
                    advance = -Vector3.UnitX * JOG_SPEED;
                }
                else
                {
                    //Walk for a while
                    this.State = EAnimState.Walk;
                    advance = -Vector3.UnitX * WALK_SPEED;
                }
            }

            // Key left for advance!
            if (this.GoLeft || inputService.KeyboardState.Left == WaveEngine.Common.Input.ButtonState.Pressed || inputService.GamePadState.DPad.Left == WaveEngine.Common.Input.ButtonState.Pressed)
            {
                isMoving = true;
                rotation = new Vector3(0, MathHelper.PiOver2, 0);

                // Key left shift for RUN!
                if (this.Run || inputService.KeyboardState.LeftShift == WaveEngine.Common.Input.ButtonState.Pressed || inputService.GamePadState.Buttons.A == WaveEngine.Common.Input.ButtonState.Pressed)
                {
                    // Run you fools!
                    this.State = EAnimState.Jog;
                    advance = Vector3.UnitX * JOG_SPEED;
                }
                else
                {
                    //Walk for a while
                    this.State = EAnimState.Walk;
                    advance = Vector3.UnitX * WALK_SPEED;
                }
            }

            if (!isMoving)
            {
                //If no action, return to idle state :)
                this.State = EAnimState.Idle;
                advance = Vector3.Zero;
            }

            this.transform3D.Position += (advance * (float)gameTime.TotalSeconds);
            this.transform3D.Rotation = rotation;
        }
    } 
        #endregion
}
