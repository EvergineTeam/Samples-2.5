#region File Description
//-----------------------------------------------------------------------------
// DirectXWidget
//
// Copyright © 2015 Wave Corporation
// Use is subject to license terms.
//-----------------------------------------------------------------------------

#endregion

#region Using Statements
using Gdk;
using Gtk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ProjectGame = TeapotSample.Game;
#endregion

namespace WaveGTK.WaveIntegration
{
    public abstract class DirectXWidget : DrawingArea
    {
        #region Generated Code
        [DllImport("libgdk-win32-2.0-0.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr gdk_win32_drawable_get_handle(IntPtr d);
        #endregion

        /// <summary>
        /// bool initialized, isResized, rendering, isRegenerateHandler
        /// </summary>
        protected bool isInitialized, isResized, rendering, isRegenerateHandler;

        /// <summary>
        /// The game app
        /// </summary>
        protected GameApp gameApp;

        /// <summary>
        /// The timer id
        /// </summary>
        protected uint timerId;

        /// <summary>
        /// Occurs when [initialized].
        /// </summary>
        public event EventHandler<GameApp> Initialized;

        #region Properties

        /// <summary>
        /// Gets the windows handler.
        /// </summary>
        public IntPtr WindowsHandler { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is minimized.
        /// </summary>
        public bool IsMinimized { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is disposed.
        /// </summary>
        public bool IsDisposed { get; private set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectXWidget" /> class.
        /// </summary>
        public DirectXWidget()
        {
            this.isInitialized = false;
        }

        /// <summary>
        /// Called when [timer].
        /// </summary>
        /// <returns>True mean execute again</returns>
        private bool OnTimer()
        {
#if DEBUG
            if (this.IsDisposed)
            {
                return false;
            }
#else
            if (this.IsDisposed)
            {
                return false;
            }
#endif
            else
            {
                if (!this.rendering && !this.IsMinimized && !this.isResized)
                {
                    //// .Net manages a ThreadPool for this operation
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
        /// Called when [expose event].
        /// </summary>
        /// <param name="evnt">The evnt.</param>
        /// <returns>Reescaled success</returns>
        protected override bool OnExposeEvent(Gdk.EventExpose evnt)
        {
            bool result = base.OnExposeEvent(evnt);

            IntPtr handler = gdk_win32_drawable_get_handle(this.GdkWindow.Handle);
            if (handler != this.WindowsHandler)
            {
                this.isRegenerateHandler = true;
                this.WindowsHandler = handler;
            }
            else
            {
                this.isRegenerateHandler = false;
            }

            if (!this.isInitialized)
            {
                this.gameApp = new GameApp(this.Allocation.Width, this.Allocation.Height);

                this.gameApp.Configure(this.WindowsHandler);

                this.gameApp.Initialized += this.OnInitialized;

                this.GdkWindow.Background = new Gdk.Color(0, 0, 0);

                this.isInitialized = true;

                this.timerId = GLib.Timeout.Add(16, new GLib.TimeoutHandler(this.OnTimer));
            }
            else if (this.isRegenerateHandler)
            {
                GLib.Source.Remove(this.timerId);

                ////this.gameApp.Width = this.Allocation.Width;
                ////this.gameApp.Height = this.Allocation.Height;
                this.gameApp.Configure(this.WindowsHandler);

                GLib.Timeout.Add(16, new GLib.TimeoutHandler(this.OnTimer));
            }

            evnt.Window.Display.Sync();

            return result;
        }

        /// <summary>
        /// On Initialized method
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">game instance</param>
        protected virtual void OnInitialized(object sender, ProjectGame e)
        {
            if (this.Initialized != null)
            {
                this.Initialized(this, this.gameApp);
            }
        }

        /// <summary>
        /// Called when [configure event].
        /// </summary>
        /// <param name="evnt">The evnt.</param>
        /// <returns>Configured success</returns>
        protected override bool OnConfigureEvent(EventConfigure evnt)
        {
            bool result = base.OnConfigureEvent(evnt);

            this.isResized = true;
            if (this.gameApp != null && evnt.Width > 1 && evnt.Height > 1)
            {
                this.gameApp.ResizeScreen(evnt.Width, evnt.Height);
            }

            this.isResized = false;

            return result;
        }

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
