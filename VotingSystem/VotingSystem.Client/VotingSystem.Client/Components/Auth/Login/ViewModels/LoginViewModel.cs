using System;
using Flurl.Http;
using Prism;
using Prism.Commands;
using Prism.Navigation;
using VotingSystem.Client.Components.Auth.Repositories;
using VotingSystem.Client.Components.NotificationPopup.Models;
using VotingSystem.Client.Components.NotificationPopup.Views;
using VotingSystem.Client.Components.Polls.List.Views;
using VotingSystem.Client.Components.ViewModels;
using VotingSystem.Client.Core.Exceptions;

namespace VotingSystem.Client.Components.Auth.Login.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly IAuthRepository _authRepository;

        private string _password = string.Empty;
        private string _userAccountId;

        public LoginViewModel(INavigationService navigationService, IAuthRepository authRepository) : base(
            navigationService)
        {
            _authRepository = authRepository;
            LoginCommand = new DelegateCommand(Login);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public DelegateCommand LoginCommand { get; set; }

        private async void Login()
        {
            if(IsBusy) return;
            
            IsBusy = true;
            await NavigationService.NavigateAsync(nameof(ActivityPopup.Views.ActivityPopup));

            var notificationTitle = string.Empty;
            var notificationMessage = string.Empty;
            var notificationType = NotificationType.Error;

            try
            {
                var userExists = await _authRepository.UserExists(_userAccountId, _password);
                if (userExists)
                {
                    PrismApplicationBase.Current.Properties["UserAccountId"] = _userAccountId;
                    PrismApplicationBase.Current.Properties["UserAccountPassword"] = _password;
                    IsBusy = false;
                    await NavigationService.NavigateAsync($"app:///NavigationPage/{nameof(PollsListPage)}");
                    return;
                }

                notificationMessage =
                    "No hemos encontrado un usuario que coincida con la información suministrada en nuestro sistema.";
                notificationTitle = "Error en la autenticación";
                notificationType = NotificationType.Warning;
            }
            catch (ValidationException e)
            {
                notificationMessage = e.Message;
                notificationTitle = "Error de validación";
                notificationType = NotificationType.Warning;

                IsBusy = false;
                await NavigationService.GoBackAsync();
                await NavigationService.NavigateAsync(nameof(NotificationPopupPage),
                    new NavigationParameters
                    {
                        {
                            "details", new NotificationDetails
                            {
                                Message = notificationMessage,
                                Title = notificationTitle,
                                Type = notificationType
                            }
                        }
                    });
                return;
            }
            catch (FlurlHttpTimeoutException)
            {
                notificationMessage = "El servidor no responde, por favor intente más tarde.";
                notificationTitle = "Error de conexión";
            }
            catch (FlurlHttpException e)
            {
                switch (e.StatusCode)
                {
                    case 401:
                        notificationMessage =
                            "No hemos encontrado un usuario que coincida con la información suministrada en nuestro sistema.";
                        notificationTitle = "Error en la autenticación";
                        notificationType = NotificationType.Warning;
                        break;
                    default:
                        notificationMessage = "Ha ocurrido un error en el servidor, por favor intente más tarde.";
                        notificationTitle = "Error";
                        break;
                }
            }
            catch (Exception)
            {
                notificationMessage = "Ha ocurrido un error, por favor intente más tarde.";
                notificationTitle = "Error";
            }
            
            IsBusy = false;
            if (string.IsNullOrWhiteSpace(notificationMessage)) return;
            
            await NavigationService.NavigateAsync($"app:///NavigationPage/{nameof(MainPage)}");
            await NavigationService.NavigateAsync(nameof(NotificationPopupPage),
                new NavigationParameters
                {
                    {
                        "details", new NotificationDetails
                        {
                            Message = notificationMessage,
                            Title = notificationTitle,
                            Type = notificationType
                        }
                    }
                });
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            _userAccountId = parameters.GetValue<string>("userAccountId");

            Title = "Iniciar Sesión";
        }
    }
}