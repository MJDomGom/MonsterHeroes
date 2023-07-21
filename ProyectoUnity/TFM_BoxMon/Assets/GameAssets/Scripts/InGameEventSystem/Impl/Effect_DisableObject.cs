using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_DisableObject : AbstracEffect
{
	[SerializeField] private GameObject objectToDisable;

	public override void TriggerEffect()
	{
		if (objectToDisable != null)
		{
			objectToDisable.SetActive(false);
		}
	}
}
