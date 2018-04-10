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
using WaveEngine.Framework.Models;
using WaveEngine.Framework.Resources;
using WaveEngine.Framework.Services;
using WaveEngine.Materials;
#endregion

namespace VideoPlayer
{
    public class MyScene : Scene
    {
        private Button playBtn { get; set; }

        private Button pauseBtn { get; set; }

        private VideoInfo bunnyVideo { get; set; }

        private VideoInfo bearVideo { get; set; }

        protected override void CreateScene()
        {
            this.Load(WaveContent.Scenes.MyScene);
            
            this.bunnyVideo = WaveServices.VideoPlayer.VideoInfoFromPath(WaveContent.Assets.Video.bear_mp4);
            this.bearVideo = WaveServices.VideoPlayer.VideoInfoFromPath(WaveContent.Assets.Video.bunny_mp4);

            WaveServices.VideoPlayer.IsLooped = true;
            WaveServices.VideoPlayer.Play(this.bunnyVideo);

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
                WaveServices.VideoPlayer.Play(this.bunnyVideo);

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
                WaveServices.VideoPlayer.Play(this.bearVideo);
            };
            controlPanel.Add(radioButton2);

            playBtn = new Button("playBtn")
            {
                Text = "Play",
                Opacity = 0.5f,
                Margin = new WaveEngine.Framework.UI.Thickness(5, 0, 5, 0),
                Width = 170
            };

            playBtn.Click += (e, s) =>
            {
                WaveServices.VideoPlayer.Resume();
                playBtn.Opacity = 0.5f;
                pauseBtn.Opacity = 1f;
            };

            controlPanel.Add(playBtn);

            pauseBtn = new Button("pauseBtn")
            {
                Text = "Pause",
                Width = 170,
                Margin = new WaveEngine.Framework.UI.Thickness(5, 0, 5, 5),
            };

            pauseBtn.Click += (e, s) =>
            {
                WaveServices.VideoPlayer.Pause();
                playBtn.Opacity = 1f;
                pauseBtn.Opacity = 0.5f;
            };

            controlPanel.Add(pauseBtn);

            EntityManager.Add(controlPanel);
        }

        protected override void Start()
        {
            base.Start();

            var tvScreenMaterial = this.Assets.LoadModel<MaterialModel>(WaveContent.Assets.Materials._02_Default).Material as StandardMaterial;
            tvScreenMaterial.Diffuse1 = WaveServices.VideoPlayer.VideoTexture;
        }
    }
}
