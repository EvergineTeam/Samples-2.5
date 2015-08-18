#region File Description
//-----------------------------------------------------------------------------
// OpenTKWidget
//
// Copyright © 2015 Wave Corporation
// Use is subject to license terms.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Gtk;
using OpenTK.Graphics;
using OpenTK.Platform;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;
using WaveEditor.Core;
#endregion

namespace WaveGTK.WaveIntegration
{
    public abstract class OpenTKWidget : DrawingArea, IDisposable
    {
        protected GameApp gameApp;

        public event EventHandler<GameApp> Initialized;

        protected Stopwatch stopwatch;

        protected bool isInitialized, isResized, rendering;

        /// <summary>
        /// The timer id
        /// </summary>
        protected uint timerId;

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

        IGraphicsContext graphicsContext;
        static int graphicsContextCount;

        #region Mac extra variables

        IntPtr nsView;
        uint uniqueId = 1;
        Gdk.Rectangle lastRectangle;
        bool lastVisible = true;

        #endregion

        /// <summary>Use a single buffer versus a double buffer.</summary>
        [Browsable(true)]
        public bool SingleBuffer { get; set; }

        /// <summary>Color Buffer Bits-Per-Pixel</summary>
        public int ColorBPP { get; set; }

        /// <summary>Accumulation Buffer Bits-Per-Pixel</summary>
        public int AccumulatorBPP { get; set; }

        /// <summary>Depth Buffer Bits-Per-Pixel</summary>
        public int DepthBPP { get; set; }

        /// <summary>Stencil Buffer Bits-Per-Pixel</summary>
        public int StencilBPP { get; set; }

        /// <summary>Number of samples</summary>
        public int Samples { get; set; }

        /// <summary>Indicates if steropic renderering is enabled</summary>
        public bool Stereo { get; set; }

        IWindowInfo windowInfo;

        /// <summary>The major version of OpenGL to use.</summary>
        public int GlVersionMajor { get; set; }

        /// <summary>The minor version of OpenGL to use.</summary>
        public int GlVersionMinor { get; set; }

        public GraphicsContextFlags GraphicsContextFlags
        {
            get { return graphicsContextFlags; }
            set { graphicsContextFlags = value; }
        }
        GraphicsContextFlags graphicsContextFlags;

        /// <summary>Constructs a new OpenTKWidget.</summary>
        public OpenTKWidget() : this(GraphicsMode.Default) { }

        /// <summary>Constructs a new OpenTKWidget using a given GraphicsMode</summary>
        public OpenTKWidget(GraphicsMode graphicsMode) : this(graphicsMode, 1, 0, GraphicsContextFlags.Default) { }

        /// <summary>Constructs a new OpenTKWidget</summary>
        public OpenTKWidget(GraphicsMode graphicsMode, int glVersionMajor, int glVersionMinor, GraphicsContextFlags graphicsContextFlags)
        {
            this.isInitialized = false;

            this.DoubleBuffered = false;

            SingleBuffer = graphicsMode.Buffers == 1;
            ColorBPP = graphicsMode.ColorFormat.BitsPerPixel;
            AccumulatorBPP = graphicsMode.AccumulatorFormat.BitsPerPixel;
            DepthBPP = graphicsMode.Depth;
            StencilBPP = graphicsMode.Stencil;
            Samples = graphicsMode.Samples;
            Stereo = graphicsMode.Stereo;

            GlVersionMajor = glVersionMajor;
            GlVersionMinor = glVersionMinor;
            GraphicsContextFlags = graphicsContextFlags;
        }

        ~OpenTKWidget() { Dispose(false); }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
            base.Dispose();
        }

        public virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                graphicsContext.MakeCurrent(windowInfo);
                
                if (GraphicsContext.ShareContexts && (Interlocked.Decrement(ref graphicsContextCount) == 0))
                {
                    OnGraphicsContextShuttingDown();
                    sharedContextInitialized = false;
                }
                graphicsContext.Dispose();
            }
        }

        // Called when the first GraphicsContext is created in the case of GraphicsContext.ShareContexts == True;
        public static event EventHandler GraphicsContextInitialized;
        static void OnGraphicsContextInitialized() { if (GraphicsContextInitialized != null) GraphicsContextInitialized(null, EventArgs.Empty); }

        // Called when the first GraphicsContext is being destroyed in the case of GraphicsContext.ShareContexts == True;
        public static event EventHandler GraphicsContextShuttingDown;
        static void OnGraphicsContextShuttingDown() { if (GraphicsContextShuttingDown != null) GraphicsContextShuttingDown(null, EventArgs.Empty); }

        // Called when this GLWidget has a valid GraphicsContext		
        protected virtual void OnInitialized(object sender, WaveEditor.Viewport.Game e)
        {
            this.gameApp = new GameApp(this.Allocation.Width, this.Allocation.Height);
            this.gameApp.Initialize();

            // Use clock
            this.stopwatch = new global::System.Diagnostics.Stopwatch();

            if (this.Initialized != null)
            {
                this.Initialized(this, this.gameApp);
            }
        }

        // Called when this GLWidget needs to render a frame		
        protected virtual void OnRenderFrame()
        {
            if (!rendering)
            {

                this.rendering = true;

                this.stopwatch.Stop();
                var time = this.stopwatch.Elapsed;
                this.stopwatch.Reset();
                this.stopwatch.Start();

                this.gameApp.Update(time);
                this.gameApp.Draw(time);

                this.rendering = false;
            }
        }            

        // Called when a widget is realized. (window handles and such are valid)
        // protected override void OnRealized() { base.OnRealized(); }

        static bool sharedContextInitialized = false;

        // Called when the widget needs to be (fully or partially) redrawn.
        protected override bool OnExposeEvent(Gdk.EventExpose eventExpose)
        {
            if (!this.isInitialized)
            {
                GLib.Timeout.Add(10, new GLib.TimeoutHandler(this.CreateGraphicsContext));
                this.isInitialized = true;
            }
            else
            {
                if (graphicsContext != null)
                {
                    graphicsContext.MakeCurrent(windowInfo);
                }
            }

            bool result = base.OnExposeEvent(eventExpose);
            //OnRenderFrame();
            eventExpose.Window.Display.Sync(); // Add Sync call to fix resize rendering problem (Jay L. T. Cornwall) - How does this affect VSync?
            //graphicsContext.SwapBuffers();
            return result;
        }

        /// <summary>
        /// Create Graphcis Context
        /// </summary>
        /// <returns></returns>
        public bool CreateGraphicsContext()
        {
            // If this looks uninitialized...  initialize.
            if (ColorBPP == 0)
            {
                ColorBPP = 32;

                if (DepthBPP == 0) DepthBPP = 16;
            }

            ColorFormat colorBufferColorFormat = new ColorFormat(ColorBPP);

            ColorFormat accumulationColorFormat = new ColorFormat(AccumulatorBPP);

            int buffers = 2;
            if (SingleBuffer) buffers--;

            GraphicsMode graphicsMode = new GraphicsMode(colorBufferColorFormat, DepthBPP, StencilBPP, Samples, accumulationColorFormat, buffers, Stereo);

#if MAC
            IntPtr windowHandle = gdk_quartz_window_get_nswindow(GdkWindow.Handle);

            // Problem: gdk_window_ensure_native() crashes when used more than once.
            // For now, just create a NSView in place and use that instead. 
            // Needs some care updating when resizing, hiding, etc, but seems to work.
            // (I'd guess this is pretty much what gdk_window_ensure_native() does internally.)

            var customView = NativeClass.AllocateClass("CustomNSView" + uniqueId++, "NSView");
            NativeClass.RegisterClass(customView);

            nsView = SendIntPtr(SendIntPtr(customView, sel_registerName("alloc")), sel_registerName("initWithFrame:"), new RectangleF(0, 0, 400, 400));

            bool native = gdk_window_ensure_native(GdkWindow.Handle);
            if (!native)
            {
                throw new PlatformNotSupportedException("Could not create native view.");
            }

            nsView = gdk_quartz_window_get_nsview(GdkWindow.Handle);

            windowInfo = OpenTK.Platform.Utilities.CreateMacOSWindowInfo(windowHandle, nsView);
            UpdateNSView();
#elif LINUX
                IntPtr display = gdk_x11_display_get_xdisplay(Display.Handle);
                int screen = Screen.Number;
                IntPtr windowHandle = gdk_x11_drawable_get_xid(GdkWindow.Handle);
                IntPtr rootWindow = gdk_x11_drawable_get_xid(RootWindow.Handle);

                IntPtr visualInfo;
                if (graphicsMode.Index.HasValue)
                {
                    XVisualInfo info = new XVisualInfo();
                    info.VisualID = graphicsMode.Index.Value;
                    int dummy;
                    visualInfo = XGetVisualInfo(display, XVisualInfoMask.ID, ref info, out dummy);
                }
                else
                {
                    visualInfo = GetVisualInfo(display);
                }

                windowInfo = OpenTK.Platform.Utilities.CreateX11WindowInfo(display, screen, windowHandle, rootWindow, visualInfo);
                XFree(visualInfo);        
#endif     

            // GraphicsContext
            graphicsContext = new GraphicsContext(graphicsMode, windowInfo, GlVersionMajor, GlVersionMinor, graphicsContextFlags);
            graphicsContext.MakeCurrent(windowInfo);

            if (GraphicsContext.ShareContexts)
            {
                Interlocked.Increment(ref graphicsContextCount);

                if (!sharedContextInitialized)
                {
                    sharedContextInitialized = true;
                    ((IGraphicsContextInternal)graphicsContext).LoadAll();
                    OnGraphicsContextInitialized();
                }
            }
            else
            {
                ((IGraphicsContextInternal)graphicsContext).LoadAll();
                OnGraphicsContextInitialized();
            }

            this.OnInitialized(this, null);

            this.timerId = GLib.Timeout.Add(16, new GLib.TimeoutHandler(this.Render));

            return false;
        }

        /// <summary>
        /// Render method
        /// </summary>
        /// <returns>Always true</returns>
        public virtual bool Render()
        {
            OnRenderFrame();
            graphicsContext.SwapBuffers();

            return true;
        }

        // Called on Resize
        protected override bool OnConfigureEvent(Gdk.EventConfigure evnt)
        {
            bool result = base.OnConfigureEvent(evnt);

#if MAC
            if (nsView != IntPtr.Zero)
            {
                this.UpdateNSView();
            }
#endif

            if (graphicsContext != null) graphicsContext.Update(windowInfo);

            if (this.gameApp != null && evnt.Width > 1 && evnt.Height > 1)
            {
                this.gameApp.ResizeScreen(evnt.Width, evnt.Height);
            }

            return result;
        }

        /// <summary>
        /// On Destroyed event
        /// </summary>
        protected override void OnDestroyed()
        {
            base.OnDestroyed();

            this.IsDisposed = true;
            GLib.Source.Remove(this.timerId);

            this.gameApp.OnDeactivate();
            this.gameApp.Dispose();
        }

        #region MacUtils

        const string mac_libgdk_name = "libgdk-quartz-2.0.0.dylib";
        const string mac_objc_name = "/usr/lib/libobjc.dylib";

        [SuppressUnmanagedCodeSecurity, DllImport(mac_libgdk_name)]
        static extern IntPtr gdk_quartz_window_get_nswindow(IntPtr gdkDisplay);

        [SuppressUnmanagedCodeSecurity, DllImport(mac_libgdk_name)]
        static extern IntPtr gdk_quartz_window_get_nsview(IntPtr gdkDisplay);

        [SuppressUnmanagedCodeSecurity, DllImport(mac_libgdk_name)]
        static extern bool gdk_window_ensure_native(IntPtr handle);

        [SuppressUnmanagedCodeSecurity, DllImport(mac_objc_name, EntryPoint = "objc_msgSend")]
        extern static IntPtr SendIntPtr(IntPtr receiver, IntPtr selector);

        [SuppressUnmanagedCodeSecurity, DllImport(mac_objc_name, EntryPoint = "objc_msgSend")]
        extern static IntPtr SendIntPtr(IntPtr receiver, IntPtr selector, RectangleF rectangle1);

        [SuppressUnmanagedCodeSecurity, DllImport(mac_objc_name)]
        extern static IntPtr sel_registerName(string name);

        [SuppressUnmanagedCodeSecurity, DllImport(mac_objc_name, EntryPoint = "objc_msgSend")]
        extern static void SendVoid(IntPtr receiver, IntPtr selector, IntPtr intPtr1);

        [SuppressUnmanagedCodeSecurity, DllImport(mac_objc_name, EntryPoint = "objc_msgSend")]
        extern static void SendVoid(IntPtr receiver, IntPtr selector, bool bool1);

        [SuppressUnmanagedCodeSecurity, DllImport(mac_objc_name, EntryPoint = "objc_msgSend")]
        extern static void SendVoid(IntPtr receiver, IntPtr selector, RectangleF rectangle1);

        [Serializable]
        struct RectangleF
        {
            public float X;
            public float Y;
            public float Width;
            public float Height;

            public RectangleF(float x, float y, float width, float height)
            {
                X = x;
                Y = y;
                Width = width;
                Height = height;
            }
        }

        // The NSView is not resized automatically, so do it ourselves when needed.
        private bool UpdateNSView()
        {
            if (nsView == IntPtr.Zero)
                return false;

            bool drawable = IsDrawable;
            var r = Allocation;
            if (lastVisible == drawable && r == lastRectangle)
                return false;

            lastRectangle = r;
            lastVisible = drawable;
            SendVoid(nsView, sel_registerName("setHidden:"), !drawable);
            SendVoid(nsView, sel_registerName("setFrame:"), new RectangleF(r.X, r.Y, r.Width, r.Height));

            if (graphicsContext != null)
            {
                graphicsContext.Update(windowInfo);
            }

            return true;
        }
        #endregion

        #region LinuxUtils

        public enum XVisualClass : int
        {
            StaticGray = 0,
            GrayScale = 1,
            StaticColor = 2,
            PseudoColor = 3,
            TrueColor = 4,
            DirectColor = 5,
        }

        [StructLayout(LayoutKind.Sequential)]
        struct XVisualInfo
        {
            public IntPtr Visual;
            public IntPtr VisualID;
            public int Screen;
            public int Depth;
            public XVisualClass Class;
            public long RedMask;
            public long GreenMask;
            public long blueMask;
            public int ColormapSize;
            public int BitsPerRgb;

            public override string ToString()
            {
                return String.Format("id ({0}), screen ({1}), depth ({2}), class ({3})",
                    VisualID, Screen, Depth, Class);
            }
        }

        [Flags]
        internal enum XVisualInfoMask
        {
            No = 0x0,
            ID = 0x1,
            Screen = 0x2,
            Depth = 0x4,
            Class = 0x8,
            Red = 0x10,
            Green = 0x20,
            Blue = 0x40,
            ColormapSize = 0x80,
            BitsPerRGB = 0x100,
            All = 0x1FF,
        }

        [DllImport("libX11", EntryPoint = "XGetVisualInfo")]
        static extern IntPtr XGetVisualInfoInternal(IntPtr display, IntPtr vinfo_mask, ref XVisualInfo template, out int nitems);
        static IntPtr XGetVisualInfo(IntPtr display, XVisualInfoMask vinfo_mask, ref XVisualInfo template, out int nitems)
        {
            return XGetVisualInfoInternal(display, (IntPtr)(int)vinfo_mask, ref template, out nitems);
        }

        const string linux_libx11_name = "libX11.so.6";

        [SuppressUnmanagedCodeSecurity, DllImport(linux_libx11_name)]
        static extern void XFree(IntPtr handle);

        const string linux_libgdk_x11_name = "libgdk-x11-2.0.so.0";

        /// <summary> Returns the X resource (window or pixmap) belonging to a GdkDrawable. </summary>
        /// <remarks> XID gdk_x11_drawable_get_xid(GdkDrawable *drawable); </remarks>
        /// <param name="gdkDisplay"> The GdkDrawable. </param>
        /// <returns> The ID of drawable's X resource. </returns>
        [SuppressUnmanagedCodeSecurity, DllImport(linux_libgdk_x11_name)]
        static extern IntPtr gdk_x11_drawable_get_xid(IntPtr gdkDisplay);

        /// <summary> Returns the X display of a GdkDisplay. </summary>
        /// <remarks> Display* gdk_x11_display_get_xdisplay(GdkDisplay *display); </remarks>
        /// <param name="gdkDisplay"> The GdkDrawable. </param>
        /// <returns> The X Display of the GdkDisplay. </returns>
        [SuppressUnmanagedCodeSecurity, DllImport(linux_libgdk_x11_name)]
        static extern IntPtr gdk_x11_display_get_xdisplay(IntPtr gdkDisplay);

        IntPtr GetVisualInfo(IntPtr display)
        {
            try
            {
                int[] attributes = AttributeList.ToArray();
                return glXChooseVisual(display, Screen.Number, attributes);
            }
            catch (DllNotFoundException e)
            {
                throw new DllNotFoundException("OpenGL dll not found!", e);
            }
            catch (EntryPointNotFoundException enf)
            {
                throw new EntryPointNotFoundException("Glx entry point not found!", enf);
            }
        }

        const int GLX_NONE = 0;
        const int GLX_USE_GL = 1;
        const int GLX_BUFFER_SIZE = 2;
        const int GLX_LEVEL = 3;
        const int GLX_RGBA = 4;
        const int GLX_DOUBLEBUFFER = 5;
        const int GLX_STEREO = 6;
        const int GLX_AUX_BUFFERS = 7;
        const int GLX_RED_SIZE = 8;
        const int GLX_GREEN_SIZE = 9;
        const int GLX_BLUE_SIZE = 10;
        const int GLX_ALPHA_SIZE = 11;
        const int GLX_DEPTH_SIZE = 12;
        const int GLX_STENCIL_SIZE = 13;
        const int GLX_ACCUM_RED_SIZE = 14;
        const int GLX_ACCUM_GREEN_SIZE = 15;
        const int GLX_ACCUM_BLUE_SIZE = 16;
        const int GLX_ACCUM_ALPHA_SIZE = 17;

        List<int> AttributeList
        {
            get
            {
                List<int> attributeList = new List<int>(24);

                attributeList.Add(GLX_RGBA);

                if (!SingleBuffer) attributeList.Add(GLX_DOUBLEBUFFER);

                if (Stereo) attributeList.Add(GLX_STEREO);

                attributeList.Add(GLX_RED_SIZE);
                attributeList.Add(ColorBPP / 4); // TODO support 16-bit

                attributeList.Add(GLX_GREEN_SIZE);
                attributeList.Add(ColorBPP / 4); // TODO support 16-bit

                attributeList.Add(GLX_BLUE_SIZE);
                attributeList.Add(ColorBPP / 4); // TODO support 16-bit

                attributeList.Add(GLX_ALPHA_SIZE);
                attributeList.Add(ColorBPP / 4); // TODO support 16-bit

                attributeList.Add(GLX_DEPTH_SIZE);
                attributeList.Add(DepthBPP);

                attributeList.Add(GLX_STENCIL_SIZE);
                attributeList.Add(StencilBPP);

                //attributeList.Add(GLX_AUX_BUFFERS);
                //attributeList.Add(Buffers);

                attributeList.Add(GLX_ACCUM_RED_SIZE);
                attributeList.Add(AccumulatorBPP / 4);// TODO support 16-bit

                attributeList.Add(GLX_ACCUM_GREEN_SIZE);
                attributeList.Add(AccumulatorBPP / 4);// TODO support 16-bit

                attributeList.Add(GLX_ACCUM_BLUE_SIZE);
                attributeList.Add(AccumulatorBPP / 4);// TODO support 16-bit

                attributeList.Add(GLX_ACCUM_ALPHA_SIZE);
                attributeList.Add(AccumulatorBPP / 4);// TODO support 16-bit

                attributeList.Add(GLX_NONE);

                return attributeList;
            }
        }

        const string linux_libgl_name = "libGL.so.1";

        [SuppressUnmanagedCodeSecurity, DllImport(linux_libgl_name)]
        static extern IntPtr glXChooseVisual(IntPtr display, int screen, int[] attr);

        #endregion
    }
}