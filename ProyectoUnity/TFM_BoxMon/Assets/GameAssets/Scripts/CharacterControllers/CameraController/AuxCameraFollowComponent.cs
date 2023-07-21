using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.blackantgames.character.controller
{
    /// <summary>
    /// Auxiliary class that will follow the position of a specific transform
    /// in the game. The objective of this class is to provide to Cinemachine a
    /// specific component to follow during the game
    /// </summary>
    public class AuxCameraFollowComponent : MonoBehaviour
    {
        [Tooltip("Multiplier to be applied to the speed with which the AuxCameraFollow component must follow the target")]
        [SerializeField] private float cameraFollowSpeedMultiplier = 3.0f;

        [Tooltip("Offset to be applied between the camera and the player character")]
        [SerializeField] private Vector3 cameraFollowTargetOffset = Vector3.zero;

        // Transform that will be tracked by this game object
        private Transform trackedTransform;

        /// <summary>
        /// Set the transform to be tracked
        /// </summary>
        /// <param name="trackedTransform">Transform to be tracked</param>
        public void SetTrackedTransform(Transform trackedTransform)
        {
            this.trackedTransform = trackedTransform;
        }


        void Update()
        {
            // Follow the target only if set
            if (trackedTransform != null)
            {
                transform.position = Vector3.Slerp(transform.position, trackedTransform.position + cameraFollowTargetOffset, Time.deltaTime * cameraFollowSpeedMultiplier);
            }
        }
    }
}