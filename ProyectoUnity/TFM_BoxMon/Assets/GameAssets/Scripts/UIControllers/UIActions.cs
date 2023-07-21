using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.blackantgames.scenenavigation;

public class UIActions : MonoBehaviour
{
	private GameObject previousEnabledPanel;

    public void StartGame()
	{
		SceneNavigator.GetInstance().OpenScene_OfflineWithLoadingTransition(SceneNavigator.EGameLevels.GAME_LEVEL);
	}

	public void RetryGame()
	{
		SceneNavigator.GetInstance().OpenScene_OfflineMode(SceneNavigator.EGameLevels.GAME_LEVEL);
	}

	public void LoadMainMenu()
	{
		SceneNavigator.GetInstance().OpenScene_OfflineWithLoadingTransition(SceneNavigator.EGameLevels.MAIN_MENU);
	}

	public void ExitGame()
	{
		PlayerPrefs.SetInt("MUTED", 0);
		Application.Quit();
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#endif
	}

	public void ChangePanelVisibility(GameObject panel)
	{
		if (previousEnabledPanel != null)
		{
			previousEnabledPanel.SetActive(false);
		}
		if (panel)
		{
			if (panel.activeSelf)
			{
				panel.SetActive(false);
				previousEnabledPanel = null;

			}
			else
			{
				panel.SetActive(true);
				previousEnabledPanel = panel;

			}
		}
	}
}
