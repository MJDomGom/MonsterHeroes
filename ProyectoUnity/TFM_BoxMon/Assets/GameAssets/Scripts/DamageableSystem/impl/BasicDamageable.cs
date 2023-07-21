using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.blackantgames.damageable
{
	/// <summary>
	/// Class that represents the basic damageable behaviour that every damageable must
	/// exhibit.
	/// </summary>
    public class BasicDamageable : MonoBehaviour, IDamageable
    {

        [Tooltip("Maximum life")]
        [SerializeField] protected int maxLife = 1;

		[Tooltip("Initial life")]
		[SerializeField] protected int amountOfLifes = 1;

        protected List<IDamageableListener> _damageableListeners = new List<IDamageableListener>();


		#region IDamageable functionality

		public virtual void KillDamageable()
		{
			amountOfLifes--;
			NotifyDamage();
		}

		public bool IsAlive()
		{
			return amountOfLifes > 0;
		}

		public void AttachDamageableListener(IDamageableListener damageableListener)
		{
			if (!_damageableListeners.Contains(damageableListener))
			{
				_damageableListeners.Add(damageableListener);
			}
		}

		public void RemoveDamageableListener(IDamageableListener damageableListener)
		{
			if (_damageableListeners.Contains(damageableListener))
			{
				_damageableListeners.Remove(damageableListener);
			}
		}

		#endregion

		#region Damageable private functionality

		/// <summary>
		/// Notify all the listeners that the current life of the Damageable object has been decreased
		/// </summary>
		private void NotifyDamage()
		{
			for (int i = 0; i < _damageableListeners.Count; i++)
			{
				_damageableListeners[i].NotifyDamage();
			}
		}

		#endregion
	}
}