using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.blackantgames.playerstoragesystem
{
    public static class LevelSelectionController
    {
        private static readonly string LEVEL_NAME_PREFIX = "lvtbl";
        private static int levelToLoad = 0;

        /// <summary>
        /// Indicates that a specific level has been completed
        /// </summary>
        /// <param name="levelCompleted"></param>
        public static void SetLevelComplete(int levelCompleted)
        {
            PlayerPrefs.SetInt(LEVEL_NAME_PREFIX + levelCompleted, 1);
        }

        /// <summary>
        /// Indicates if a specific level can be loaded or not
        /// </summary>
        /// <param name="level">Level to load</param>
        /// <returns>True if the level is loadable, false otherwise</returns>
        public static bool IsLevelLoadable(int level) {
            if (level == 0) return true;
            else return (PlayerPrefs.GetInt(LEVEL_NAME_PREFIX + (level - 1), 0) > 0);
        }

        public static void SetLevelToLoad(int levelGroupToLoad)
        {
			levelToLoad = levelGroupToLoad;
        }

        public static int GetLevelToLoad()
        {
            return levelToLoad;
        }
    }
}