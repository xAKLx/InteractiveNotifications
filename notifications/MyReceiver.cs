
using System;
using Android.Content;
using Android.Widget;
using notifications;

public class MyReceiver: BroadcastReceiver
{
	public override void OnReceive(Context context, Intent intent)
	{
		Store.getUniqueInstance()
		     .increaseCounter(2);
	}
}