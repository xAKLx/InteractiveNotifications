using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;

namespace notifications
{
	[Activity(Label = "notifications", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
		private BoundServiceConnection serviceConnection = null;

		Intent stockServiceIntent = null;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.Main);
			stockServiceIntent = new Intent(this, typeof(HybridService));

			var store = Store.getUniqueInstance();
			Button button = FindViewById<Button>(Resource.Id.myButton);

			store.addListener(onUpdate);
			button.Click += delegate { store.increaseCounter(1); };
			pushNotification(store.count);

			Button btnstarthybridservice = (Button)FindViewById(Resource.Id.btnstarthybridservice);
			Button btnstophybridservice = (Button)FindViewById(Resource.Id.btnstophybridservice);

			btnstarthybridservice.Click += Btnstarthybridservice_Click;  
     		btnstophybridservice.Click += Btnstophybridservice_Click; 
		}

		private void Btnstophybridservice_Click(object sender, System.EventArgs e)
		{
			if (null == serviceConnection)
			{
				Toast.MakeText(this,  "There is no connection", ToastLength.Long).Show();
				return;
			}
				
			UnbindService(serviceConnection);
			serviceConnection = null;
			StopService(new Intent(this, typeof(HybridService)));
		}

		private void Btnstarthybridservice_Click(object sender, System.EventArgs e)
		{
			if (null != serviceConnection)
			{
				Toast.MakeText(this,  "There is a connection already", ToastLength.Long).Show();
				return;
			}

			Intent startIntent = new Intent(this, typeof(HybridService));

			StartService(startIntent);

			serviceConnection = new BoundServiceConnection();
			
			//StartService(new Intent(this, typeof(HybridService)));
			BindService((new Intent(this, typeof(HybridService))), serviceConnection, Bind.AutoCreate);

            //ScheduleStockUpdates();	
		}  

		protected void onUpdate(int count)
		{
            RunOnUiThread(() =>
			{
				Button button = FindViewById<Button>(Resource.Id.myButton);
				button.Text = $"{count} clicks!, click again to increase in 1";

				pushNotification(count);
			});

		}

		protected void pushNotification(int count)
		{
			var action = buildNotificationAction();
			// Instantiate the builder and set notification elements:
			Notification notification = new Notification.Builder(this)
				.SetContentTitle("Sample Notification")
				.SetContentText($"Hello World! This is my notification! #{count}")
				.SetSmallIcon(Resource.Mipmap.Icon)
				.AddAction(action)
			    .Build();

			// Get the notification manager:
			NotificationManager notificationManager =
				GetSystemService(Context.NotificationService) as NotificationManager;

			// Publish the notification:
			const int notificationId = 0;
			notificationManager.Notify (notificationId, notification);
		}

		private Notification.Action buildNotificationAction()
		{
			Intent intent = new Intent();
			intent.SetAction("com.tutorialspoint.CUSTOM_INTENT");
			PendingIntent pendingIntent = PendingIntent.GetBroadcast(this, 1, intent, 0);

			return new Notification
				.Action
				.Builder(Resource.Mipmap.Icon, "Increase in 2", pendingIntent)
				.Build();
		}
	}
}

