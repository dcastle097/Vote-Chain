using Xamarin.Forms.Xaml;

namespace VotingSystem.Client.Components.ActivityPopup.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActivityPopup
    {
        public ActivityPopup()
        {
            InitializeComponent();
            CloseWhenBackgroundIsClicked = false;
        }
    }
}