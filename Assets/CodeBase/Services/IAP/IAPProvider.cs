using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Data;
using UnityEngine;
using UnityEngine.Purchasing;

namespace CodeBase.Services.IAP
{
	public class IAPProvider : IStoreListener
	{
		public bool IsInitialize => _controller != null && _extensions != null;
		public event Action Initialized;

		public Dictionary<string, ProductConfig> Configs { get; private set; }
		public Dictionary<string, Product> Products { get; private set; }
		private IStoreController _controller;
		private IExtensionProvider _extensions;
		private const string IAPConfigsPath = "IAP/products";
		private IAPService _iapService;

		public void Initialize(IAPService iapService)
		{
			_iapService = iapService;
			Configs = new Dictionary<string, ProductConfig>();
			Products = new Dictionary<string, Product>();
			
			ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
			Load();

			foreach (ProductConfig productConfig in Configs.Values)
			{
				builder.AddProduct(productConfig.ID, productConfig.ProductType);
			}

			UnityPurchasing.Initialize(this, builder);
		}

		public void StartPurchase(string productId)
		{
			_controller.InitiatePurchase(productId);
		}

		public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
		{
			_controller = controller;
			_extensions = extensions;

			foreach (Product product in _controller.products.all)
			{
				Products.Add(product.definition.id, product);
			}

			Initialized?.Invoke();
			
			Debug.Log("UnityPurchasing initialized success");
		}

		public void OnInitializeFailed(InitializationFailureReason error)
		{
			Debug.Log($"UnityPurchasing initialized failed {error}");
		}

		public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
		{
			Debug.Log($"Unity Purchasing success {purchaseEvent.purchasedProduct.definition.id}");

			return _iapService.ProcessPurchase(purchaseEvent.purchasedProduct);
		}

		public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
		{
			Debug.Log($"Purchasing failed {product.definition.id} PurchaseFailureReason {failureReason}");
		}

		private void Load()
		{
			Configs = Resources.Load<TextAsset>(IAPConfigsPath).text.ToDeserialized<ProductConfigWrapper>().Configs.ToDictionary(x => x.ID, x => x);
		}
	}
}