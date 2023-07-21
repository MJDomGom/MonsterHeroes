using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Precondition_OnTriggerEnter : AbstractPrecondition
{
	private bool onTriggerEntered;

	public override bool IsPreconditionActive()
	{
		return onTriggerEntered;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			onTriggerEntered = true;
			NotifyPreconditionTriggered();
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
		{
			onTriggerEntered = false;
		}
	}
}
