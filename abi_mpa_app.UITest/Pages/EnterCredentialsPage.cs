using System;
using Xamarin.UITest;
using WebQuery = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppWebQuery>;

namespace abi_mpa_app.UITest
{
    public class EnterCredentialsPage : BasePage
    {
        readonly WebQuery logInWebpage;
        readonly WebQuery usernameField;
        readonly WebQuery passwordField;
        readonly WebQuery signInButton;

        public EnterCredentialsPage()
            : base(x => x.WebView(), x => x.WebView())
        {
            logInWebpage = x => x.Css("#login_panel");
            usernameField = x => x.Css("#cred_userid_inputtext");
            passwordField = x => x.Css("#passwordInput");
            signInButton = x => x.Css("#submitButton");

            app.WaitForElement(logInWebpage);
            app.Screenshot("Webpage loaded");
        }

        public EnterCredentialsPage EnterCredentials(string username, string password)
        {
            app.EnterText(usernameField, username);
            app.PressEnter();
            app.WaitForElement(passwordField);

            app.EnterText(passwordField, password);
            app.DismissKeyboard();
            app.Screenshot("Credentials entered");

            return this;
        }

        public void Submit()
        {
            app.Screenshot("Tapping sign in button");
            app.Tap(signInButton);
        }
    }
}
