using System;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Android;
using Xamarin.UITest.iOS;
using Xamarin.UITest.Queries;

//Aliases Func<AppQuery, AppQuery> for readability
using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace abi_mpa_app.UITest
{
    public abstract class BasePage
    {
        protected readonly IApp app;

        protected bool OnAndroid { get; private set; }
        protected bool OniOS { get; private set; }

        //Trait is a element that is unique to each page
        //Used as a way to assert you are on a page
        protected Query Trait { get; set; }

        protected BasePage()
        {
            //Retrieve instance of started app from AppInitializer
            app = AppInitializer.App;

            //Save platform booleans for use in pages
            OnAndroid = app.GetType() == typeof(AndroidApp);
            OniOS = app.GetType() == typeof(iOSApp);

            //Query values that are common across all pages are defined in BasePage
            //and inherited by all pages
            InitializeCommonQueries();
        }

        protected BasePage(Query androidTrait, Query iOSTrait)
            : this()
        {
            if (OnAndroid)
                Trait = androidTrait;
            if (OniOS)
                Trait = iOSTrait;

            //Waits for the Trait to become visible 
            AssertOnPage(TimeSpan.FromSeconds(30));

            //Take a screenshot once page has been verified
            app.Screenshot("On " + this.GetType().Name);
        }

        protected BasePage(string androidTrait, string iOSTrait)
            : this(x => x.Marked(androidTrait), x => x.Marked(iOSTrait))
        {
        }

        /// <summary>
        /// Verifies that the trait is still present. Defaults to no wait.
        /// </summary>
        /// <param name="timeout">Time to wait before the assertion fails</param>
        protected void AssertOnPage(TimeSpan? timeout = default(TimeSpan?))
        {
            if (Trait == null)
                throw new NullReferenceException("Trait not set");

            var message = "Unable to verify on page: " + this.GetType().Name;

            if (timeout == null)
                Assert.IsNotEmpty(app.Query(Trait), message);
            else
                Assert.DoesNotThrow(() => app.WaitForElement(Trait, timeout: timeout), message);
        }

        /// <summary>
        /// Verifies that the trait is no longer present. Defaults to a two second wait.
        /// </summary>
        /// <param name="timeout">Time to wait before the assertion fails</param>
        protected void WaitForPageToLeave(TimeSpan? timeout = default(TimeSpan?))
        {
            if (Trait == null)
                throw new NullReferenceException("Trait not set");

            timeout = timeout ?? TimeSpan.FromSeconds(2);
            var message = "Unable to verify *not* on page: " + this.GetType().Name;

            Assert.DoesNotThrow(() => app.WaitForNoElement(Trait, timeout: timeout), message);
        }

        /// <summary>
        /// Enter text and dismiss keyboard
        /// </summary>
        /// <param name="marked">Element marked.</param>
        /// <param name="text">Text to enter.</param>
        public void EnterAndDismiss(string marked, string text)
        {
            app.EnterText(marked, text);
            app.DismissKeyboard();
        }

        /// <summary>
        /// Enter text and dismiss keyboard
        /// </summary>
        /// <param name="query">Query of element.</param>
        /// <param name="text">Text to enter.</param>
        public void EnterAndDismiss(Query query, string text)
        {
            app.EnterText(query, text);
            app.DismissKeyboard();
        }

        #region CommonPageActions

        // Use this region to define functionality that is common across many or all pages in your app.
        // Eg tapping the back button of a page or selecting the tabs of a tab bar

        void InitializeCommonQueries()
        {
            if (OnAndroid)
            {
            }
            if (OniOS)
            {
            }
        }

        #endregion
    }
}