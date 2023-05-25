using System;
using Prism.Commands;
using Prism.Navigation;
using VotingSystem.Client.Components.ViewModels;
using Xamarin.Forms;
using ZXing;

namespace VotingSystem.Client.Components.Auth.QrScanner.ViewModels
{
    public class QrScannerViewModel : ViewModelBase
    {
        private bool _isAnalyzing = true;
        private bool _isCompletingRegistration;

        private bool _isScanning = true;

        // TEST CODE

        private Result _result;

        public QrScannerViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        public Result Result
        {
            get => _result;
            set => SetProperty(ref _result, value);
        }

        public bool IsScanning
        {
            get => _isScanning;
            set => SetProperty(ref _isScanning, value);
        }

        public bool IsAnalyzing
        {
            get => _isAnalyzing;
            set => SetProperty(ref _isAnalyzing, value);
        }

        public Command QrScanResultCommand
        {
            get
            {
                return new Command(() =>
                {
                    IsAnalyzing = false;
                    IsScanning = false;

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Console.WriteLine("scan result :==============================:" + Result.Text);
                    });
                });
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (!parameters.ContainsKey("action")) return;

            _isCompletingRegistration = parameters.GetValue<string>("action") == "completeRegistration";

            Title = _isCompletingRegistration ? "Completar Registro" : "Iniciar sesión";
        }
    }
}