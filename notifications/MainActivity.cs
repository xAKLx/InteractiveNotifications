using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;

namespace notifications
{
	[Activity(Label = "notifications", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.Main);

			var store = Store.getUniqueInstance();
			Button button = FindViewById<Button>(Resource.Id.myButton);

			button.Click += delegate { store.increaseCounter(1); };
			store.addListener(onUpdate);
			pushNotification(store.count);
		}

		private Notification.Action buildNotificationAction()
		{
			Intent intent = new Intent();
			intent.SetAction("com.tutorialspoint.CUSTOM_INTENT");
			PendingIntent pendingIntent = PendingIntent.GetBroadcast(this, 1, intent, 0);

			return new Notification.Action.Builder(Resource.Mipmap.Icon,"Increase in 2", pendingIntent).Build();
		}

		protected void onUpdate(int count)
		{
			Button button = FindViewById<Button>(Resource.Id.myButton);
			button.Text = $"{count} clicks!, click again to increase in 1";

			pushNotification(count);
		}

		protected void pushNotification(int count)
		{
			var action = buildNotificationAction();
			// Instantiate the builder and set notification elements:
			Notification.Builder builder = new Notification.Builder(this)
				.SetContentTitle("Sample Notification")
				.SetContentText($"Hello World! This is my notification! #{count}")
				.SetSmallIcon(Resource.Mipmap.Icon)
				.AddAction(action);

			// Build the notification:
			Notification notification = builder.Build();

			// Get the notification manager:
			NotificationManager notificationManager =
				GetSystemService(Context.NotificationService) as NotificationManager;

			// Publish the notification:
			const int notificationId = 0;
			notificationManager.Notify (notificationId, notification);
		}
	}
}

