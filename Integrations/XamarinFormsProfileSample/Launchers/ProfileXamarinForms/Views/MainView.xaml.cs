using Xamarin.Forms;
using XamarinFormsProfileSample.ViewModels;

namespace XamarinFormsProfileSample.Views
{
	public partial class MainView : ContentPage
	{
		public MainView()
		{
			InitializeComponent();
			
			BindingContext = new MainViewModel();
		}  
		
		protected override void OnAppearing()
        {
            base.OnAppearing();

            WaveEngineSurface.Game = App.Game;

            ForceLayout();
        }
	}
}