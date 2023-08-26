using System;
using Flurl.Http;
using Prism.Commands;
using Prism.Navigation;
using VotingSystem.Client.Components.Auth.Repositories;
using VotingSystem.Client.Components.NotificationPopup.Models;
using VotingSystem.Client.Components.NotificationPopup.Views;
using VotingSystem.Client.Components.ViewModels;
using VotingSystem.Client.Core.Exceptions;

namespace VotingSystem.Client.Components.Auth.Register.ViewModels
{
    public class RegisterViewModel : ViewModelBase
    {
        private readonly IAuthRepository _authRepository;

        private long _createdAt;
        private string _password = string.Empty;
        private string _passwordConfirmation = string.Empty;
        private Guid _userId;

        public RegisterViewModel(INavigationService navigationService, IAuthRepository authRepository) : base(
            navigationService)
        {
            _authRepository = authRepository;

            RegisterCommand = new DelegateCommand(CompleteRegistration);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public string PasswordConfirmation
        {
            get => _passwordConfirmation;
            set => SetProperty(ref _passwordConfirmation, value);
        }

        public DelegateCommand RegisterCommand { get; set; }

        private async void CompleteRegistration()
        {
            if(IsBusy) return;
            
            IsBusy = true;
            await NavigationService.NavigateAsync(nameof(ActivityPopup.Views.ActivityPopup));

            string notificationTitle;
            string notificationMessage;
            var notificationType = NotificationType.Error;

            try
            {
                await _authRepository.CompleteRegistration(_userId, _createdAt, _password, _passwordConfirmation);
                notificationMessage =
                    "El registro se ha completado con éxito, para iniciar sesión, debe usar el código QR que se envío a su correo electrónico.";
                notificationTitle = "¡Bienvenido!";
                notificationType = NotificationType.Success;
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
                var responseBody = await e.GetResponseStringAsync();
                switch (e.StatusCode)
                {
                    case 400:
                        notificationMessage = "El código QR no es válido.";
                        notificationTitle = "Error de validación";
                        break;
                    case 403:
                        notificationMessage = "Para iniciar sesión, debe usar el código QR que se envío a su correo electrónico previamente.";
                        notificationTitle = "Registro existente";
                        notificationType = NotificationType.Warning;
                        break;
                    case 404:
                        notificationMessage = "La información no coincide con la registrada en nuestro sistema, por favor póngase con el registrador.";
                        notificationTitle = "Usuario no registrado";
                        break;
                    default:
                        notificationMessage = "Ha ocurrido un error, por favor intente más tarde.";
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
            await NavigationService.GoBackAsync();
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

            _userId = Guid.Parse(parameters.GetValue<string>("userId"));
            _createdAt = long.Parse(parameters.GetValue<string>("createdAt"));

            Title = "Completar Registro";
        }
    }
}