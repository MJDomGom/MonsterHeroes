using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.blackantgames.playerstoragesystem
{
	/// <summary>
	/// Class to handle the Player Preferences database
	/// This class will be responsible to store and load data using the
	/// internal Player Preferences, avoiding the direct usage of the
	/// PlayerPrefs to ensure the safety access to all the PlayerPrefs
	/// storage system.
	/// </summary>
	public static class PlayerPreferencesDataBase
	{


		private static readonly string SCENE_TO_LOAD = "LTLSTRVAL";         // Level to load preferences key
		private static readonly string AMOUNT_OF_COINS = "AOFTSIDB";        // Amount of coins to storage in DB
		private static readonly string AMOUNT_OF_GOLDEN_COINS = "AOFGCIDB";	// Database ID for the Amount of golden coins
		private static readonly string ID_GOLDENEGG = "IFGEEFNR";           // Id for Golden Eggs for no respawn

		/// <summary>
		/// Set the level to be loaded from the LOADING scene
		/// </summary>
		/// <param name="levelToLoad">Level ID to be loaded from the LOADING scene</param>
		public static void SetLevelToLoad(int levelToLoad)
		{
			PlayerPrefs.SetInt(SCENE_TO_LOAD, levelToLoad);
		}

		/// <summary>
		/// Provides the level to load from the LOADING scene
		/// </summary>
		/// <param name="defaultScene">Default scene to load in case that the SCENE_TO_LOAD is not set</param>
		/// <returns>Level ID to load from the LOADING scene</returns>
		public static int GetLevelToLoad(int defaultScene)
		{
			return PlayerPrefs.GetInt(SCENE_TO_LOAD, defaultScene);
		}

		/// <summary>
		/// Provides the amount of coins that the playeer have
		/// </summary>
		public static int GetCoinsToLoad() {
			return PlayerPrefs.GetInt(AMOUNT_OF_COINS, 0);
		}

		/// <summary>
		/// Set the coins for the player, after the player pick a coin
		/// </summary>
		/// <param name="coins">Total of coins that the player have</param>
		public static void SetCoinsToLoad(int coins) {
			PlayerPrefs.SetInt(AMOUNT_OF_COINS, coins);
			if (CurrencyProxyManager.GetInstance() != null)
			{
				CurrencyProxyManager.GetInstance().NotifyNormalEggChange(coins);
			}
		}

		/// <summary>
		/// Provide the count of golden coins that were collected by the user
		/// </summary>
		/// <returns>Amount of golden coins collected by the user</returns>
		public static int GetGoldenCoinsCount()
		{
			return PlayerPrefs.GetInt(AMOUNT_OF_GOLDEN_COINS, 0);
		}

		/// <summary>
		/// Update the amount of GCs picked by the user
		/// </summary>
		/// <param name="coins">Amount of golden coins to change</param>
		public static void SetGCToLoad(int coins)
		{
			PlayerPrefs.SetInt(AMOUNT_OF_GOLDEN_COINS, coins);
			if (CurrencyProxyManager.GetInstance() != null)
			{
				CurrencyProxyManager.GetInstance().NotifyGoldenEggChange(coins);
			}
		}

		/// <summary>
		/// Provides the id of the golden eggs(coins)
		/// </summary>
		public static int GetIdGCToLoad(string id) {
			return PlayerPrefs.GetInt(ID_GOLDENEGG + id, 0);
		}

		/// <summary>
		/// Set the id of the golden eggs(coins).
		/// </summary>
		/// <param name="id">ID for the golden eggs that the player pick up. With this ID, the golden egg not gonna respawn if the player plays the level again</param>
		public static void SetIdGCToLoad(string id) {
			PlayerPrefs.SetInt(ID_GOLDENEGG + id, 1);
		}
		
		/// <summary>
		/// Delete all the data in the player prefs.
		/// </summary>
		public static void DeletePlayerPrefs(){
			PlayerPrefs.DeleteAll();
		}
	}
}
