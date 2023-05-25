using Prism.Navigation;
using VotingSystem.Client.Components.NotificationPopup.Models;
using VotingSystem.Client.Components.ViewModels;

namespace VotingSystem.Client.Components.NotificationPopup.ViewModels
{
    public class NotificationPopupViewModel : ViewModelBase
    {
        private NotificationDetails _details;

        public NotificationDetails Details
        {
            get => _details;
            set => SetProperty(ref _details, value);
        }

        public NotificationPopupViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (!parameters.ContainsKey("details")) return;

            Details = parameters.GetValue<NotificationDetails>("details");
        }
    }
}