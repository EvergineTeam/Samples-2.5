using WaveEngine.Adapter;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
namespace LauncherWindowsStore
{
    public partial class MainPage : Page
    {
        private WaveEngine.Adapter.Application application;

        public MainPage()
        {
            InitializeComponent();

            application = new GameRenderer(this.SwapChainPanel);
            application.Initialize();

            // register size changed event
            Window.Current.SizeChanged += OnSizeChanged;
        }

        private void OnSizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            switch (ApplicationView.Value)
            {
                case ApplicationViewState.Filled:
                    VisualStateManager.GoToState(this, "Filled", false);
                    break;
                case ApplicationViewState.FullScreenLandscape:
                    VisualStateManager.GoToState(this, "FullScreenLandscape", false);
                    break;
                case ApplicationViewState.Snapped:
                    VisualStateManager.GoToState(this, "Snapped", false);
                    break;
                case ApplicationViewState.FullScreenPortrait:
                    VisualStateManager.GoToState(this, "FullScreenPortrait", false);
                    break;
                default:
                    break;
            }
        }
    }
}