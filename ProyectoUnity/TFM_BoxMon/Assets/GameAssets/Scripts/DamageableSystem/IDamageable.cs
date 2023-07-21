namespace com.blackantgames.damageable
{
	/// <summary>
	/// Class that represents the behacviour of a Damageable component
	/// </summary>
    public interface IDamageable
    {

		/// <summary>
		/// Damage the object according to the indicated ammount of damage
		/// </summary>
		/// <param name="damage">Damage amount that the object receives</param>
		void KillDamageable();

		/// <summary>
		/// Indicates if the damageable object is still alive or not
		/// </summary>
		/// <returns>True if not fully damaged, false otherwise</returns>
		bool IsAlive();

		/// <summary>
		/// Attach a new Damageable Listener to the current Damageable element
		/// </summary>
		/// <param name="damageableListener">Damageable listener to attach to the object</param>
		void AttachDamageableListener(IDamageableListener damageableListener);

		/// <summary>
		/// Remove a damageable listener from the list of Damageable Listeners attached to the current Damageable Component
		/// Note: The Damageable listener will be removed if, and only if, it is attached to the Damageable Component
		/// </summary>
		/// <param name="damageableListener">Damageable listener to be removed from the Damagable listener´s list</param>
		void RemoveDamageableListener(IDamageableListener damageableListener);
	}

}