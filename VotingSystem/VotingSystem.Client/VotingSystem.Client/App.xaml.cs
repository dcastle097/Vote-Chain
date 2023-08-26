using System;
using System.Threading.Tasks;
using Prism;
using Prism.Ioc;
using Prism.Plugin.Popups;
using VotingSystem.Client.Components.ActivityPopup.Views;
using VotingSystem.Client.Components.Auth.Login.ViewModels;
using VotingSystem.Client.Components.Auth.Login.Views;
using VotingSystem.Client.Components.Auth.QrScanner.ViewModels;
using VotingSystem.Client.Components.Auth.QrScanner.Views;
using VotingSystem.Client.Components.Auth.Register.ViewModels;
using VotingSystem.Client.Components.Auth.Register.Views;
using VotingSystem.Client.Components.Auth.Repositories;
using VotingSystem.Client.Components.MainPage.ViewModels;
using VotingSystem.Client.Components.MainPage.Views;
using VotingSystem.Client.Components.NotificationPopup.ViewModels;
using VotingSystem.Client.Components.NotificationPopup.Views;
using VotingSystem.Client.Components.Polls.List.ViewModels;
using VotingSystem.Client.Components.Polls.List.Views;
using VotingSystem.Client.Components.Polls.Repositories;
using VotingSystem.Client.Components.Polls.Vote.ViewModels;
using VotingSystem.Client.Components.Polls.Vote.Views;
using Xamarin.Forms;
using Xamarin.Forms.Svg;

namespace VotingSystem.Client
{
    public partial class App
    {
        public App() : this(null)
        {
        }

        public App(IPlatformInitializer initializer) : base(initializer)
        {
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterPopupNavigationService();
            containerRegistry.RegisterPopupDialogService();
            containerRegistry.RegisterForNavigation<NavigationPage>();

            #region AuthPages

            containerRegistry
                .RegisterForNavigation<MainPage,
                    MainViewModel>();
            containerRegistry
                .RegisterForNavigation<QrScannerPage,
                    QrScannerViewModel>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginViewModel>();
            containerRegistry.RegisterForNavigation<RegisterPage, RegisterViewModel>();

            #endregion

            #region PollPages

            containerRegistry
                .RegisterForNavigation<PollsListPage,
                    PollsListViewModel>();
            containerRegistry
                .RegisterForNavigation<PollsVotePage,
                    PollsVoteViewModel>();

            #endregion

            #region Popups
            
            containerRegistry
                .RegisterForNavigation<ActivityPopup>();
            containerRegistry
                .RegisterForNavigation<NotificationPopupPage,
                    NotificationPopupViewModel>();

            #endregion

            #region Repositories

            containerRegistry.Register<IAuthRepository, AuthRepository>();
            containerRegistry.Register<IPollsRepository, PollsRepository>();

            #endregion
        }

        protected override async void OnInitialized()
        {
            try
            {
                TaskScheduler.UnobservedTaskException += (sender, e) => { Console.WriteLine(e.Exception.ToString()); };
                InitializeComponent();
                SvgImageSource.RegisterAssembly();
                await NavigationService.NavigateAsync($"app:///NavigationPage/{nameof(MainPage)}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        protected override void OnResume()
        {
            this.PopupPluginOnResume();
        }

        protected override void OnSleep()
        {
            this.PopupPluginOnSleep();
        }
    }
}