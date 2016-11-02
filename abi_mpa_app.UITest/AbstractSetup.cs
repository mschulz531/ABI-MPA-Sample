using System;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Android;
using Xamarin.UITest.iOS;

namespace abi_mpa_app.UITest
{
    [TestFixture(Platform.Android)]
    [TestFixture(Platform.iOS)]
    public abstract class AbstractSetup
    {
        protected IApp app;
        protected Platform platform;

        protected bool OnAndroid { get; set; }
        protected bool OniOS { get; set; }

        protected AbstractSetup(Platform platform)
        {
            //platform is passed in from the TestFixture values
            this.platform = platform;
        }

        [SetUp]
        public virtual void BeforeEachTest()
        {
            //Start are app instance
            app = AppInitializer.StartApp(platform);

            //Save booleans based on platform
            OnAndroid = app.GetType() == typeof(AndroidApp);
            OniOS = app.GetType() == typeof(iOSApp);

            LogIn();
        }

        /// <summary>
        /// Logs user into application
        /// </summary>
        protected void LogIn()
        {
            //Page objects are used for code re-use and sharing between platforms
            new SignInPage()
                .SignIn();

            new EnterCredentialsPage()
                .EnterCredentials("michael.schultz@anheuser-busch.com", "Beer1234")
                .Submit();
        }
    }
}