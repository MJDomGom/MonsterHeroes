using com.blackantgames.managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Precondition_OnPlayerRespawn : AbstractPrecondition, IPlayerRespawnListener
{

	private void Start()
	{
		GameManager.GetInstance().AddRespawnListener(this);
	}

	private void OnDestroy()
	{
		GameManager.GetInstance().RemoveRespawnListener(this);
	}

	private bool actionTriggered = false;

	public override bool IsPreconditionActive()
	{
		return actionTriggered;
	}

	public void NotifyPlayerRespawn()
	{
		actionTriggered = true;
		NotifyPreconditionTriggered();
		actionTriggered= false;
	}
}
