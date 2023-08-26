using LeoJHarris.FormsPlugin.Abstractions.Effects;
using Xamarin.Forms.Xaml;

namespace VotingSystem.Client.Components.Auth.Login.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage
    {
        public LoginPage()
        {
            InitializeComponent();

            PasswordEntry.Effects.Add(new ShowHiddenEntryEffect());
        }
    }
}