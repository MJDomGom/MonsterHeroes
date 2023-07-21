using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFX : MonoBehaviour {

	[SerializeField] BulletImpactInfo _impactInfo;
	
	public BulletImpactInfo GetImpactInfo()
	{
		return _impactInfo;
	}

	public static void TriggerImpactInfo(BulletImpactInfo impactInfo, Vector3 position, Quaternion rotation, Transform parent)
	{
		if (impactInfo.fxBulletImpactPrefab != null) {
			GameObject generatedFX = Instantiate (impactInfo.fxBulletImpactPrefab, position, rotation);
			generatedFX.transform.parent = parent;
			Destroy(generatedFX, impactInfo.liveTime);
		}

		if (impactInfo.fxAudioClip.Length > 0)
		{
			AudioSource.PlayClipAtPoint(impactInfo.fxAudioClip[Random.Range(0, impactInfo.fxAudioClip.Length)], position);
		}
	}
}
