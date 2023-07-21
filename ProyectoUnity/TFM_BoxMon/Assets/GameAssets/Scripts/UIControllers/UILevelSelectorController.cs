using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using com.blackantgames.playerstoragesystem;
using UnityEngine.UI;

public class UILevelSelectorController : MonoBehaviour
{
    [SerializeField] private int levelId;
    [SerializeField] private TMPro.TMP_Text levelText;
    [SerializeField] private Image levelSelectorImage;
    [SerializeField] private AudioSource uiAudioSource;
    [SerializeField] private AudioClip uiNegativeAudioClip;

    private bool isLevelLoadable = false;
    private UIActions uiActions;

    // Start is called before the first frame update
    void Start()
    {
        uiActions = FindObjectOfType<UIActions>();
        if (LevelSelectionController.IsLevelLoadable(levelId)) {
            levelText.SetText("Level " + (levelId + 1));
            levelText.color = Color.black;
            levelSelectorImage.color = Color.white;
            isLevelLoadable = true;
        } else {
			levelText.SetText("Blocked");
            levelText.color = Color.white;
            levelSelectorImage.color = Color.grey;
            isLevelLoadable = false;
		}
    }

    public void OnClickAction()
    {
        if (isLevelLoadable)
        {
            LevelSelectionController.SetLevelToLoad(levelId);
            uiActions.StartGame();
		} else
        {
            uiAudioSource.PlayOneShot(uiNegativeAudioClip);

		}
    }
}
