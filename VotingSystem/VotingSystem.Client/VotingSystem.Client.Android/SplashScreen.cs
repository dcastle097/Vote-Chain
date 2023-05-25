using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;

namespace VotingSystem.Client.Droid
{
    [Activity(Label = "Voting System", Theme = "@style/VotingSystemTheme", MainLauncher = true,
        NoHistory = true, ConfigurationChanges = ConfigChanges.ScreenSize)]
    public class SplashScreen : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Window?.AddFlags(WindowManagerFlags.Fullscreen);
            Window?.ClearFlags(WindowManagerFlags.ForceNotFullscreen);
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }
    }
}