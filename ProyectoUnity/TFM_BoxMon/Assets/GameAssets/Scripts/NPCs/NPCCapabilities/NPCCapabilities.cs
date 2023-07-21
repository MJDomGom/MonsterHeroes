using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace com.blackantgames.npc.ai {

    /// <summary>
    /// This class represents the capabilities of a specific NPC in the game,
    /// indicating the kind of actions that the character can perform
    /// </summary>
    [CreateAssetMenu(fileName = "NPCCability", menuName = "AI/NPCCapabilities")]
    public class NPCCapabilities : ScriptableObject
    {
        // Set of capabilities that a monster implementation can have
        [Header("NPC Capabilities")]

        [Tooltip("Indicates if the NPC can chase the player (true) or not (false)")]
        [SerializeField] private bool canChasePlayer;

        [Tooltip("Indicates if the NPC must run away from the player (true) or not (false)")]
        [SerializeField] private bool mustRunAwayFromPlayer;

        [Tooltip("Indicates if the NPC can jump from the limit of a platform when calmed")]
        [SerializeField] private bool canJumpFromPlatformsOnCalmedState;

        [Tooltip("Indicates if the NPC, while calmed, must patrol around")]
        [SerializeField] private bool isPatrollerCharacter;

        [Tooltip("Indicates if the NPC must look around while IDLE")]
        [SerializeField] private bool isLookAroundWhileIdle;

        [Tooltip("Indicates if the NPC can jump from the platforms when it is on a combat state")]
        [SerializeField] private bool canJumpFromPlatformsOnCombatState;

        [Tooltip("Indicates if the NPC can shoot")]
        [SerializeField] private bool canShoot;

        [Tooltip("Indicates if the NPC must aim to shoot")]
        [SerializeField] private bool mustAim;

        // Configuration of the specific NPC actions
        [Header("NPC Configuration")]

        [Tooltip("Character's movement speed while walking")]
        [SerializeField] private float walkingSpeedMultiplier;

        [Tooltip("Character's movement speed while running")]
        [SerializeField] private float runningSpeedMultiplier;

        [Tooltip("Jump force that the character will apply when jumping")]
        [SerializeField] private float jumpForce;

        [Tooltip("Indicates how far the NPC can see")]
        [SerializeField] private float visionDistance = 5.0f;

        [Tooltip("Indicates the character stop distance. This is the distance to stop when getting close to the player")]
        [SerializeField] private float stopDistance = 0f;

        #region Capabilities checks

        /// <summary>
        /// Indicates if the character can chase the player or not
        /// </summary>
        /// <returns>True if can chase, false otherwise</returns>
        public bool CanCharacterChaseThePlayer()
        {
            return canChasePlayer;
        }

        /// <summary>
        /// Indicates if the character must run away from the player or not
        /// </summary>
        /// <returns>True if NPC must run away from the character, false otherwise</returns>
        public bool MustRunAwayFromPlayer()
        {
            return mustRunAwayFromPlayer;
        }

        /// <summary>
        /// Indicates if the character can jump from the limit of a platform or not when it is
        /// on a calmed state
        /// </summary>
        /// <returns>True if NPC can jump from a platform's limit, false otherwise</returns>
        public bool CanJumpFromPlatformsOnCalmedState()
        {
            return canJumpFromPlatformsOnCalmedState;
        }

        /// <summary>
        /// Indicates if the character is a patroller or not
        /// </summary>
        /// <returns>True if the character is a patroller, false otherwise</returns>
        public bool IsPatrollerCharacter()
        {
            return isPatrollerCharacter;
		}

        /// <summary>
        /// Indicates if the character must look around while IDLE
        /// This defines the behavior of turning around while the character is not moving
        /// on a specific position or if it must always look front.
        /// </summary>
        /// <returns>True if must look around, false if must always look front</returns>
        public bool IsLookingAroundWhileIdle()
        {
            return isLookAroundWhileIdle;
		}


        /// <summary>
        /// Indicates if the character can jump from the platforms when it is on a combat
        /// state
        /// </summary>
        /// <returns>True if can jump from the platforms, false otherwise</returns>
        public bool CanJumpFromPlatformsOnCombatState()
        {
            return canJumpFromPlatformsOnCombatState;
        }

        /// <summary>
        /// Indicates if the character can shoot or not
        /// </summary>
        /// <returns>True if the character can shot, false otherwise</returns>
        public bool CanShoot()
        {
            return canShoot;
        }

        /// <summary>
        /// Indicates if the character must aim to shoot or not
        /// </summary>
        /// <returns>True if the character must aim to shoot, false otherwise</returns>
        public bool MustAim()
        {
            return mustAim;
        }

		#endregion

		#region NPC Configuration checks

		/// <summary>
		/// Provides the walking speed multiplier when the NPC is walking
		/// </summary>
		/// <returns>Walking speed multiplier</returns>
		public float GetWalkingSpeedMultiplier()
        {
            return walkingSpeedMultiplier;
        }


        /// <summary>
        /// Provides the running speed multiplier for the NPC when it is running
        /// </summary>
        /// <returns>Running speed multiplier</returns>
        public float GetRunningSpeedMultiplier()
        {
            return runningSpeedMultiplier;
        }

        /// <summary>
        /// Provides the jump force when the NPC jumps from a specific platform
        /// </summary>
        /// <returns>Jump force when the NPC jumps from a platform</returns>
        public float GetJumpForce()
        {
            return jumpForce;
        }

        /// <summary>
        /// Provides the distance from which the NPC can detect the player
        /// </summary>
        /// <returns>Distance from which the character can detect the player</returns>
        public float GetVisionDistance()
        {
            return visionDistance;

		}

		internal float GetStopDistance()
		{
            return stopDistance;
		}

		#endregion


	}
}
