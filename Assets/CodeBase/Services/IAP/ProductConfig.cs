using System;
using UnityEngine.Purchasing;

namespace CodeBase.Services.IAP
{
	[Serializable]
	public class ProductConfig
	{
		public string ID;
		public ProductType Type;
		public int MaxPurchaseCount;
	}
}