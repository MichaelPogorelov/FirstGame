using System.Collections;
using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using TMPro;
using UnityEngine;

namespace CodeBase.Loot
{
	public class LootPiece : MonoBehaviour, ISaveProgress
	{
		public GameObject Skull;
		public GameObject FXPrefab;
		public GameObject PickupPopup;
		public TextMeshPro LootText;
		
		private LootData _lootData;
		private bool _isPicked;
		private WorldData _worldData;

		public void Constructor(WorldData worldData)
		{
			_worldData = worldData;
		}
		
		public void Initialize(LootData lootData)
		{
			_lootData = lootData;
		}

		private void OnTriggerEnter(Collider other)
		{
			Pickup();
		}

		private void Pickup()
		{
			if (_isPicked)
			{
				return;
			}
			_isPicked = true;

			_worldData.LootSaveData.Collect(_lootData);
			
			Skull.SetActive(false);
			PlayFX();
			ShowText();

			StartCoroutine(StartDestroy());
		}

		private IEnumerator StartDestroy()
		{
			yield return new WaitForSeconds(1.5f);
			Destroy(gameObject);
		}

		private void PlayFX()
		{
			Instantiate(FXPrefab, transform.position, Quaternion.identity);
		}

		private void ShowText()
		{
			LootText.text = $"{_lootData.Value}";
			PickupPopup.SetActive(true);
		}
		
		public void SaveProgress(PlayerProgress progress)
		{
			if (!_isPicked)
			{
				LootSavePositionData lootSavePositionData = new LootSavePositionData();
				lootSavePositionData.LootPosition = transform.position;
				lootSavePositionData.Value = _lootData.Value;
				progress.LootSavePositionData.Add(lootSavePositionData);
			}
		}
	}
}