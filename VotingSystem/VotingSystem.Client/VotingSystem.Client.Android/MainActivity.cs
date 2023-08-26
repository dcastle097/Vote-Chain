using Android.App;
using Android.Content.PM;
using Android.OS;
using AndroidX.AppCompat.App;
using LeoJHarris.FormsPlugin.Droid;
using Prism;
using Prism.Ioc;
using Prism.Plugin.Popups;
using Rg.Plugins.Popup;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Svg.Droid;
using ZXing.Net.Mobile.Android;
using Platform = Xamarin.Essentials.Platform;

namespace VotingSystem.Client.Droid
{
    [Activity(Label = "Voting System", Theme = "@style/MainTheme",
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            AppCompatDelegate.DefaultNightMode = AppCompatDelegate.ModeNightNo;
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Forms.Init(this, savedInstanceState);
            SvgImage.Init(this);
            Popup.Init(this);
            Platform.Init(this, savedInstanceState);
            ZXing.Net.Mobile.Forms.Android.Platform.Init();
            EnhancedEntryRenderer.Init(this);

            LoadApplication(new App(new AndroidInitializer()));
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions,
            Permission[] grantResults)
        {
            PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public override void OnBackPressed()
        {
            PopupPlugin.OnBackPressed();
        }
    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }
    }
}