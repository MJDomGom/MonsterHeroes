using com.blackantgames.character.controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.blackantgames.damageable
{
    /// <summary>
    /// Represents a point of contact damager
    /// This component is able to damage others when touching them, but will
    /// not damage the salf damageable.
    /// 
    /// This object must be in the same hierarchy as the IDamageable that the
    /// object must not damage
    /// </summary>
    public class DamagerPointOfContact : MonoBehaviour
    {
        [SerializeField] private LayerMask damageableLayerMask;
        [SerializeField] private float damageableRadius;

        private IDamageable selfDamageable;
        private PlayerCharacterController characterController;

		private void Awake()
		{
            Transform damageableSearcher = transform;
            while (damageableSearcher != null && (selfDamageable == null || characterController != null)) {
                if (selfDamageable == null)
                {
                    selfDamageable = damageableSearcher.GetComponent<IDamageable>();
                }

                if (characterController == null)
                {
                    characterController = damageableSearcher.GetComponent<PlayerCharacterController>();
                }

				damageableSearcher = damageableSearcher.parent;
            }

#if UNITY_EDITOR
            if (selfDamageable == null) {
                Debug.LogError("DamagerPointOfContact " + transform.name + " could not find a IDamageable in the parent's hierarchy");
            }
#endif
        }

		private void Update()
		{
			Collider[] hittedComponents = Physics.OverlapSphere(transform.position, damageableRadius, damageableLayerMask);
            foreach(Collider col in hittedComponents)
            {
                IDamageable otherDamageable = col.GetComponent<IDamageable>();
                if (otherDamageable != selfDamageable) {
                    otherDamageable.KillDamageable();

                    if (characterController != null)
                    {
                        characterController.PerformJumpFeedback();
                    }
                }
            }
		}

	}
}