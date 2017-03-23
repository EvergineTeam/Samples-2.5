using System;
using Android.App;
using Android.Content.PM;
using Android.Views;
using Android.OS;
using WaveEngine.Common;
using WaveEngine.Adapter;
using WaveEngine.Common.Input;

namespace XamarinFormsProfileSample.Droid
{
	[Activity (Label = "XamarinFormsProfileSample", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity, IAndroidApplication
    {
        private IGame game;
        private GLView view;
        private bool isGameInitialized = false;

        public GLView View
        {
            get
            {
                return this.view;
            }

            set
            {
                this.view = value;
            }
        }

        public Activity Activity
        {
            get
            {
                return this;
            }
        }

        public int LayoutId { get; set; }

        private bool fullScreen;

        public WaveEngine.Common.IAdapter Adapter
        {
            get
            {
                if (this.View == null)
                {
                    return null;
                }
                else
                {
                    return View.Adapter;
                }
            }
        }

        public string WindowTitle
        {
            get { return string.Empty; }
        }

        public bool FullScreen
        {
            get
            {
                return this.fullScreen;
            }

            set
            {
                if (this.fullScreen != value)
                {
                    this.fullScreen = value;

                    if (this.Window != null)
                    {
                        if (value)
                        {
                            this.Window.AddFlags(WindowManagerFlags.Fullscreen);
                        }
                        else
                        {
                            this.Window.ClearFlags(WindowManagerFlags.Fullscreen);
                        }
                    }
                }
            }
        }

        public int Width
        {
            get { return this.Adapter.Width; }
        }

        public int Height
        {
            get { return this.Adapter.Height; }
        }

        private DisplayOrientation defaultOrientation;

        public DisplayOrientation SupportedOrientations { get; set; }

        public DisplayOrientation DefaultOrientation
        {
            get
            {
                return this.defaultOrientation;
            }

            set
            {
                if (this.defaultOrientation != value)
                {
                    this.defaultOrientation = value;
                }
            }
        }

        public bool SkipDefaultSplash
        {
            get; set;
        }

        public void Initialize()
        {
        }

        public void Initialize(IGame theGame)
        {
            game = theGame;
            isGameInitialized = false;
        }

        public void Update(TimeSpan elapsedTime)
        {
            if (game != null)
            {
                if (!isGameInitialized)
                {
                    game.Initialize(this);
                    isGameInitialized = true;
                }

                game.UpdateFrame(elapsedTime);
            }
        }

        public void Draw(TimeSpan elapsedTime)
        {
            if (game != null)
            {
                game.DrawFrame(elapsedTime);
            }
        }

        public virtual void Exit()
        {
            this.Finish();
            Java.Lang.JavaSystem.Exit(0);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            this.VolumeControlStream = Android.Media.Stream.Music;

            if (this.FullScreen)
            {
                Window.AddFlags(WindowManagerFlags.Fullscreen);
            }
            else
            {
                Window.ClearFlags(WindowManagerFlags.Fullscreen);
            }


            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }

        protected override void OnPause()
        {
            base.OnPause();

            if (this.View != null)
            {
                this.View.Pause();
            }
        }

        protected override void OnResume()
        {
            base.OnResume();

            if (this.View != null)
            {
                this.View.Resume();
            }
        }

        public override bool OnKeyDown(Keycode keyCode, KeyEvent e)
        {
            bool handled = false;

            if (this.View != null)
            {
                handled = this.View.OnKeyDown(keyCode, e);
            }

            return handled;
        }

        public override bool OnKeyUp(Keycode keyCode, KeyEvent e)
        {
            bool handled = false;

            if (this.View != null)
            {
                handled = this.View.OnKeyUp(keyCode, e);
            }

            return handled;
        }

        public override void OnConfigurationChanged(Android.Content.Res.Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
        }

        public static ScreenOrientation GetNativeOrientation(DisplayOrientation orientation)
        {
            ScreenOrientation nativeOrientation = ScreenOrientation.Sensor;

            switch (orientation)
            {
                case DisplayOrientation.Default:
                case DisplayOrientation.LandscapeLeft:
                case DisplayOrientation.LandscapeRight:
                    nativeOrientation = ScreenOrientation.SensorLandscape;
                    break;
                case DisplayOrientation.Portrait:
                    nativeOrientation = ScreenOrientation.SensorPortrait;
                    break;
            }

            return nativeOrientation;
        }
    }
}

