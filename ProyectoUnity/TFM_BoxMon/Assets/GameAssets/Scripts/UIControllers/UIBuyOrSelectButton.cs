using com.blackantgames.playerstoragesystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using com.blackantgames.character.controller;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class UIBuyOrSelectButton : MonoBehaviour
{
    [SerializeField] private int boximonId;
    [SerializeField] private TMPro.TMP_Text buySelectText;
	[SerializeField] private TMPro.TMP_Text normalCoincostText;
    [SerializeField] private TMPro.TMP_Text goldenCoinCostText;
    [SerializeField] private Image normalCoinImage;
    [SerializeField] private Image goldenCoinImage;

	[SerializeField] private int normalCoinsCost;
    [SerializeField] private int goldenCoinsCost;

    [SerializeField] private AudioClip notEnoughCoinsClip;
    [SerializeField] private AudioClip boughtClip;
    [SerializeField] private AudioClip selectedClip;
    [SerializeField] private Animator characterAnimator;

	private AudioSource audioSource;
    UIBuyOrSelectButton[] notifyComponents;


	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();
        notifyComponents = FindObjectsOfType<UIBuyOrSelectButton>();
	}

	// Start is called before the first frame update
	void Start()
    {
        UpdateButtonAction();
	}

    public void UpdateButtonAction()
    {
        // Ensure that Character ID 0 is always available
        if (boximonId == 0 && !MonsterStoreController.IsCharacterBought(0)) {
            Debug.Log("Initial buy of the 0 boximon");
            normalCoinsCost = 0;
            goldenCoinsCost = 0;
			MonsterStoreController.BuyBoxMonWithID(0, 0, 0);
        }

		if (MonsterStoreController.GetSelectedBoximonId().Equals(this.boximonId))
		{
			buySelectText.text = "SELECTED!";
			normalCoinImage.gameObject.SetActive(false);
			goldenCoinImage.gameObject.SetActive(false);
			normalCoincostText.gameObject.SetActive(false);
			goldenCoinCostText.gameObject.SetActive(false);
		}
		else if (MonsterStoreController.IsCharacterBought(boximonId))
		{
            buySelectText.text = "Select";
            normalCoinImage.gameObject.SetActive(false);
            goldenCoinImage.gameObject.SetActive(false);
            normalCoincostText.gameObject.SetActive(false);
            goldenCoinCostText.gameObject.SetActive(false);
		} 
        else if (!MonsterStoreController.CanBuyItem(normalCoinsCost, goldenCoinsCost))
		{
			buySelectText.text = "No coins!";
            normalCoincostText.text = normalCoinsCost.ToString();
            goldenCoinCostText.text = goldenCoinsCost.ToString();
		}
		else
		{
			buySelectText.text = "Buy";
			normalCoincostText.text = normalCoinsCost.ToString();
			goldenCoinCostText.text = goldenCoinsCost.ToString();
		}
	}

    /// <summary>
    /// Click action to be triggered
    /// </summary>
    public void ClickAction()
    {
		if (MonsterStoreController.IsCharacterBought(boximonId))
		{
			MonsterStoreController.SelectBoximonId(boximonId);
			PlayFeedback(selectedClip);
			if (characterAnimator)
			{
				characterAnimator.SetTrigger(AC_CharacterUtils.AC_TRIGGER_SELECTED);
			}
		}
		else if (!MonsterStoreController.CanBuyItem(normalCoinsCost, goldenCoinsCost))
        {
            PlayFeedback(notEnoughCoinsClip);
            characterAnimator.SetTrigger(AC_CharacterUtils.AC_TRIGGER_CANNOT_BUY);
        }
        else
        {
			MonsterStoreController.BuyBoxMonWithID(boximonId, normalCoinsCost, goldenCoinsCost);
            PlayFeedback(boughtClip);
			if (characterAnimator)
			{
				characterAnimator.SetTrigger(AC_CharacterUtils.AC_TRIGGER_BOUGHT);
			}
		}

        foreach (UIBuyOrSelectButton listener in notifyComponents)
        {
            listener.UpdateButtonAction();
        }
	}

    /// <summary>
    /// Play a feedback sound clip for the interaction with the UI
    /// </summary>
    /// <param name="feedbackClip">Clip to play</param>
    public void PlayFeedback(AudioClip feedbackClip)
    {
        if (audioSource != null && feedbackClip!= null)
        {
            audioSource.PlayOneShot(feedbackClip);
        }
    }
}
