using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.blackantgames.playerstoragesystem;

public class CurrencyProxyManager : MonoBehaviour
{
    public static CurrencyProxyManager INSTANCE;
	private List<ICurrencyStoreListener> currencyListeners = new List<ICurrencyStoreListener>();

	private void Awake()
	{
		if (INSTANCE != null)
		{
			Destroy(this);
			return;
		}

		INSTANCE = this;
	}

	public static CurrencyProxyManager GetInstance()
	{
		return INSTANCE;
	}

	public void AddCurrencyListener(ICurrencyStoreListener listener)
	{
		if (!currencyListeners.Contains(listener))
		{
			currencyListeners.Add(listener);
			listener.NotifyNormalEggCount(PlayerPreferencesDataBase.GetCoinsToLoad());
			listener.NotifyGoldenEggCount(PlayerPreferencesDataBase.GetGoldenCoinsCount());
		}
	}

	public void RemoveCurrencyListener(ICurrencyStoreListener listener)
	{
		currencyListeners.Remove(listener);
	}

	public void NotifyNormalEggChange(int currentNormalEggCount)
	{
		foreach(ICurrencyStoreListener listener in currencyListeners)
		{
			listener.NotifyNormalEggCount(currentNormalEggCount);
		}
	}

	public void NotifyGoldenEggChange(int goldenEggCount)
	{
		foreach (ICurrencyStoreListener listener in currencyListeners)
		{
			listener.NotifyGoldenEggCount(goldenEggCount);
		}
	}

}
