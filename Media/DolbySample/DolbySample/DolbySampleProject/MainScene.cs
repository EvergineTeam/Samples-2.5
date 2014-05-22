using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Common.Media;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Components.UI;
using WaveEngine.Framework;
using WaveEngine.Framework.Animation;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;

namespace DolbySampleProject
{
    public class MainScene : Scene
    {
        /// <summary>
        /// The buttonwidth
        /// </summary>
        private readonly int BUTTONWIDTH = 200;
        
        /// <summary>
        /// The buttonheight
        /// </summary>
        private readonly int BUTTONHEIGHT = 50;

        /// <summary>
        /// The fade in
        /// </summary>
        private SingleAnimation fadeIn;

        /// <summary>
        /// The button panel
        /// </summary>
        private StackPanel buttonPanel;

        /// <summary>
        /// The information panel
        /// </summary>
        private StackPanel infoPanel;

        /// <summary>
        /// The enable dolby button
        /// </summary>
        private Button enableDolbyButton;

        /// <summary>
        /// The profile game button
        /// </summary>
        private Button profileGameButton;

        /// <summary>
        /// The profile music button
        /// </summary>
        private Button profileMusicButton;

        /// <summary>
        /// The profile voice button
        /// </summary>
        private Button profileVoiceButton;

        /// <summary>
        /// The profile movie button
        /// </summary>
        private Button profileMovieButton;

        /// <summary>
        /// The is dolby enabled text box
        /// </summary>
        private TextBlock isDolbyEnabledTextBox;

        /// <summary>
        /// The selected dolby profile text box
        /// </summary>
        private TextBlock selectedDolbyProfileTextBox;

        /// <summary>
        /// Creates the scene.
        /// </summary>
        /// <remarks>
        /// This method is called before all
        /// <see cref="T:WaveEngine.Framework.Entity" /> instances in this instance are initialized.
        /// </remarks>
        protected override void CreateScene()
        {
            // Allow transparent background
            this.RenderManager.ClearFlags = ClearFlags.DepthAndStencil;

            // Music Player
            MusicInfo musicInfo = new MusicInfo("Content/audiodolby.mp3");
            WaveServices.MusicPlayer.Play(musicInfo);
            WaveServices.MusicPlayer.IsRepeat = true;

            WaveServices.MusicPlayer.Play(new MusicInfo("Content/audiodolby.mp3"));
            WaveServices.MusicPlayer.IsDolbyEnabled = true;
            WaveServices.MusicPlayer.DolbyProfile = DolbyProfiles.GAME;

            // Button Stack Panel
            buttonPanel = new StackPanel();
            buttonPanel.Entity.AddComponent(new AnimationUI());
            buttonPanel.VerticalAlignment = WaveEngine.Framework.UI.VerticalAlignment.Center;
            buttonPanel.HorizontalAlignment = WaveEngine.Framework.UI.HorizontalAlignment.Center;

            // Enable/Disable Dolby Button
            this.CreateButton(out this.enableDolbyButton, string.Empty, this.EnableDolbyButtonClick);
            buttonPanel.Add(this.enableDolbyButton);

            // Profile Buttons
            this.CreateButton(out this.profileGameButton, WaveEngine.Common.Media.DolbyProfiles.GAME.ToString(), this.ProfileGameButtonClick);
            buttonPanel.Add(this.profileGameButton);
            this.CreateButton(out this.profileMovieButton, WaveEngine.Common.Media.DolbyProfiles.MOVIE.ToString(), this.ProfileMovieButtonClick);
            buttonPanel.Add(this.profileMovieButton);
            this.CreateButton(out this.profileMusicButton, WaveEngine.Common.Media.DolbyProfiles.MUSIC.ToString(), this.ProfileMusicButtonClick);
            buttonPanel.Add(this.profileMusicButton);
            this.CreateButton(out this.profileVoiceButton, WaveEngine.Common.Media.DolbyProfiles.VOICE.ToString(), this.ProfileVoiceButtonClick);
            buttonPanel.Add(this.profileVoiceButton);
            EntityManager.Add(buttonPanel);

            // Information Text
            infoPanel = new StackPanel();
            infoPanel.Entity.AddComponent(new AnimationUI());
            infoPanel.VerticalAlignment = WaveEngine.Framework.UI.VerticalAlignment.Bottom;
            infoPanel.HorizontalAlignment = WaveEngine.Framework.UI.HorizontalAlignment.Center;

            infoPanel.Orientation = Orientation.Vertical;
            this.CreateLabel(out this.isDolbyEnabledTextBox, string.Empty);
            infoPanel.Add(this.isDolbyEnabledTextBox);
            this.CreateLabel(out this.selectedDolbyProfileTextBox, string.Empty);
            infoPanel.Add(this.selectedDolbyProfileTextBox);
            EntityManager.Add(infoPanel);

            // Sets text
            this.SetDolbyText();

            //// Animations
            this.fadeIn = new SingleAnimation(0.0f, 1.0f, TimeSpan.FromSeconds(8), EasingFunctions.Cubic);
        }

        /// <summary>
        /// Allows to perform custom code when this instance is started.
        /// </summary>
        /// <remarks>
        /// This base method perfoms a layout pass.
        /// </remarks>
        protected override void Start()
        {
            base.Start();

            // FadeIn animations
            var buttonPanelAnimation = this.buttonPanel.Entity.FindComponent<AnimationUI>();
            buttonPanelAnimation.BeginAnimation(Transform2D.OpacityProperty, this.fadeIn);

            var infoPanelAnimation = this.infoPanel.Entity.FindComponent<AnimationUI>();
            infoPanelAnimation.BeginAnimation(Transform2D.OpacityProperty, this.fadeIn);
        }

        /// <summary>
        /// Creates the button.
        /// </summary>
        /// <param name="button">The button.</param>
        /// <param name="text">The text.</param>
        /// <param name="clickHandler">The click handler.</param>
        private void CreateButton(out Button button, string text, EventHandler clickHandler)
        {
            button = new Button();
            button.Text = text;
            button.Width = this.BUTTONWIDTH;
            button.Height = this.BUTTONHEIGHT;
            button.IsBorder = false;
            button.Click += clickHandler;
        }

        /// <summary>
        /// Creates the label.
        /// </summary>
        /// <param name="textblock">The textblock.</param>
        /// <param name="text">The text.</param>
        private void CreateLabel(out TextBlock textblock, string text)
        {
            textblock = new TextBlock();
            textblock.Text = text;
            //textblock.Width = this.BUTTONWIDTH;
            //textblock.Height = this.BUTTONHEIGHT;
        }

        /// <summary>
        /// Sets the bolby button text.
        /// </summary>
        private void SetDolbyText()
        {
            if (enabled)
            {
                this.enableDolbyButton.Text = "Disable Dolby";
                this.isDolbyEnabledTextBox.Text = "DOLBY ON";
            }
            else
            {
                this.enableDolbyButton.Text = "Enable Dolby";
                this.isDolbyEnabledTextBox.Text = "DOLBY OFF";
            }
            
            this.selectedDolbyProfileTextBox.Text = WaveServices.MusicPlayer.DolbyProfile.ToString();
        }

        bool enabled = true;

        /// <summary>
        /// Enables the dolby button click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void EnableDolbyButtonClick(object sender, EventArgs e)
        {
            enabled = !enabled;
            WaveServices.MusicPlayer.IsDolbyEnabled = enabled;
            this.SetDolbyText();
        }

        /// <summary>
        /// Profiles the music button click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ProfileMusicButtonClick(object sender, EventArgs e)
        {
            this.SetProfile(WaveEngine.Common.Media.DolbyProfiles.MUSIC);
        }

        /// <summary>
        /// Profiles the movie button click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ProfileMovieButtonClick(object sender, EventArgs e)
        {
            this.SetProfile(WaveEngine.Common.Media.DolbyProfiles.MOVIE);
        }

        /// <summary>
        /// Profiles the voice button click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ProfileVoiceButtonClick(object sender, EventArgs e)
        {
            this.SetProfile(WaveEngine.Common.Media.DolbyProfiles.VOICE);
        }

        /// <summary>
        /// Profiles the game button click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ProfileGameButtonClick(object sender, EventArgs e)
        {
            this.SetProfile(WaveEngine.Common.Media.DolbyProfiles.GAME);
        }

        /// <summary>
        /// Sets the profile.
        /// </summary>
        /// <param name="profile">The profile.</param>
        private void SetProfile(WaveEngine.Common.Media.DolbyProfiles profile)
        {
            this.enabled = true;
            WaveServices.MusicPlayer.DolbyProfile = profile;
            this.SetDolbyText();
        }
    }
}