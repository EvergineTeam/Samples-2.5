using System.Threading.Tasks;

namespace XamarinFormsProfileSampleXamarinForms.Services
{
    public class DialogService
    {
        private static DialogService _instance;

        public static DialogService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DialogService();
                }

                return _instance;
            }
        }

        public async Task ShowAlertAsync(string message, string title, string okButton = "Ok")
        {
            await Acr.UserDialogs.UserDialogs.Instance.AlertAsync(message, title, okButton);
        }
    }
}
