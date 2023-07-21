using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.blackantgames.playerstoragesystem;

namespace com.blackantgames.managers
{

    public class LevelLoaderManager : MonoBehaviour
    {
        [Tooltip("Reference to the current level to load")]
        [SerializeField] private LevelController[] levelGroupsStarter = null;
		[SerializeField] private LevelController currentLevel = null;

		private LevelController previousLevel = null;
        private LevelController nextLevel = null;


		private void Awake()
		{
#if UNITY_EDITOR
            if (currentLevel == null) {
                Debug.LogError(
                    "Error: LevelLoaderManager " 
                    + transform.name 
                    + " has no level current level assigned. Level loading is not expected to work!");
            }
#endif

        }

		private void Start()
		{
			// Previous level doesn't exists
			previousLevel = null;
			// Load the current level
			this.currentLevel = Instantiate(levelGroupsStarter[LevelSelectionController.GetLevelToLoad()]);

			// Load the next level
			nextLevel = LoadNextLevel();
		}

		/// <summary>
		/// Load the next level if it exists
		/// </summary>
		private LevelController LoadNextLevel()
        {
            if (!currentLevel.IsFinalLevel())
            {
				return Instantiate<LevelController>(
                    currentLevel.GetNextLevel(),
                    currentLevel.GetNextLevelAttachmentPoint().position,
                    Quaternion.identity,
                    null);
            }
            return null;
        }

        /// <summary>
        /// Indicates that the current level end was reached
        /// </summary>
        public void EndOfLevelreached()
        {
            if (nextLevel != null)
            {
                if (previousLevel != null)
                {
                    Destroy(previousLevel.gameObject);
                    previousLevel = null;
                }

                previousLevel = currentLevel;
                currentLevel = nextLevel;
                nextLevel = LoadNextLevel();
            }
		}

        /// <summary>
        /// Provides the current level's character spawn point
        /// </summary>
        /// <returns>Current level's character spawn point</returns>
        public CharacterSpawnPoint GetCurrentSpawnPoint()
        {
            return currentLevel.GetLevelSpawnPoint();
        }
    }
}