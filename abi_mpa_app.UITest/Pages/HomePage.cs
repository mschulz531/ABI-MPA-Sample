using System;
using NUnit.Framework;
using Xamarin.UITest;

using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace abi_mpa_app.UITest
{
    public class HomePage : BasePage
    {
        // Set query values as class fields
        readonly Query signInResultAlert;
        readonly Query alertOKButton;
        readonly Query messageTextField;
        readonly Query sendMessageButton;
        readonly Query messageList;
        readonly Query completeButton;
        readonly Func<string, Query> messageWithText;

        // Pass the Page Trait to the BasePage constructor
        public HomePage()
            : base(x => x.Class("ConditionalFocusLayout"), x => x.Class("Xamarin_Forms_Platform_iOS_ContextActionsCell"))
        {
            // Assign the Query fields values
			signInResultAlert = x => x.Marked("Sign-in result");
            sendMessageButton = x => x.Button("+");
            completeButton = x => x.Marked("Complete");

            // If queries are different on platforms, use booleans set in BasePage to conditionally assign them
            if (OnAndroid)
            {
                messageTextField = x => x.Class("EntryEditText");
                messageList = x => x.Class("ListView");
				messageWithText = text => x => x.Class("ConditionalFocusLayout").Descendant().Marked(text);
            }

            if (OniOS)
            {
                alertOKButton = x => x.Marked("OK");
                messageTextField = x => x.Class("UITextField");
                messageList = x => x.Class("UITableView");
                messageWithText = text => x => x.Class("Xamarin_Forms_Platform_iOS_ContextActionsCell").Descendant().Marked(text);
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
            app.Tap(messageTextField);
            app.EnterText(text);
            app.Screenshot("Entered message text");
            app.DismissKeyboard();

            app.Tap(sendMessageButton);
            app.Screenshot("Tapped: 'Send'");

			// Invoke methods on the native views to get the amount of rows in the List
            Func<long> listLength = () => OnAndroid ?
                Convert.ToInt64(app.Query(x => messageList(x).Invoke("getAdapter").Invoke("getCount"))[0]) :
                Convert.ToInt64(app.Query(x => messageList(x).Invoke("numberOfRowsInSection:"))[0]);

            // Assert that the list has some items.
            // Ideally we would assert that the number of items in the list incresed,
            // but this will cause failures when using the same account to run on many devices at once
            app.WaitFor(() => listLength() > 0, "Length of list wasn't greater than 0");

            return this;
        }

        /// <summary>
        /// Locates a message.
        /// </summary>
        /// <returns>The HomePage instance</returns>
        /// <param name="text">Message text</param>
        public HomePage LocateMessage(string text)
        {
            // Use the scrollview withinQuery to speed up scrolling
            app.ScrollDownTo(x => x.Marked(text), messageList, timeout: TimeSpan.FromSeconds(30));
            app.ScrollDown(messageList, swipePercentage: 0.2);
            app.Screenshot("Located message");

            return this;
        }

        /// <summary>
        /// Completes a message.
        /// </summary>
        /// <returns>The HomePage instance</returns>
        /// <param name="text">Message text</param>
        public HomePage CompleteMessage(string text)
        {
            if (OnAndroid)
            {
                app.TouchAndHold(messageWithText(text));
            }

            if (OniOS)
            {
                app.SwipeRightToLeft(messageWithText(text));
            }

            app.Screenshot("Tapping complete button");
			app.Tap(completeButton);
            app.WaitForNoElement(messageWithText(text));
            app.Screenshot("Message complete");

            return this;
        }
    }
}
