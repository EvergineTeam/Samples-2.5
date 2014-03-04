#region Using Statements
using System;
using System.Diagnostics;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Media;
using WaveEngine.Components.UI;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;
using WaveEngine.Framework.Sound;
#endregion

namespace MicrophoneProject
{
    public class MyScene : Scene
    {
        private const string RECORDFILE = "record.wpk";

        private const string STARTTEXT = "Record";

        private const string STOPTEXT = "Stop";

        private const string PLAYTEXT = "Play";

        private const string ERROREXT = "Microphone not found!";

        private Button recordButton;

        private Button playButton;

        private SoundBank bank;

        private SoundInfo sound;

        private ProgressBar progressBar;

        protected override void CreateScene()
        {
            RenderManager.BackgroundColor = Color.Black;

            // Adds record button.
            this.recordButton = new Button("recordButton")
            {
                Text = STARTTEXT,
                Margin = new WaveEngine.Framework.UI.Thickness(100),
                HorizontalAlignment = WaveEngine.Framework.UI.HorizontalAlignment.Left,
                VerticalAlignment = WaveEngine.Framework.UI.VerticalAlignment.Top
            };

            this.recordButton.Click += OnRecordButtonClicked;
            this.EntityManager.Add(this.recordButton);

            // Adds play button
            this.playButton = new Button("playButton")
            {
                Text = PLAYTEXT,
                Margin = new WaveEngine.Framework.UI.Thickness(100),
                HorizontalAlignment = WaveEngine.Framework.UI.HorizontalAlignment.Right,
                VerticalAlignment = WaveEngine.Framework.UI.VerticalAlignment.Top,
                IsVisible = false
            };
            this.playButton.Click += OnPlayButtonClicked;
            this.EntityManager.Add(this.playButton);

            // Shows error label if microphone is not available
            if (!WaveServices.Microphone.IsConnected)
            {
                TextBlock errorText = new TextBlock()
                {
                    Text = ERROREXT,
                    HorizontalAlignment = WaveEngine.Framework.UI.HorizontalAlignment.Center,
                    VerticalAlignment = WaveEngine.Framework.UI.VerticalAlignment.Bottom,
                    Margin = new WaveEngine.Framework.UI.Thickness(50)
                };

                this.EntityManager.Add(errorText);
            }

            this.progressBar = new ProgressBar()
            {
                Maximum = 255,
                Minimum = 0,
                Value = 0,
                Width = 400,
                Height = 10,
                VerticalAlignment = WaveEngine.Framework.UI.VerticalAlignment.Center,
                HorizontalAlignment = WaveEngine.Framework.UI.HorizontalAlignment.Center,
                IsVisible = false
            };
            
            this.EntityManager.Add(this.progressBar);

            //Register bank
            this.bank = new SoundBank(this.Assets);
            WaveServices.SoundPlayer.RegisterSoundBank(this.bank);
        }

        private void OnRecordButtonClicked(object sender, EventArgs e)
        {
            if (!WaveServices.Microphone.IsConnected)
            {
                return;
            }

            if (!WaveServices.Microphone.IsRecording)
            {
                this.recordButton.Text = STOPTEXT;
                this.progressBar.IsVisible = true;
                this.progressBar.Value = 0;
                WaveServices.Microphone.DataAvailable += this.Microphone_DataAvailable;
                WaveServices.Microphone.Start(RECORDFILE);
            }
            else
            {
                this.recordButton.Text = STARTTEXT;
                WaveServices.Microphone.Stop();
                WaveServices.Microphone.DataAvailable -= this.Microphone_DataAvailable;

                //Register sounds

                if ((this.sound != null) && (this.sound.SoundEffect != null))
                {
                    this.sound.SoundEffect.Unload();
                    WaveServices.Assets.Global.UnloadAsset(RECORDFILE);
                }

                this.sound = new SoundInfo(RECORDFILE);
                var stream = WaveServices.Storage.OpenStorageFile(RECORDFILE, WaveEngine.Common.IO.FileMode.Open);
                this.sound.SoundEffect = WaveServices.Assets.Global.LoadAsset<SoundEffect>(RECORDFILE, stream);
                this.bank.AddWithouthLoad(this.sound);

                this.playButton.IsVisible = true;
                this.progressBar.IsVisible = false;
            }
        }

        private void Microphone_DataAvailable(object sender, MicrophoneDataEventArgs e)
        {
            float averageFloat = 0;

            var buffer = e.Buffer;
            int value;

            for (int i = 0; i < buffer.Length; i++)
            {
                value = buffer[i];
                if (value < 0)
                {
                    value = -value;
                }
                averageFloat = ((averageFloat * (i)) + value) / (i + 1);
            }

            int average = (int)averageFloat;

            if (average > this.progressBar.Maximum)
            {
                average = this.progressBar.Maximum;
            }
            else if(average < 0)
            {
                average = 0;
            }

            this.progressBar.Value = average;
        }

        private void OnPlayButtonClicked(object sender, EventArgs e)
        {
            WaveServices.SoundPlayer.Play(this.sound);
        }
    }
}
