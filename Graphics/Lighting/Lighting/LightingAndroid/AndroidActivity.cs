using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using WaveEngine.Common.Input;
using Android.Content.Res;

namespace LightingAndroid
{
    [Activity(Label = "LightingAndroid",
        MainLauncher = true,
        Icon = "@drawable/icon",
        LaunchMode = LaunchMode.SingleTask,
        ConfigurationChanges = ConfigChanges.Orientation)]

    public class AndroidActivity : Activity
    {
        private OrientationListener orientationListener;
        public static GLView view;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            orientationListener = new OrientationListener(this);
            if (orientationListener.CanDetectOrientation())
            {
                orientationListener.Enable();
            }

            // hide window title 
            this.RequestedOrientation = ScreenOrientation.Landscape;
            RequestWindowFeature(WindowFeatures.NoTitle);

            // make app fullscreen 
            Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);

            // Create our OpenGL view, and display it
            view = new GLView(this);

            SetContentView(view);

            view.Run();

            //Accelerometer.SetupAccelerometer(this);
        }

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            if (newConfig.Orientation == Android.Content.Res.Orientation.Portrait)
            {
                this.RequestedOrientation = ScreenOrientation.Landscape;
            }

            base.OnConfigurationChanged(newConfig);
        }

        protected override void OnPause()
        {
            view.Pause();
            base.OnPause();
        }

        protected override void OnResume()
        {
            view.Resume();
            base.OnResume();
        }

        public override bool OnKeyDown(Keycode keyCode, KeyEvent e)
        {
            view.OnKeyDown(keyCode, e);
            return true;
        }

        public override bool OnKeyUp(Keycode keyCode, KeyEvent e)
        {
            view.OnKeyUp(keyCode, e);
            return true;
        }
    }

    internal class OrientationListener : OrientationEventListener
    {
        AndroidActivity activity;

        public OrientationListener(AndroidActivity activity)
            : base(activity.ApplicationContext)
        {
            this.activity = activity;
        }

        private bool inprogress = false;

        public override void OnOrientationChanged(int orientation)
        {
            if (!inprogress)
            {
                inprogress = true;
                // Divide by 90 into an int to round, then multiply out to one of 5 positions, either 0,90,180,270,360. 
                int ort = (90 * (int)Math.Round(orientation / 90f)) % 360;

                // Convert 360 to 0
                if (ort == 360)
                {
                    ort = 0;
                }

                //var disporientation = DisplayOrientation.Unknown;
                var disporientation = DisplayOrientation.Default;

                switch (ort)
                {
                    case 90: disporientation = DisplayOrientation.LandscapeRight;
                        break;
                    case 270: disporientation = DisplayOrientation.LandscapeLeft;
                        break;
                    //case 0: disporientation = DisplayOrientation.Portrait;
                    //    break;
                    default:
                        disporientation = DisplayOrientation.LandscapeLeft;
                        break;
                }

                //if (AndroidGameActivity.Game.Window.CurrentOrientation != disporientation)
                //{
                //    AndroidGameActivity.Game.Window.SetOrientation(disporientation);
                //}
                inprogress = false;
            }
        }
    }
}

