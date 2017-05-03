using Xamarin.Forms;
using XamarinFormsProfileSample.Views;

namespace XamarinFormsProfileSample
{
	public partial class App : Application
	{
		private static Game _game;

        public static Game Game
        {
            get { return _game; }
            set { _game = value; }
        }
		
		public App ()
		{
			InitializeComponent();
			
			MainPage = new NavigationPage(new MainView());
		}
 
		private void InitGame()
        {
            _game = new Game();
        }

        protected override void OnStart ()
		{
            InitGame();
        }
		
		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
