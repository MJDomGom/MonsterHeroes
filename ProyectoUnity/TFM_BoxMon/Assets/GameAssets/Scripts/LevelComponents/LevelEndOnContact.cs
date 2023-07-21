using com.blackantgames.managers;
using com.blackantgames.playerstoragesystem;
using com.blackantgames.scenenavigation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndOnContact : MonoBehaviour
{
	private LevelController parentLevelController;
	[SerializeField] private int completedLevelGroupId = -1;
	[SerializeField] private DoorController doorController;
	[SerializeField] private GameObject particleEffect;


	private void Awake()
	{
		parentLevelController = GetComponentInParent<LevelController>();
#if UNITY_EDITOR
		if (parentLevelController == null)
		{
			Debug.LogError("Error - Parent LevelController could not be found for " + transform.name);
		}
#endif

	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag.Equals("Player"))
		{
			if (parentLevelController.IsFinalLevel())
			{
				Instantiate(particleEffect, transform.position, Quaternion.identity, null);
				FinishGame();
			}
			else
			{
				GameManager.GetInstance().NotifyEndOfLevelReached();
				if (doorController != null)
				{
					doorController.SetDoorOpenStatus(false);
				}
				Destroy(this);
			}
		}
		
	}

	private void FinishGame()
	{
		GameManager.GetInstance().ShowLevelEnd();
		Invoke("TransitionToMainMenu", 5f);
		GameManager.GetInstance().SetPlayerMovementAllowed(false);

		if (completedLevelGroupId >= 0)
		{
			LevelSelectionController.SetLevelComplete(completedLevelGroupId);
		}
	}

	private void TransitionToMainMenu()
	{
		SceneNavigator.GetInstance().OpenScene_OfflineWithLoadingTransition(SceneNavigator.EGameLevels.MAIN_MENU);
	}
}
