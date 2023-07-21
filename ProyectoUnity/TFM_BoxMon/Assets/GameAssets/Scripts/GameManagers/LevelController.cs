using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.blackantgames.managers
{

    /// <summary>
    /// Auxiliary class that represents the main common artifacts of a level to load and
    /// remove it.
    /// This object is used to load and download the levels
    /// </summary>
    public class LevelController : MonoBehaviour
    {
        [SerializeField] private LevelController nextLevel;
        [SerializeField] private Transform nextLevelAttachmentPoint;
        [SerializeField] private CharacterSpawnPoint levelSpawnPoint;

		public LevelController GetNextLevel()
        {
            return nextLevel;
        }

        public bool IsFinalLevel()
        {
            return nextLevel == null;
        }

        public Transform GetNextLevelAttachmentPoint()
        {
            return nextLevelAttachmentPoint;
        }

        public CharacterSpawnPoint GetLevelSpawnPoint()
        {
            return levelSpawnPoint;
        }
    }
}