using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace com.blackantgames.managers
{

    /// <summary>
    /// Monster class provider
    /// </summary>
    public class MonsterCharacterProvider : MonoBehaviour
    {
        [SerializeField] private GameObject[] monsterList;

        /// <summary>
        /// Provides a monster GameObject according to the indicated monster index
        /// </summary>
        /// <param name="index">Index of the monster</param>
        /// <returns>Monster attached to the indicated monster index, null if index is not valid</returns>
        public GameObject GetMonsterByIndex(int index)
        {
            if (index >= 0 && index < monsterList.Length) {
                return monsterList[index];
            } else
            {
#if UNITY_EDITOR
                Debug.LogError("Error: Requested an invalid monster index " + index + ". Current amount of monsters: " + monsterList.Length);
#endif
                return null;
            }
        }
    }
}