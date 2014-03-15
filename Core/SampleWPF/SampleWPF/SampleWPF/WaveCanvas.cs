using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WaveEngine.Common.Input;
using WaveEngine.Framework.Services;

namespace SampleWPF
{
    /// <summary>
    /// Class that represents a canvas that contains the wave application.
    /// </summary>
    public class WaveCanvas : Image
    {
        /// <summary>
        /// Gets a value indicating whether the control is in design mode
        /// (running in Blend or Visual Studio).
        /// </summary>
        public static readonly bool IsInDesignMode = (bool)DependencyPropertyDescriptor.FromProperty(DesignerProperties.IsInDesignModeProperty, typeof(FrameworkElement)).Metadata.DefaultValue;

        /// <summary>
        /// Occurs when the wave game is loaded.
        /// </summary>
        public event EventHandler<SampleWPFProject.Game> GameLoaded;

        /// <summary>
        /// The application
        /// </summary>
        private MainApp app;

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
        /// Initializes a new instance of the <see cref="WaveCanvas"/> class.
        /// </summary>
        public WaveCanvas()
        {
            if (!IsInDesignMode)
            {
                this.Loaded += this.OnLoaded;
                this.Unloaded += OnUnloaded;

                EventManager.RegisterClassHandler(typeof(Window), Keyboard.KeyDownEvent, new KeyEventHandler(this.OnKeyDown), true);
                EventManager.RegisterClassHandler(typeof(Window), Keyboard.KeyUpEvent, new KeyEventHandler(this.OnKeyUp), true);
            }
        }

        /// <summary>
        /// Called when [unloaded].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            this.Unload();
        }

        /// <summary>
        /// Unloads this instance.
        /// </summary>
        private void Unload()
        {
            this.MouseMove -= this.OnMouseMove;
            this.MouseWheel -= this.OnMouseWheel;
            this.MouseDown -= this.OnMouseDown;
            this.MouseUp -= this.OnMouseUp;
            this.MouseLeave -= this.OnMouseLeave;
            this.LostMouseCapture -= this.OnMouseLostCapture;
            CompositionTarget.Rendering -= this.OnRendering;
        }

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

            #region Key up switch

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

            #endregion
        }

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

            System.Diagnostics.Debug.WriteLine(e.Key + " " + e.KeyStates);

            #region Key down switch

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

            #endregion
        }

        /// <summary>
        /// Called when [mouse lost capture].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void OnMouseLostCapture(object sender, MouseEventArgs e)
        {
            WaveServices.Input.TouchPanelState.Clear();
        }

        /// <summary>
        /// Called when [mouse leave].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            WaveServices.Input.TouchPanelState.Clear();
        }

        /// <summary>
        /// Called when [loaded].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void OnLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            this.app = new MainApp();
            this.Source = this.app.RunImageSource();

            this.MouseMove += this.OnMouseMove;
            this.MouseWheel += this.OnMouseWheel;
            this.MouseDown += this.OnMouseDown;
            this.MouseUp += this.OnMouseUp;
            this.MouseLeave += this.OnMouseLeave;
            this.LostMouseCapture += this.OnMouseLostCapture;

            CompositionTarget.Rendering += this.OnRendering;
            CompositionTarget.Rendering += this.CheckInit;
        }

        /// <summary>
        /// Checks the initialize.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        private void CheckInit(object sender, EventArgs e)
        {
            if (this.app.Game != null 
                && WaveServices.ScreenContextManager.CurrentContext != null)
            {
                WaveServices.Input.IsEnabled = false;
                
                if (this.GameLoaded != null)
                {
                    this.GameLoaded(this, this.app.Game);
                }

                CompositionTarget.Rendering -= this.CheckInit;
            }
        }

        /// <summary>
        /// Called when [rendering].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnRendering(object sender, EventArgs e)
        {
            this.app.Render();

            if ((WaveServices.Platform == null) || WaveServices.Platform.HasExited)
            {
                this.Unload();
            }
        }

        /// <summary>
        /// Called when [mouse wheel].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseWheelEventArgs"/> instance containing the event data.</param>
        private void OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            WaveServices.Input.MouseState.Wheel += e.Delta;
        }

        /// <summary>
        /// Called when [mouse down].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            WaveServices.Input.MouseState.LeftButton = WaveEngine.Common.Input.ButtonState.Pressed;

            WaveServices.Input.TouchPanelState.AddTouchLocation(
                0,
                TouchLocationState.Pressed,
                WaveServices.Input.MouseState.X,
                WaveServices.Input.MouseState.Y);
        }

        /// <summary>
        /// Called when [mouse up].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            WaveServices.Input.MouseState.LeftButton = WaveEngine.Common.Input.ButtonState.Release;
            WaveServices.Input.TouchPanelState.Clear();
        }

        /// <summary>
        /// Called when [mouse move].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseEventArgs"/> instance containing the event data.</param>
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            var position = e.GetPosition(this);
            WaveServices.Input.MouseState.X = (int)position.X;
            WaveServices.Input.MouseState.Y = (int)position.Y;

            if (WaveServices.Input.TouchPanelState.Count > 0)
            {
                WaveServices.Input.TouchPanelState.ModifyTouch(
                    0,
                    TouchLocationState.Pressed,
                    WaveServices.Input.MouseState.X,
                    WaveServices.Input.MouseState.Y);
            }
        }
    }
}
