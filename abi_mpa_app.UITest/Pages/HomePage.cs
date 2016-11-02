using System;
using NUnit.Framework;
using Xamarin.UITest;

using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace abi_mpa_app.UITest
{
    public class HomePage : BasePage
    {
        //Set query values as class fields
        readonly Query signInResultAlert;
        readonly Query alertOKButton;
        readonly Query messageTextField;
        readonly Query sendMessageButton;
        readonly Query scrollView;
        readonly Query messageList;

        //Pass the Page Trait to the BasePage constructor
        public HomePage()
            : base(x => x.Class("ConditionalFocusLayout"), x => x.Class("Xamarin_Forms_Platform_iOS_ContextActionsCell"))
        {
            //Assign the Query fields values
            sendMessageButton = x => x.Button("+");

            //If queries are different on platforms, use booleans set in BasePage to conditionally assign them
            if (OnAndroid)
            {
                signInResultAlert = x => x.Marked("Sign-in result");
                messageTextField = x => x.Class("EntryEditText");
                scrollView = x => x.Class("ListView");
                messageList = x => x.Class("ListView");
            }

            if (OniOS)
            {
                signInResultAlert = x => x.Marked("Sign-in result");
                alertOKButton = x => x.Marked("OK");
                messageTextField = x => x.Class("UITextField");
                scrollView = x => x.Class("UITableView");
                messageList = x => x.Class("UITableView");
            }
        }

        /// <summary>
        /// Dismisses the successful sign in alert.
        /// </summary>
        /// <returns>The HomePage instance</returns>
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

        /// <summary>
        /// Sends a message.
        /// </summary>
        /// <returns>The HomePage instance</returns>
        /// <param name="text">Message Text</param>
        public HomePage SendMessage(string text)
        {
            //Invokes methods on the native methods to get the amount of rows in the List
            var initialLength = OnAndroid ?
                Convert.ToInt64(app.Query(x => messageList(x).Invoke("getAdapter").Invoke("getCount"))[0]) :
                Convert.ToInt64(app.Query(x => messageList(x).Invoke("numberOfRowsInSection:"))[0]);

            app.Tap(messageTextField);
            app.EnterText(text);
            app.Screenshot($"Entered message text: {text}");
            app.DismissKeyboard();

            app.Tap(sendMessageButton);
            app.Screenshot("Tapped: 'Send'");

            app.ScrollUp(strategy: ScrollStrategy.Gesture);

            var finalLength = OnAndroid ?
                Convert.ToInt64(app.Query(x => messageList(x).Invoke("getAdapter").Invoke("getCount"))[0]) :
                Convert.ToInt64(app.Query(x => messageList(x).Invoke("numberOfRowsInSection:"))[0]);

            //Asserts the number of items in the list increased
            app.WaitFor(() => initialLength < finalLength, $"Length of list wasn't greater after sending message ({initialLength} !< {finalLength})");

            return this;
        }

        /// <summary>
        /// Locates a message.
        /// </summary>
        /// <returns>The HomePage instance</returns>
        /// <param name="marked">Message text</param>
        public HomePage LocateMessage(string marked)
        {
            //Use the scrollview withinQuery to speed up scrolling
            app.ScrollDownTo(x => x.Marked(marked), scrollView, timeout: TimeSpan.FromSeconds(30));
            app.Screenshot($"Located message with text: {marked}");

            return this;
        }
    }
}
