using System;
using Xamarin.UITest;

using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace abi_mpa_app.UITest
{
    public class HomePage : BasePage
    {
        readonly Query signInResultAlert;

        public HomePage()
            : base(x => x.Class("ConditionalFocusLayout"), null)
        {
            if (OnAndroid)
            {
                signInResultAlert = x => x.Marked("Sign-in result");
            }

            if (OniOS)
            {
                signInResultAlert = x => x.Marked("Sign-in result");
            }
        }
    }
}
