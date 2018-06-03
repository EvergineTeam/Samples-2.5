using Physics3DSample.Extensions;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using WaveEngine.Adapter.Input;
using WaveEngine.Common.Input;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics3D;
using WaveEngine.Framework.Services;
#if WINDOWS
#elif UWP
using Windows.UI.Core;
#endif

namespace Physics3DSample.Behaviors
{
    [DataContract]
    public class PlayerBehavior : Behavior
    {
        [RequiredComponent]
        protected CharacterController3D characterController;

        protected Transform3D transform;

#if WINDOWS
        private MouseState lastMouseState;
#elif UWP
        private CoreWindow currentWindow;
#endif

        [RequiredService]
        protected Input inputService;

        private InputManager inputManager;

        [RequiredService]
        protected Platform platformService;

        private Vector3 maxVelocity;
        private Vector3 currentVelocity;

        private Vector3 desiredRotation;
        private KeyboardState lastKeyboardStatus;
        private TimeSpan escapeKeyPressed;

        private bool isPointerCaptured;
        private TimeSpan spacePressedTime;

        protected override void DefaultValues()
        {
            base.DefaultValues();

            this.maxVelocity = Vector3.One * 3f;
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.inputManager = (InputManager)Game.Current.Application.Adapter.InputManager;
            this.transform = this.Owner.ChildEntities.First().FindComponent<Transform3D>();

            this.RegisterPointerEvents();
        }

        protected override void Update(TimeSpan gameTime)
        {
            var mouseState = this.inputService.MouseState;
            var screenRectangle = this.platformService.ScreenRectangle;
            if (!this.isPointerCaptured && (mouseState.RightButton == ButtonState.Pressed) && screenRectangle.Contains(mouseState.Position))
            {
                this.ResetPointer();
            }

            if (this.isPointerCaptured)
            {
                var currentKeyboardState = this.inputService.KeyboardState;

                this.UwpKeyboardWorkAround(ref currentKeyboardState);

                this.UpdateVelocity(gameTime, currentKeyboardState);
                this.UpdateOrientation();

                var spaceKey = Keys.Space;
                if (currentKeyboardState.KeyTouched(ref this.lastKeyboardStatus, ref spaceKey, ref gameTime, ref this.spacePressedTime))
                {
                    this.characterController.Jump();
                }

                var escapeKey = Keys.Escape;
                if (currentKeyboardState.KeyTouched(ref this.lastKeyboardStatus, ref escapeKey, ref gameTime, ref this.escapeKeyPressed))
                {
                    this.ReleasePointer();
                }

                this.lastKeyboardStatus = currentKeyboardState;
            }
        }

        private void UpdateVelocity(TimeSpan gameTime, KeyboardState keyboardState)
        {
            var velocity = Vector3.Zero;

            var multiplier = 2f;

            if (keyboardState.IsKeyPressed(Keys.W) || keyboardState.IsKeyPressed(Keys.Up))
            {
                velocity += transform.WorldTransform.Forward;
            }
            if (keyboardState.IsKeyPressed(Keys.S) || keyboardState.IsKeyPressed(Keys.Down))
            {
                velocity += transform.WorldTransform.Backward;
            }
            if (keyboardState.IsKeyPressed(Keys.A) || keyboardState.IsKeyPressed(Keys.Left))
            {
                velocity += transform.WorldTransform.Left;
            }
            if (keyboardState.IsKeyPressed(Keys.D) || keyboardState.IsKeyPressed(Keys.Right))
            {
                velocity += transform.WorldTransform.Right;
            }
            if (keyboardState.IsKeyPressed(Keys.Shift))
            {
                multiplier = 2f;
            }

            if (velocity != Vector3.Zero)
            {
                this.currentVelocity = Vector3.SmoothStep(this.currentVelocity, this.maxVelocity, 0.2f);
            }

            velocity *= this.currentVelocity * multiplier;

            characterController.SetVelocity(velocity);
        }

        private void UpdateOrientation()
        {
            var dif = GetPointerDif();
            this.CenterPointer();

            var rotationSpeed = 0.002f;
            var rotation = this.transform.LocalRotation;
            rotation.Y += rotationSpeed * dif.X;
            rotation.X += rotationSpeed * dif.Y;

            System.Diagnostics.Debug.WriteLine(rotation);

            this.desiredRotation = rotation;
            this.transform.LocalRotation = Vector3.SmoothStep(this.desiredRotation, this.transform.LocalRotation, 0.01f);
        }

#if WINDOWS
        private void RegisterPointerEvents()
        {
        }

        public void ResetPointer()
        {
            this.isPointerCaptured = true;
        }

        private void CenterPointer()
        {
            ////var screenRectangle = this.platformService.ScreenRectangle;
            ////SetCursorPos((int)screenRectangle.Center.X, (int)screenRectangle.Center.Y);
        }

        private void ReleasePointer()
        {
            this.isPointerCaptured = false;
        }

        private Vector2 GetPointerDif()
        {
            var mouseState = this.inputService.MouseState;

            var dif = Vector2.Zero;
            if (this.lastMouseState.RightButton == ButtonState.Pressed)
            {
                dif = this.lastMouseState.Position - mouseState.Position;
            }

            this.lastMouseState = mouseState;
            return dif;
        }

        private void UwpKeyboardWorkAround(ref KeyboardState currentKeyboardState)
        {
        }
#elif UWP
        private async void RegisterPointerEvents()
        {
            this.currentWindow = ((GameRenderer)Game.Current.Application).CurrentWindow;
            await this.currentWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                this.currentWindow.Activated += this.CurrentWindowActivated;
            });
        }

        private void CurrentWindowActivated(CoreWindow sender, WindowActivatedEventArgs args)
        {
            if (args.WindowActivationState == CoreWindowActivationState.Deactivated)
            {
                this.ReleasePointer();
            }
        }

        public async void ResetPointer()
        {
            await this.inputManager.CurrentInputSource.Dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
            {
                this.inputManager.CurrentInputSource.PointerCursor = null;
            });

            this.currentWindow.SetPointerCapture();
            this.CenterPointer();
            this.isPointerCaptured = true;
        }

        private void CenterPointer()
        {
            var windowBounds = this.currentWindow.Bounds;
            var windowCenter = new Vector2((float)(windowBounds.X + windowBounds.Width * 0.5), (float)(windowBounds.Y + windowBounds.Height * 0.5));
            this.currentWindow.PointerPosition = new Windows.Foundation.Point(windowCenter.X, windowCenter.Y);
        }

        private async void ReleasePointer()
        {
            this.isPointerCaptured = false;
            this.currentWindow.ReleasePointerCapture();
            await this.inputManager.CurrentInputSource.Dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
            {
                this.inputManager.CurrentInputSource.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 0);
            });
        }

        private Vector2 GetPointerDif()
        {
            var windowBounds = this.currentWindow.Bounds;
            var windowCenter = new Vector2((float)(windowBounds.X + windowBounds.Width * 0.5), (float)(windowBounds.Y + windowBounds.Height * 0.5));
            var pointerPosition = new Vector2((float)this.currentWindow.PointerPosition.X, (float)this.currentWindow.PointerPosition.Y);

            var dif = windowCenter - pointerPosition;
            if(Math.Abs(dif.X) < 1 && Math.Abs(dif.Y) < 1)
            {
                return Vector2.Zero;
            }

            return dif;
        }

        private void UwpKeyboardWorkAround(ref KeyboardState currentKeyboardState)
        {
            currentKeyboardState.W = this.currentWindow.GetAsyncKeyState(Windows.System.VirtualKey.W) == CoreVirtualKeyStates.None ? ButtonState.Released : ButtonState.Pressed;
            currentKeyboardState.A = this.currentWindow.GetAsyncKeyState(Windows.System.VirtualKey.A) == CoreVirtualKeyStates.None ? ButtonState.Released : ButtonState.Pressed;
            currentKeyboardState.S = this.currentWindow.GetAsyncKeyState(Windows.System.VirtualKey.S) == CoreVirtualKeyStates.None ? ButtonState.Released : ButtonState.Pressed;
            currentKeyboardState.D = this.currentWindow.GetAsyncKeyState(Windows.System.VirtualKey.D) == CoreVirtualKeyStates.None ? ButtonState.Released : ButtonState.Pressed;
            currentKeyboardState.Up = this.currentWindow.GetAsyncKeyState(Windows.System.VirtualKey.Up) == CoreVirtualKeyStates.None ? ButtonState.Released : ButtonState.Pressed;
            currentKeyboardState.Down = this.currentWindow.GetAsyncKeyState(Windows.System.VirtualKey.Down) == CoreVirtualKeyStates.None ? ButtonState.Released : ButtonState.Pressed;
            currentKeyboardState.Left = this.currentWindow.GetAsyncKeyState(Windows.System.VirtualKey.Left) == CoreVirtualKeyStates.None ? ButtonState.Released : ButtonState.Pressed;
            currentKeyboardState.Right = this.currentWindow.GetAsyncKeyState(Windows.System.VirtualKey.Right) == CoreVirtualKeyStates.None ? ButtonState.Released : ButtonState.Pressed;
            currentKeyboardState.Shift = this.currentWindow.GetAsyncKeyState(Windows.System.VirtualKey.Shift) == CoreVirtualKeyStates.None ? ButtonState.Released : ButtonState.Pressed;
            currentKeyboardState.Space = this.currentWindow.GetAsyncKeyState(Windows.System.VirtualKey.Space) == CoreVirtualKeyStates.None ? ButtonState.Released : ButtonState.Pressed;
            currentKeyboardState.Escape = this.currentWindow.GetAsyncKeyState(Windows.System.VirtualKey.Escape) == CoreVirtualKeyStates.None ? ButtonState.Released : ButtonState.Pressed;
        }
#else
        private void RegisterPointerEvents()
        {
        }

        public void ResetPointer()
        {
        }

        private void CenterPointer()
        {
        }

        private void ReleasePointer()
        {
        }

        private Vector2 GetPointerDif()
        {
            return Vector2.Zero;
        }

        private void UwpKeyboardWorkAround(ref KeyboardState currentKeyboardState)
        {
        }
#endif
    }
}
