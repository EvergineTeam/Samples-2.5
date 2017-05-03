using Android.App;
using System;
using System.ComponentModel;
using WaveEngine.Adapter;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XamarinFormsProfileSample.Droid;
using XamarinFormsProfileSample.Droid.Renderers;
using XamarinFormsProfileSample.Controls;

[assembly: ExportRendererAttribute(typeof(WaveEngineSurface), typeof(WaveEngineSurfaceRenderer))]
namespace XamarinFormsProfileSample.Droid.Renderers
{
    public class WaveEngineSurfaceRenderer : ViewRenderer<WaveEngineSurface, GLView>
    {
        private GLView _gameView;

        protected override void OnElementChanged(ElementChangedEventArgs<WaveEngineSurface> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                _gameView = null;
            }

            if (e.NewElement != null)
            {
                _gameView = new GLView(Context, null);
                Activity activity = Context as Activity;
                SetNativeControl(_gameView);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName.Equals("Game", StringComparison.CurrentCultureIgnoreCase))
            {
                InitGame();
            }
        }

        private void InitGame()
        {
            MainActivity activity = Context as MainActivity;
            activity.Initialize(Element.Game);
        }
    }
}