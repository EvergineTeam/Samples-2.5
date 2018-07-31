using System;
using WaveEngine.Framework.Services;
using Android.App;
using Android.Content.PM;
using Android.Views;

namespace SocialService
{
    [Activity(Label = "SocialService",
            Icon = "@drawable/icon",
            Theme = "@style/AppTheme.Launcher",
            ScreenOrientation = ScreenOrientation.Portrait,
            MainLauncher = true,
            LaunchMode = LaunchMode.SingleTask,
            ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class AndroidActivity : WaveEngine.Adapter.Application
    {
        private SocialService.Game game;
        private bool splashScreenReplaced;

        public AndroidActivity()
        {
            this.FullScreen = true;
    
            // Set the app layout
            this.LayoutId = Resource.Layout.Main;
        }

        public override void OnWindowFocusChanged(bool hasFocus)
        {
            base.OnWindowFocusChanged(hasFocus);

            if (hasFocus)
            {
                int options = (int)this.Window.DecorView.SystemUiVisibility;
                options |= (int)SystemUiFlags.LowProfile;
                options |= (int)SystemUiFlags.HideNavigation;
    
                if ((int)Android.OS.Build.VERSION.SdkInt >= 19)
                {
                    options |= (int)2048; // SystemUiFlags.Inmersive;
                    options |= (int)4096; // SystemUiFlags.ImmersiveSticky;
                }
    
                this.Window.DecorView.SystemUiVisibility = (StatusBarVisibility)options;
            }
        }

        protected override void OnCreate(Android.OS.Bundle savedInstanceState)
        {
            this.RequestedOrientation = ScreenOrientation.SensorLandscape;
            base.OnCreate(savedInstanceState);
            this.FindViewById(Resource.Id.root).Visibility = ViewStates.Invisible;
        }

        public override void Initialize()
        {
            game = new SocialService.Game();
            game.Initialize(this);
    
            this.Window.AddFlags(WindowManagerFlags.KeepScreenOn);
        }

        public override void Update(TimeSpan elapsedTime)
        {
            game.UpdateFrame(elapsedTime);
        }

        public override void Draw(TimeSpan elapsedTime)
        {
            game.DrawFrame(elapsedTime);
    
            if (!this.splashScreenReplaced)
            {
                this.splashScreenReplaced = true;
                this.SetTheme(Android.Resource.Style.ThemeNoTitleBarFullScreen);
                this.FindViewById(Resource.Id.root).Visibility = ViewStates.Visible;
            }
        }

        public override bool OnKeyDown(Keycode keyCode, KeyEvent e)
        {
            if (keyCode == Keycode.Back && WaveServices.Platform != null)
            {
                WaveServices.Platform.Exit();
            }
    
            return base.OnKeyDown(keyCode, e);
        }

        protected override void OnPause()
        {
            if (game != null)
            {
                game.OnDeactivated();
            }
    
            base.OnPause();
        }

        protected override void OnResume()
        {
            base.OnResume();

            if (game != null)
            {
                game.OnActivated();
            }
        }
    }
}
