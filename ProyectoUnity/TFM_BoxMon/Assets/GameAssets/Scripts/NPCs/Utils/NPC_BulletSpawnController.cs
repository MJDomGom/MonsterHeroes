using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.blackantgames.bullets;

namespace com.blackantgames.npc.utils
{
    [RequireComponent(typeof(AudioSource))] 
    public class NPC_BulletSpawnController : MonoBehaviour
    {
        [Tooltip("Reference to the bullet to shoot by this character")]
        [SerializeField] private BasicBullet bulletToShoot;

        [Tooltip("Reference to the shoot FX effect")]
        [SerializeField] private GameObject shootFXEffect;

        [Tooltip("Reference to the possible audio clip to play when shooting")]
        [SerializeField] private AudioClip shootClip;

		private void Awake()
		{
            
#if UNITY_EDITOR
            if (bulletToShoot == null)
            {
                Debug.LogError("ERROR: Basic bullet to shoot is not assigned to the " + transform.name + " BasicBulletSpawnController");
            }
#endif
        }

        /// <summary>
        /// Trigger the shoot effect, including the shoot of the bullet, visuals and audio effects
        /// for the shoot
        /// </summary>
		public void Shoot()
        {
            // Instantiate the bullet
            BasicBullet shootedBullet = Instantiate<BasicBullet>(bulletToShoot, transform.position, transform.rotation, null);
            shootedBullet.SetBulletInfo(1, 10f, false);

            // Trigger the FX effectW
            Instantiate(shootFXEffect, transform.position, transform.rotation, null);

            // Play the shoot audio effect
            AudioSource.PlayClipAtPoint(shootClip, transform.position);
        }
    }
}