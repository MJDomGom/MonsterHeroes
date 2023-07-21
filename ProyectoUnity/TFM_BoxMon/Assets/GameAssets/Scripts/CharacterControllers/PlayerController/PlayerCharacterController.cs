using com.blackantgames.managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.blackantgames.damageable;

namespace com.blackantgames.character.controller
{

	/// <summary>
	/// Player character controller
	/// This class implements the functionality of a basic player character
	/// </summary>
    public class PlayerCharacterController : MonoBehaviour, IPlayerInputListener
    {

		[Header("Movement values")]
		[Tooltip("Indicates the player maximum movement speed")]
		[SerializeField] private float playerSpeedModifier = 3f;

		[Tooltip("Indicates the gravity force to be applied to the player")]
		[SerializeField] private float gravity = -9.8f;

		[Tooltip("Indicates how fast the change of speed must be performed to smooth the speed changes")]
		[SerializeField] private float changeSpeedMultiplier = 3.0f;

		[Tooltip("Indicates the jump speed of the character (speed to be applied when jump is done)")]
		[SerializeField] private float jumpSpeed = 5.0f;

		[Tooltip("Jump feedback value")]
		[SerializeField] private float jumpFeedback = 2.5f;

		[Tooltip("Indicates the slerp rotation multiplier to be applied to the monster orientation script")]
		[SerializeField] private float rotationMultiplier = 5.0f;

		[Tooltip("Transform used to indicate the orientation that must have the boximon character")]
		[SerializeField] private Transform orientationIndicator;

		// TODO This should be dependent of the current level
		[SerializeField] private Vector3 currentVectorMovement;

		[Header("Generic references")]
		[Tooltip("Reference to the transfrom position to detect if the character is grounded")]
		[SerializeField] private Transform groundSphereColliderDetector;

		[Tooltip("Reference to the transform position to detect impacts with platform when jumping")]
		[SerializeField] private Transform headSphereColliderDetector;

		[Tooltip("size of the sphere to detect floor collisions")]
		[SerializeField] private float groundSphereDetectionRadius = .2f;

		[Tooltip("Reference to the on death audio clips to play")]
		[SerializeField] private AudioClip[] deathAudioClips;

		[Tooltip("Layer mask to detect the floor")]
		[SerializeField] private LayerMask floorLayerMask;

		[Tooltip("Reference to the character mass center position")]
		[SerializeField] private Transform massCenterReference;

		// Object references
		private Animator animator;
		private CharacterController characterController;
		private GameObject monsterModel;
		private AudioSource audioSource;
		private IDamageable playerDamageable;
		private bool isMovementAllowed;

		// Character movement variables
		private Vector3 movementDirection;
		private float playerSpeed;
		private float fallSpeed;
		private bool performJump;
		private bool doubleJumpAllowed;
		private bool deathHandled;

		// TODO This is a workaround until a new approach is designed
		// This workaround works as there are only 2 possible inputs for each
		// kind of device. In other situations with more than 2 inputs it would
		// not work
		private float lastReceivedLateralDirection = 0f;

		private void Awake()
		{
			isMovementAllowed = true;
			monsterModel = Instantiate(GameManager.GetInstance().GetPlayerModel(), transform.position, transform.rotation, transform);
			if (monsterModel != null) {
				animator = monsterModel.GetComponent<Animator>();
				orientationIndicator.position = monsterModel.transform.position;
			}
			characterController = GetComponent<CharacterController>();
			playerDamageable = GetComponent<IDamageable>();
			audioSource = GetComponent<AudioSource>();

#if UNITY_EDITOR
			if (monsterModel == null)
			{
				Debug.LogError("Error - player character model is null: " + transform.name);
			}
			if (animator == null)
			{
				Debug.LogError("Error - player character controller: " + transform.name + " has no animator attached");
			}
			if (characterController == null)
			{
				Debug.LogError("Error - player character controller is null: " + transform.name);
			}
			if (playerDamageable == null) {
				Debug.LogError("Error - player character has no damageable component " + transform.name);
			}
			if (deathAudioClips.Length <= 0)
			{
				Debug.LogError("Error - player has no death audio clips attached. No audio clip will be played on player's death: " + transform.name);
			}
			if (audioSource == null) {
				Debug.LogError("Error - no audio source attached to player " + transform.name);
			}
#endif
		}

		void Start()
        {
			playerSpeed = 0f;			
        }

        void Update()
        {
			if (playerDamageable != null && playerDamageable.IsAlive())
			{
				// Check if the player is grounded before moving the player
				bool isGrounded = Physics.OverlapSphere(groundSphereColliderDetector.position, groundSphereDetectionRadius, floorLayerMask).Length > 0;

				// Check liveliness before moving the character
				MoveCharacter(isGrounded);
			} else if (!deathHandled)
			{ 
				deathHandled = true;
				animator.SetTrigger(AC_CharacterUtils.AC_TRIGGER_DEATH);
				if (deathAudioClips.Length > 0)
				{
					audioSource.PlayOneShot(deathAudioClips[Random.Range(0, deathAudioClips.Length)]);
				}
			}
        }

		/// <summary>
		/// Performs the character movement according to if it is grounded by overlap or not
		/// </summary>
		/// <param name="isGroundedByOverlap">True if character is grounded, false otherwise</param>
		private void MoveCharacter(bool isGroundedByOverlap)
		{

			// Calculate character movement
			Vector3 projectWorldMovement = Vector3.ProjectOnPlane(movementDirection, currentVectorMovement);

			if (!isMovementAllowed)
			{
				movementDirection = Vector3.zero;
				performJump = false;
			}

			playerSpeed = Mathf.Lerp(
				playerSpeed,
				movementDirection.magnitude * changeSpeedMultiplier,
				Time.deltaTime * playerSpeedModifier);

			projectWorldMovement = projectWorldMovement.normalized * playerSpeed * Time.deltaTime;

			if (isGroundedByOverlap)
			{
				doubleJumpAllowed = true;
				if (performJump)
				{
					animator.SetTrigger(AC_CharacterUtils.AC_TRIGGER_JUMP);
					fallSpeed = jumpSpeed;
				} else if (fallSpeed < 0f)
				{
					fallSpeed = 0f;
				}
			} else
			{
				if (doubleJumpAllowed && performJump)
				{
					doubleJumpAllowed = false;
					fallSpeed = jumpSpeed;
				}
				if (fallSpeed > 0)
				{
					if(Physics.OverlapSphere(headSphereColliderDetector.position, groundSphereDetectionRadius, floorLayerMask).Length > 0)
					{
						fallSpeed = 0f;
					}
				}
			}
			performJump = false;

			// Orientate the character
			monsterModel.transform.rotation = Quaternion.Lerp(monsterModel.transform.rotation, orientationIndicator.transform.rotation, Time.deltaTime * rotationMultiplier);
			
			// Calculate gravity
			fallSpeed += gravity * Time.deltaTime;
			Vector3 verticalVelocity = Vector3.up * fallSpeed * Time.deltaTime;

			// Set animations parameters
			animator.SetBool(AC_CharacterUtils.AC_BOOL_ON_AIR, !isGroundedByOverlap);
			animator.SetFloat(AC_CharacterUtils.AC_FLOAT_MOVEMENT_SPEED, playerSpeed);

			// Move the character
			characterController.Move(verticalVelocity + projectWorldMovement);

			transform.position = new Vector3(transform.position.x, transform.position.y, GameManager.GetInstance().GetZAssignationValue());
		}


		/// <summary>
		/// Indicates if the player is allowed to move or not
		/// </summary>
		/// <param name="isMovementAllowed">True to allow movement, false otherwise</param>
		public void NotifyMovementAllowed(bool isMovementAllowed)
		{
			this.isMovementAllowed = isMovementAllowed;
		}

		/// <summary>
		/// Provides the mass center position of the Character
		/// </summary>
		/// <returns>Mass center position of the character</returns>
		public Transform GetMassCenter()
		{
			return massCenterReference;
		}

		#region IPlayerInputListener implementation
		public void NotifyJumpRequest()
		{
			performJump = true;
		}

		/// <summary>
		/// Provides specific jump feedback
		/// </summary>
		public void PerformJumpFeedback()
		{
			if (fallSpeed < jumpFeedback)
			{
				fallSpeed = jumpFeedback;
			}
			doubleJumpAllowed = false;
		}

		public void NotifyMovementValues(float lateralDirection)
		{
			// TODO This is a workaround due to the input overlapping
			if (Mathf.Abs(lastReceivedLateralDirection) > 0.1f && Mathf.Abs(lateralDirection) <= 0.1f)
			{
				lastReceivedLateralDirection = lateralDirection;
				return;
			}
			lastReceivedLateralDirection = lateralDirection;

			movementDirection = new Vector3(lateralDirection, 0f, 0f);

			// Change the orientation of the character according to where is it moving to
			if (isMovementAllowed)
			{
				if (lateralDirection > 0.1f)
				{
					orientationIndicator.transform.LookAt(transform.position + transform.forward + ((Camera.main.transform.position - transform.position).normalized * .05f));
				}
				else if (lateralDirection < -0.1f)
				{
					orientationIndicator.transform.LookAt(transform.position - transform.forward + ((Camera.main.transform.position - transform.position).normalized * .055f));
				}
			}
		}


		/// <summary>
		/// Indicates if the current character is alive or not
		/// </summary>
		/// <returns>True if player character is alive, false otherwise</returns>
		public bool IsAlive()
		{
			return playerDamageable.IsAlive();
		}


		#endregion
	}
}