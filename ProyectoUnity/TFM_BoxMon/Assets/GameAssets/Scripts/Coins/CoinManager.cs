using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.blackantgames.playerstoragesystem;
using TMPro;

namespace com.blackantgames.managers
{
    [RequireComponent(typeof(AudioSource))]
    public class CoinManager : MonoBehaviour
    {
        [SerializeField] private AudioClip normalCoinClip;
        [SerializeField] private AudioClip goldenCoinClip;
        [SerializeField] private TMP_Text normalCoinCounter;
        [SerializeField] private TMP_Text goldenCoinCounter;

        private AudioSource audioSource;
        private int numOfNormalCoins = 0;
        private int numOfGoldenCoins = 0;

		private void Awake()
		{
            audioSource = GetComponent<AudioSource>();
            NormalCoinCounterUIUpdate(numOfNormalCoins);
            GoldenCoinCounterUIUpdate(numOfGoldenCoins);

#if UNITY_EDITOR
            if (normalCoinClip == null)
            {
                Debug.LogError("CoinManager has no clip assigned for the normal coin");
            }
			if (goldenCoinClip == null)
			{
				Debug.LogError("CoinManager has no clip assigned for the golden coin");
			}
            if (audioSource == null)
            {
                Debug.LogError("Coin Manager has no access to any audiosource");
            }
#endif
		}

        private void NormalCoinCounterUIUpdate(int newNum)
        {
            if (normalCoinCounter != null)
            {
                normalCoinCounter.text = newNum.ToString();
            }
        }

        private void GoldenCoinCounterUIUpdate(int newNum) 
        {
            if (goldenCoinCounter != null)
            {
                goldenCoinCounter.text = newNum.ToString();
            }
        }

		public void AddCoin(){
            PlayerPreferencesDataBase.SetCoinsToLoad(PlayerPreferencesDataBase.GetCoinsToLoad() + 1);
            if (normalCoinClip != null)
            {
				audioSource.PlayOneShot(normalCoinClip);
                numOfNormalCoins++;
                NormalCoinCounterUIUpdate(numOfNormalCoins);
            }
		}

        public void AddIdGoldenCoin(string ID){
            PlayerPreferencesDataBase.SetIdGCToLoad(ID);
            PlayerPreferencesDataBase.SetGCToLoad(PlayerPreferencesDataBase.GetGoldenCoinsCount() + 1);
            if (goldenCoinClip != null)
            {
                audioSource.PlayOneShot(goldenCoinClip);
                numOfGoldenCoins++;
                GoldenCoinCounterUIUpdate(numOfGoldenCoins);
            }
        }

        public int GetIdGoldenCoin(string ID){
            return PlayerPreferencesDataBase.GetIdGCToLoad(ID);
        }
    }
}
