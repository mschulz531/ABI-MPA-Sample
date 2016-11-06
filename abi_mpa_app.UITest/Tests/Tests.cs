using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace abi_mpa_app.UITest
{
    public class Tests : AbstractSetup
    {
        //Test methods only call into page classes.
        //They do not directly interact with the app

        public Tests(Platform platform)
            : base(platform)
        {
        }

        [Test]
        public void AppLaunches()
        {
            new HomePage()
                .DismissSignInAlert();
        }

        [Test]
        [TestCase("Xamarin UITest Message")]
        public void SendAndCompleteMessage(string text)
        {
            // Adding a unique suffix so devices can work together in the cloud
            var suffix = OnAndroid ? "A" : "I";
            suffix += Environment.GetEnvironmentVariable("XTC_DEVICE_INDEX") ?? "L";
            text += " " + suffix;

            new HomePage()
                .DismissSignInAlert()
                .SendMessage(text)
                .LocateMessage(text)
                .CompleteMessage(text);
        }
    }
}
