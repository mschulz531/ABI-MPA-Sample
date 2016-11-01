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
        public Tests(Platform platform)
            : base(platform)
        {
        }

        [Test]
        public void AppLaunches()
        {
            new HomePage();
        }
    }
}
