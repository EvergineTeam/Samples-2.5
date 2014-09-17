using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;
using WaveEngine.Common.Input;
using WaveEngine.Framework.Services;
using NetTask = System.Threading.Tasks.Task;

namespace WaveWPF
{
    /// <summary>
    /// Class that represents a canvas that contains the wave application.
    /// </summary>
    public class WaveCanvas : FrameworkElement
    {
        /// <summary>
        #region CompositionTargetEx

        /// <summary>
        /// Extended class for composition target
        /// </summary>
        public static class CompositionTargetEx
        {
            /// <summary>
            /// The last timespan
            /// </summary>
            private static TimeSpan last = TimeSpan.Zero;

            /// <summary>
            /// Occurs when [unsafe frame updating].
            /// </summary>
            private static event EventHandler<RenderingEventArgs> UnsafeFrameUpdating;

            /// <summary>
            /// Occurs when [frame updating].
            /// </summary>
            public static event EventHandler<RenderingEventArgs> Rendering
            {
                add
                {
                    if (UnsafeFrameUpdating == null)
                    {
                        CompositionTarget.Rendering += OnRendering;
                    }

                    UnsafeFrameUpdating += value;
                }

                remove
                {
                    UnsafeFrameUpdating -= value;

                    if (UnsafeFrameUpdating == null)
                    {
                        CompositionTarget.Rendering -= OnRendering;
                    }
                }
            }

            /// <summary>
            /// Called when [rendering].
            /// </summary>
            /// <param name="sender">The sender.</param>
            /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
            private static void OnRendering(object sender, EventArgs e)
            {
                RenderingEventArgs args = (RenderingEventArgs)e;
                if (args.RenderingTime == last)
                {
                    return;
                }

                last = args.RenderingTime;
                UnsafeFrameUpdating(sender, args);
            }
        }

        #endregion

        /// <summary>
        /// Gets a value indicating whether the control is in design mode
        /// (running in Blend or Visual Studio).
        /// </summary>
        public static readonly bool IsInDesignMode = (bool)DependencyPropertyDescriptor.FromProperty(DesignerProperties.IsInDesignModeProperty, typeof(FrameworkElement)).Metadata.DefaultValue;

        /// <summary>
        /// Indicates the time delay before the resize event will be propagated to the bound Game class which will cause its backbuffer resize.
        /// Default is 1 second.
        /// </summary>
        public static readonly DependencyProperty SendResizeDelayProperty = DependencyProperty
            .Register("SendResizeDelay", typeof(TimeSpan), typeof(WaveCanvas), new FrameworkPropertyMetadata(TimeSpan.FromSeconds(0.1f), HandleResizeDelayChanged));

        /// <summary>
        /// Indicates whether the wave canvas will render automatically.
        /// </summary>
        public static readonly DependencyProperty AutoRenderProperty = DependencyProperty
            .Register("AutoRender", typeof(bool), typeof(WaveCanvas), new PropertyMetadata(true));

        /// <summary>
        /// Indicates whether the rendering should be done in the <see cref="System.Windows.Threading.DispatcherPriority.Input"/> priority.
        /// This may cause loss of FPS, but will not interfere with the input processing.
        /// Default is false.
        /// </summary>
        public static readonly DependencyProperty LowPriorityRenderingProperty = DependencyProperty
            .Register("LowPriorityRendering", typeof(bool), typeof(WaveCanvas), new PropertyMetadata(default(bool)));

        /// <summary>
        /// Occurs when the wave game is loaded.
        /// </summary>
        public event EventHandler<CubeTestProject.Game> GameLoaded;

        /// <summary>
        /// The resize delay timer
        /// </summary>
        private DispatcherTimer resizeDelayTimer;

        /// <summary>
        /// If the canvas is dirty
        /// </summary>
        private bool isDirty;

        /// <summary>
        /// Render delegate
        /// </summary>
        private readonly Action renderDelegate;

        /// <summary>
        /// The previous render call
        /// </summary>
        private System.Windows.Threading.DispatcherOperation previousRenderCall;

        /// <summary>
        /// The application
        /// </summary>
        private MainApp app;

        /// <summary>
        /// The source
        /// </summary>
        private D3DImage image;

        /// <summary>
        /// Gets or sets the the value of the <see cref="SendResizeDelay"/> dependency property.
        /// </summary>
        public TimeSpan SendResizeDelay
        {
            get { return (TimeSpan)this.GetValue(SendResizeDelayProperty); }
            set { this.SetValue(SendResizeDelayProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the rendering is set in low priority.
        /// </summary>
        /// <value>
        /// <c>true</c> if [low priority rendering]; otherwise, <c>false</c>.
        /// </value>
        public bool LowPriorityRendering
        {
            get { return (bool)this.GetValue(LowPriorityRenderingProperty); }
            set { this.SetValue(LowPriorityRenderingProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [automatic render].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [automatic render]; otherwise, <c>false</c>.
        /// </value>
        public bool AutoRender
        {
            get { return (bool)this.GetValue(AutoRenderProperty); }
            set { this.SetValue(AutoRenderProperty, value); }
        }

        /// <summary>
        /// Gets the wave application.
        /// </summary>
        /// <value>
        /// The application.
        /// </value>
        public MainApp App
        {
            get
            {
                return this.app;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [is dirty].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is dirty]; otherwise, <c>false</c>.
        /// </value>
        public bool IsDirty
        {
            get
            {
                return this.isDirty;
            }

            set
            {
                this.isDirty = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WaveCanvas"/> class.
        /// </summary>
        public WaveCanvas()
        {
            this.SnapsToDevicePixels = true;

            this.resizeDelayTimer = new DispatcherTimer(DispatcherPriority.Normal);
            this.resizeDelayTimer.Tick += this.ResizeGame;
            this.resizeDelayTimer.Interval = this.SendResizeDelay;

            this.renderDelegate = this.Render;

            this.IsDirty = true;

            if (!IsInDesignMode)
            {
                this.Loaded += this.OnLoaded;

                EventManager.RegisterClassHandler(typeof(Window), Keyboard.KeyDownEvent, new KeyEventHandler(this.OnKeyDown), true);
                EventManager.RegisterClassHandler(typeof(Window), Keyboard.KeyUpEvent, new KeyEventHandler(this.OnKeyUp), true);
            }
        }

        /// <summary>
        /// Unloads this instance.
        /// </summary>
        private void Unload()
        {
            if (this.counters > 0)
            {
                CompositionTargetEx.Rendering -= this.OnRendering;

                WaveServices.Input.MouseState.X = 0;
                WaveServices.Input.MouseState.Y = 0;
                WaveServices.Input.MouseState.LeftButton = ButtonState.Release;
                WaveServices.Input.MouseState.MiddleButton = ButtonState.Release;
                WaveServices.Input.MouseState.RightButton = ButtonState.Release;

                this.counters--;
            }
        }

        /// <summary>
        /// Handles the resize delay changed.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void HandleResizeDelayChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var element = dependencyObject as WaveCanvas;

            if (element == null)
            {
                return;
            }

            if (e.NewValue != DependencyProperty.UnsetValue)
            {
                element.resizeDelayTimer.Interval = (TimeSpan)e.NewValue;
            }
        }

        #region Key up switch

        /// <summary>
        /// Called when [key up].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (WaveServices.Input == null)
            {
                return;
            }

            switch (e.Key)
            {
                case Key.A:
                    WaveServices.Input.KeyboardState.A = ButtonState.Release;
                    break;
                case Key.B:
                    WaveServices.Input.KeyboardState.B = ButtonState.Release;
                    break;
                case Key.Back:
                    WaveServices.Input.KeyboardState.Back = ButtonState.Release;
                    break;
                case Key.C:
                    WaveServices.Input.KeyboardState.C = ButtonState.Release;
                    break;
                case Key.CapsLock:
                    WaveServices.Input.KeyboardState.CapsLock = ButtonState.Release;
                    break;
                case Key.CrSel:
                    WaveServices.Input.KeyboardState.Crsel = ButtonState.Release;
                    break;
                case Key.D:
                    WaveServices.Input.KeyboardState.D = ButtonState.Release;
                    break;
                case Key.D0:
                    WaveServices.Input.KeyboardState.D0 = ButtonState.Release;
                    break;
                case Key.D1:
                    WaveServices.Input.KeyboardState.D1 = ButtonState.Release;
                    break;
                case Key.D2:
                    WaveServices.Input.KeyboardState.D2 = ButtonState.Release;
                    break;
                case Key.D3:
                    WaveServices.Input.KeyboardState.D3 = ButtonState.Release;
                    break;
                case Key.D4:
                    WaveServices.Input.KeyboardState.D4 = ButtonState.Release;
                    break;
                case Key.D5:
                    WaveServices.Input.KeyboardState.D5 = ButtonState.Release;
                    break;
                case Key.D6:
                    WaveServices.Input.KeyboardState.D6 = ButtonState.Release;
                    break;
                case Key.D7:
                    WaveServices.Input.KeyboardState.D7 = ButtonState.Release;
                    break;
                case Key.D8:
                    WaveServices.Input.KeyboardState.D8 = ButtonState.Release;
                    break;
                case Key.D9:
                    WaveServices.Input.KeyboardState.D9 = ButtonState.Release;
                    break;
                case Key.Delete:
                    WaveServices.Input.KeyboardState.Delete = ButtonState.Release;
                    break;
                case Key.Down:
                    WaveServices.Input.KeyboardState.Down = ButtonState.Release;
                    break;
                case Key.E:
                    WaveServices.Input.KeyboardState.E = ButtonState.Release;
                    break;
                case Key.Enter:
                    WaveServices.Input.KeyboardState.Enter = ButtonState.Release;
                    break;
                case Key.Escape:
                    WaveServices.Input.KeyboardState.Escape = ButtonState.Release;
                    break;
                case Key.Execute:
                    WaveServices.Input.KeyboardState.Execute = ButtonState.Release;
                    break;
                case Key.F:
                    WaveServices.Input.KeyboardState.F = ButtonState.Release;
                    break;
                case Key.F1:
                    WaveServices.Input.KeyboardState.F = ButtonState.Release;
                    break;
                case Key.F10:
                    WaveServices.Input.KeyboardState.F10 = ButtonState.Release;
                    break;
                case Key.F11:
                    WaveServices.Input.KeyboardState.F11 = ButtonState.Release;
                    break;
                case Key.F12:
                    WaveServices.Input.KeyboardState.F12 = ButtonState.Release;
                    break;
                case Key.F2:
                    WaveServices.Input.KeyboardState.F2 = ButtonState.Release;
                    break;
                case Key.F3:
                    WaveServices.Input.KeyboardState.F3 = ButtonState.Release;
                    break;
                case Key.F4:
                    WaveServices.Input.KeyboardState.F4 = ButtonState.Release;
                    break;
                case Key.F5:
                    WaveServices.Input.KeyboardState.F5 = ButtonState.Release;
                    break;
                case Key.F6:
                    WaveServices.Input.KeyboardState.F6 = ButtonState.Release;
                    break;
                case Key.F7:
                    WaveServices.Input.KeyboardState.F7 = ButtonState.Release;
                    break;
                case Key.F8:
                    WaveServices.Input.KeyboardState.F8 = ButtonState.Release;
                    break;
                case Key.F9:
                    WaveServices.Input.KeyboardState.F9 = ButtonState.Release;
                    break;
                case Key.G:
                    WaveServices.Input.KeyboardState.G = ButtonState.Release;
                    break;
                case Key.H:
                    WaveServices.Input.KeyboardState.H = ButtonState.Release;
                    break;
                case Key.I:
                    WaveServices.Input.KeyboardState.I = ButtonState.Release;
                    break;
                case Key.J:
                    WaveServices.Input.KeyboardState.J = ButtonState.Release;
                    break;
                case Key.K:
                    WaveServices.Input.KeyboardState.K = ButtonState.Release;
                    break;
                case Key.L:
                    WaveServices.Input.KeyboardState.L = ButtonState.Release;
                    break;
                case Key.Left:
                    WaveServices.Input.KeyboardState.Left = ButtonState.Release;
                    break;
                case Key.LeftAlt:
                    WaveServices.Input.KeyboardState.LeftAlt = ButtonState.Release;
                    break;
                case Key.LeftCtrl:
                    WaveServices.Input.KeyboardState.LeftControl = ButtonState.Release;
                    break;
                case Key.LeftShift:
                    WaveServices.Input.KeyboardState.LeftShift = ButtonState.Release;
                    break;
                case Key.LWin:
                    WaveServices.Input.KeyboardState.LeftWindows = ButtonState.Release;
                    break;
                case Key.M:
                    WaveServices.Input.KeyboardState.M = ButtonState.Release;
                    break;
                case Key.N:
                    WaveServices.Input.KeyboardState.N = ButtonState.Release;
                    break;
                case Key.O:
                    WaveServices.Input.KeyboardState.O = ButtonState.Release;
                    break;
                case Key.P:
                    WaveServices.Input.KeyboardState.P = ButtonState.Release;
                    break;
                case Key.Q:
                    WaveServices.Input.KeyboardState.Q = ButtonState.Release;
                    break;
                case Key.R:
                    WaveServices.Input.KeyboardState.R = ButtonState.Release;
                    break;
                case Key.Right:
                    WaveServices.Input.KeyboardState.Right = ButtonState.Release;
                    break;
                case Key.RightAlt:
                    WaveServices.Input.KeyboardState.RightAlt = ButtonState.Release;
                    break;
                case Key.RightCtrl:
                    WaveServices.Input.KeyboardState.RightControl = ButtonState.Release;
                    break;
                case Key.RightShift:
                    WaveServices.Input.KeyboardState.RightShift = ButtonState.Release;
                    break;
                case Key.RWin:
                    WaveServices.Input.KeyboardState.RightWindows = ButtonState.Release;
                    break;
                case Key.S:
                    WaveServices.Input.KeyboardState.S = ButtonState.Release;
                    break;
                case Key.Space:
                    WaveServices.Input.KeyboardState.Space = ButtonState.Release;
                    break;
                case Key.Subtract:
                    WaveServices.Input.KeyboardState.Subtract = ButtonState.Release;
                    break;
                case Key.T:
                    WaveServices.Input.KeyboardState.T = ButtonState.Release;
                    break;
                case Key.Tab:
                    WaveServices.Input.KeyboardState.Tab = ButtonState.Release;
                    break;
                case Key.U:
                    WaveServices.Input.KeyboardState.U = ButtonState.Release;
                    break;
                case Key.Up:
                    WaveServices.Input.KeyboardState.Up = ButtonState.Release;
                    break;
                case Key.V:
                    WaveServices.Input.KeyboardState.V = ButtonState.Release;
                    break;
                case Key.W:
                    WaveServices.Input.KeyboardState.W = ButtonState.Release;
                    break;
                case Key.X:
                    WaveServices.Input.KeyboardState.X = ButtonState.Release;
                    break;
                case Key.Y:
                    WaveServices.Input.KeyboardState.Y = ButtonState.Release;
                    break;
                case Key.Z:
                    WaveServices.Input.KeyboardState.Z = ButtonState.Release;
                    break;
            }
        }

        #endregion

        #region Key down switch

        /// <summary>
        /// Called when [key down].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (WaveServices.Input == null)
            {
                return;
            }

            switch (e.Key)
            {
                case Key.A:
                    WaveServices.Input.KeyboardState.A = ButtonState.Pressed;
                    break;
                case Key.B:
                    WaveServices.Input.KeyboardState.B = ButtonState.Pressed;
                    break;
                case Key.Back:
                    WaveServices.Input.KeyboardState.Back = ButtonState.Pressed;
                    break;
                case Key.C:
                    WaveServices.Input.KeyboardState.C = ButtonState.Pressed;
                    break;
                case Key.CapsLock:
                    WaveServices.Input.KeyboardState.CapsLock = ButtonState.Pressed;
                    break;
                case Key.CrSel:
                    WaveServices.Input.KeyboardState.Crsel = ButtonState.Pressed;
                    break;
                case Key.D:
                    WaveServices.Input.KeyboardState.D = ButtonState.Pressed;
                    break;
                case Key.D0:
                    WaveServices.Input.KeyboardState.D0 = ButtonState.Pressed;
                    break;
                case Key.D1:
                    WaveServices.Input.KeyboardState.D1 = ButtonState.Pressed;
                    break;
                case Key.D2:
                    WaveServices.Input.KeyboardState.D2 = ButtonState.Pressed;
                    break;
                case Key.D3:
                    WaveServices.Input.KeyboardState.D3 = ButtonState.Pressed;
                    break;
                case Key.D4:
                    WaveServices.Input.KeyboardState.D4 = ButtonState.Pressed;
                    break;
                case Key.D5:
                    WaveServices.Input.KeyboardState.D5 = ButtonState.Pressed;
                    break;
                case Key.D6:
                    WaveServices.Input.KeyboardState.D6 = ButtonState.Pressed;
                    break;
                case Key.D7:
                    WaveServices.Input.KeyboardState.D7 = ButtonState.Pressed;
                    break;
                case Key.D8:
                    WaveServices.Input.KeyboardState.D8 = ButtonState.Pressed;
                    break;
                case Key.D9:
                    WaveServices.Input.KeyboardState.D9 = ButtonState.Pressed;
                    break;
                case Key.Delete:
                    WaveServices.Input.KeyboardState.Delete = ButtonState.Pressed;
                    break;
                case Key.Down:
                    WaveServices.Input.KeyboardState.Down = ButtonState.Pressed;
                    break;
                case Key.E:
                    WaveServices.Input.KeyboardState.E = ButtonState.Pressed;
                    break;
                case Key.Enter:
                    WaveServices.Input.KeyboardState.Enter = ButtonState.Pressed;
                    break;
                case Key.Escape:
                    WaveServices.Input.KeyboardState.Escape = ButtonState.Pressed;
                    break;
                case Key.Execute:
                    WaveServices.Input.KeyboardState.Execute = ButtonState.Pressed;
                    break;
                case Key.F:
                    WaveServices.Input.KeyboardState.F = ButtonState.Pressed;
                    break;
                case Key.F1:
                    WaveServices.Input.KeyboardState.F = ButtonState.Pressed;
                    break;
                case Key.F10:
                    WaveServices.Input.KeyboardState.F10 = ButtonState.Pressed;
                    break;
                case Key.F11:
                    WaveServices.Input.KeyboardState.F11 = ButtonState.Pressed;
                    break;
                case Key.F12:
                    WaveServices.Input.KeyboardState.F12 = ButtonState.Pressed;
                    break;
                case Key.F2:
                    WaveServices.Input.KeyboardState.F2 = ButtonState.Pressed;
                    break;
                case Key.F3:
                    WaveServices.Input.KeyboardState.F3 = ButtonState.Pressed;
                    break;
                case Key.F4:
                    WaveServices.Input.KeyboardState.F4 = ButtonState.Pressed;
                    break;
                case Key.F5:
                    WaveServices.Input.KeyboardState.F5 = ButtonState.Pressed;
                    break;
                case Key.F6:
                    WaveServices.Input.KeyboardState.F6 = ButtonState.Pressed;
                    break;
                case Key.F7:
                    WaveServices.Input.KeyboardState.F7 = ButtonState.Pressed;
                    break;
                case Key.F8:
                    WaveServices.Input.KeyboardState.F8 = ButtonState.Pressed;
                    break;
                case Key.F9:
                    WaveServices.Input.KeyboardState.F9 = ButtonState.Pressed;
                    break;
                case Key.G:
                    WaveServices.Input.KeyboardState.G = ButtonState.Pressed;
                    break;
                case Key.H:
                    WaveServices.Input.KeyboardState.H = ButtonState.Pressed;
                    break;
                case Key.I:
                    WaveServices.Input.KeyboardState.I = ButtonState.Pressed;
                    break;
                case Key.J:
                    WaveServices.Input.KeyboardState.J = ButtonState.Pressed;
                    break;
                case Key.K:
                    WaveServices.Input.KeyboardState.K = ButtonState.Pressed;
                    break;
                case Key.L:
                    WaveServices.Input.KeyboardState.L = ButtonState.Pressed;
                    break;
                case Key.Left:
                    WaveServices.Input.KeyboardState.Left = ButtonState.Pressed;
                    break;
                case Key.LeftAlt:
                    WaveServices.Input.KeyboardState.LeftAlt = ButtonState.Pressed;
                    break;
                case Key.LeftCtrl:
                    WaveServices.Input.KeyboardState.LeftControl = ButtonState.Pressed;
                    break;
                case Key.LeftShift:
                    WaveServices.Input.KeyboardState.LeftShift = ButtonState.Pressed;
                    break;
                case Key.LWin:
                    WaveServices.Input.KeyboardState.LeftWindows = ButtonState.Pressed;
                    break;
                case Key.M:
                    WaveServices.Input.KeyboardState.M = ButtonState.Pressed;
                    break;
                case Key.N:
                    WaveServices.Input.KeyboardState.N = ButtonState.Pressed;
                    break;
                case Key.O:
                    WaveServices.Input.KeyboardState.O = ButtonState.Pressed;
                    break;
                case Key.P:
                    WaveServices.Input.KeyboardState.P = ButtonState.Pressed;
                    break;
                case Key.Q:
                    WaveServices.Input.KeyboardState.Q = ButtonState.Pressed;
                    break;
                case Key.R:
                    WaveServices.Input.KeyboardState.R = ButtonState.Pressed;
                    break;
                case Key.Right:
                    WaveServices.Input.KeyboardState.Right = ButtonState.Pressed;
                    break;
                case Key.RightAlt:
                    WaveServices.Input.KeyboardState.RightAlt = ButtonState.Pressed;
                    break;
                case Key.RightCtrl:
                    WaveServices.Input.KeyboardState.RightControl = ButtonState.Pressed;
                    break;
                case Key.RightShift:
                    WaveServices.Input.KeyboardState.RightShift = ButtonState.Pressed;
                    break;
                case Key.RWin:
                    WaveServices.Input.KeyboardState.RightWindows = ButtonState.Pressed;
                    break;
                case Key.S:
                    WaveServices.Input.KeyboardState.S = ButtonState.Pressed;
                    break;
                case Key.Space:
                    WaveServices.Input.KeyboardState.Space = ButtonState.Pressed;
                    break;
                case Key.Subtract:
                    WaveServices.Input.KeyboardState.Subtract = ButtonState.Pressed;
                    break;
                case Key.T:
                    WaveServices.Input.KeyboardState.T = ButtonState.Pressed;
                    break;
                case Key.Tab:
                    WaveServices.Input.KeyboardState.Tab = ButtonState.Pressed;
                    break;
                case Key.U:
                    WaveServices.Input.KeyboardState.U = ButtonState.Pressed;
                    break;
                case Key.Up:
                    WaveServices.Input.KeyboardState.Up = ButtonState.Pressed;
                    break;
                case Key.V:
                    WaveServices.Input.KeyboardState.V = ButtonState.Pressed;
                    break;
                case Key.W:
                    WaveServices.Input.KeyboardState.W = ButtonState.Pressed;
                    break;
                case Key.X:
                    WaveServices.Input.KeyboardState.X = ButtonState.Pressed;
                    break;
                case Key.Y:
                    WaveServices.Input.KeyboardState.Y = ButtonState.Pressed;
                    break;
                case Key.Z:
                    WaveServices.Input.KeyboardState.Z = ButtonState.Pressed;
                    break;
            }
        }

        #endregion

        /// <summary>
        /// The counters
        /// </summary>
        private int counters;

        /// <summary>
        /// Called when [loaded].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void OnLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.app == null)
            {
                this.app = new MainApp((int)this.ActualWidth, (int)this.ActualHeight);

                this.image = this.app.Configure() as D3DImage;

                this.InvalidateVisual();

                this.UpdateLayout();
            }

            if (this.counters == 0)
            {
                this.SizeChanged += this.OnSizeChanged;

                CompositionTargetEx.Rendering += this.CheckInit;

                this.counters++;
            }
        }

        /// <summary>
        /// Called when [size changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="SizeChangedEventArgs"/> instance containing the event data.</param>
        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.resizeDelayTimer.Stop();
            this.resizeDelayTimer.Start();
        }

        /// <summary>
        /// Resizes the game.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ResizeGame(object sender, EventArgs e)
        {
            this.resizeDelayTimer.Stop();
            this.app.ResizeScreen((int)this.ActualWidth, (int)this.ActualHeight);
            this.UpdateLayout();
        }

        /// <summary>
        /// When overridden in a derived class, participates in rendering operations that are directed by the layout system. The rendering instructions for this element are not used directly when this method is invoked, and are instead preserved for later asynchronous use by layout and drawing.
        /// </summary>
        /// <param name="drawingContext">The drawing instructions for a specific element. This context is provided to the layout system.</param>
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (this.image != null && this.image.IsFrontBufferAvailable)
            {
                drawingContext.DrawImage(this.image, new Rect(new Point(), RenderSize));
            }
        }

        /// <summary>
        /// Checks the initialize.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>ç
        private void CheckInit(object sender, EventArgs e)
        {
            this.Render();

            if (this.app.Game != null
                && WaveServices.ScreenContextManager.CurrentContext != null)
            {
                WaveServices.Input.IsEnabled = false;

                if (this.GameLoaded != null)
                {
                    this.GameLoaded(this, this.app.Game);
                }

                CompositionTargetEx.Rendering -= this.CheckInit;
                CompositionTargetEx.Rendering += this.OnRendering;
            }
        }

        /// <summary>
        /// Renders the action.
        /// </summary>
        private void Render()
        {
            this.app.Render();
            this.app.RefreshImageSource();

            if ((WaveServices.Platform == null) || WaveServices.Platform.HasExited)
            {
                this.Unload();
            }
        }

        /// <summary>
        /// Called when [rendering].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnRendering(object sender, EventArgs e)
        {
            if (this.isDirty)
            {
                if (this.LowPriorityRendering)
                {
                    // if we called render previously...
                    if (this.previousRenderCall != null)
                    {
                        var previousStatus = this.previousRenderCall.Status;

                        // ... and the operation didn't finish yet - then skip the current call
                        if (previousStatus == System.Windows.Threading.DispatcherOperationStatus.Pending
                            || previousStatus == System.Windows.Threading.DispatcherOperationStatus.Executing)
                        {
                            return;
                        }
                    }

                    this.previousRenderCall = this.Dispatcher.BeginInvoke(this.renderDelegate, System.Windows.Threading.DispatcherPriority.Input);
                }
                else
                {
                    this.renderDelegate();
                }

                this.IsDirty = this.AutoRender;
            }
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.Mouse.MouseWheel" /> attached event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseWheelEventArgs" /> that contains the event data.</param>
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            if (e.Delta > 0)
            {
                WaveServices.Input.MouseState.Wheel += 1;
            }
            else
            {
                WaveServices.Input.MouseState.Wheel -= 1;
            }
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.Mouse.MouseDown" /> attached event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseButtonEventArgs" /> that contains the event data. This event data reports details about the mouse button that was pressed and the handled state.</param>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            switch (e.ChangedButton)
            {
                case MouseButton.Left:
                    WaveServices.Input.MouseState.LeftButton = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case MouseButton.Middle:
                    WaveServices.Input.MouseState.MiddleButton = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                case MouseButton.Right:
                    WaveServices.Input.MouseState.RightButton = WaveEngine.Common.Input.ButtonState.Pressed;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.Mouse.MouseUp" /> routed event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseButtonEventArgs" /> that contains the event data. The event data reports that the mouse button was released.</param>
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            switch (e.ChangedButton)
            {
                case MouseButton.Left:
                    WaveServices.Input.MouseState.LeftButton = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case MouseButton.Middle:
                    WaveServices.Input.MouseState.MiddleButton = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                case MouseButton.Right:
                    WaveServices.Input.MouseState.RightButton = WaveEngine.Common.Input.ButtonState.Release;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.Mouse.MouseMove" /> attached event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseEventArgs" /> that contains the event data.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            var position = e.GetPosition(this);
            WaveServices.Input.MouseState.X = (int)position.X;
            WaveServices.Input.MouseState.Y = (int)position.Y;
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.Mouse.MouseLeave" /> attached event is raised on this element. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseEventArgs" /> that contains the event data.</param>
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            WaveServices.Input.TouchPanelState.Clear();
        }
    }
}
