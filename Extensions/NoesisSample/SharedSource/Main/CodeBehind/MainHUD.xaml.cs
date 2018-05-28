using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoesisWPFLibrary;
using System.Windows.Input;

#if WPF
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
#else
using Noesis;
using NoesisSample;
#endif

namespace NoesisWPF
{
    /// <summary>
    /// Interaction logic for MainHUD.xaml
    /// </summary>
    public partial class MainHUD : UserControl
    {
        public MainHUD()
        {
#if WPF
            InitializeComponent();
#else
            Noesis.GUI.LoadComponent(this, WaveContent.Assets.Xaml.MainHUD_xaml);
#endif
            StateManager.Instance.StateChanging += OnStateChanged;

            this.DataContext = new MainPanelVM();

            this.Initialized += delegate
            {
                VisualStateManager.GoToState(this, "CuriositySelect", true);
            };
        }

        private void OnStateChanged(object sender, MissionEnum e)
        {
            switch (e)
            {
                case MissionEnum.Curiosity:
                    VisualStateManager.GoToState(this, "CuriositySelect", true);
                    break;
                case MissionEnum.Spirit:
                    VisualStateManager.GoToState(this, "SpiritSelect", true);
                    break;
                case MissionEnum.MarsGlobarSurveyor:
                    VisualStateManager.GoToState(this, "MGOSelect", true);
                    break;
                default:
                    break;
            }
        }
    }
}
