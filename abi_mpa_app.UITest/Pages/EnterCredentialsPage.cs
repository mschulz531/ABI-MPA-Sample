using System;
using System.Threading;
using Xamarin.UITest;

//Aliases Func<AppQuery, AppWebQuery> because page contains Webview elements
using WebQuery = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppWebQuery>;

namespace abi_mpa_app.UITest
{
    public class EnterCredentialsPage : BasePage
    {
        //Set query values as class fields
        readonly WebQuery logInWebpage;
        readonly WebQuery usernameField;
        readonly WebQuery passwordField;
        readonly WebQuery signInButton;

        //Pass the Page Trait to the BasePage constructor
        public EnterCredentialsPage()
            : base(x => x.WebView(), x => x.WebView())
        {
            //Assign the Query fields values
            logInWebpage = x => x.Css("#login_panel");
            usernameField = x => x.Css("#cred_userid_inputtext");
            passwordField = x => x.Css("#passwordInput");
            signInButton = x => x.Css("#submitButton");

            app.WaitForElement(logInWebpage);
            app.Screenshot("Webpage loaded");
        }

        /// <summary>
        /// Enters user credentials.
        /// </summary>
        /// <returns>The EnterCredentialsPage instance</returns>
        /// <param name="username">Username.</param>
        /// <param name="password">Password.</param>
        public EnterCredentialsPage EnterCredentials(string username, string password)
        {
            app.Tap(usernameField);
            app.EnterText(usernameField, username);
            app.PressEnter();
            app.WaitForElement(passwordField, timeout: TimeSpan.FromSeconds(90));
            app.Screenshot("Username entered, entering password");

            app.Tap(passwordField);
            app.EnterText(passwordField, password);
            app.DismissKeyboard();
            app.Screenshot("Credentials entered");

            return this;
        }

        /// <summary>
        /// Taps Sign In Button
        /// </summary>
        public void Submit()
        {
            app.Screenshot("Tapping sign in button");
            app.Tap(signInButton);
        }
    }
}
