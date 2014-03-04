#region File Description
//-----------------------------------------------------------------------------
// GLView
//
// Copyright © 2011 Weekend Game Studio. All rights reserved.
// Use is subject to license terms.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.ES20;
using OpenTK.Platform;
using OpenTK.Platform.Android;

using Android.Views;
using Android.Content;
using System.Diagnostics;
using Android.App;
using WaveEngine.Common;
using WaveEngine.Common.Input;
#endregion

namespace WaveAndroid
{
    public class GLView : AndroidGameView
    {
        //        public bool AccelerometerConnected;

        private BasicRigidBody2DProject.Game game;
        private DateTime currentTime, lastTime;
        private IAdapter adapter;
        //        private WaveEngine.Common.Math.Vector3 accelerometer;
        private Window window;

        public GLView(Context context)
            : base(context)
        {
            Activity parent = (Activity)context;
            this.window = parent.Window;

            GLContextVersion = GLContextVersion.Gles2_0;
            //            accelerometer = new WaveEngine.Common.Math.Vector3();

            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(OnUnhandledException);
        }

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            //game.UnexpectedException(e.ExceptionObject as Exception);
        }

        protected override void CreateFrameBuffer()
        {
            base.CreateFrameBuffer();

            GL.Viewport(0, 0, Size.Width, Size.Height);

            game = new BasicRigidBody2DProject.Game();
            adapter = new WaveEngine.Adapter.Adapter(this, window, Size.Height, Size.Width);
            adapter.DefaultOrientation = DisplayOrientation.LandscapeLeft;
            adapter.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
            game.Initialize(adapter);

            //            InitializeAccelerometer(AccelerometerConnected);

            lastTime = DateTime.Now;
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            currentTime = DateTime.Now;

            if (game.HasExited)
            {
                (Context as Activity).Finish();
                game.Unload();
            }

            game.UpdateFrame(currentTime - lastTime);

            lastTime = currentTime;
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            MakeCurrent();

            base.OnRenderFrame(e);

            game.DrawFrame(currentTime - lastTime);

            SwapBuffers();
        }

        public override bool OnKeyDown(Keycode keyCode, KeyEvent e)
        {
            if (keyCode == Keycode.Back)
            {
                WaveEngine.Adapter.Input.InputManager inputManager = (WaveEngine.Adapter.Input.InputManager)adapter.InputManager;
                inputManager.GamePadState.Buttons.Back = ButtonState.Pressed;
            }
            return true;
        }

        public override bool OnKeyUp(Keycode keyCode, KeyEvent e)
        {
            if (keyCode == Keycode.Back)
            {
                WaveEngine.Adapter.Input.InputManager inputManager = (WaveEngine.Adapter.Input.InputManager)adapter.InputManager;
                inputManager.GamePadState.Buttons.Back = ButtonState.Release;
            }
            return true;
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            if (adapter == null)
                return true;

            WaveEngine.Adapter.Input.InputManager inputManager = (WaveEngine.Adapter.Input.InputManager)adapter.InputManager;
            inputManager.TouchPanelState.Clear();

            float x, y;
            int id;
            for (int i = 0; i < e.PointerCount; i++)
            {
                id = e.GetPointerId(i);
                x = e.GetX(i);
                y = e.GetY(i);
                TouchLocationState state;

                if (i == e.ActionIndex)
                {
                    switch (e.ActionMasked)
                    {
                        //DOWN
                        case MotionEventActions.Down:
                        case MotionEventActions.PointerDown:
                            state = TouchLocationState.Pressed;
                            break;
                        //UP
                        case MotionEventActions.Up:
                        case MotionEventActions.PointerUp:
                            state = TouchLocationState.Release;
                            break;
                        //MOVE
                        case MotionEventActions.Move:
                            state = TouchLocationState.Moved;
                            break;
                        //CANCEL, OUTSIDE
                        case MotionEventActions.Cancel:
                        case MotionEventActions.Outside:
                        default:
                            state = TouchLocationState.Invalid;
                            break;
                    }
                }
                else
                {
                    state = TouchLocationState.Moved;
                }

                if (state != TouchLocationState.Invalid && state != TouchLocationState.Release)
                {
                    inputManager.TouchPanelState.AddTouchLocation(id, TouchLocationState.Moved, x, y);
                }
            }

            return true;
        }

        public override void Pause()
        {
            adapter.MusicPlayer.Stop();
            base.Pause();
        }
        //public void InitializeAccelerometer(bool isConnected)
        //{
        //    WaveEngine.Adapter.Input.InputManager inputManager = (WaveEngine.Adapter.Input.InputManager)adapter.InputManager;
        //    inputManager.AccelerometerState.IsConnected = isConnected;
        //}

        //public void AccelerometerChanged(float x, float y, float z)
        //{
        //    WaveEngine.Adapter.Input.InputManager inputManager = (WaveEngine.Adapter.Input.InputManager)adapter.InputManager;
        //    accelerometer.X = x;
        //    accelerometer.Y = y;
        //    accelerometer.Z = z;
        //    inputManager.AccelerometerState.RawAcceleration = accelerometer;
        //    inputManager.AccelerometerState.SmoothAcceleration = inputManager.AccelerometerState.RawAcceleration;
        //}
    }
}
