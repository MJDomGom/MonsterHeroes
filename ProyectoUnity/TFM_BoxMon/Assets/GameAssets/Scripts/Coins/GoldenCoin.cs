using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.blackantgames.managers
{
    public class GoldenCoin : MonoBehaviour, ICoinListener
    {
        [Header("Golden Eggs Settings")]
        [Tooltip("Parameters for Golden Eggs")]
        [SerializeField]
        public string ID;
        protected List<ICoinListener> _coinsListeners = new List<ICoinListener>();

        //TODO, LA INSTANCIA DEL GAME MANAGER ME DEVUELVE NULL EN EL AWAKE
        
        private void Start() {
            //Debug.Log(GameManager.GetInstance().GetCoinManager().GetIdGoldenCoin(ID));
            //Debug.Log(ID);
            if((GameManager.GetInstance().GetCoinManager().GetIdGoldenCoin(ID)) != 0){
                Destroy(this.gameObject);
            }
        }

        void OnTriggerEnter(Collider Col)
        {
            if (Col.gameObject.tag == "Player")
            {
                GameManager.GetInstance().GetCoinManager().AddIdGoldenCoin(ID);

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