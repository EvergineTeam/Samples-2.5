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

namespace CameraCapture
{
    public class MyScene : Scene
    {
        /// <summary>
        /// The is recording
        /// </summary>
        private bool isRecording;

        /// <summary>
        /// The is video playing
        /// </summary>
        private bool isVideoPlaying;

        /// <summary>
        /// The final video path
        /// </summary>
        private string finalVideoPath;

        /// <summary>
        /// The record button
        /// </summary>
        Button RecButton;

        /// <summary>
        /// The play recorded button
        /// </summary>
        private Button PlayRecordedButton;

        private StandardMaterial tvScreenMaterial;

        private int recordCount = 0;

        protected override void CreateScene()
        {
            this.Load(WaveContent.Scenes.MyScene);

            if (WaveServices.CameraCapture.IsConnected)
            {
                StackPanel controlPanel = new StackPanel()
                {
                    VerticalAlignment = WaveEngine.Framework.UI.VerticalAlignment.Bottom,
                    HorizontalAlignment = WaveEngine.Framework.UI.HorizontalAlignment.Right,
                    Margin = new WaveEngine.Framework.UI.Thickness(0, 0, 30, 30),
                    BorderColor = Color.White,
                    IsBorder = true,
                };

                this.RecButton = new Button("RecBtn")
                {
                    Text = "Start Rec",
                    Margin = new WaveEngine.Framework.UI.Thickness(5, 5, 5, 0),
                    Width = 170
                };

                controlPanel.Add(this.RecButton);

                this.PlayRecordedButton = new Button("PlayRecordedBtn")
                {
                    Text = "Play",
                    Width = 170,
                    Margin = new WaveEngine.Framework.UI.Thickness(5, 0, 5, 5),
                    Opacity = 0.5f
                };

                controlPanel.Add(this.PlayRecordedButton);

                this.RecButton.Click += (e, s) =>
                {
                    if (!this.isRecording)
                    {
                        this.StartRecording();
                    }
                    else
                    {
                        this.StopRecording();
                    }
                };

                this.PlayRecordedButton.Click += (e, s) =>
                {
                    if (!this.isVideoPlaying)
                    {
                        this.PlayVideo();
                    }
                    else
                    {
                        this.StopVideo();
                    }
                };

                EntityManager.Add(controlPanel);
            }
            else
            {
                TextBlock text = new TextBlock()
                {
                    Text = "There is no connected camera",
                    Width = 300,
                    VerticalAlignment = WaveEngine.Framework.UI.VerticalAlignment.Bottom,
                    HorizontalAlignment = WaveEngine.Framework.UI.HorizontalAlignment.Right,
                    Margin = new WaveEngine.Framework.UI.Thickness(0, 0, 30, 30)
                };

                EntityManager.Add(text);
            }
        }



        protected override void Start()
        {
            base.Start();

            if (WaveServices.CameraCapture.IsConnected)
            {

                WaveServices.VideoPlayer.OnComplete += (s, e) => { this.StopVideo(); };
                WaveServices.CameraCapture.Start(CameraCaptureType.Front);

                this.tvScreenMaterial = this.Assets.LoadModel<MaterialModel>(WaveContent.Assets.Material.TVScreenMaterial).Material as StandardMaterial;
                this.tvScreenMaterial.Diffuse = WaveServices.CameraCapture.PreviewTexture;
            }
        }

        protected override void End()
        {
            if (WaveServices.CameraCapture.IsConnected)
            {
                WaveServices.CameraCapture.Stop();
            }
            base.End();
        }

        private void StartRecording()
        {
            if (WaveServices.CameraCapture.IsConnected)
            {
                this.isRecording = true;
                this.RecButton.Text = "Stop Rec";
                WaveServices.CameraCapture.StartRecording(string.Format("MyVideo_{0}.mp4", this.recordCount++));
            }
        }

        private void StopRecording()
        {
            if (WaveServices.CameraCapture.IsConnected)
            {
                if (WaveServices.CameraCapture.State != CameraCaptureState.Recording)
                {
                    return;
                }

                this.isRecording = false;
                this.RecButton.Text = "Start Rec";
                this.finalVideoPath = WaveServices.CameraCapture.StopRecording();

                if (this.finalVideoPath != null)
                {
                    this.PlayRecordedButton.Opacity = 1f;
                }
            }
        }

        private void PlayVideo()
        {
            this.StopRecording();

            if (this.finalVideoPath != null)
            {
                VideoInfo videoInfo = WaveServices.VideoPlayer.VideoInfoFromPath(this.finalVideoPath);

                WaveServices.VideoPlayer.Play(videoInfo);

                this.tvScreenMaterial.Diffuse = WaveServices.VideoPlayer.VideoTexture;

                this.PlayRecordedButton.Text = "Stop";

                this.isVideoPlaying = true;
            }
        }

        private void StopVideo()
        {
            if (WaveServices.VideoPlayer.State == VideoState.Playing)
            {
                WaveServices.VideoPlayer.Stop();
            }

            this.tvScreenMaterial.Diffuse = WaveServices.CameraCapture.PreviewTexture;

            this.PlayRecordedButton.Text = "Play";

            this.isVideoPlaying = false;
        }
    }
}
