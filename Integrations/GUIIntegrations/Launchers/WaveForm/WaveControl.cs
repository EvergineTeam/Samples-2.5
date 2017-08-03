#region Using Statements
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WaveEngine.Common.Input;
using WaveEngine.Framework.Services;
using WaveEngine.Adapter;
#endregion

namespace WaveForm
{
    public partial class WaveControl : Control
    {
        private bool isInitialized, isDisposed, isResizing;

        private GameApp gameApp;
        private System.Threading.Tasks.Task renderTask;
        private WaveEngine.Framework.Services.Input input;
        private Font fontForDesignMode;

        /// <summary>
        /// Initializes a new instance of the <see cref="WaveControl" /> class.
        /// </summary>
        public WaveControl()
        {
            InitializeComponent();

            this.Width = 1024;
            this.Height = 768;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque | ControlStyles.UserPaint, true);
            UpdateStyles();

            this.gameApp = new GameApp(this.Bounds.Width, this.Bounds.Height);
            this.gameApp.Configure(this.Handle);

            this.isInitialized = true;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Resize" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (this.gameApp != null && this.Bounds.Width > 0 && this.Bounds.Height > 0)
            {
                this.gameApp.ResizeScreen(this.Bounds.Width, this.Bounds.Height);
            }
        }

        /// <summary>
        /// Controls the render.
        /// </summary>
        public void ControlRender()
        {
            this.gameApp.BaseRender();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.HandleDestroyed" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnHandleDestroyed(EventArgs e)
        {
            base.OnHandleDestroyed(e);

            this.isDisposed = true;

            this.gameApp.OnDeactivate();
            this.gameApp.Dispose();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Paint" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data.</param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (DesignMode)
                base.OnPaintBackground(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Paint"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"/> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (DesignMode)
            {
                if (fontForDesignMode == null)
                    fontForDesignMode = new Font("Calibri", 24, FontStyle.Regular);

                e.Graphics.Clear(System.Drawing.Color.WhiteSmoke);
                string text = "Wave Engine Control";
                var sizeText = e.Graphics.MeasureString(text, fontForDesignMode);

                e.Graphics.DrawString(text, fontForDesignMode, new SolidBrush(System.Drawing.Color.Black), (Width - sizeText.Width) / 2, (Height - sizeText.Height) / 2);
            }
        }

        #region MouseEvents

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseMove" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (input != null)
            {
                this.input.MouseState.X = e.X;
                this.input.MouseState.Y = e.Y;

                this.input.TouchPanelState = new TouchPanelState() { IsConnected = true };

                if (this.input.MouseState.LeftButton == WaveEngine.Common.Input.ButtonState.Pressed)
                {
                    this.input.TouchPanelState.AddTouchLocation(0, TouchLocationState.Pressed, e.X, e.Y);
                }
            }
            else
            {
                this.input = WaveServices.Input;
                if (this.input != null)
                {
                    this.input.IsEnabled = false;
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseUp" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (input != null)
            {
                if (e.Button == MouseButtons.Left)
                {
                    this.input.MouseState.LeftButton = WaveEngine.Common.Input.ButtonState.Released;
                }
                else if (e.Button == MouseButtons.Middle)
                {
                    this.input.MouseState.MiddleButton = WaveEngine.Common.Input.ButtonState.Released;
                }
                else if (e.Button == MouseButtons.Right)
                {
                    this.input.MouseState.RightButton = WaveEngine.Common.Input.ButtonState.Released;
                }
            }
            else
            {
                this.input = WaveServices.Input;
                if (this.input != null)
                {
                    this.input.IsEnabled = false;
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseDown" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (this.input != null)
            {
                if (e.Button == MouseButtons.Left)
                {
                    this.input.MouseState.LeftButton = WaveEngine.Common.Input.ButtonState.Pressed;
                }
                else if (e.Button == MouseButtons.Middle)
                {
                    this.input.MouseState.MiddleButton = WaveEngine.Common.Input.ButtonState.Pressed;
                }
                else if (e.Button == MouseButtons.Right)
                {
                    this.input.MouseState.RightButton = WaveEngine.Common.Input.ButtonState.Pressed;
                }
            }
            else
            {
                this.input = WaveServices.Input;
                if (this.input != null)
                {
                    this.input.IsEnabled = false;
                }
            }

            this.Focus();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseWheel" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            if (e.Delta > 0)
            {
                this.input.MouseState.Wheel += 1;
            }
            else
            {
                this.input.MouseState.Wheel -= 1;
            }
        }

        #endregion

        #region KeyboardEvents
        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.KeyUp" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs" /> that contains the event data.</param>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            if (input == null)
            {
                this.input = WaveServices.Input;
                if (this.input != null)
                {
                    this.input.IsEnabled = false;
                }
                else
                {
                    return;
                }
            }

            #region Key mapping
            switch (e.KeyCode)
            {
                case System.Windows.Forms.Keys.A:
                    this.input.KeyboardState.A = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.B:
                    this.input.KeyboardState.B = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.Back:
                    this.input.KeyboardState.Back = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.C:
                    this.input.KeyboardState.C = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.CapsLock:
                    this.input.KeyboardState.CapitalLock = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.Crsel:
                    this.input.KeyboardState.Cancel = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.D:
                    this.input.KeyboardState.D = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.D0:
                    this.input.KeyboardState.Number0 = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.D1:
                    this.input.KeyboardState.Number1 = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.D2:
                    this.input.KeyboardState.Number2 = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.D3:
                    this.input.KeyboardState.Number3 = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.D4:
                    this.input.KeyboardState.Number4 = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.D5:
                    this.input.KeyboardState.Number5 = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.D6:
                    this.input.KeyboardState.Number6 = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.D7:
                    this.input.KeyboardState.Number7 = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.D8:
                    this.input.KeyboardState.Number8 = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.D9:
                    this.input.KeyboardState.Number9 = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.Delete:
                    this.input.KeyboardState.Delete = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.Down:
                    this.input.KeyboardState.Down = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.E:
                    this.input.KeyboardState.E = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.Enter:
                    this.input.KeyboardState.Enter = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.Escape:
                    this.input.KeyboardState.Escape = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.Execute:
                    this.input.KeyboardState.Execute = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.F:
                    this.input.KeyboardState.F = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.F1:
                    this.input.KeyboardState.F = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.F10:
                    this.input.KeyboardState.F10 = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.F11:
                    this.input.KeyboardState.F11 = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.F12:
                    this.input.KeyboardState.F12 = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.F2:
                    this.input.KeyboardState.F2 = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.F3:
                    this.input.KeyboardState.F3 = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.F4:
                    this.input.KeyboardState.F4 = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.F5:
                    this.input.KeyboardState.F5 = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.F6:
                    this.input.KeyboardState.F6 = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.F7:
                    this.input.KeyboardState.F7 = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.F8:
                    this.input.KeyboardState.F8 = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.F9:
                    this.input.KeyboardState.F9 = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.G:
                    this.input.KeyboardState.G = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.H:
                    this.input.KeyboardState.H = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.I:
                    this.input.KeyboardState.I = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.J:
                    this.input.KeyboardState.J = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.K:
                    this.input.KeyboardState.K = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.L:
                    this.input.KeyboardState.L = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.Left:
                    this.input.KeyboardState.Left = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.LMenu:
                    this.input.KeyboardState.LeftAlt = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.LControlKey:
                    this.input.KeyboardState.LeftControl = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.LShiftKey:
                    this.input.KeyboardState.LeftShift = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.LWin:
                    this.input.KeyboardState.LeftWindows = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.M:
                    this.input.KeyboardState.M = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.N:
                    this.input.KeyboardState.N = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.O:
                    this.input.KeyboardState.O = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.P:
                    this.input.KeyboardState.P = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.Q:
                    this.input.KeyboardState.Q = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.R:
                    this.input.KeyboardState.R = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.Right:
                    this.input.KeyboardState.Right = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.RMenu:
                    this.input.KeyboardState.RightAlt = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.RControlKey:
                    this.input.KeyboardState.RightControl = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.RShiftKey:
                    this.input.KeyboardState.RightShift = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.RWin:
                    this.input.KeyboardState.RightWindows = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.S:
                    this.input.KeyboardState.S = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.Space:
                    this.input.KeyboardState.Space = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.Subtract:
                    this.input.KeyboardState.Subtract = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.T:
                    this.input.KeyboardState.T = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.Tab:
                    this.input.KeyboardState.Tab = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.U:
                    this.input.KeyboardState.U = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.Up:
                    this.input.KeyboardState.Up = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.V:
                    this.input.KeyboardState.V = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.W:
                    this.input.KeyboardState.W = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.X:
                    this.input.KeyboardState.X = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.Y:
                    this.input.KeyboardState.Y = WaveEngine.Common.Input.ButtonState.Released;
                    break;
                case System.Windows.Forms.Keys.Z:
                    this.input.KeyboardState.Z = WaveEngine.Common.Input.ButtonState.Released;
                    break;
            #endregion

            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.KeyDown" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs" /> that contains the event data.</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (input == null)
            {
                this.input = WaveServices.Input;
                if (this.input != null)
                {
                    this.input.IsEnabled = false;
                }
                else
                {
                    return;
                }
            }

            #region Key mapping
            switch (e.KeyCode)
            {
                case System.Windows.Forms.Keys.A:
                    this.input.KeyboardState.A = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.B:
                    this.input.KeyboardState.B = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.Back:
                    this.input.KeyboardState.Back = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.C:
                    this.input.KeyboardState.C = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.CapsLock:
                    this.input.KeyboardState.CapitalLock = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.Crsel:
                    this.input.KeyboardState.Cancel = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.D:
                    this.input.KeyboardState.D = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.D0:
                    this.input.KeyboardState.Number0 = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.D1:
                    this.input.KeyboardState.Number1 = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.D2:
                    this.input.KeyboardState.Number2 = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.D3:
                    this.input.KeyboardState.Number3 = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.D4:
                    this.input.KeyboardState.Number4 = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.D5:
                    this.input.KeyboardState.Number5 = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.D6:
                    this.input.KeyboardState.Number6 = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.D7:
                    this.input.KeyboardState.Number7 = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.D8:
                    this.input.KeyboardState.Number8 = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.D9:
                    this.input.KeyboardState.Number9 = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.Delete:
                    this.input.KeyboardState.Delete = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.Down:
                    this.input.KeyboardState.Down = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.E:
                    this.input.KeyboardState.E = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.Enter:
                    this.input.KeyboardState.Enter = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.Escape:
                    this.input.KeyboardState.Escape = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.Execute:
                    this.input.KeyboardState.Execute = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.F:
                    this.input.KeyboardState.F = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.F1:
                    this.input.KeyboardState.F = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.F10:
                    this.input.KeyboardState.F10 = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.F11:
                    this.input.KeyboardState.F11 = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.F12:
                    this.input.KeyboardState.F12 = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.F2:
                    this.input.KeyboardState.F2 = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.F3:
                    this.input.KeyboardState.F3 = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.F4:
                    this.input.KeyboardState.F4 = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.F5:
                    this.input.KeyboardState.F5 = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.F6:
                    this.input.KeyboardState.F6 = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.F7:
                    this.input.KeyboardState.F7 = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.F8:
                    this.input.KeyboardState.F8 = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.F9:
                    this.input.KeyboardState.F9 = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.G:
                    this.input.KeyboardState.G = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.H:
                    this.input.KeyboardState.H = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.I:
                    this.input.KeyboardState.I = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.J:
                    this.input.KeyboardState.J = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.K:
                    this.input.KeyboardState.K = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.L:
                    this.input.KeyboardState.L = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.Left:
                    this.input.KeyboardState.Left = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.LMenu:
                    this.input.KeyboardState.LeftAlt = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.LControlKey:
                    this.input.KeyboardState.LeftControl = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.LShiftKey:
                    this.input.KeyboardState.LeftShift = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.LWin:
                    this.input.KeyboardState.LeftWindows = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.M:
                    this.input.KeyboardState.M = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.N:
                    this.input.KeyboardState.N = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.O:
                    this.input.KeyboardState.O = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.P:
                    this.input.KeyboardState.P = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.Q:
                    this.input.KeyboardState.Q = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.R:
                    this.input.KeyboardState.R = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.Right:
                    this.input.KeyboardState.Right = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.RMenu:
                    this.input.KeyboardState.RightAlt = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.RControlKey:
                    this.input.KeyboardState.RightControl = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.RShiftKey:
                    this.input.KeyboardState.RightShift = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.RWin:
                    this.input.KeyboardState.RightWindows = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.S:
                    this.input.KeyboardState.S = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.Space:
                    this.input.KeyboardState.Space = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.Subtract:
                    this.input.KeyboardState.Subtract = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.T:
                    this.input.KeyboardState.T = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.Tab:
                    this.input.KeyboardState.Tab = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.U:
                    this.input.KeyboardState.U = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.Up:
                    this.input.KeyboardState.Up = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.V:
                    this.input.KeyboardState.V = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.W:
                    this.input.KeyboardState.W = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.X:
                    this.input.KeyboardState.X = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.Y:
                    this.input.KeyboardState.Y = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case System.Windows.Forms.Keys.Z:
                    this.input.KeyboardState.Z = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
            #endregion
            }

        #endregion

        }
    }
}
