using com.blackantgames.scenenavigation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISplashTextAnimation : MonoBehaviour
{
    [Tooltip("Reference to the text transition material")]
	[SerializeField] Material textTransitionMaterial;

    [Tooltip("Current progress. Note: This variable is exposed to be used from the Animation")]
    public float currentProgress;

	private string materialPropertyName = "_progress";


	/// <summary>
	/// Load the main menu. This function should be called from the
	/// Splash Scene animation
	/// </summary>
	public void LoadMainMenu()
    {
        SceneNavigator.GetInstance().OpenScene_OfflineWithLoadingTransition(SceneNavigator.EGameLevels.MAIN_MENU);
    }

	private void Update()
	{
		textTransitionMaterial.SetFloat(materialPropertyName, currentProgress);
	}

}
