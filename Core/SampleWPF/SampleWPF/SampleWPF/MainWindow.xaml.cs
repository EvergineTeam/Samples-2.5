using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SampleWPFProject;

namespace SampleWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            this.waveCanvas.GameLoaded += this.OnGameLoaded;
        }

        /// <summary>
        /// Called when [game loaded].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="game">The game.</param>
        private void OnGameLoaded(object sender, Game game)
        {
            var viewModel = this.DataContext as SolarSystemViewModel;
            viewModel.Initialize(game);
        }
    }
}
