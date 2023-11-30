using System;
using UnityEngine.Purchasing;

namespace CodeBase.Services.IAP
{
	[Serializable]
	public class ProductConfig
	{
		public string ID;
		public ProductType ProductType;
		public int MaxPurchaseCount;
		public ItemType ItemType;
		public int Quantity;
	}
}