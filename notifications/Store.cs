using System;
using System.Collections.Generic;

namespace notifications
{
	public class Store
	{
		public int count = 0;

		public delegate void onUpdateCalback(int newValue);

		private static Store uniqueInstance = null;
		private List<onUpdateCalback> onUpdate = null;


		private Store()
		{
			onUpdate = new List<onUpdateCalback>();
		}

		public void addListener(onUpdateCalback callback)
		{
			onUpdate.Add(callback);
		}

		public static Store getUniqueInstance()
		{
			if (uniqueInstance == null)
				uniqueInstance = new Store();

			return uniqueInstance;
		}

		public void increaseCounter(int step)
		{
			count += step;

			foreach(var callback in onUpdate)
				callback(count);
		}
	}
}
