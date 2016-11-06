using System;
using Xamarin.UITest;

namespace abi_mpa_app.UITest
{
    static class AppInitializer
    {
        //For more information on this pattern, please refer to:
        //https://github.com/xamarin-automation-service/uitest-pop-example

        //Save App as property to allow page objects to access instance
        static IApp app;
        public static IApp App
        {
            get
            {
                if (app == null)
                    throw new NullReferenceException("'AppInitializer.App' not set. Call 'AppInitializer.StartApp(platform)' before trying to access it.");
                return app;
            }
        }

        public static IApp StartApp(Platform platform)
        {
            //App located by the "Test Apps" selection in the Unit Test Pad
            //Other options to connect to app shown below

            if (platform == Platform.Android)
            {
                app = ConfigureApp
                    .Android
                    //.ApkFile("../../../path/to/.apk")
                    .StartApp();
            }
            else
            {
                app = ConfigureApp
                    .iOS
                    //.AppBundle("../../../path/to/.app")
                    //.InstalledApp("your.bundle.id")
                    .StartApp();
            }

            return app;
        }
    }
}