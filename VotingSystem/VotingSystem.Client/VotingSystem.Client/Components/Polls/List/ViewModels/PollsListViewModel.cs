using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Prism;
using Prism.Commands;
using Prism.Navigation;
using VotingSystem.Client.Components.NotificationPopup.Models;
using VotingSystem.Client.Components.NotificationPopup.Views;
using VotingSystem.Client.Components.Polls.Repositories;
using VotingSystem.Client.Components.Polls.Vote.Views;
using VotingSystem.Client.Components.ViewModels;
using VotingSystem.Domain.DTOs.Responses;

namespace VotingSystem.Client.Components.Polls.List.ViewModels
{
    public class PollsListViewModel : ViewModelBase
    {
        private readonly IPollsRepository _pollsRepository;
        private ObservableCollection<PollResponseDto> _polls;
        private int _callsCounter = 0;

        public PollsListViewModel(INavigationService navigationService, IPollsRepository pollsRepository) :
            base(navigationService)
        {
            _pollsRepository = pollsRepository;
            Title = "Consultas";
            PollSelectedCommand =
                new DelegateCommand<PollResponseDto>(PollSelected, _ => !IsBusy).ObservesProperty(() => IsBusy);
        }

        public ObservableCollection<PollResponseDto> Polls
        {
            get => _polls;
            set => SetProperty(ref _polls, value);
        }

        public DelegateCommand<PollResponseDto> PollSelectedCommand { get; }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if(parameters.GetNavigationMode() == NavigationMode.Back) return;
            await LoadPolls();
        }

        public int CallsCounter
        {
            get => _callsCounter;
            set => SetProperty(ref _callsCounter, value);
        }

        private async Task LoadPolls()
        {
            if(IsBusy) return;
            
            await NavigationService.NavigateAsync(nameof(ActivityPopup.Views.ActivityPopup));
            CallsCounter += 1;
            IsBusy = true;
            NotificationDetails notificationDetails = null;
            try
            {
                var userId = PrismApplicationBase.Current.Properties["UserAccountId"] as string;
                var polls = await _pollsRepository.GetPollsAsync(userId);
                Polls = new ObservableCollection<PollResponseDto>(polls);
            }
            catch (Exception e)
            {
                notificationDetails = new NotificationDetails
                {
                    Message = "No tienes ninguna consulta asignada",
                    Title = "Sin datos",
                    Type = NotificationType.Warning
                };
            }
            IsBusy = false;
            await NavigationService.GoBackAsync();
            if (notificationDetails != null)
            {
                await NavigationService.NavigateAsync(nameof(NotificationPopupPage), new NavigationParameters{{"details", notificationDetails}});
            }
        }

        private async void PollSelected(PollResponseDto poll)
        {
            IsBusy = true;
            var p = new NavigationParameters
            {
                { "id", poll.Address }
            };
            await NavigationService.NavigateAsync(nameof(PollsVotePage), p);
            IsBusy = false;
        }
    }
}