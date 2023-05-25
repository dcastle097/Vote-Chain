using Prism.Mvvm;
using Prism.Navigation;

namespace VotingSystem.Client.Components.ViewModels
{
    public class ViewModelBase : BindableBase, INavigatedAware, IDestructible
    {
        private string _title;
        private bool _isBusy;

        public ViewModelBase(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }
        
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public INavigationService NavigationService { get; }

        public void Destroy()
        {
        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
        }
    }
}