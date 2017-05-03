using WaveEngine.Common;
using Xamarin.Forms;

namespace XamarinFormsProfileSample.Controls
{
    public class WaveEngineSurface : View, IViewController
    {
        public static readonly BindableProperty GameProperty = BindableProperty.Create(
          propertyName: "Game",
          returnType: typeof(IGame),
          declaringType: typeof(WaveEngineSurface),
          defaultValue: null);

        public IGame Game
        {
            get { return (IGame)GetValue(GameProperty); }
            set { SetValue(GameProperty, value); }
        }
    }
}