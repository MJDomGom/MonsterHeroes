using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_EnableObject : AbstracEffect
{
	[SerializeField] private GameObject objectToEnable;

	public override void TriggerEffect()
	{
		if (objectToEnable != null)
		{
			objectToEnable.SetActive(true);
		}
	}
}
