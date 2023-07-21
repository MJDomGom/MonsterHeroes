using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.blackantgames.playerstoragesystem {

	public static class MonsterStoreController
	{
		private static readonly string BOX_MON_BOUGHT_PREFIX = "BMB_";      // Prefix for the bought state of the boximon
		private static readonly string SELECTED_BOX_MON_ID = "BM_ID";       // Indicates the selected boximon ID

		/// <summary>
		/// Indicates if a specific item can be bought or not according to the current amount of coins that
		/// the character has
		/// </summary>
		/// <param name="normalEggCost">Normal coins cost</param>
		/// <param name="goldenEggCost">Golden coins cost</param>
		/// <returns>True if the character can be bought, false otherwise</returns>
		public static bool CanBuyItem(int normalEggCost, int goldenEggCost)
		{
			return PlayerPreferencesDataBase.GetCoinsToLoad() >= normalEggCost && PlayerPreferencesDataBase.GetGoldenCoinsCount() >= goldenEggCost;
		}

		/// <summary>
		/// Buy a specific boximon and store its state
		/// </summary>
		/// <param name="id"></param>
		public static void BuyBoxMonWithID(int id, int normalEggCost, int goldenEggCost)
		{
			if (!IsCharacterBought(id) && CanBuyItem(normalEggCost, goldenEggCost))
			{
				PlayerPreferencesDataBase.SetCoinsToLoad(PlayerPreferencesDataBase.GetCoinsToLoad() - normalEggCost);
				PlayerPreferencesDataBase.SetGCToLoad(PlayerPreferencesDataBase.GetGoldenCoinsCount() - goldenEggCost);
				PlayerPrefs.SetInt(BOX_MON_BOUGHT_PREFIX + id, 1);
			}
		}

		/// <summary>
		/// Select a specific boximon to use in the game by ID
		/// </summary>
		/// <param name="id">ID of the selected boximon</param>
		public static void SelectBoximonId(int id)
		{
			PlayerPrefs.SetInt(SELECTED_BOX_MON_ID, id);
		}

		/// <summary>
		/// Get the selected boximon ID to use in the game
		/// </summary>
		public static int GetSelectedBoximonId()
		{
			return PlayerPrefs.GetInt(SELECTED_BOX_MON_ID, 0);
		}

		/// <summary>
		/// Indicates if the character can be bought
		/// </summary>
		/// <returns>True if the character can be bought, false otherwise</returns>
		public static bool IsCharacterBought(int id)
		{
			return PlayerPrefs.GetInt(BOX_MON_BOUGHT_PREFIX + id, 0) != 0;
		}
	}
}