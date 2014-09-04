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

namespace CameraCaptureProject
{
    public class MyScene : Scene
    {
        private bool isRecording = false;
        private bool isVideoPlaying = false;
        private string finalVideoPath;

        Entity TvRoomEntity;
        Button RecBtn;
        Button PlayRecordedBtn;


        protected override void CreateScene()
        {

            FreeCamera camera = new FreeCamera("camera", new Vector3(-1.3f, 1.6f, 2.5f), new Vector3(-0.25f, 1.375f, 1.275f))
            {
                Speed = 5,
                NearPlane = 0.1f
            };

            EntityManager.Add(camera);

            this.TvRoomEntity = new Entity("tvRoom")
            .AddComponent(new Transform3D())
            .AddComponent(new Model("Content/TvRoom.wpk"))
            .AddComponent(new ModelRenderer())
            .AddComponent(new MaterialsMap(new System.Collections.Generic.Dictionary<string, Material>
                {
                    {"floor", new DualTextureMaterial("Content/parketFloor_Difuse.wpk", "Content/TvRoomLightingMap.wpk", DefaultLayers.Opaque)},
                    {"tv", new DualTextureMaterial("Content/Tv_Difuse.wpk", "Content/TvRoomLightingMap.wpk", DefaultLayers.Opaque)},
                    {"table", new DualTextureMaterial("Content/table_Difuse.wpk", "Content/TvRoomLightingMap.wpk", DefaultLayers.Opaque)},
                    {"chair", new DualTextureMaterial("Content/Chair_Difuse.wpk", "Content/TvRoomLightingMap.wpk", DefaultLayers.Opaque)},

                    // Camera preview texture used in a basic material
                    {"tv_screen", new BasicMaterial(Color.Black)}
                }
                ));

            EntityManager.Add(this.TvRoomEntity);

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

                this.RecBtn = new Button("RecBtn")
                {
                    Text = "Start Rec",
                    Margin = new WaveEngine.Framework.UI.Thickness(5, 5, 5, 0),
                    Width = 170
                };

                controlPanel.Add(this.RecBtn);

                this.PlayRecordedBtn = new Button("PlayRecordedBtn")
                {
                    Text = "Play",
                    Width = 170,
                    Margin = new WaveEngine.Framework.UI.Thickness(5, 0, 5, 5),
                    Opacity = 0.5f
                };

                controlPanel.Add(this.PlayRecordedBtn);

                this.RecBtn.Click += (e, s) =>
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

                this.PlayRecordedBtn.Click += (e, s) =>
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

                this.TvRoomEntity.FindComponent<MaterialsMap>().Materials["tv_screen"] = new BasicMaterial(WaveServices.CameraCapture.PreviewTexture);
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
                this.RecBtn.Text = "Stop Rec";
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
                this.RecBtn.Text = "Start Rec";
                this.finalVideoPath = WaveServices.CameraCapture.StopRecording();

                if (this.finalVideoPath != null)
                {
                    this.PlayRecordedBtn.Opacity = 1f;
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

                this.TvRoomEntity.FindComponent<MaterialsMap>().Materials["tv_screen"] = new BasicMaterial(WaveServices.VideoPlayer.VideoTexture);
                this.TvRoomEntity.RefreshDependencies();

                this.PlayRecordedBtn.Text = "Stop";

                this.isVideoPlaying = true;
            }
        }

        private void StopVideo()
        {
            if (WaveServices.VideoPlayer.State == VideoState.Playing)
            {
                WaveServices.VideoPlayer.Stop();
            }

            this.TvRoomEntity.FindComponent<MaterialsMap>().Materials["tv_screen"] = new BasicMaterial(WaveServices.CameraCapture.PreviewTexture);
            this.TvRoomEntity.RefreshDependencies();

            this.PlayRecordedBtn.Text = "Play";

            this.isVideoPlaying = false;
        }
    }
}
