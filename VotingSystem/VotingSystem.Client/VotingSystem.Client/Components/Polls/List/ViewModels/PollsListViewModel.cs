using System;
using System.Collections.ObjectModel;
using Prism.Commands;
using Prism.Navigation;
using VotingSystem.Client.Components.Polls.Repositories;
using VotingSystem.Client.Components.ViewModels;
using VotingSystem.Domain.DTOs.Responses;

namespace VotingSystem.Client.Components.Polls.List.ViewModels
{
    public class PollsListViewModel : ViewModelBase
    {
        private readonly IPollsListRepository _pollsListRepository;
        private ObservableCollection<PollResponseDto> _polls;

        public PollsListViewModel(INavigationService navigationService, IPollsListRepository pollsListRepository) :
            base(navigationService)
        {
            _pollsListRepository = pollsListRepository;
            Title = "Consultas";
            PollSelectedCommand = new DelegateCommand<PollResponseDto>(PollSelected, _ => !IsBusy).ObservesProperty(() => IsBusy);
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
            Polls = new ObservableCollection<PollResponseDto>(await _pollsListRepository.GetPollsAsync());
        }

        private async void PollSelected(PollResponseDto poll)
        {
            IsBusy = true;
            var p = new NavigationParameters
            {
                {"id", poll.Address}
            };
            await NavigationService.NavigateAsync("PollsVotePage", p);
            IsBusy = false;
        }
    }
}