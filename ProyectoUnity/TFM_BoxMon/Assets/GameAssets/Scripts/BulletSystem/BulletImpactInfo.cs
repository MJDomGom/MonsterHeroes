using UnityEngine;

[System.Serializable]
public struct BulletImpactInfo {

	// Bullet impact effect to trigger in hit point
	public GameObject fxBulletImpactPrefab;

	// Time that the impact effect object is alive
	public float liveTime;

	// Audio clips that can be reproduced in the position where the bullet hits. If no elements are assigned, no audio clip will be played
	public AudioClip[] fxAudioClip;

	// Maximum distance to which the audio can be listened
	public float maxAudioDistance;

}
