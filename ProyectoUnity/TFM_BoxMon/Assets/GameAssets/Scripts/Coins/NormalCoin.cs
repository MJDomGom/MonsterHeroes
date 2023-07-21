using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.blackantgames.managers
{
    public class NormalCoin : MonoBehaviour, ICoinListener
    {
        [Header("Coins Settings")]
        [Tooltip(
            "Parameters for differents coins, such value and if the coin in respawneable after ending the level"
        )]
        [SerializeField]
        public int value = 1;
        protected List<ICoinListener> _coinsListeners = new List<ICoinListener>();

        void OnTriggerEnter(Collider Col)
        {
            if (Col.gameObject.tag == "Player")
            {
                //Call GameManager to add up the coin pickup and call the database to store
                GameManager.GetInstance().GetCoinManager().AddCoin();
                NotifyPickupCoin();
                Destroy(this.gameObject);
            }
        }

        public void NotifyPickupCoin()
        {
            for (int i = 0; i < _coinsListeners.Count; i++)
            {
                _coinsListeners[i].NotifyPickupCoin();
            }
        }
    }
}
