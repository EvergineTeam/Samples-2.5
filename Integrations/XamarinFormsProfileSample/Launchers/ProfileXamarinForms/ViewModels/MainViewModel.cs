using System.Windows.Input;
using XamarinFormsProfileSample.Helpers;
using Xamarin.Forms;
using XamarinFormsProfileSampleXamarinForms.Models;
using System;
using XamarinFormsProfileSample.Events;
using XamarinFormsProfileSampleXamarinForms.Services;

namespace XamarinFormsProfileSample.ViewModels
{
    public class MainViewModel : BindableObject
    {
        private bool _isCamera1;
        private bool _isCamera2;
        private bool _isCamera3;

        public MainViewModel()
        {
            IsCamera1 = true;
            IsCamera2 = false;
            IsCamera3 = false;

            WaveEngineFacade.PushpinTapped += OnPushpinTapped;
        }

        public bool IsCamera1
        {
            get { return _isCamera1; }
            set
            {
                _isCamera1 = value;
                OnPropertyChanged("IsCamera1");
            }
        }

        public bool IsCamera2
        {
            get { return _isCamera2; }
            set
            {
                _isCamera2 = value;
                OnPropertyChanged("IsCamera2");
            }
        }

        public bool IsCamera3
        {
            get { return _isCamera3; }
            set
            {
                _isCamera3 = value;
                OnPropertyChanged("IsCamera3");
            }
        }

        public ICommand ChangeCameraCommand => new Command<string>(ChangeCamera);

        private void ChangeCamera(string camera)
        {
            var cameraType = Enum.Parse(typeof(CameraType), camera);

            IsCamera1 = false;
            IsCamera2 = false;
            IsCamera3 = false;

            switch ((CameraType)cameraType)
            {
                case CameraType.Camera1:
                    IsCamera1 = true;
                    break;
                case CameraType.Camera2:
                    IsCamera2 = true;
                    break;
                case CameraType.Camera3:
                    IsCamera3 = true;
                    break;
            }

            WaveEngineFacade.SetActiveCamera((int)cameraType);
        }

        private async void OnPushpinTapped(object sender, PushpinTappedEventArgs e)
        {
            await DialogService.Instance.ShowAlertAsync("Wave Engine Sample using Xamarin.Forms Profile.", "Wave Engine");
        }
    }
}
