using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Gcm.Client;

using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;

namespace abi_mpa_app.Droid
{
	[Activity (Label = "Abi Android App",
		Icon = "@drawable/icon",
		MainLauncher = true,
		ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
		Theme = "@android:style/Theme.Holo.Light")]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity, IAuthenticate
	{
		// Create a new instance field for this activity.
		static MainActivity instance = null;
		// Define a authenticated user.
		private MobileServiceUser user;

		public async Task<bool> Authenticate()
		{
			var success = false;
			var message = string.Empty;
			try
			{
				// Sign in with AAD login using a server-managed flow.
				user = await TodoItemManager.DefaultManager.CurrentClient.LoginAsync(this,
					MobileServiceAuthenticationProvider.WindowsAzureActiveDirectory);
				if (user != null)
				{
					message = string.Format("you are now signed-in as {0}.",
						user.UserId);
					success = true;
					RegisterAuthorizedUserForNotification();
				}
			}
			catch (Exception ex)
			{
				message = ex.Message;
			}

			// Display the success or failure message.
			AlertDialog.Builder builder = new AlertDialog.Builder(this);
			builder.SetMessage(message);
			builder.SetTitle("Sign-in result");
			builder.Create().Show();

			return success;
		}

		// Return the current activity instance.
		public static MainActivity CurrentActivity
		{
			get
			{
				return instance;
			}
		}

		private void RegisterAuthorizedUserForNotification()
		{
			try
			{
				if (user != null)
				{
					// Check to ensure everything's setup right
					GcmClient.CheckDevice(this);
					GcmClient.CheckManifest(this);

					// Register for push notifications
					System.Diagnostics.Debug.WriteLine("Registering...");
					if (App.Authenticator != null)
					{
						GcmClient.Register(this, PushHandlerBroadcastReceiver.SENDER_IDS);
					}
				}
			}
			catch (Java.Net.MalformedURLException)
			{
				CreateAndShowDialog("There was an error creating the client. Verify the URL.", "Error");
			}
			catch (Exception e)
			{
				CreateAndShowDialog(e.Message, "Error");
			}
		}

		protected override void OnCreate (Bundle bundle)
		{
			// Set the current instance of MainActivity.
			instance = this;
			base.OnCreate (bundle);

			// Initialize Azure Mobile Apps
			Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();

			// Initialize Xamarin Forms
			global::Xamarin.Forms.Forms.Init (this, bundle);

			// Initialize the authenticator before loading the app.
			App.Init((IAuthenticate)this);

			// Load the main application
			LoadApplication (new App ());

			RegisterAuthorizedUserForNotification();
		}

		private void CreateAndShowDialog(String message, String title)
		{
			AlertDialog.Builder builder = new AlertDialog.Builder(this);

			builder.SetMessage(message);
			builder.SetTitle(title);
			builder.Create().Show();
		}
	}
}

