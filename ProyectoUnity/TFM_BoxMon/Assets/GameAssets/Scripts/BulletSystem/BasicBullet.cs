using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.blackantgames.damageable;

namespace com.blackantgames.bullets
{
	/// <summary>
	/// Class that represents a basic bullet
	/// 
	/// Basic bullet will impact any object in the space and will cause a
	/// hit impact effect on the impacted element
	/// </summary>
	public class BasicBullet : MonoBehaviour
	{
		[Header("References")]
		[Tooltip("Time while the bullet will be in the scene")]
		[SerializeField] protected float livingTime = 5.0f;

		[Tooltip("Impact position point. Child transform used to calculate the bullet impact with other objects")]
		[SerializeField] protected Transform impactPositionPoint;

		[Header("Values")]
		[Tooltip("Bullet movement speed")]
		[SerializeField] protected float bulletSpeed = 100.0f;
		[SerializeField] protected LayerMask hitLayer;
		[SerializeField] protected BulletFX bulletImpactFX;
		[SerializeField] protected int bounces = 0;

		[Header("Bullet pool has been disabled. This is a workaround until the BP is re-enabled")]
		[SerializeField] protected bool destroyBullAfterImpact = true;

		protected Vector3 lastPosition = Vector3.zero;
		protected float gunBulletDamage = 1f;
		protected float defaultBulletSpeed = 200f;
		protected int remainingBounces;

		private void Awake()
		{
#if UNITY_EDITOR
			if (bulletImpactFX == null)
			{
				Debug.LogWarning("ERROR: Bullet " + transform.name + " has no bullet impact FX assigned");
			}

			if (impactPositionPoint == null)
			{
				Debug.LogError("ERROR: Bullet " + transform.name + " has no bullet impact position assigned");
			}
#endif

			defaultBulletSpeed = bulletSpeed;
			this.bulletSpeed = defaultBulletSpeed;
		}

		private void OnEnable()
		{
			lastPosition = impactPositionPoint.position;
			remainingBounces = bounces;
			Invoke("DisableBullet", livingTime);
		}

		private void OnDisable()
		{
			// If bullet was disabled already, dont disable it again
			CancelInvoke();
		}

		public void DisableBullet()
		{
			gameObject.SetActive(false);
		}

		private void Update()
		{
			moveBullet();
			processBulletImpact();
			changeLastPosition();
		}

		/// <summary>
		/// Process the bullet impact on any element of the world
		/// </summary>
		protected virtual void processBulletImpact()
		{
			Vector3 bulletDirection = impactPositionPoint.position - lastPosition;
			RaycastHit hit;
			if (Physics.Raycast(lastPosition, bulletDirection.normalized, out hit, bulletDirection.magnitude, hitLayer))
			{
				BulletImpactInfoProvider impactProvider = hit.transform.GetComponent<BulletImpactInfoProvider>();
				if (impactProvider != null)
				{
					// Show the bullet impact effect
					BulletFX.TriggerImpactInfo(impactProvider.GetBulletFX().GetImpactInfo(), hit.point, Quaternion.LookRotation(hit.normal), hit.transform);
				}
				
				// Default efffect is not attached to its parent
				BulletFX.TriggerImpactInfo(bulletImpactFX.GetImpactInfo(), hit.point, Quaternion.LookRotation(hit.normal), hit.transform);

				IDamageable hittedDamageable = hit.transform.GetComponent<BasicDamageable>();
				if (hittedDamageable != null)
				{
					if (hittedDamageable.IsAlive())
					{
						hittedDamageable.KillDamageable();
					}
				}

				// After hit something, the bullet must be disabled
				if (remainingBounces == 0)
				{
					DisableBullet();

					// TODO - Workaround to avoid a memory leak due to the fact that the
					// bullet's object pool has not been imported in this project
					if(destroyBullAfterImpact)
					{
						Destroy(this.gameObject);
					}
				} else
				{
					remainingBounces--;
					transform.rotation = Quaternion.LookRotation(
						Vector3.Reflect(transform.forward.normalized, hit.normal));
					transform.position = hit.point;
					lastPosition = hit.point;
				}
			}
		}

		/// <summary>
		/// Performs the bullet movement
		/// </summary>
		protected virtual void moveBullet()
		{
			transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);
		}

		/// <summary>
		/// Change the last known position of the bullet to the current bullet position
		/// </summary>
		protected virtual void changeLastPosition()
		{
			lastPosition = impactPositionPoint.position;
		}

		/// <summary>
		/// Enable the bullet and configure it
		/// </summary>
		/// <param name="position"> Position where the bullet is respawned</param>
		/// <param name="rotation"> Rotation of the bulled when respawned</param>
		public void EnableBullet(Vector3 position, Quaternion rotation)
		{
			// Enable gameobject and set the current position and rotation
			this.gameObject.SetActive(true);
			transform.position = position;
			transform.rotation = rotation;

			// Avoid wrong position calculations, make last and current position to be the same
			lastPosition = impactPositionPoint.position;
		}


		/// <summary>
		/// Set the bullet information, indicating the damage that the bullet must apply, the kind of damage that the bullet can
		/// apply and the team that should be hurted by that bullet
		/// </summary>
		/// <param name="bulletDamage">Damage that must be performed on the hitted object</param>
		public void SetBulletInfo(float bulletDamage, float bulletSpeed, bool isPlayerBullet)
		{
			this.bulletSpeed = bulletSpeed;
			gunBulletDamage = bulletDamage;
		}
	}
}
