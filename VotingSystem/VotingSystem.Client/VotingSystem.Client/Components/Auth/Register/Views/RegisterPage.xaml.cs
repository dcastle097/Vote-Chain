using LeoJHarris.FormsPlugin.Abstractions.Effects;
using Xamarin.Forms.Xaml;

namespace VotingSystem.Client.Components.Auth.Register.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage
    {
        public RegisterPage()
        {
            InitializeComponent();

            PasswordEntry.NextEntry = PasswordConfirmationEntry;
            PasswordEntry.Effects.Add(new ShowHiddenEntryEffect());

            PasswordConfirmationEntry.Effects.Add(new ShowHiddenEntryEffect());
        }
    }
}