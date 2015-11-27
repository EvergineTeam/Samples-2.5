using CoreAnimation;
using Foundation;
using ObjCRuntime;
using OpenGLES;
using UIKit;
using OpenTK;
using OpenTK.Graphics.ES20;
using OpenTK.Platform.iPhoneOS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using WaveEngine.Common;
using WaveEngine.Common.Input;
using WaveEngine.OpenGL;

namespace Sample.Adapter
{
    /// <summary>
    /// OpenGL view used for drawing.
    /// </summary>
    [Register("GameView")]
    public class GameView : iPhoneOSGameView
    {
        /// <summary>
        /// The parent
        /// </summary>
        private GameApplication parent;

        public GameApplication Application
        {
            get
            {
                return this.parent;
            }

            set
            {
                this.parent = value;
            }
        }

        /// <summary>
        /// The adapter
        /// </summary>
        private IAdapter adapter;

        /// <summary>
        /// The depth framebuffer
        /// </summary>
        private int depthFramebuffer;

        /// <summary>
        /// The screen width
        /// </summary>
        private int screenWidth;

        /// <summary>
        /// The screen height
        /// </summary>
        private int screenHeight;

        /// <summary>
        /// The is paused
        /// </summary>
        private bool isPaused;

        /// <summary>
        /// CADisplayLink used to create draw events
        /// </summary>
        private CADisplayLink displayLink;

        /// <summary>
        /// StopWatch used to measure the game time
        /// </summary>
        private Stopwatch stopwatch;

        /// <summary>
        /// The framebuffer call counter
        /// </summary>
        private int counter = 0;

        /// <summary>
        /// Gets the adapter.
        /// </summary>
        /// <value>
        /// The adapter.
        /// </value>
        public IAdapter Adapter
        {
            get { return this.adapter; }
        }

        [Export("initWithCoder:")]
        public GameView(NSCoder coder)
            : base(coder)
        {
            this.LayerRetainsBacking = true;
            this.LayerColorFormat = EAGLColorFormat.RGBA8;
            this.MultipleTouchEnabled = true;
            this.ContentScaleFactor = UIScreen.MainScreen.Scale;
            this.ContextRenderingApi = EAGLRenderingAPI.OpenGLES2;

            this.stopwatch = new Stopwatch();

            UIApplication.SharedApplication.IdleTimerDisabled = true;

            this.depthFramebuffer = 0;
            this.isPaused = true;
        }

        /// <summary>
        /// Gets the layer class.
        /// </summary>
        /// <returns>Layer class.</returns>
        [Export("layerClass")]
        public static new Class GetLayerClass()
        {
            return iPhoneOSGameView.GetLayerClass();
        }

        /// <summary>
        /// Configures the layer.
        /// </summary>
        /// <param name="eaglLayer">The eagl layer.</param>
        protected override void ConfigureLayer(CAEAGLLayer eaglLayer)
        {
            eaglLayer.Opaque = true;
        }

        /// <summary>
        /// Creates the frame buffer.
        /// </summary>
        protected override void CreateFrameBuffer()
        {
            if (this.counter > 1)
            {
                return;
            }

            this.counter++;

            if (this.isPaused)
            {
                return;
            }

            if (!UIDevice.CurrentDevice.CheckSystemVersion(8, 0)
                && (UIApplication.SharedApplication.StatusBarOrientation == UIInterfaceOrientation.LandscapeLeft || UIApplication.SharedApplication.StatusBarOrientation == UIInterfaceOrientation.LandscapeRight))
            {
                this.Bounds = new RectangleF(0, 0, (float)this.Bounds.Height, (float)this.Bounds.Width);
            }

            this.screenWidth = (int)(this.Bounds.Width * this.ContentScaleFactor);
            this.screenHeight = (int)(this.Bounds.Height * this.ContentScaleFactor);

            base.CreateFrameBuffer();

            if (this.depthFramebuffer > 0)
            {
                GL.DeleteFramebuffers(1, ref this.depthFramebuffer);
                this.depthFramebuffer = 0;
            }

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, this.Framebuffer);

            GL.GenRenderbuffers(1, out this.depthFramebuffer);
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, this.depthFramebuffer);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferInternalFormat.DepthComponent16, this.screenWidth, this.screenHeight);
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferSlot.DepthAttachment, RenderbufferTarget.Renderbuffer, this.depthFramebuffer);

            GLHelpers.InitExtensionList();
        }

        /// <summary>
        /// Destroys the frame buffer.
        /// </summary>
        protected override void DestroyFrameBuffer()
        {
            if (this.isPaused)
            {
                return;
            }

            base.DestroyFrameBuffer();
        }

        /// <summary>
        /// Activates this instance.
        /// </summary>
        public void Activate()
        {
            if (!this.isPaused)
            {
                return;
            }

            // Restart stopwatch
            this.stopwatch.Reset();
            this.stopwatch.Start();

            this.CreateDisplayLink();

            this.isPaused = false;
            if (this.adapter != null)
            {
                ((WaveEngine.Adapter.Input.InputManager)this.adapter.InputManager).CurrentState.Clear();
            }
        }

        /// <summary>
        /// Deactivates this instance.
        /// </summary>
        public void Deactivate()
        {
            if (this.isPaused)
            {
                return;
            }

            this.DestroyDisplayLink();

            // Stop the stopwatch
            this.stopwatch.Stop();
            this.isPaused = true;
            this.Stop();
        }

        /// <summary>
        /// Raises the <see cref="E:UpdateFrame" /> event.
        /// </summary>
        /// <param name="e">The <see cref="FrameEventArgs"/> instance containing the event data.</param>
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            if (this.isPaused)
            {
                return;
            }

            if (this.adapter == null)
            {
                this.CreateFrameBuffer();
                this.MakeCurrent();

                this.InitAdapter();
                this.parent.Initialize();
            }

            base.OnUpdateFrame(e);

            this.parent.Update(TimeSpan.FromSeconds(e.Time));
        }

        /// <summary>
        /// Raises the <see cref="E:RenderFrame" /> event.
        /// </summary>
        /// <param name="e">The <see cref="FrameEventArgs"/> instance containing the event data.</param>
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            if (this.isPaused)
            {
                return;
            }

            base.OnRenderFrame(e);

            if (this.adapter == null)
            {
                this.InitAdapter();
            }

            this.MakeCurrent();
            this.parent.Draw(TimeSpan.FromSeconds(e.Time));

            this.SwapBuffers();
        }

        /// <summary>
        /// Inits the adapter.
        /// </summary>
        private void InitAdapter()
        {
            this.adapter = new WaveEngine.Adapter.Adapter(this.screenWidth, this.screenHeight, this);
            this.adapter.DefaultOrientation = DisplayOrientation.LandscapeLeft;
            this.adapter.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
        }

        /// <summary>
        /// Toucheses the began.
        /// </summary>
        /// <param name="touches">The touches.</param>
        /// <param name="evt">The evt.</param>
        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);

            this.FillTouches(touches);
        }

        /// <summary>
        /// Toucheses the moved.
        /// </summary>
        /// <param name="touches">The touches.</param>
        /// <param name="evt">The evt.</param>
        public override void TouchesMoved(NSSet touches, UIEvent evt)
        {
            base.TouchesMoved(touches, evt);

            this.FillTouches(touches);
        }

        /// <summary>
        /// Toucheses the cancelled.
        /// </summary>
        /// <param name="touches">The touches.</param>
        /// <param name="evt">The evt.</param>
        public override void TouchesCancelled(NSSet touches, UIEvent evt)
        {
            base.TouchesCancelled(touches, evt);

            this.FillTouches(touches);
        }

        /// <summary>
        /// Toucheses the ended.
        /// </summary>
        /// <param name="touches">The touches.</param>
        /// <param name="evt">The evt.</param>
        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);

            this.FillTouches(touches);
        }

        /// <summary>
        /// Initializes CADisplayLink
        /// </summary>
        private void CreateDisplayLink()
        {
            CADisplayLink displayLink = UIScreen.MainScreen.CreateDisplayLink(this, new Selector("drawFrame"));
            displayLink.FrameInterval = 0;
            displayLink.AddToRunLoop(NSRunLoop.Current, NSRunLoop.NSDefaultRunLoopMode);
            this.displayLink = displayLink;
        }

        private void DestroyDisplayLink()
        {
            this.displayLink.Invalidate();
            this.displayLink = null;
        }

        [Export("drawFrame")]
        private void DrawFrame()
        {
            // Reset and calculate the next time
            this.stopwatch.Stop();
            var time = this.stopwatch.Elapsed;
            this.stopwatch.Reset();
            this.stopwatch.Start();
            var e = new FrameEventArgs(time.TotalSeconds);
            this.OnUpdateFrame(e);
            this.OnRenderFrame(e);
        }

        /// <summary>
        /// Fills the touches.
        /// </summary>
        /// <param name="touches">The touches.</param>
        private void FillTouches(NSSet touches)
        {
            UITouch[] touchArray = touches.ToArray<UITouch>();
            WaveEngine.Adapter.Input.InputManager inputManager = (WaveEngine.Adapter.Input.InputManager)this.adapter.InputManager;

            List<int> movedIds = new List<int>();

            TouchPanelState oldState = inputManager.CurrentState;
            inputManager.CurrentState.Clear();

            for (int i = 0; i < touchArray.Length; i++)
            {
                UITouch touch = touchArray[i];

                // Position is always the same since we rotate the view
                var position = touch.LocationInView(touch.View);
                position.X *= this.ContentScaleFactor;
                position.Y *= this.ContentScaleFactor;

                int touchId = touch.Handle.ToInt32();

                TouchLocationState state;

                // Touch Type
                switch (touch.Phase)
                {
                    case UITouchPhase.Began:
                        state = TouchLocationState.Pressed;
                        break;
                    case UITouchPhase.Ended:
                    case UITouchPhase.Cancelled:
                        state = TouchLocationState.Release;
                        movedIds.Add(touchId);
                        break;
                    case UITouchPhase.Stationary:
                    case UITouchPhase.Moved:
                        state = TouchLocationState.Moved;
                        movedIds.Add(touchId);
                        break;
                    default:
                        state = TouchLocationState.Invalid;
                        break;
                }

                if (state != TouchLocationState.Release)
                {
                    inputManager.CurrentState.AddTouchLocation(touchId, state, (float)position.X, (float)position.Y);
                }
            }

            foreach (TouchLocation location in oldState)
            {
                if ((location.State == TouchLocationState.Moved || location.State == TouchLocationState.Pressed) && !movedIds.Contains(location.Id))
                {
                    inputManager.CurrentState.AddTouchLocation(location.Id, location.State, location.Position.X, location.Position.Y);
                }
            }
        }
    }
}
