using System;
using Xamarin.UITest;

using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace abi_mpa_app.UITest
{
    public class HomePage : BasePage
    {
        readonly Query signInResultAlert;
        readonly Query alertOKButton;

        public HomePage()
            : base(x => x.Class("ConditionalFocusLayout"), x => x.Class("Xamarin_Forms_Platform_iOS_ContextActionsCell"))
        {
            if (OnAndroid)
            {
                signInResultAlert = x => x.Marked("Sign-in result");
            }

            if (OniOS)
            {
                signInResultAlert = x => x.Marked("Sign-in result");
                alertOKButton = x => x.Marked("OK");
            }
        }

        public HomePage DismissSignInAlert()
        {
            app.WaitForElement(signInResultAlert);
            app.Screenshot("Signed in successfully, closing alert");

            if (OnAndroid)
            {
                app.Back();
            }

            if (OniOS)
            {
                app.Tap(alertOKButton);
            }

            return this;
        }
    }
}
