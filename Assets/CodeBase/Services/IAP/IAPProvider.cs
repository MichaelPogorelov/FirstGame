using System;
using System.Collections.Generic;
using CodeBase.Data;
using UnityEngine;
using UnityEngine.Purchasing;

namespace CodeBase.Services.IAP
{
	public class IAPProvider : IStoreListener
	{
		public bool IsInitialize => _controller != null && _extensions != null;
		public event Action Initialized;

		private List<ProductConfig> _configs;
		private IStoreController _controller;
		private IExtensionProvider _extensions;
		private const string IAPConfigsPath = "IAP/products";

		public void Initialize()
		{
			ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
			Load();

			foreach (ProductConfig productConfig in _configs)
			{
				builder.AddProduct(productConfig.ID, productConfig.Type);
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

			return PurchaseProcessingResult.Complete;
		}

		public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
		{
			Debug.Log($"Purchasing failed {product.definition.id} PurchaseFailureReason {failureReason}");
		}

		private void Load()
		{
			_configs = Resources.Load<TextAsset>(IAPConfigsPath).text.ToDeserialized<ProductConfigWrapper>().Configs;
		}
	}
}