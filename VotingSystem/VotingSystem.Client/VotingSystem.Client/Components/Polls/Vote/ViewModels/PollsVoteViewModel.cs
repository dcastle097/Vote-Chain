using System.Collections.ObjectModel;
using System.Linq;
using Prism.Commands;
using Prism.Navigation;
using VotingSystem.Client.Components.NotificationPopup.Models;
using VotingSystem.Client.Components.Polls.Repositories;
using VotingSystem.Client.Components.ViewModels;
using VotingSystem.Client.Core.UI.Models;
using VotingSystem.Domain.DTOs.Responses;

namespace VotingSystem.Client.Components.Polls.Vote.ViewModels
{
    public class PollsVoteViewModel : ViewModelBase
    {
        private readonly IPollsListRepository _pollsListRepository;
        private ObservableCollection<SelectableItem> _options;
        private PollResponseDto _pollDetail;

        public PollsVoteViewModel(INavigationService navigationService, IPollsListRepository pollsListRepository) :
            base(navigationService)
        {
            _pollsListRepository = pollsListRepository;
            Title = "Consulta";
            VotePollCommand = new DelegateCommand(VotePoll);
        }

        public DelegateCommand VotePollCommand { get; set; }

        public ObservableCollection<SelectableItem> Options
        {
            get => _options;
            set => SetProperty(ref _options, value);
        }

        public PollResponseDto PollDetail
        {
            get => _pollDetail;
            set => SetProperty(ref _pollDetail, value);
        }

        private async void VotePoll()
        {
            IsBusy = true;
            var selected = Options.FirstOrDefault(o => o.IsSelected);
            if (selected == null)
            {
                await NavigationService.NavigateAsync("NotificationPopupPage",
                    new NavigationParameters
                    {
                        {
                            "details",
                            new NotificationDetails
                            {
                                Message = "Sebes seleccionar una opción", Title = "Lo sentimos",
                                Type = NotificationType.Warning
                            }
                        }
                    });
                IsBusy = false;
                return;
            }

            IsBusy = true;
            await NavigationService.NavigateAsync("NotificationPopupPage",
                new NavigationParameters
                {
                    {
                        "details", new NotificationDetails
                        {
                            Message = "Tu voto se ha registrado con éxito",
                            Title = "¡Gracias!",
                            Type = NotificationType.Success
                        }
                    }
                });
            IsBusy = false;
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (!parameters.ContainsKey("id")) return;

            PollDetail = await _pollsListRepository.GetPollDetailAsync(parameters.GetValue<string>("id"));
            Title = PollDetail.Title;
            Options = new ObservableCollection<SelectableItem>(PollDetail.Options.Select(o => new SelectableItem
            {
                Id = $"{o.Index}",
                Value = o.Value
            }));
        }
    }
}