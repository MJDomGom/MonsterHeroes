using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMessageEffect : AbstracEffect
{
	public override void TriggerEffect()
	{
		Debug.Log("TRIGGER EFFECT");
	}
}
