using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;
using com.blackantgames.damageable;
using Unity.VisualScripting;
using com.blackantgames.character.controller;
using com.blackantgames.managers;
using com.blackantgames.npc.utils;

namespace com.blackantgames.npc.ai
{

    /// <summary>
    /// Basic monster character controller
    /// 
    /// By default, all the NPCs in the game will be considered as monster. The
    /// behvaiour that each character will exhibit will depend on the capabilities
    /// that each of the monster has
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class BasicMonsterController : MonoBehaviour
    {
        [Header("NPC generic configuration")]
        [Tooltip("Reference to the NPC Capabilities")]
        [SerializeField] private NPCCapabilities npcCapabilities;

        [Tooltip("Reference to the NPC visual game object")]
        [SerializeField] private GameObject npcModel;

		[Tooltip("Indicates the gravity force to be applied to the player")]
		[SerializeField] private float gravity = -9.8f;


        [Tooltip("Indicates the waiting time")]
        [SerializeField] private float patrolWaitingTime = 3.0f;

        [Tooltip("NPC Transport position to calculate the vision")]
        [SerializeField] private Transform visionSensorPosition;

        [Tooltip("NPC Vision layermask")]
		[SerializeField] private LayerMask visionLayerMask;

        [Tooltip("Tine between shoots")]
        [SerializeField] private float timeBetweenShoots = 1.0f;

		[Header("Movement auxiliary parameters")]

		[Tooltip("Indicates the slerp rotation multiplier to be applied to the monster orientation script")]
		[SerializeField] private float rotationMultiplier = 5.0f;

        [Tooltip("Indicates the character movement speed")]
		[SerializeField] private float movementWalkSpeed = 1f;

		[Tooltip("Indicates the character movement speed")]
		[SerializeField] private float movementRunningSpeed = 3f;

        [Tooltip("Indicates the jump speed to be applied to the character's jump")]
        [SerializeField] private float jumpSpeed = 5.0f;

		[Tooltip("Transform used to detect the floor collision")]
        [SerializeField] private Transform groundSphereColliderDetector;

		[Tooltip("size of the sphere to detect floor collisions")]
		[SerializeField] private float groundSphereDetectionRadius = .2f;

		[Tooltip("Auxiliary component that indicates the direction to which the character is oriented")]
		[SerializeField] private Transform orientationIndicator;

        [Tooltip("Auxiliary component to detect walls on front of the character and stop it if necessary")]
        [SerializeField] private Transform frontCollisionDetector;

		[Tooltip("Auxiliary component that is used to check if jumping, the character will be able to skip an obstacle")]
		[SerializeField] private Transform frontJumpCollisionDetector;

		[Tooltip("Auxiliary component to indicate if the floor is going to finish front-forward the character")]
        [SerializeField] private Transform frontFloorDetector;

        [Tooltip("Indicates the mini jump force to be applied when appling detection feedback")]
        [SerializeField] private float miniJumpForce = 2.5f;

		// TODO This should be done dependant of the current level
		[SerializeField] private Vector3 currentVectorMovement;

		[Tooltip("Layer mask to detect the floor")]
		[SerializeField] private LayerMask floorLayerMask;

        [Tooltip("Indicates how fast the change of speed must be performed to smooth the speed changes")]
		[SerializeField] private float changeSpeedMultiplier = 3.0f;


		[Header("Feedback variables")]

		[Tooltip("Reference to the on death audio clips to play")]
		[SerializeField] private AudioClip[] deathAudioClips;

        [Tooltip("Possible alert detection audio clips to play")]
        [SerializeField] private AudioClip[] alertClip;

		// Other character components
		private IDamageable npcDamageable;
        private Animator animator;
        private AudioSource audioSource;
        private StateIndicatorController stateIndicatorController;
        private NPC_BulletSpawnController[] bulletSpawners;

		// Movement parameters
		private float movementSpeedMultiplier;
		private float fallSpeed;
        private float npcSpeed;
        private bool deathHandled;
        private bool miniJump;
        private bool aiming;
        private Vector3 movementDirection;
        private CharacterController characterController;

        // TODO This assignation should be done according to the world settings to be able to change the movement plane
        // This will keep as a workaround until this is done in the Game Manager as it is required to ensure the right
        // movement of the characters
		private Vector3 forwardVector = Vector3.right;
        private Vector3 currentMovementVector;
        private bool isGrounded;
        private bool canAlert;
        private float nextShootTime;
        private bool playerDetected;
		private float nextWaitingTime;
        private bool forceCombatMode = false;

        private NPC_AimingAuxiliaryIndicator[] aimingAuxiliaryIndicators;

		#region Unity functionality

		private void Awake()
		{
			npcDamageable = GetComponent<IDamageable>();
            animator = npcModel.GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
			characterController = GetComponent<CharacterController>();
            stateIndicatorController = GetComponentInChildren<StateIndicatorController>();

			bulletSpawners = GetComponentsInChildren<NPC_BulletSpawnController>();

			aimingAuxiliaryIndicators = GetComponentsInChildren<NPC_AimingAuxiliaryIndicator>();

			canAlert = true;
            nextShootTime = -1;

			// By default, the character must start moving forward in the level
			currentMovementVector = forwardVector;

#if UNITY_EDITOR
			// Error checks to execute only on the Unity Editor runs
			if (npcCapabilities == null)
            {
                Debug.LogError("NPC character " + transform.name + " has no capabilities assigned");
            }

            if (npcDamageable == null)
            {
                Debug.LogError("NPC Character " + transform.name + " has no damageable component assigned");
            }
            if (animator == null)
            {
                Debug.LogError("NPC Character " + transform.name + " has no animator assigned");
            }
            if (audioSource == null)
            {
                Debug.LogError("NPC Character " + transform.name + " has no Audio Source assigned");
            }
            if (characterController == null)
            {
                Debug.LogError("NPC Character " + transform.name + " has no CharacterController assigned");
            }
            if (stateIndicatorController == null)
            {
                Debug.LogError("NPC Character " + transform.name + " has no StateIndicatorController attached");

			}
#endif
        }

		private void Start()
		{
            npcSpeed = 0;
	    }

		private void Update()
		{
            // TODO Check if this can be move to a not-active listening
            if (npcDamageable != null && npcDamageable.IsAlive())
            {
                MoveCharacter(isGrounded);
			} else if (!deathHandled)
            {
                deathHandled = true;
                animator.SetTrigger(AC_CharacterUtils.AC_TRIGGER_DEATH);
				if (deathAudioClips.Length > 0)
				{
					audioSource.PlayOneShot(deathAudioClips[Random.Range(0, deathAudioClips.Length)]);
				}
                // Make the Character Controller capsule size 0
                //characterController.enabled = false;
                //characterController.radius = 0.01f;
                //characterController.height = 0.01f;
                CheckIsGrounded();
				ApplyGravityAfterDeath(isGrounded);

                Invoke("DestroyCharacerAfterDeath", 5.0f);
			} else if (isGrounded)
            {
                characterController.enabled = false;
            } else
            {
                CheckIsGrounded();
				ApplyGravityAfterDeath(isGrounded);
			}
		}

        /// <summary>
        /// Destroy the character after it's death
        /// </summary>
        private void DestroyCharacerAfterDeath()
        {
            Destroy(this.gameObject);
        }

		/// <summary>
		/// Performs the NPC movement according to if it is grounded by overlap
		/// or not, and the decissions taken by the Behavior Tree
		/// </summary>
		/// <param name="isGroundedByOverlap"></param>
		private void MoveCharacter(bool isGroundedByOverlap)
		{
            Vector3 projectWorldMovement = Vector3.ProjectOnPlane(movementDirection, currentVectorMovement);

            if (movementSpeedMultiplier > 0.01f)
            {
                npcSpeed = Mathf.Lerp(
                    npcSpeed,
                    movementDirection.magnitude * changeSpeedMultiplier,
                    Time.deltaTime * movementSpeedMultiplier);
            } else
            {
                npcSpeed = 0;
            }

            if (!miniJump && isGroundedByOverlap)
            {
                if (fallSpeed < 0f)
                {
                    fallSpeed = 0f;
                }
            } else if (miniJump)
            {
                fallSpeed = miniJumpForce;
                miniJump = false;
			}
            else 
            {
				fallSpeed += gravity * Time.deltaTime;
			}

			projectWorldMovement = projectWorldMovement.normalized * npcSpeed * Time.deltaTime;

            // Orientate the NPC
            npcModel.transform.rotation = Quaternion.Lerp(npcModel.transform.rotation, orientationIndicator.transform.rotation, Time.deltaTime * rotationMultiplier);

			// Set the animation parameters
			animator.SetBool(AC_CharacterUtils.AC_BOOL_ON_AIR, !isGroundedByOverlap);
			animator.SetFloat(AC_CharacterUtils.AC_FLOAT_MOVEMENT_SPEED, npcSpeed);

			Vector3 verticalVelocity = Vector3.up * fallSpeed * Time.deltaTime;

            // Move the character
            if (characterController.gameObject.active)
            {
                characterController.Move(verticalVelocity + projectWorldMovement);
            }
		}

        /// <summary>
        /// Function to apply the gravity to the character after death
        /// </summary>
        private void ApplyGravityAfterDeath(bool isGroundedByOverlap)
        {
			if (isGroundedByOverlap)
			{
				if (fallSpeed < 0f)
				{
					fallSpeed = 0f;
				}
			} else
			{
				fallSpeed += gravity * Time.deltaTime;
			}
			Vector3 verticalVelocity = Vector3.up * fallSpeed * Time.deltaTime;
			characterController.Move(verticalVelocity);
		}

        /// <summary>
        /// Update the check if the character is grounded or not
        /// </summary>
        private void CheckIsGrounded()
        {
			isGrounded = Physics.OverlapSphere(groundSphereColliderDetector.position, groundSphereDetectionRadius, floorLayerMask).Length > 0;
		}

        /// <summary>
        /// Task to recalculate when is the next moment when the NPC can shoot
        /// </summary>
        private void RecalculateNextShootTime(float timeToShoot = -1f)
        {
            if (timeToShoot <= 0f)
            {
                timeToShoot = timeBetweenShoots;
			}
            // TODO Provide the time through the capabilities and do not hardcode it
            nextShootTime = Time.time + timeToShoot;
        }

        /// <summary>
        /// Update the aiming state
        /// </summary>
        /// <param name="aimingState">Current aiming state</param>
        private void SetAimingState(bool aimingState)
        {
			aiming = aimingState;
			foreach (NPC_AimingAuxiliaryIndicator indicator in aimingAuxiliaryIndicators)
			{
				indicator.SetAimingState(aimingState);
			}
		}

        public void ForceCombatMode()
        {
            forceCombatMode = true;
		}

		#endregion

		#region BT Action tasks

		/// <summary>
		/// Task to turn and move in the opposite direction
		/// </summary>
		/// <TaskResult>
		///     <Succeed>The NPC turns</Succeed>
		///     <Failure>Not expected failure</Failure>
		/// </TaskResult>
		[Task]
        public void Task_Turn()
        {
            forwardVector = -forwardVector;
            orientationIndicator.transform.Rotate(new Vector3(0, 180, 0));
            Task.current.Succeed();
        }

        /// <summary>
        /// Task to make the character to move forward
        /// </summary>
        /// <TaskResult>
        ///     <Succeed>Tha NPC walks forward with walking speed</Succeed>
        ///     <Failure>Not expected failure</Failure>
        /// </TaskResult>
        [Task]
        public void Task_WalkForward()
        {
			movementDirection = forwardVector * movementWalkSpeed;
			movementSpeedMultiplier = movementWalkSpeed;
            changeSpeedMultiplier = npcCapabilities.GetWalkingSpeedMultiplier();

			Task.current.Succeed();
		}

        /// <summary>
        /// Task to make the NPC run in the forward direction
        /// </summary>
        /// <TaskResult>
        ///     <Succeed>The NPC run in the forward direction (running speed)</Succeed>
        ///     <Failure>Not expected failure</Failure>
        /// </TaskResult>
        [Task]
        public void Task_RunForward()
        {
			movementDirection = forwardVector;
            movementSpeedMultiplier = movementRunningSpeed;
            changeSpeedMultiplier = npcCapabilities.GetRunningSpeedMultiplier();
			Task.current.Succeed();
		}

        /// <summary>
        /// Task to make the NPC stops immediately
        /// </summary>
        /// <TaskResult>
        ///     <Succeed>The NPC stops its movement</Succeed>
        ///     <Failure>Not expected failure</Failure>
        /// </TaskResult>
        [Task]
        public void Task_Stop()
        {
            movementDirection = Vector3.zero;
            movementSpeedMultiplier = 0f;
            Task.current.Succeed();
        }

        /// <summary>
        /// Task to make the character to look to the player
        /// </summary>
        /// <TaskResult>
        ///     <Succeed>Looking to the player</Succeed>
        ///     <Failure>Not expected failure</Failure>
        /// </TaskResult>
        [Task]
		public void Task_LookToPlayer()
        {
			float currentDistance = Vector3.Distance(
                GameManager.GetInstance().GetCurrentPlayerCharacterController().transform.position,
                (transform.position + orientationIndicator.transform.forward));
            float rotatedDistance = Vector3.Distance(
                GameManager.GetInstance().GetCurrentPlayerCharacterController().transform.position,
                (transform.position - orientationIndicator.transform.forward));

            if (rotatedDistance < currentDistance) {
                Task_Turn();
            } else
            {
                Task.current.Succeed();
            }
        }

        /// <summary>
        /// Indicates if the NPC can shoot or not according to two criterias:
        /// </summary>
        /// <TaskResult>
        ///     <Success>The NPC can shoot to the player</Success>
        ///     <Failure>The NPC can not shoot to the player</Failure>
        /// </TaskResult>
        [Task]
        public void Task_CanShootToPlayer()
        {
            if (nextShootTime < 0)
            {
                RecalculateNextShootTime();
            }

            if (!npcCapabilities.MustAim())
            {
                float yDifference = Mathf.Abs(transform.position.y - GameManager.GetInstance().GetCurrentPlayerCharacterController().GetMassCenter().position.y);
                if (yDifference < 1.0f)
                {
                    Task.current.Complete(nextShootTime > 0 && Time.time > nextShootTime);
                }
                else
                {
                    Task.current.Fail();
                }
            } else
            {
				Task.current.Complete(nextShootTime > 0 && Time.time > nextShootTime);
			}
        }

        /// <summary>
        /// Indicates if the character must aim before shooting or not
        /// </summary>
        /// <TaskResult>
        ///     <Succeed>The NPC must aim before shooting</Succeed>
        ///     <Failure>The NPC must not aim before shooting</Failure>
        /// </TaskResult>
        [Task]
        public void Task_MustAimToShoot()
        {
            Task.current.Complete(npcCapabilities.MustAim());
		}

        [Task]
        public void Task_Aim()
        {
            SetAimingState(true);
			Task.current.Complete(aiming);
		}

        /// <summary>
        /// Task to shoot the current NPC gun
        /// </summary>
        /// <TaskResult> 
        ///     <Success>The NPC shoots its gun</Success>
        ///     <Failure>Not expected failure</Failure>
        /// </TaskResult>
        [Task]
        public void Task_Shoot()
        {
            RecalculateNextShootTime();
            animator.SetTrigger(AC_CharacterUtils.AC_TRIGGER_SHOOT);
			
            foreach(NPC_BulletSpawnController spawner in bulletSpawners)
            {
                spawner.Shoot();
            }

            foreach(NPC_AimingAuxiliaryIndicator indicator in aimingAuxiliaryIndicators)
            {
                indicator.TriggerShootEffect();
			}

		}

        /// <summary>
        /// Task to make the NPC prepare Shoot/attack
        /// </summary>
		[Task]
        public void Task_LoadShoot() {
            // TODO Implement this functionality
            Task.current.Succeed();
        }

		/// <summary>
		/// Task to make the character to look in the opposing direction to the player
		/// </summary>
		/// <TaskResult>
		///     <Succeed>Looking to escape from the player</Succeed>
		///     <Failure>Not expected failure</Failure>
		/// </TaskResult>
		[Task]
		public void Task_LookOppossiteToPlayer()
        {
			float currentDistance = Vector3.Distance(
				GameManager.GetInstance().GetCurrentPlayerCharacterController().transform.position,
				(transform.position + forwardVector));
			float rotatedDistance = Vector3.Distance(
				GameManager.GetInstance().GetCurrentPlayerCharacterController().transform.position,
				(transform.position - forwardVector));

			if (rotatedDistance > currentDistance)
			{
				Task_Turn();
			}
			else
			{
				Task.current.Succeed();
			}
		}

		/// <summary>
		/// Set a waiting time for the character
		/// </summary>
		/// <TaskResult>
		///     <Succeed>The waiting time is set correctly</Succeed>
		/// </TaskResult>
		[Task]
		public void Task_SetPatrolWaitingTime()
		{
			nextWaitingTime = Time.time + patrolWaitingTime;
			Task.current.Succeed();
		}


		/// <summary>
		/// Set a waiting time for the character
		/// </summary>
		/// <TaskResult>
		///     <Succeed>The waiting time is set correctly</Succeed>
        ///     <Failure>Not expected failure situation</Failure>
		/// </TaskResult>
		[Task]
		public void Task_SetWaitToChaseTime()
		{
			nextWaitingTime = Time.time + 0.25f;
			Task.current.Succeed();
		}

        /// <summary>
        /// Trigger the ALERT state of the character
        /// </summary>
        /// <TaskResult>
        ///     <Succeed>Alert is triggered</Succeed>
        ///     <Failure>Not expected failure situation</Failure>
        /// </TaskResult>
		[Task]
		public void Task_Alert()
		{
			if (canAlert)
			{
				miniJump = true;
				animator.SetTrigger(AC_CharacterUtils.AC_TRIGGER_ALERT);
				canAlert = false;
                nextWaitingTime = Time.time + .25f;
                RecalculateNextShootTime();
                audioSource.PlayOneShot(alertClip[Random.Range(0, alertClip.Length)]);

                // Trigger the alert indicator if it is available
                if (stateIndicatorController != null)
                {
                    stateIndicatorController.TriggerAlert();
                }
			}
			Task.current.Succeed();
		}

        /// <summary>
        /// Task to check if jumping, the character could be able to skip an obstacle
        /// </summary>
        /// <TaskResul>
        ///     <Succeed>If jumping, the character will be able suitable to skip an obstacle</Succeed>
        ///     <Failure>If jumping would not allow to skip the obstacle</Failure>
        /// </TaskResul>
        [Task]
		public void Task_CanJumpToAvoidObstacle()
        {
			Task.current.Complete(Physics.OverlapSphere(
                frontJumpCollisionDetector.transform.position, .25f, floorLayerMask).Length == 0);
		}

		#endregion

		#region BT Query tasks

		/// <summary>
		/// Task to check if the character can keep moving forward
		/// </summary>
		/// <TaskResult>
		///     <Succeed>If the character detects that he can move forward</Succeed>
		///     <Failure>If the character detects that he cannot move forward</Failure>
		/// </TaskResult>
		[Task]
        public void Task_CanMoveForward()
        {
            Task.current.Complete(
                (Physics.OverlapSphere(frontCollisionDetector.transform.position, groundSphereDetectionRadius, floorLayerMask).Length == 0)
                && (Physics.OverlapSphere(frontFloorDetector.transform.position, groundSphereDetectionRadius, floorLayerMask).Length > 0));
        }

        /// <summary>
        /// Task to check if the charactar can move forward ignoring tha falling condition
        /// </summary>
        /// <TaskResult>
        ///     <Succeed>The character can move forward without blockers (ignoring falling condition)</Succeed>
        ///     <Failure>There is a blocker on the way that is not the falling condition</Failure>
        /// </TaskResult>
        [Task]
        public void Task_CanMoveForwardIgnoringFall()
        {
            Task.current.Complete(Physics.OverlapSphere(
                frontCollisionDetector.transform.position, groundSphereDetectionRadius, floorLayerMask)
                        .Length == 0);
		}

        /// <summary>
        /// Indicates if the player has been detected already or not
        /// </summary>
        /// <TaskResult>
        ///     <Succeed>The player has been already detected</Succeed>
        ///     <Failure>The player has not been detected already</Failure>
        /// </TaskResult>
        [Task]
        public void Task_HasPlayerBeenDetected()
        {
            Task.current.Complete(playerDetected || forceCombatMode);
		}

        /// <summary>
        /// Task that indicates if the player is visible or not
        /// </summary>
        /// <TaskResult>
        ///     <Suceed>If the player is visible</Suceed>
        ///     <Failure>If the player is not visible</Failure>
        /// </TaskResult>
        [Task]
        public void Task_IsPlayerDetected()
        {
            // Do not detect a dead player
            if (!GameManager.GetInstance().GetCurrentPlayerCharacterController().IsAlive())
            {
                Task.current.Fail();
                return;
            }

            if (forceCombatMode)
            {
                Task.current.Succeed();
            }

			// First: if the character is too far, don't attempt to detect it
			if (Vector3.Distance(transform.position, GameManager.GetInstance().GetCurrentPlayerCharacterController().transform.position) > npcCapabilities.GetVisionDistance())
            {
                Task.current.Fail();
                return;
            }

			// Second: The NPC is not looking in the right direction
			float currentDistance = Vector3.Distance(
				GameManager.GetInstance().GetCurrentPlayerCharacterController().transform.position,
				(transform.position + orientationIndicator.transform.forward));
            Debug.DrawLine(transform.position, transform.position + orientationIndicator.transform.forward * 500f, Color.red);
			float rotatedDistance = Vector3.Distance(
				GameManager.GetInstance().GetCurrentPlayerCharacterController().transform.position,
				(transform.position - orientationIndicator.transform.forward));

			if (rotatedDistance < currentDistance)
			{
                Task.current.Fail();
                return;
			}

            // Third: There is not a ray that could cause the detection
            Debug.DrawLine(visionSensorPosition.position, GameManager.GetInstance().GetCurrentPlayerCharacterController().GetMassCenter().position, Color.red);
            RaycastHit hit;
			if (Physics.Raycast(visionSensorPosition.position,
				(GameManager.GetInstance().GetCurrentPlayerCharacterController().GetMassCenter().position - visionSensorPosition.position),
                out hit,
                npcCapabilities.GetVisionDistance() + 1.0f,
                visionLayerMask))
            {
                if (hit.collider.transform.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
                {
                    playerDetected = true;
					Task.current.Succeed();
                    return;
                }
            }
            Task.current.Fail();
        }

        /// <summary>
        /// Indicates if the NPC is alive or not
        /// </summary>
        /// <TaskResult>
        ///     <Suceed>If the NPC is currently alive</Suceed>
        ///     <Faillure>If the NPC is not alive</Faillure>
        /// </TaskResult>
        [Task]
        public void Task_IsAlive()
        {
            Task.current.Complete(npcDamageable.IsAlive());
        }

		/// <summary>
		/// Task that indicates if the NPC can chase or not the player
		/// </summary>
		/// <TaskResult>
		///     <Succed>The NPC can chase the player</Succed>
		///     <Failure>The NPC can not chase the player</Failure>
		/// </TaskResult>
		[Task]
        public void Task_CanChasePlayer()
        {
            Task.current.Complete(npcCapabilities.CanCharacterChaseThePlayer());
        }

        /// <summary>
        /// Task that indicates if the NPC must run away from the player or not
        /// </summary>
        /// <TaskResult>
        ///     <Succeed>The NPC must run away from the player</Succeed>
        ///     <Failure>The NPC must not run away from the player</Failure>
        /// </TaskResult>
        [Task]
        public void Task_MustRunAwayFromPlayer()
        {
            Task.current.Complete(npcCapabilities.MustRunAwayFromPlayer());
        }

        /// <summary>
        /// Task that indicates if the NPC can jump or not when it is calmed
        /// </summary>
        /// <TaskResult>
        ///     <Succed>The NPC can jump</Succed>
        ///     <Failure>The NPC cannot jump</Failure>
        /// </TaskResult>
        [Task]
        public void Task_CanNPCJumpOnCalmedState()
        {
            Task.current.Complete(npcCapabilities.CanJumpFromPlatformsOnCalmedState());
        }

        /// <summary>
        /// Task that indicates if the NPC can jump or not when it is on combat
        /// </summary>
        /// <TaskResult>
        ///     <Succeed>The NPC can jump on combat state</Succeed>
        ///     <Failure>The NPC cannot jump on combat state</Failure>
        /// </TaskResult>
        [Task]
        public void Task_CanNPCJumpOnCombatState()
        {
            Task.current.Complete(npcCapabilities.CanJumpFromPlatformsOnCombatState());
        }

        /// <summary>
        /// Task that indicates if the NPC is a patrolling character or not
        /// </summary>
        /// <TaskResult>
        ///     <Succeed>The character is a patroller</Succeed>
        ///     <Failure>The character is not a patroller</Failure>
        /// </TaskResult>
        [Task]
        public void Task_IsPatrollerNPC()
        {
            Task.current.Complete(npcCapabilities.IsPatrollerCharacter());
        }

		/// <summary>
		/// Task to precalculate the IsFalling status and ensure that it is updated
		/// when the rest of tasks are executed in the BT
		/// </summary>
		/// <TaskResult>
		///     <Succeed>Expected succeed. This is a precalculationtask to be executed on every update</Succeed>
		///     <Failure>Not expected failure</Failure>
		/// </TaskResult>
		[Task]
		public void Task_PreCalculateIsGrounded()
		{
            CheckIsGrounded();
			Task.current.Succeed();
		}

		/// <summary>
		/// Indicates if the character is falling or not
		/// </summary>
		/// <TaskResult>
		///     <Suceed>If the player is falling</Suceed>
		///     <Failure>If the player is not falling</Failure>
		/// </TaskResult>
		[Task]
		public void Task_IsFalling()
		{
			Task.current.Complete(!isGrounded);

		}

		/// <summary>
		/// Task that indicates if the NPC has a target assigned
		/// </summary>
		[Task]
        public void Task_HasTargetAssigned()
        {
            // TODO Decide if this behaviour is required or not
            // TODO Implement this functionality
        }

        /// <summary>
        /// Task to make the NPC jump
        /// </summary>
        /// <TaskResult>
        ///     <Succeed>The NPC jumps</Succeed>
        ///     <Failure>Not expected</Failure>
        /// </TaskResult>
        [Task]
        public void Task_Jump()
        {
            fallSpeed = npcCapabilities.GetJumpForce();
            Task.current.Succeed();
        }


		/// <summary>
		/// Indicates if the character should wait some time or not before performing the next action
		/// </summary>
		/// <TaskResult>
		///     <Succeed>The NPC must wait some time</Succeed>
		///     <Failure>The NPC does not have to wait</Failure>
		/// </TaskResult>
		[Task]
        public void Task_MustNPCWait()
        {
            Task.current.Complete(nextWaitingTime > Time.time);
		}


        /// <summary>
        /// Task that indicates if the NPC must look around of if he must remain in position
        /// </summary>
        /// <TaskResult>
        ///     <Succeed>The NPC must look around</Succeed>
        ///     <Failure>The NPC must not look around</Failure>
        /// </TaskResult>
        [Task]
		public void Task_MustLookAround()
        {
            Task.current.Complete(npcCapabilities.IsLookingAroundWhileIdle());
        }

        /// <summary>
        /// Task that indicates if the character is at the stop and attack distance
        /// </summary>
        /// <TaskResult>
        ///     <Succeed>The charater is at the stop distance</Succeed>
        ///     <Failure>The character is not close enough to stop</Failure>
        /// </TaskResult>
        [Task]
        public void Task_IsCharacterAtStopDistance()
        {
            Task.current.Complete(Vector3.Distance(transform.position, GameManager.GetInstance().GetCurrentPlayerCharacterController().transform.position) < npcCapabilities.GetStopDistance());
        }

		/// <summary>
		/// Task to check if the character should return to a calmed state
		/// </summary>
		/// <TaskResult>
		///     <Succeed>The NPC must return to a calmed state</Succeed>
		///     <Failure>The NPC must not return to a calmed state</Failure>
		/// </TaskResult>
		[Task]
		public void Task_CheckReturnToCalmedState()
        {
            // TODO This is a transition from COMBAT to CALMED. The current
            // implementation only allows this transition when the player dies
            if(!GameManager.GetInstance().GetCurrentPlayerCharacterController().IsAlive())
            {
                canAlert = true;
                playerDetected = false;
                SetAimingState(false);
				Task.current.Succeed();
                return;
            }
            Task.current.Fail();
        }

		#endregion


	}

}