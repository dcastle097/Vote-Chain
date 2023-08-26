using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Prism;
using Prism.Commands;
using Prism.Navigation;
using VotingSystem.Client.Components.NotificationPopup.Models;
using VotingSystem.Client.Components.NotificationPopup.Views;
using VotingSystem.Client.Components.Polls.Repositories;
using VotingSystem.Client.Components.ViewModels;
using VotingSystem.Client.Core.UI.Models;
using VotingSystem.Domain.DTOs.Responses;

namespace VotingSystem.Client.Components.Polls.Vote.ViewModels
{
    public class PollsVoteViewModel : ViewModelBase
    {
        private readonly IPollsRepository _pollsRepository;
        private ObservableCollection<SelectableItem> _options;
        private PollResponseDto _pollDetail;
        private string _pollAddress;
        private bool _isActive;
        private bool _isExpired;
        private bool _isPending;

        public PollsVoteViewModel(INavigationService navigationService, IPollsRepository pollsRepository) :
            base(navigationService)
        {
            _pollsRepository = pollsRepository;
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

        public bool IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value);
        }

        public bool IsExpired
        {
            get => _isExpired;
            set => SetProperty(ref _isExpired, value);
        }

        public bool IsPending
        {
            get => _isPending;
            set => SetProperty(ref _isPending, value);
        }

        private async void VotePoll()
        {
            IsBusy = true;
            var selected = Options.FirstOrDefault(o => o.IsSelected);
            if (selected == null)
            {
                await NavigationService.NavigateAsync(nameof(NotificationPopupPage),
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
            var userId = PrismApplicationBase.Current.Properties["UserAccountId"] as string;
            var userPass = PrismApplicationBase.Current.Properties["UserAccountPassword"] as string;
            
            byte.TryParse(selected.Id, out var option);

            var voteWasSuccessful = await _pollsRepository.CastVoteAsync(PollDetail.Address, option, userId, userPass);

            if (voteWasSuccessful)
            {
                await NavigationService.NavigateAsync(nameof(NotificationPopupPage),
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
            }

            IsBusy = false;
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (!parameters.ContainsKey("id")) return;

            _pollAddress = parameters.GetValue<string>("id");
            await LoadPoll();
        }

        private async Task LoadPoll()
        {
            if(IsBusy) return;

            IsBusy = true;
            await NavigationService.NavigateAsync(nameof(ActivityPopup.Views.ActivityPopup));

            try
            {
                PollDetail = await _pollsRepository.GetPollDetailAsync(_pollAddress);
                IsActive = PollDetail.StartDate <= DateTime.UtcNow && PollDetail.EndDate >= DateTime.UtcNow;
                IsExpired = PollDetail.EndDate < DateTime.UtcNow;
                IsPending = PollDetail.StartDate > DateTime.UtcNow;
                
                Title = PollDetail.Title;
                Options = new ObservableCollection<SelectableItem>(PollDetail.Options.Select(o => new SelectableItem
                {
                    Id = $"{o.Index}",
                    Value = o.Value
                }));
            }
            catch (Exception e)
            {
                await NavigationService.GoBackAsync();
                await NavigationService.NavigateAsync(nameof(NotificationPopupPage),
                    new NavigationParameters
                    {
                        {
                            "details", new NotificationDetails
                            {
                                Message = "Ha ocurrido un error",
                                Title = "Error",
                                Type = NotificationType.Error
                            }
                        }
                    });
            }
            finally
            {
                IsBusy = false;
                await NavigationService.GoBackAsync();
            }
        }
    }
}