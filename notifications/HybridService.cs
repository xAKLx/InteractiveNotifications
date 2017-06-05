using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using Android.Widget;
using System.Threading;
using notifications;
using Android.Graphics;
using Java.Util;
using Java.Lang;
using Java.Util.Concurrent;

[Service]
class HybridService : Service
{
	private MyBoundServiceBinder binder_hybrid;
	private int count = 0;
	Intent stockServiceIntent = null;
	Store store = Store.getUniqueInstance();

	public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
	{
		if (stockServiceIntent == null)
		{
			var mhandler = new Handler();

			Runnable torun = new Runnable(() =>
			{
				while (true)
				{
					store.increaseCounter(5);
					Java.Lang.Thread.Sleep(3000);
				}

			});

			stockServiceIntent = new Intent(this, typeof(HybridService));
			Executors.NewSingleThreadScheduledExecutor().Schedule(torun, 3, TimeUnit.Seconds);

		}

		if (intent.Action == "klk")
		{
			Toast.MakeText(this, $"increasing counter to {++count}", ToastLength.Short).Show();
		}
		pushNotification(count);
	    return StartCommandResult.Sticky;	
	}

	void ScheduleStockUpdates()
	{
		if (!IsAlarmSet())
		{
			var alarm = (AlarmManager)GetSystemService(Context.AlarmService);
			stockServiceIntent.SetAction("klk");

			var pendingServiceIntent = PendingIntent.GetService(this, 0, stockServiceIntent, PendingIntentFlags.CancelCurrent);
			alarm.SetRepeating(AlarmType.Rtc, 0, 500, pendingServiceIntent);
		}
	}

	bool IsAlarmSet()
	{
		return PendingIntent.GetBroadcast(this, 0, stockServiceIntent, PendingIntentFlags.NoCreate) != null;	}

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
		const int notificationId = 1;
		notificationManager.Notify(notificationId, notification);
	}

	private Notification.Action buildNotificationAction()
	{
		Intent intent = new Intent();
		intent.SetAction("com.tutorialspoint.CUSTOM_INTENT");
		PendingIntent pendingIntent = PendingIntent.GetBroadcast(this, 1, intent, 0);

		return new Notification
			.Action
			.Builder(Resource.Mipmap.Icon, "Increase in 2", pendingIntent)
			.Build();	}

	public override IBinder OnBind(Intent intent)
	{
		binder_hybrid = new MyBoundServiceBinder(this);
		Toast.MakeText(this, "OnBind() method start from HybridService", ToastLength.Long).Show();
		Toast.MakeText(this, "Hybrid Service is started", ToastLength.Long).Show();
		return binder_hybrid;
	}

	public override bool OnUnbind(Intent intent)
	{
		Toast.MakeText(this, "OnUnBind() Method Called from HybridService.cs", ToastLength.Long).Show();
		return base.OnUnbind(intent);
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		Toast.MakeText(this, "Hybrid Service is Destroyed", ToastLength.Long).Show();
	}
			 
}

