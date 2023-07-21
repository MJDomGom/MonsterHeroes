using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.blackantgames.ui.widgets
{

    public class UILifeIndicator : MonoBehaviour
    {
        [Tooltip("Reference to the")]
        [SerializeField] private GameObject[] lifeIndicators;

        public void SetAmountOfLifeIndicatore(int amount)
        {
            if (amount < 0 || amount > lifeIndicators.Length)
            {
                return;
            } else
            {
                for (int i=0; i<lifeIndicators.Length; i++)
                {
                    lifeIndicators[i].SetActive((amount > i));
                }
            }
        }
    }
}