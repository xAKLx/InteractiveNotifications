using Android.OS;

class MyBoundServiceBinder : Binder
{
	HybridService hybridService;
	BoundServiceConnection service;

	public MyBoundServiceBinder(HybridService hybridService)
	{
		this.hybridService = hybridService;
	}

	public MyBoundServiceBinder(BoundServiceConnection service)
	{
		this.service = service;
	}
}