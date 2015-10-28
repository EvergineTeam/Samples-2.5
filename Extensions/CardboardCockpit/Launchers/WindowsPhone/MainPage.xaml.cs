using System.Security;
using Microsoft.Phone.Controls;
using System.Windows.Controls;
using System.Windows.Media;
using WaveEngine.Adapter;

namespace CardboardCockpit
{
    public partial class MainPage : PhoneApplicationPage
    {
        private Application application;

        [SecuritySafeCritical]
        public MainPage()
        {
            InitializeComponent();

            MediaElement media = new MediaElement();
            DrawingSurface.Children.Add(media);

            application = new GameRenderer(new Windows.Foundation.Size(App.Current.Host.Content.ActualWidth, App.Current.Host.Content.ActualHeight), 100, media);

            DrawingSurface.SetBackgroundContentProvider(application.ContentProvider);
            DrawingSurface.SetBackgroundManipulationHandler(application.ManipulationHandler);
        }

        protected override void OnOrientationChanged(OrientationChangedEventArgs e)
        {
            base.OnOrientationChanged(e);
        }
    }
}