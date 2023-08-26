using Foundation;
using LeoJHarris.FormsPlugin.iOS;
using Prism;
using Prism.Ioc;
using Rg.Plugins.Popup;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms.Svg.iOS;
using Platform = ZXing.Net.Mobile.Forms.iOS.Platform;

namespace VotingSystem.Client.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Forms.Init();
            SvgImage.Init();
            Popup.Init();
            Platform.Init();
            EnhancedEntryRenderer.Init();

            LoadApplication(new App(new iOSInitializer()));

            return base.FinishedLaunching(app, options);
        }
    }

    public class iOSInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }
    }
}