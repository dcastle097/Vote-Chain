using Prism.Navigation;
using VotingSystem.Client.Components.Auth.Login.Views;
using VotingSystem.Client.Components.Auth.Register.Views;
using VotingSystem.Client.Components.ViewModels;
using Xamarin.Forms;
using ZXing;

namespace VotingSystem.Client.Components.Auth.QrScanner.ViewModels
{
    public class QrScannerViewModel : ViewModelBase
    {
        private bool _isAnalyzing = true;
        private bool _isCompletingRegistration;
        private bool _isLogin;
        private bool _isScanning = true;
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

                    Device.BeginInvokeOnMainThread(OperateResult);
                });
            }
        }

        private void OperateResult()
        {
            if (_isCompletingRegistration)
            {
                var qrInfo = Result.Text.Split(';');
                if (qrInfo.Length == 2)
                {
                    NavigationService.NavigateAsync(nameof(RegisterPage),
                        new NavigationParameters { { "userId", qrInfo[0] }, { "createdAt", qrInfo[1] } });
                    return;
                }
            }

            if (_isLogin && !string.IsNullOrWhiteSpace(Result.Text))
            {
                NavigationService.NavigateAsync(nameof(LoginPage),
                    new NavigationParameters { { "userAccountId", Result.Text } });
                return;
            }

            IsAnalyzing = true;
            IsScanning = true;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (!parameters.ContainsKey("action")) return;

            _isCompletingRegistration = parameters.GetValue<string>("action") == "completeRegistration";
            _isLogin = parameters.GetValue<string>("action") == "login";

            Title = _isCompletingRegistration ? "Completar Registro" : "Iniciar sesión";
        }
    }
}