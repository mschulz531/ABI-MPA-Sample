using System;
using NUnit.Framework;
using Xamarin.UITest;

using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace abi_mpa_app.UITest
{
    public class HomePage : BasePage
    {
        readonly Query signInResultAlert;
        readonly Query alertOKButton;
        readonly Query messageTextField;
        readonly Query sendMessageButton;
        readonly Query scrollView;
        readonly Query messageList;

        public HomePage()
            : base(x => x.Class("ConditionalFocusLayout"), x => x.Class("Xamarin_Forms_Platform_iOS_ContextActionsCell"))
        {
            sendMessageButton = x => x.Button("+");

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

        public HomePage SendMessage(string text)
        {
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

            app.WaitFor(() => initialLength < finalLength, $"Lenght of list wasn't greater after sending message ({initialLength} !< {finalLength})");

            return this;
        }

        public HomePage LocateMessage(string marked)
        {
            app.ScrollDownTo(x => x.Marked(marked), scrollView, timeout: TimeSpan.FromSeconds(30));
            app.Screenshot($"Located message with text: {marked}");

            return this;
        }
    }
}
