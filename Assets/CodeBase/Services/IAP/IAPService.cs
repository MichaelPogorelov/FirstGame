using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine.Purchasing;

namespace CodeBase.Services.IAP
{
	public class IAPService : IIAPService
	{
		public bool IsInitialized => _iapProvider.IsInitialize;
		public event Action Initialized;

		private readonly IAPProvider _iapProvider;
		private readonly IPersistentProgressService _progressService;

		public IAPService(IAPProvider iapProvider, IPersistentProgressService progressService)
		{
			_iapProvider = iapProvider;
			_progressService = progressService;
		}

		public void Initialize()
		{
			_iapProvider.Initialize(this);
			_iapProvider.Initialized += () => Initialized?.Invoke();
		}

		public List<ProductDescription> Products()
		{
			return ProductDescriptions().ToList();
		}

		public void StartPurchase(string productId)
		{
			_iapProvider.StartPurchase(productId);
		}

		public PurchaseProcessingResult ProcessPurchase(Product purchaseProduct)
		{
			ProductConfig productConfig = _iapProvider.Configs[purchaseProduct.definition.id];
			switch (productConfig.ItemType)
			{
				case ItemType.None:
					break;
				case ItemType.Skulls:
					_progressService.Progress.WorldData.LootSaveData.Add(productConfig.Quantity);
					_progressService.Progress.PurchaseProduct.AddPurchase(purchaseProduct.definition.id);
					break;
			}

			return PurchaseProcessingResult.Complete;
		}

		private IEnumerable<ProductDescription> ProductDescriptions()
		{
			PurchaseProductData purchaseProduct = _progressService.Progress.PurchaseProduct;
			foreach (string productId in _iapProvider.Products.Keys)
			{
				ProductConfig productConfig = _iapProvider.Configs[productId];
				Product product = _iapProvider.Products[productId];
				
				BoughtIAP boughtIAP = purchaseProduct.BoughtIAPs.Find(x => x.IAPid == productId);
				if (boughtIAP != null && boughtIAP.Count >= productConfig.MaxPurchaseCount)
				{
					continue;
				}

				yield return new ProductDescription
				{
					Id = productId,
					ProductConfig = productConfig,
					Product = product,
					AvailablePurchaseLeft = boughtIAP != null
						? productConfig.MaxPurchaseCount - boughtIAP.Count
						: productConfig.MaxPurchaseCount,
				};
			}
		}
	}
}