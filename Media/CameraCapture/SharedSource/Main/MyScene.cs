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
        Button PlayRecordedButton;

        /// <summary>
        /// The tv room entity
        /// </summary>
        Entity TvRoomEntity;

        protected override void CreateScene()
        {
            this.Load(@"Content/Scenes/MyScene.wscene");

            this.TvRoomEntity = this.EntityManager.Find("tvModel");

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

                this.TvRoomEntity.FindComponent<MaterialsMap>().Materials["tv_screen"] = new StandardMaterial(DefaultLayers.Opaque, WaveServices.CameraCapture.PreviewTexture);
                this.TvRoomEntity.RefreshDependencies();
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
                WaveServices.CameraCapture.StartRecording("MyVideo.mp4");
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


                this.TvRoomEntity.FindComponent<MaterialsMap>().Materials["tv_screen"] = new StandardMaterial(DefaultLayers.Opaque, WaveServices.VideoPlayer.VideoTexture);

                this.TvRoomEntity.RefreshDependencies();

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

            this.TvRoomEntity.FindComponent<MaterialsMap>().Materials["tv_screen"] = new StandardMaterial(DefaultLayers.Opaque, WaveServices.CameraCapture.PreviewTexture);
            this.TvRoomEntity.RefreshDependencies();

            this.PlayRecordedButton.Text = "Play";

            this.isVideoPlaying = false;
        }
    }
}
