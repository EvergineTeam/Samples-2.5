
#if PANELS_PROJECT
using System.Windows.Controls;
#else
using Noesis;
#endif

namespace NoesisSample
{
    /// <summary>
    /// Interaction logic for MainHUD.xaml
    /// </summary>
    public partial class PlanetHUD : UserControl
    {
        public PlanetHUD()
        {
#if PANELS_PROJECT
            InitializeComponent();
#else
            Noesis.GUI.LoadComponent(this, WaveContent.Assets.Xaml.PlanetHUD_xaml);
#endif
        }
    }
}
