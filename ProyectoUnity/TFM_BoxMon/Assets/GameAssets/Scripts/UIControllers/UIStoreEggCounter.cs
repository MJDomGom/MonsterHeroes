using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIStoreEggCounter : MonoBehaviour, ICurrencyStoreListener
{
	[SerializeField] TMP_Text normalCoinsText;
	[SerializeField] TMP_Text goldenCoinsText;
	[SerializeField] private UIGoldenPanelAnimater goldenPanelAnimator;

	private int currentNormalEggCount = 0;
	private int currentGoldenEggCount = 0;
	
	public void NotifyGoldenEggCount(int goldenEggCount)
	{
		currentGoldenEggCount = goldenEggCount;
		UpdateText();
		if (goldenPanelAnimator != null)
		{
			goldenPanelAnimator.TriggerGoldenCoinAnimator();
		}
	}

	public void NotifyNormalEggCount(int normalEggCount)
	{
		currentNormalEggCount = normalEggCount;
		UpdateText();
	}

	private void UpdateText()
	{
		normalCoinsText.text = currentNormalEggCount.ToString();
		goldenCoinsText.text = currentGoldenEggCount.ToString();
	}

	// Start is called before the first frame update
	void Start()
    {
		if(CurrencyProxyManager.GetInstance() != null)
		{
			CurrencyProxyManager.GetInstance().AddCurrencyListener(this);
		}
    }
}
