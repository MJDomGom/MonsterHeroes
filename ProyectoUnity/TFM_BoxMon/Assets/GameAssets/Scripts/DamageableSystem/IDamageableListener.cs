using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.blackantgames.damageable
{
	/// <summary>
	/// Interface to control the current damageable status of the Damageable component attached to the object.
	/// This interface is used to implement a Observer pattern. The Damageable Component should be responsible to notify the
	/// listener, so this can act in consequence.
	/// </summary>
	public interface IDamageableListener
	{

		/// <summary>
		/// Notifies that the player died and lost some lifes
		/// </summary>
		/// <param name="currentLifes">Current damageable object amount of lifes</param>
		void NotifyDamage();

		/// <summary>
		/// Notifies that the current object has been healed and provide the current amount of lifes attached to the Damageable component of the object
		/// </summary>
		/// <param name="currentLifes">Current amount of lifes after healing</param>
		void NotifyHeal(int currentLifes);

	}

}
