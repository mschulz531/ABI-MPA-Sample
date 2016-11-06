using System;
using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace abi_mpa_app.UITest
{
    public class SignInPage : BasePage
    {
        //Set query values as class fields
        readonly Query signInButton;

        //Pass the Page Trait to the BasePage constructor
        public SignInPage()
            : base(x => x.Marked("Sign-in"), x => x.Marked("Sign-in"))
        {
            //If queries are different on platforms, use booleans set in BasePage to conditionally assign them
            if (OnAndroid)
            {
                signInButton = x => x.Marked("Sign-in");
            }

            if (OniOS)
            {
                signInButton = x => x.Marked("Sign-in");
            }
        }

        /// <summary>
        /// Taps Sign In Button
        /// </summary>
        public void SignIn()
        {
            app.Screenshot("Tapping Sign In Button");
            app.Tap(signInButton);
        }
    }
}
