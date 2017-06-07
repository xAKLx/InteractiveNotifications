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
using System.Threading.Tasks;

[Service]
class HybridService : Service
{
	private MyBoundServiceBinder binder_hybrid;
	private int count = 0;
	Store store = Store.getUniqueInstance();
	System.Threading.Thread counterIncreaser = null;

	public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
	{
		if (counterIncreaser == null)
		{
			var toRun = new ThreadStart(() => 
			{
				while (true)
				{
					System.Threading.Thread.Sleep(3000);
					store.increaseCounter(5);
				}

			});
			                        
			counterIncreaser = new System.Threading.Thread(toRun);
			counterIncreaser.Start();

		}

	    return StartCommandResult.Sticky;	
	}

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
		if (counterIncreaser != null)
		{
			Toast.MakeText(this, "Stoping counter increase", ToastLength.Long).Show();
			counterIncreaser.Abort();
		}
		
		Toast.MakeText(this, "Hybrid Service is Destroyed", ToastLength.Long).Show();
	}
			 
}

