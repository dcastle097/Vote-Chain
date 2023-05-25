using Prism.Commands;
using Prism.Navigation;
using VotingSystem.Client.Components.ViewModels;

namespace VotingSystem.Client.Components.MainPage.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel(INavigationService navigationService) : base(navigationService)
        {
            GoToPollsCommand = new DelegateCommand(GoToPolls);
            GoToScannerCommand = new DelegateCommand(GoToScanner);
        }

        public DelegateCommand GoToPollsCommand { get; set; }

        public DelegateCommand GoToScannerCommand { get; set; }

        private void GoToScanner()
        {
            NavigationService.NavigateAsync("QrScannerPage",
                new NavigationParameters {{"action", "completeRegistration"}});
        }

        private void GoToPolls()
        {
            NavigationService.NavigateAsync("app:///NavigationPage/PollsListPage");
        }
    }
}