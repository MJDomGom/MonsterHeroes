using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCWaveStarter : MonoBehaviour
{
	[SerializeField] private NPCWaveController waveController;

	private bool activated = false;


	private void OnTriggerEnter(Collider other)
	{
		if (activated) return;

		if (other.tag.Equals("Player"))
		{
			waveController.StartWave();
			activated = true;

			// Wave can be activated only once. Destroy this object to avoid
			// possible errors re-activating the wave
			Destroy(this);
		}
	}
}
