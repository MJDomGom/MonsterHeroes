using UnityEngine;


namespace com.blackantgames.damageable
{
    /// <summary>
    /// Damage on contact
    /// When other collider enters in contact with this one, if the other has
    /// a Damageable Component, it will be damaged
    /// </summary>
    public class DamageOnContact : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            IDamageable damageable = other.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.KillDamageable();
            }
        }
    }
}