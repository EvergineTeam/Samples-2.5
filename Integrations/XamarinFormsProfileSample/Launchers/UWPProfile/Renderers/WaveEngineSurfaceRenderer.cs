using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using WaveEngine.Adapter;
using XamarinFormsProfileSample.Controls;
using XamarinFormsProfileSample.UWP.Renderers;

[assembly: ExportRendererAttribute(typeof(WaveEngineSurface), typeof(WaveEngineSurfaceRenderer))]
namespace XamarinFormsProfileSample.UWP.Renderers
{
    public class WaveEngineSurfaceRenderer : ViewRenderer<WaveEngineSurface, Canvas>
    {
        private Canvas _gameView;
        private SwapChainPanel _swapChainPanel;
        private WaveApplication _gameContext;

        protected override void OnElementChanged(ElementChangedEventArgs<WaveEngineSurface> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                _gameView = new Canvas();
                _swapChainPanel = new SwapChainPanel();
                _gameView.Children.Add(_swapChainPanel);
                Canvas.SetTop(_swapChainPanel, 0);
                Canvas.SetLeft(_swapChainPanel, 0);
                _gameContext = new WaveApplication(_swapChainPanel);

                SetNativeControl(_gameView);
                Element.InputTransparent = false;

                Element.InputTransparent = false;
                Element.SizeChanged += Element_SizeChanged;
            }
        }

        private void Element_SizeChanged(object sender, EventArgs e)
        {
            double h = ((VisualElement)sender).Height;
            double w = ((VisualElement)sender).Width;

            if (h > 0 && w > 0)
            {
                _swapChainPanel.Height = h;
                _swapChainPanel.Width = w;
                ((Adapter)_gameContext.Adapter).Width = (int)w;
                ((Adapter)_gameContext.Adapter).Height = (int)h;

                RectangleGeometry rectClip = new RectangleGeometry();
                rectClip.Rect = new Windows.Foundation.Rect(0, 0, w, h);
                _gameView.Clip = rectClip;
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName.Equals("Game", System.StringComparison.CurrentCultureIgnoreCase))
            {
                InitGame();
            }
        }

        private void InitGame()
        {
            _gameContext.Initialize(Element.Game);
        }
    }
}