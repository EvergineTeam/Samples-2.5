
#if PANELS_PROJECT
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
#else
using Noesis;
#endif

namespace NoesisSample
{
    /// <summary>
    /// Interaction logic for MainHUD.xaml
    /// </summary>
    public partial class MissionPanel : UserControl
    {
        private Storyboard show;
        private Storyboard hide;

        public MissionPanel()
        {
#if PANELS_PROJECT
            InitializeComponent();
#else
            Noesis.GUI.LoadComponent(this, WaveContent.Assets.Xaml.MissionPanel_xaml);
#endif
            this.show = this.FindResource("show") as Storyboard;
            this.hide = this.FindResource("hide") as Storyboard;
        }

        public void ShowPanel()
        {
            this.show.Begin();
        }

        public void HidePanel()
        {
            this.hide.Begin();
        }
    }
}
