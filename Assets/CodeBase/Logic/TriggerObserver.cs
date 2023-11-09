using System;
using UnityEngine;

namespace CodeBase.Logic
{
	[RequireComponent(typeof(Collider))]
	public class TriggerObserver : MonoBehaviour
	{
		public event Action<Collider> TriggerEnter;
		public event Action<Collider> TriggerExit;

		private void OnTriggerEnter(Collider collider)
		{
			TriggerEnter?.Invoke(collider);
		}

		private void OnTriggerExit(Collider collider)
		{
			TriggerExit?.Invoke(collider);
		}
	}
}