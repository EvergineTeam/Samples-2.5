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
        /// <summary>
        /// Gets or sets the play BTN.
        /// </summary>
        /// <value>
        /// The play BTN.
        /// </value>
        public Button PlayBtn { get; set; }

        /// <summary>
        /// Gets or sets the pause BTN.
        /// </summary>
        /// <value>
        /// The pause BTN.
        /// </value>
        public Button PauseBtn { get; set; }

        /// <summary>
        /// Gets or sets the bunny video.
        /// </summary>
        /// <value>
        /// The bunny video.
        /// </value>
        public VideoInfo bunnyVideo { get; set; }

        /// <summary>
        /// Gets or sets the bear video.
        /// </summary>
        /// <value>
        /// The bear video.
        /// </value>
        public VideoInfo bearVideo { get; set; }

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

        protected override void Start()
        {
            base.Start();

            var tvScreenMaterial = this.Assets.LoadModel<MaterialModel>(WaveContent.Assets.Materials.TVScreenMaterial).Material as StandardMaterial;
            tvScreenMaterial.Diffuse = WaveServices.VideoPlayer.VideoTexture;
        }
    }
}
