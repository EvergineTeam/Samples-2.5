#region Using Statements
using Gdk;
using Gtk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WaveEngine.Common.Input;
using WaveEngine.Framework.Services;
#endregion

namespace WaveGTK
{
    public class WaveWidget : DrawingArea
    {
        [DllImport("libgdk-win32-2.0-0.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr gdk_win32_drawable_get_handle(IntPtr d);

        private bool isInitialized, isResized, rendering;

        private GameApp gameApp;
        private System.Threading.Tasks.Task renderTask;
        private WaveEngine.Framework.Services.Input input;

        public IntPtr WindowsHandler { get; private set; }
        public bool isMinimized { get; set; }
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WaveWidget" /> class.
        /// </summary>
        public WaveWidget()
        {
            this.isInitialized = false;
            this.WidthRequest = 800;
            this.HeightRequest = 600;
            this.CanFocus = true;

            this.Realized += OnRealized;
        }

        /// <summary>
        /// Called when [timer].
        /// </summary>
        /// <returns></returns>
        private bool OnTimer()
        {
            if (this.IsDisposed)
            {
                return false;
            }
            else
            {
                if (!this.rendering  && !this.isMinimized)
                {
                    System.Threading.Tasks.Task.Factory.StartNew(() =>
                    {
                        lock (this)
                        {
                            this.rendering = true;
                            this.gameApp.Render();
                            this.rendering = false;
                        }
                    });
                }
                return true;
            }
        }

        /// <summary>
        /// Called when [realized].
        /// </summary>
        /// <param name="o">The o.</param>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>         
        private void OnRealized(object o, EventArgs args)
        {
            this.WindowsHandler = gdk_win32_drawable_get_handle(this.GdkWindow.Handle);

            this.gameApp = new GameApp(this.WidthRequest, this.HeightRequest);
            this.gameApp.Configure(WindowsHandler);

            this.AddEvents((int)
                            (EventMask.ButtonPressMask
                            | EventMask.ButtonReleaseMask
                            | EventMask.KeyPressMask
                            | EventMask.KeyReleaseMask
                            | EventMask.PointerMotionMask
                            | EventMask.ScrollMask));

            this.GdkWindow.Background = new Gdk.Color(0, 0, 0);

            this.isInitialized = true;

            // 60 fps
            GLib.Timeout.Add(16, new GLib.TimeoutHandler(OnTimer));
        }

        /// <summary>
        /// Called when [expose event].
        /// </summary>
        /// <param name="evnt">The evnt.</param>
        /// <returns></returns>
        protected override bool OnExposeEvent(Gdk.EventExpose evnt)
        {
            bool result = base.OnExposeEvent(evnt);

            evnt.Window.Display.Sync();

            return result;
        }

        /// <summary>
        /// Called when [configure event].
        /// </summary>
        /// <param name="evnt">The evnt.</param>
        /// <returns></returns>
        protected override bool OnConfigureEvent(EventConfigure evnt)
        {
            bool result = base.OnConfigureEvent(evnt);

            this.isResized = true;
            if (evnt.Width > 1 && evnt.Height > 1)
            {
                this.gameApp.ResizeScreen(evnt.Width, evnt.Height);
            }

            this.isResized = false;

            return result;
        }       

        #region MouseEvents
        /// <summary>
        /// Called when [button press event].
        /// </summary>
        /// <param name="evnt">The evnt.</param>
        /// <returns></returns>
        protected override bool OnButtonPressEvent(Gdk.EventButton evnt)
        {
            if (this.input != null)
            {
                if (evnt.Button == 1) // Left mouse button
                {
                    this.input.MouseState.LeftButton = ButtonState.Pressed;
                }
                else if (evnt.Button == 2) // Middle mouse button
                {
                    this.input.MouseState.MiddleButton = ButtonState.Pressed;
                }
                else if (evnt.Button == 3) // Right mouse button
                {
                    this.input.MouseState.RightButton = ButtonState.Pressed;
                }
            }

            this.GrabFocus();

            return base.OnButtonPressEvent(evnt);
        }

        /// <summary>
        /// Called when [button release event].
        /// </summary>
        /// <param name="evnt">The evnt.</param>
        /// <returns></returns>
        protected override bool OnButtonReleaseEvent(Gdk.EventButton evnt)
        {
            if (this.input != null)
            {
                if (evnt.Button == 1) // Left mouse button
                {
                    this.input.MouseState.LeftButton = ButtonState.Release;
                }
                else if (evnt.Button == 2) // Middle mouse button
                {
                    this.input.MouseState.MiddleButton = ButtonState.Release;
                }
                else if (evnt.Button == 3) // Right mouse button
                {
                    this.input.MouseState.RightButton = ButtonState.Release;
                }
            }

            return base.OnButtonReleaseEvent(evnt);
        }

        /// <summary>
        /// Called when [motion notify event].
        /// </summary>
        /// <param name="evnt">The evnt.</param>
        /// <returns></returns>
        protected override bool OnMotionNotifyEvent(EventMotion evnt)
        {
            if (input != null)
            {
                this.input.MouseState.X = (int)evnt.X;
                this.input.MouseState.Y = (int)evnt.Y;
            }
            else
            {
                this.input = WaveServices.Input;
                if (this.input != null)
                {
                    this.input.IsEnabled = false;
                }
            }

            return base.OnMotionNotifyEvent(evnt);
        }

        /// <summary>
        /// Called when [scroll event].
        /// </summary>
        /// <param name="evnt">The evnt.</param>
        /// <returns></returns>
        protected override bool OnScrollEvent(EventScroll evnt)
        {
            if (evnt.Direction == ScrollDirection.Up)
            {
                WaveServices.Input.MouseState.Wheel += 1;
            }
            else if (evnt.Direction == ScrollDirection.Down)
            {
                WaveServices.Input.MouseState.Wheel -= 1;
            }

            return base.OnScrollEvent(evnt);
        }

        #endregion

        #region KeyboardEvents

        /// <summary>
        /// Called when [key press event].
        /// </summary>
        /// <param name="evnt">The evnt.</param>
        /// <returns></returns>
        [GLib.ConnectBefore]
        protected override bool OnKeyPressEvent(EventKey evnt)
        {
            #region Key mapping
            switch (evnt.Key)
            {
                case Gdk.Key.a:
                    this.input.KeyboardState.A = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case Gdk.Key.b:
                    this.input.KeyboardState.B = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case Gdk.Key.BackSpace:
                    this.input.KeyboardState.Back = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case Gdk.Key.c:
                    this.input.KeyboardState.C = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case Gdk.Key.Shift_Lock:
                    this.input.KeyboardState.CapsLock = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case Gdk.Key.Cancel:
                    this.input.KeyboardState.Crsel = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case Gdk.Key.d:
                    this.input.KeyboardState.D = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case Gdk.Key.Key_0:
                    this.input.KeyboardState.D0 = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case Gdk.Key.Key_1:
                    this.input.KeyboardState.D1 = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case Gdk.Key.Key_2:
                    this.input.KeyboardState.D2 = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case Gdk.Key.Key_3:
                    this.input.KeyboardState.D3 = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case Gdk.Key.Key_4:
                    this.input.KeyboardState.D4 = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case Gdk.Key.Key_5:
                    this.input.KeyboardState.D5 = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case Gdk.Key.Key_6:
                    this.input.KeyboardState.D6 = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case Gdk.Key.Key_7:
                    this.input.KeyboardState.D7 = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case Gdk.Key.Key_8:
                    this.input.KeyboardState.D8 = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case Gdk.Key.Key_9:
                    this.input.KeyboardState.D9 = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case Gdk.Key.Delete:
                    this.input.KeyboardState.Delete = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case Gdk.Key.Down:
                    this.input.KeyboardState.Down = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case Gdk.Key.e:
                    this.input.KeyboardState.E = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case Gdk.Key.Return:
                    this.input.KeyboardState.Enter = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case Gdk.Key.Escape:
                    this.input.KeyboardState.Escape = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case Gdk.Key.Execute:
                    this.input.KeyboardState.Execute = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case Gdk.Key.f:
                    this.input.KeyboardState.F = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case Gdk.Key.F1:
                    this.input.KeyboardState.F = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case Gdk.Key.F10:
                    this.input.KeyboardState.F10 = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case Gdk.Key.F11:
                    this.input.KeyboardState.F11 = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case Gdk.Key.F12:
                    this.input.KeyboardState.F12 = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case Gdk.Key.F2:
                    this.input.KeyboardState.F2 = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case Gdk.Key.F3:
                    this.input.KeyboardState.F3 = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case Gdk.Key.F4:
                    this.input.KeyboardState.F4 = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case Gdk.Key.F5:
                    this.input.KeyboardState.F5 = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case Gdk.Key.F6:
                    this.input.KeyboardState.F6 = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case Gdk.Key.F7:
                    this.input.KeyboardState.F7 = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case Gdk.Key.F8:
                    this.input.KeyboardState.F8 = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case Gdk.Key.F9:
                    this.input.KeyboardState.F9 = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case Gdk.Key.g:
                    this.input.KeyboardState.G = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case Gdk.Key.h:
                    this.input.KeyboardState.H = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case Gdk.Key.i:
                    this.input.KeyboardState.I = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case Gdk.Key.j:
                    this.input.KeyboardState.J = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case Gdk.Key.k:
                    this.input.KeyboardState.K = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case Gdk.Key.l:
                    this.input.KeyboardState.L = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case Gdk.Key.Left:
                    this.input.KeyboardState.Left = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case Gdk.Key.Alt_L:
                    this.input.KeyboardState.LeftAlt = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case Gdk.Key.Control_L:
                    this.input.KeyboardState.LeftControl = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case Gdk.Key.Shift_L:
                    this.input.KeyboardState.LeftShift = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case Gdk.Key.m:
                    this.input.KeyboardState.M = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case Gdk.Key.n:
                    this.input.KeyboardState.N = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case Gdk.Key.o:
                    this.input.KeyboardState.O = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case Gdk.Key.p:
                    this.input.KeyboardState.P = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case Gdk.Key.q:
                    this.input.KeyboardState.Q = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case Gdk.Key.r:
                    this.input.KeyboardState.R = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case Gdk.Key.Right:
                    this.input.KeyboardState.Right = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case Gdk.Key.Alt_R:
                    this.input.KeyboardState.RightAlt = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case Gdk.Key.Control_R:
                    this.input.KeyboardState.RightControl = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case Gdk.Key.Shift_R:
                    this.input.KeyboardState.RightShift = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case Gdk.Key.s:
                    this.input.KeyboardState.S = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case Gdk.Key.space:
                    this.input.KeyboardState.Space = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case Gdk.Key.KP_Subtract:
                    this.input.KeyboardState.Subtract = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case Gdk.Key.t:
                    this.input.KeyboardState.T = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case Gdk.Key.Tab:
                    this.input.KeyboardState.Tab = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case Gdk.Key.u:
                    this.input.KeyboardState.U = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case Gdk.Key.Up:
                    this.input.KeyboardState.Up = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case Gdk.Key.v:
                    this.input.KeyboardState.V = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case Gdk.Key.w:
                    this.input.KeyboardState.W = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case Gdk.Key.x:
                    this.input.KeyboardState.X = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case Gdk.Key.y:
                    this.input.KeyboardState.Y = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case Gdk.Key.z:
                    this.input.KeyboardState.Z = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
            }
            #endregion

            //System.Console.WriteLine("pressed {0}", evnt.Key);

            return base.OnKeyPressEvent(evnt);
        }

        /// <summary>
        /// Called when [key release event].
        /// </summary>
        /// <param name="evnt">The evnt.</param>
        /// <returns></returns>
        [GLib.ConnectBefore]
        protected override bool OnKeyReleaseEvent(EventKey evnt)
        {
            #region Key mapping
            switch (evnt.Key)
            {
                case Gdk.Key.a:
                    this.input.KeyboardState.A = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case Gdk.Key.b:
                    this.input.KeyboardState.B = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case Gdk.Key.BackSpace:
                    this.input.KeyboardState.Back = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case Gdk.Key.c:
                    this.input.KeyboardState.C = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case Gdk.Key.Shift_Lock:
                    this.input.KeyboardState.CapsLock = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case Gdk.Key.Cancel:
                    this.input.KeyboardState.Crsel = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case Gdk.Key.d:
                    this.input.KeyboardState.D = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case Gdk.Key.Key_0:
                    this.input.KeyboardState.D0 = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case Gdk.Key.Key_1:
                    this.input.KeyboardState.D1 = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case Gdk.Key.Key_2:
                    this.input.KeyboardState.D2 = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case Gdk.Key.Key_3:
                    this.input.KeyboardState.D3 = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case Gdk.Key.Key_4:
                    this.input.KeyboardState.D4 = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case Gdk.Key.Key_5:
                    this.input.KeyboardState.D5 = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case Gdk.Key.Key_6:
                    this.input.KeyboardState.D6 = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case Gdk.Key.Key_7:
                    this.input.KeyboardState.D7 = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case Gdk.Key.Key_8:
                    this.input.KeyboardState.D8 = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case Gdk.Key.Key_9:
                    this.input.KeyboardState.D9 = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case Gdk.Key.Delete:
                    this.input.KeyboardState.Delete = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case Gdk.Key.Down:
                    this.input.KeyboardState.Down = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case Gdk.Key.e:
                    this.input.KeyboardState.E = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case Gdk.Key.Return:
                    this.input.KeyboardState.Enter = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case Gdk.Key.Escape:
                    this.input.KeyboardState.Escape = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case Gdk.Key.Execute:
                    this.input.KeyboardState.Execute = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case Gdk.Key.f:
                    this.input.KeyboardState.F = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case Gdk.Key.F1:
                    this.input.KeyboardState.F = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case Gdk.Key.F10:
                    this.input.KeyboardState.F10 = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case Gdk.Key.F11:
                    this.input.KeyboardState.F11 = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case Gdk.Key.F12:
                    this.input.KeyboardState.F12 = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case Gdk.Key.F2:
                    this.input.KeyboardState.F2 = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case Gdk.Key.F3:
                    this.input.KeyboardState.F3 = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case Gdk.Key.F4:
                    this.input.KeyboardState.F4 = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case Gdk.Key.F5:
                    this.input.KeyboardState.F5 = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case Gdk.Key.F6:
                    this.input.KeyboardState.F6 = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case Gdk.Key.F7:
                    this.input.KeyboardState.F7 = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case Gdk.Key.F8:
                    this.input.KeyboardState.F8 = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case Gdk.Key.F9:
                    this.input.KeyboardState.F9 = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case Gdk.Key.g:
                    this.input.KeyboardState.G = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case Gdk.Key.h:
                    this.input.KeyboardState.H = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case Gdk.Key.i:
                    this.input.KeyboardState.I = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case Gdk.Key.j:
                    this.input.KeyboardState.J = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case Gdk.Key.k:
                    this.input.KeyboardState.K = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case Gdk.Key.l:
                    this.input.KeyboardState.L = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case Gdk.Key.Left:
                    this.input.KeyboardState.Left = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case Gdk.Key.Alt_L:
                    this.input.KeyboardState.LeftAlt = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case Gdk.Key.Control_L:
                    this.input.KeyboardState.LeftControl = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case Gdk.Key.Shift_L:
                    this.input.KeyboardState.LeftShift = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case Gdk.Key.m:
                    this.input.KeyboardState.M = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case Gdk.Key.n:
                    this.input.KeyboardState.N = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case Gdk.Key.o:
                    this.input.KeyboardState.O = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case Gdk.Key.p:
                    this.input.KeyboardState.P = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case Gdk.Key.q:
                    this.input.KeyboardState.Q = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case Gdk.Key.r:
                    this.input.KeyboardState.R = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case Gdk.Key.Right:
                    this.input.KeyboardState.Right = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case Gdk.Key.Alt_R:
                    this.input.KeyboardState.RightAlt = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case Gdk.Key.Control_R:
                    this.input.KeyboardState.RightControl = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case Gdk.Key.Shift_R:
                    this.input.KeyboardState.RightShift = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case Gdk.Key.s:
                    this.input.KeyboardState.S = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case Gdk.Key.space:
                    this.input.KeyboardState.Space = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case Gdk.Key.KP_Subtract:
                    this.input.KeyboardState.Subtract = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case Gdk.Key.t:
                    this.input.KeyboardState.T = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case Gdk.Key.Tab:
                    this.input.KeyboardState.Tab = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case Gdk.Key.u:
                    this.input.KeyboardState.U = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case Gdk.Key.Up:
                    this.input.KeyboardState.Up = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case Gdk.Key.v:
                    this.input.KeyboardState.V = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case Gdk.Key.w:
                    this.input.KeyboardState.W = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case Gdk.Key.x:
                    this.input.KeyboardState.X = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case Gdk.Key.y:
                    this.input.KeyboardState.Y = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case Gdk.Key.z:
                    this.input.KeyboardState.Z = WaveEngine.Common.Input.ButtonState.Release;
                    break;
            }
            #endregion

            return base.OnKeyReleaseEvent(evnt);
        }
        #endregion

        /// <summary>
        /// Called when [destroyed]. (Dispose)
        /// </summary>
        protected override void OnDestroyed()
        {
            base.OnDestroyed();

            this.IsDisposed = true;

            this.gameApp.OnDeactivate();
            this.gameApp.Dispose();
        }
    }
}
