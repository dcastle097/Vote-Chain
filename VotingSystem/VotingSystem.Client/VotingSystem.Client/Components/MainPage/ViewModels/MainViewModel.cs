using System.Threading.Tasks;
using Prism.Commands;
using Prism.Navigation;
using VotingSystem.Client.Components.Auth.QrScanner.Views;
using VotingSystem.Client.Components.ViewModels;

namespace VotingSystem.Client.Components.MainPage.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel(INavigationService navigationService) : base(navigationService)
        {
            LoginCommand = new DelegateCommand(GoToLogin);
            RegisterCommand = new DelegateCommand(GoToRegistration);
        }

        public DelegateCommand LoginCommand { get; set; }

        public DelegateCommand RegisterCommand { get; set; }

        private void GoToRegistration()
        {
            NavigationService.NavigateAsync(nameof(QrScannerPage),
                new NavigationParameters { { "action", "completeRegistration" } });
        }

        private void GoToLogin()
        {
            NavigationService.NavigateAsync(nameof(QrScannerPage),
                new NavigationParameters { { "action", "login" } });
        }
    }
}