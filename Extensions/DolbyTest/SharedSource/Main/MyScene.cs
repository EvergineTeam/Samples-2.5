#region Using Statements
using System;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Resources;
using WaveEngine.Framework.Services;
using WaveEngine.Dolby;
using WaveEngine.Common.Media;
using WaveEngine.Components.UI;
using WaveEngine.Framework.Diagnostic;
using WaveEngine.Framework.UI;


#endregion

namespace DolbyTest
{
    public class MyScene : Scene
    {
		private DolbyService dolbyService;
		
        protected override void CreateScene()
        {
            // this.Load(WaveContent.Scenes.MyScene);  
			WaveServices.ScreenContextManager.SetDiagnosticsActive(true);

			WaveServices.RegisterService (new DolbyService ());
			this.dolbyService = WaveServices.GetService<DolbyService> ();

			var music = new MusicInfo (WaveContent.Assets.met_mp3);
			WaveServices.MusicPlayer.Play (music);

			Button button1 = this.AddButton ("Enable Dolby", 100, 200);
			button1.Click += (s, o) =>
			{
				this.dolbyService.IsEnabled = true;
				this.UpdateLabels();
			};


			Button button2 = this.AddButton ("Disable Dolby", 300, 200);
			button2.Click += (s, o) =>
			{
				this.dolbyService.IsEnabled = false;
				this.UpdateLabels();
			};

			Button button3 = this.AddButton ("GAME Profile", 100, 500);
			button3.Click += (s, o) =>
			{
				this.dolbyService.DolbyProfile = DolbyProfile.GAME;
				this.UpdateLabels();
			};
				
			Button button4 = this.AddButton ("MOVIE Profile", 300, 500);
			button4.Click += (s, o) =>
			{
				this.dolbyService.DolbyProfile = DolbyProfile.MOVIE;
				this.UpdateLabels();
			};
			Button button5 = this.AddButton ("MUSIC Profile", 500, 500);
			button5.Click += (s, o) =>
			{
				this.dolbyService.DolbyProfile = DolbyProfile.MUSIC;
				this.UpdateLabels();
			};

			Button button6 = this.AddButton ("VOICE Profile", 700, 500);
			button6.Click += (s, o) =>
			{
				this.dolbyService.DolbyProfile = DolbyProfile.VOICE;
				this.UpdateLabels();
			};

			this.UpdateLabels();
        }

		private void UpdateLabels()
		{
			Labels.Add ("Dolby.IsEnabled", dolbyService.IsEnabled);
			Labels.Add ("Dolby.Profile", dolbyService.DolbyProfile);
		}

		private Button AddButton(string text, int xpos, int ypos)
		{
			Button button = new Button();
			button.Text = text;
			button.Width = 150;
			button.Height = 100;
			button.Margin = new Thickness(xpos, ypos, 0, 0);
			button.Foreground = Color.Black;
			button.BackgroundColor = Color.Blue;
			this.EntityManager.Add(button);

			return button;
		}
    }
}
