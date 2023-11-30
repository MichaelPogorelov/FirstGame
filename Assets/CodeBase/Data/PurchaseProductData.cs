using System;
using System.Collections.Generic;

namespace CodeBase.Data
{
	[Serializable]
	public class PurchaseProductData
	{
		public List<BoughtIAP> BoughtIAPs = new List<BoughtIAP>();
		public event Action Change;

		public void AddPurchase(string id)
		{
			BoughtIAP boughtIAP = BoughtIAPs.Find(x => x.IAPid == id);

			if (boughtIAP != null)
			{
				boughtIAP.Count++;
			}
			else
			{
				BoughtIAPs.Add(new BoughtIAP{IAPid = id, Count = 1});
			}
			
			Change?.Invoke();
		}
	}
}