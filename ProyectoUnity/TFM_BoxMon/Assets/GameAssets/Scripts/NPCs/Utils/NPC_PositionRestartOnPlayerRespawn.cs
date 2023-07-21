using com.blackantgames.managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_PositionRestartOnPlayerRespawn : MonoBehaviour, IPlayerRespawnListener
{

	private Vector3 positionToRestart;

	private void Start()
	{
		positionToRestart = transform.position;
		GameManager.GetInstance().AddRespawnListener(this);
	}


	private void OnDestroy()
	{
		GameManager.GetInstance().RemoveRespawnListener(this);
	}

	public void NotifyPlayerRespawn()
	{
		gameObject.SetActive(false);
		transform.position = positionToRestart;
		gameObject.SetActive(true);
	}
}
