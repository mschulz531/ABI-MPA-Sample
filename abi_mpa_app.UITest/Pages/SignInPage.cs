using System;
using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace abi_mpa_app.UITest
{
    public class SignInPage : BasePage
    {
        readonly Query signInButton;

        public SignInPage()
            : base(x => x.Marked("Sign-in"), x => x.Marked("Sign-in"))
        {
            if (OnAndroid)
            {
                signInButton = x => x.Marked("Sign-in");
            }

            if (OniOS)
            {
                signInButton = x => x.Marked("Sign-in");
            }
        }

        public void SignIn()
        {
            app.Screenshot("Tapping Sign In Button");
            app.Tap(signInButton);
        }
    }
}
