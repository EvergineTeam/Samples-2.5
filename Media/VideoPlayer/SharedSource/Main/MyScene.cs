#region Using Statements
using System;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Common.Media;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Components.UI;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Resources;
using WaveEngine.Framework.Services;
#endregion

namespace VideoPlayer
{
    public class MyScene : Scene
    {
        public Button PlayBtn { get; set; }
        public Button PauseBtn { get; set; }
        public VideoInfo Video1 { get; set; }
        public VideoInfo Video2 { get; set; }

        protected override void CreateScene()
        {
            this.Load(@"Content/Scenes/MyScene.wscene");

            this.Video1 = WaveServices.VideoPlayer.VideoInfoFromPath(@"Content/Assets/Video/bunny.mp4");
            this.Video2 = WaveServices.VideoPlayer.VideoInfoFromPath(@"Content/Assets/Video/bear.mp4");

            WaveServices.VideoPlayer.IsLooped = true;
            WaveServices.VideoPlayer.Play(this.Video1);

            StackPanel controlPanel = new StackPanel()
            {
                VerticalAlignment = WaveEngine.Framework.UI.VerticalAlignment.Bottom,
                HorizontalAlignment = WaveEngine.Framework.UI.HorizontalAlignment.Right,
                Margin = new WaveEngine.Framework.UI.Thickness(0, 0, 30, 30),
                BorderColor = Color.White,
                IsBorder = true,
            };

            ToggleSwitch muteToggle = new ToggleSwitch()
            {
                OnText = "Mute",
                OffText = "Mute",
                Margin = new WaveEngine.Framework.UI.Thickness(5, 5, 5, 10)
            };

            muteToggle.Toggled += (e, s) =>
            {
                WaveServices.VideoPlayer.IsMuted = muteToggle.IsOn;
            };

            controlPanel.Add(muteToggle);

            RadioButton radioButton1 = new RadioButton()
            {
                GroupName = "Videos",
                Text = "Channel 1",
                Margin = new WaveEngine.Framework.UI.Thickness(5, 0, 5, 0),
                IsChecked = true
            };
            radioButton1.Checked += (e, s) =>
            {
                WaveServices.VideoPlayer.Play(this.Video1);

            };
            controlPanel.Add(radioButton1);

            RadioButton radioButton2 = new RadioButton()
            {
                GroupName = "Videos",
                Text = "Channel 2",
                Margin = new WaveEngine.Framework.UI.Thickness(5, 0, 5, 10)
            };
            radioButton2.Checked += (e, s) =>
            {
                WaveServices.VideoPlayer.Play(this.Video2);
            };
            controlPanel.Add(radioButton2);

            PlayBtn = new Button("playBtn")
            {
                Text = "Play",
                Opacity = 0.5f,
                Margin = new WaveEngine.Framework.UI.Thickness(5, 0, 5, 0),
                Width = 170
            };

            PlayBtn.Click += (e, s) =>
            {
                WaveServices.VideoPlayer.Resume();
                PlayBtn.Opacity = 0.5f;
                PauseBtn.Opacity = 1f;
            };

            controlPanel.Add(PlayBtn);

            PauseBtn = new Button("pauseBtn")
            {
                Text = "Pause",
                Width = 170,
                Margin = new WaveEngine.Framework.UI.Thickness(5, 0, 5, 5),
            };

            PauseBtn.Click += (e, s) =>
            {
                WaveServices.VideoPlayer.Pause();
                PlayBtn.Opacity = 1f;
                PauseBtn.Opacity = 0.5f;
            };

            controlPanel.Add(PauseBtn);

            EntityManager.Add(controlPanel);
        }
    }
}
