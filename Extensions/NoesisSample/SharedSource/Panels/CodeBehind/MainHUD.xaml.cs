using NoesisSample.ViewModels;

#if PANELS_PROJECT
using System.Windows;
using System.Windows.Controls;
#else
using Noesis;
#endif

namespace NoesisSample
{
    /// <summary>
    /// Interaction logic for MainHUD.xaml
    /// </summary>
    public partial class MainHUD : UserControl
    {
        public MainHUD()
        {
#if PANELS_PROJECT
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
