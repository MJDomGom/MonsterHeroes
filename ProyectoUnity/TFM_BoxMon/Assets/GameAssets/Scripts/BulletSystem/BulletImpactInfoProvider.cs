using UnityEngine;

public class BulletImpactInfoProvider : MonoBehaviour
{
	[SerializeField] private BulletFX impactInfo;

	public BulletFX GetBulletFX()
	{
		return impactInfo;
	}
}
