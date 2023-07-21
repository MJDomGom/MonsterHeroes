using com.blackantgames.character.controller;
using com.blackantgames.damageable;
using com.blackantgames.musiccontroller;
using com.blackantgames.playerstoragesystem;
using com.blackantgames.scenenavigation;
using com.blackantgames.ui.widgets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace com.blackantgames.managers
{
	/// <summary>
	/// Class that controls the normal executin of a level
	/// 
	/// This class is responsible to perform the central game management and will
	/// perform all the tasks that are commonly required by all the levels in the
	/// game.
	/// 
	/// This class implements a strict singleton pattern. Only one instance of the
	/// class is allowed. Any attempt to create an additional instance will result
	/// on its deletion
	/// </summary>
	[RequireComponent(typeof(MonsterCharacterProvider))]
	public class GameManager : MonoBehaviour, IDamageableListener
	{
		// Singleton instance
		private static GameManager INSTANCE;

		private static bool isGamePaused = false;

		[Header("Player references")]
		[Tooltip("Reference to the spawneable player container prefab")]
		[SerializeField] private GameObject playerContainerPrefab;

		[Header("Levels management")]
		[SerializeField] private LevelLoaderManager levelLoaderManager;

		[Header("Player input references")]
		[Tooltip("Device player input (keyboard/mouse)")]
		[SerializeField] private DevicePlayerInput devicePlayerInput;

		[Tooltip("Touch screen player input")]
		[SerializeField] private TouchScreenPlayerInput touchScreenPlayerInput;

		[Tooltip("GamePad player input")]
		[SerializeField] private DevicePlayerInput gamePadPlayerInput;

		[Tooltip("Indicates the amount of life that the player has on start")]
		[SerializeField] private int amountOfLifes = 3;

		[Tooltip("Indicates the maximum amount of lifes that the player can have")]
		[SerializeField] private int maxAmountOfLifes = 3;

		[Tooltip("Indicates the time required to restart the game if the player still has valid lifes")]
		[SerializeField] private float timeToRestartAfterDeath = 3.5f;

		[Tooltip("Indicates the time required to activate the game over screen if the player lost all lifes")]
		[SerializeField] private float timeToGameOverAfterDeath = 3.2f;

		[Header("UI Components")]
		[SerializeField] private GameObject gameOverObject;
		[SerializeField] private GameObject pauseObject;
		[SerializeField] private GameObject levelCompleteCanvas;

		[Header("Auxiliary components")]
		[SerializeField] private float zAssignationValue = 0f;

		[Header("Debugger mode")]
		[SerializeField] private GameObject debuggerModeIndicator;

		private List<AbstractPlayerInput> playerInputs;
		//private CharacterSpawnPoint currentPlayerSpawnPoint;
		private MonsterCharacterProvider monsterCharacterProvider;
		private AuxCameraFollowComponent auxCameraFollowComponent;
		private UILifeIndicator uiLifeIndicator;
		private int currentAmountOfLifes;
		private PlayerCharacterController currentPlayerObject;
		private PlayerCharacterController previousPlayerObject;
		private CoinManager coinManager;
		private int selectedCharacterIndex = 0;
		private UIBossTextController uiBossTextController;

		// TODO Debugger mode - to be deleted
		private bool debuggerMode = false;
		
		private List<IPlayerRespawnListener> respawnListeners = new List<IPlayerRespawnListener>();

		private void Awake()
		{
			if (INSTANCE != null)
			{
				Destroy(this);
				return;
			}

			INSTANCE = this;

			selectedCharacterIndex = MonsterStoreController.GetSelectedBoximonId();
			monsterCharacterProvider = GetComponent<MonsterCharacterProvider>();
			InitializeInteractionController();
			auxCameraFollowComponent = FindObjectOfType<AuxCameraFollowComponent>();
			currentAmountOfLifes = amountOfLifes;
			uiLifeIndicator = FindObjectOfType<UILifeIndicator>();
			coinManager = GetComponent<CoinManager>();
			uiBossTextController = transform.parent.GetComponentInChildren<UIBossTextController>();
		}

		private void Start()
		{
			// Spawn the player when the game starts
			SpawnPlayer();
			uiLifeIndicator.SetAmountOfLifeIndicatore(currentAmountOfLifes);
			gameOverObject.SetActive(false);
		}

		private void Update()
		{
			UpdateIsGamePaused();

			UpdatePauseObjectVisibility();
		}

		public void AddRespawnListener(IPlayerRespawnListener listener)
		{
			respawnListeners.Add(listener);
		}

		public void RemoveRespawnListener(IPlayerRespawnListener listener)
		{
			respawnListeners.Remove(listener);
		}

		#region Singleton instance management

		/// <summary>
		/// Provides access to the singleton instance
		/// </summary>
		/// <returns>Reference to the GameManager singleton instance</returns>
		public static GameManager GetInstance()
		{
			return INSTANCE;
		}

		#endregion

		#region Player input

		/// <summary>
		/// Initialize the interaction system controller to communicate
		/// the player with the different elements of the game
		/// </summary>
		private void InitializeInteractionController()
		{
			// ----------------------------------------------------------------------
			// TODO - Debug code to set keyboard input 
			// ----------------------------------------------------------------------
			//playerInput = Instantiate<DevicePlayerInput>(devicePlayerInput, null);
			// ----------------------------------------------------------------------

			// TODO Enable the creation of the device player input depending on the kind
			// of OS on which the game is running
			// TODO Should this be a list with multiple player inputs? That would nededed
			// to support for example bluetooth controllers in the game

			// OS dependant interaction system creation
			playerInputs = new List<AbstractPlayerInput>();
#if UNITY_EDITOR
			// PC - Mac interaction system for the Unity Editor
			// Gamepad and keyboard inputs are allowed
			playerInputs.Add(Instantiate<DevicePlayerInput>(devicePlayerInput, null));
			playerInputs.Add(Instantiate<DevicePlayerInput>(gamePadPlayerInput, null));

#elif UNITY_STANDALONE
			// PC - Mac interaction system
			// Gamepad and keyboard inputs are allowed
			playerInputs.Add(Instantiate<DevicePlayerInput>(devicePlayerInput, null));
			playerInputs.Add(Instantiate<DevicePlayerInput>(gamePadPlayerInput, null));
#elif UNITY_ANDROID
			// Android devices
			// Gamepad and touch-screen inputs are allowed
			playerInputs.Add(Instantiate<TouchScreenPlayerInput>(touchScreenPlayerInput, null));
			playerInputs.Add(Instantiate<DevicePlayerInput>(gamePadPlayerInput, null));
#elif UNITY_IOS
			// iOS devices (iPhone/iPad)
			// Gamepad and touch-screen inputs are allowed
			playerInputs.Add(Instantiate<TouchScreenPlayerInput>(touchScreenPlayerInput, null));
			playerInputs.Add(Instantiate<DevicePlayerInput>(gamePadPlayerInput, null));
#endif


		}

		/// <summary>
		/// Provides a reference to the Player Input being usid in the game
		/// </summary>
		/// <returns>Reference to the current used player input list</returns>
		public List<AbstractPlayerInput> GetPlayerInput()
		{
			return playerInputs;
		}

		public void ActivateDebuggerMode()
		{
			debuggerMode = true;
			debuggerModeIndicator.SetActive(true);
		}

		#endregion

		#region Player handling

		/// <summary>
		/// Spawn the player in the current spawn point position
		/// </summary>
		private void SpawnPlayer()
		{
			if (levelLoaderManager.GetCurrentSpawnPoint() != null)
			{
				// Create the player object
				GameObject player = levelLoaderManager.GetCurrentSpawnPoint().SpawnPlayer(playerContainerPrefab);
				if (previousPlayerObject != null) {
					Destroy(previousPlayerObject.gameObject);
				}
				currentPlayerObject = player.GetComponent<PlayerCharacterController>();
				previousPlayerObject = currentPlayerObject;
				if (currentPlayerObject != null) {
					// Attach the Player input listener to each of the allowed player inputs
					foreach(AbstractPlayerInput playerInput in playerInputs)
					{
						playerInput.AddPlayerInputListener(currentPlayerObject);
					}
					auxCameraFollowComponent.SetTrackedTransform(player.transform);
				}
				currentPlayerObject.GetComponent<IDamageable>().AttachDamageableListener(this);

				if (!MusicManager.GetInstance().GetMuteMusicState())
				{
					MusicManager.GetInstance().PlayInGameMusic();
				} else
				{
					MusicManager.GetInstance().PlayInGameMusic();
					MusicManager.GetInstance().ChangeMuteMusicState();
				}

				foreach (IPlayerRespawnListener respawnListener in respawnListeners)
				{
					respawnListener.NotifyPlayerRespawn();
				}

			} else
			{
#if UNITY_EDITOR
				Debug.LogError("Error: Attempting to spawn player - Current player spawn point is not assigned in the GameManager");
#endif
			}

		}

		/// <summary>
		/// Provides access to the Current Player Character controller
		/// </summary>
		/// <returns>Currently active player character controller</returns>
		public PlayerCharacterController GetCurrentPlayerCharacterController()
		{
			return currentPlayerObject;
		}

		/// <summary>
		/// Provides the current Player Character Model
		/// </summary>
		/// <returns>Current player character model</returns>
		public GameObject GetPlayerModel()
		{
			// TODO Missing functionality - monster selection is still not implemented
			// A tomprary monster character provider has been created pending on the
			// creation of the final monster provider
			return monsterCharacterProvider.GetMonsterByIndex(selectedCharacterIndex);
		}

		#endregion

		#region Level handling
		// TODO Handle the level creation and configuration in this section

		/// <summary>
		/// Set if the player is allowed to move or not
		/// </summary>
		/// <param name="movementAllowed">True if movement is allowed, false otherwise</param>
		public void SetPlayerMovementAllowed(bool movementAllowed)
		{
			currentPlayerObject.NotifyMovementAllowed(movementAllowed);
		}


		public void ShowLevelEnd()
		{
			levelCompleteCanvas.SetActive(true);
		}

		public UIBossTextController GetUIBossTextController()
		{
			return uiBossTextController;
		}

		#endregion

		#region Player damageable listener

		public void NotifyDamage()
		{
			MusicManager.GetInstance().PlayDeathMusic();
			if (!debuggerMode)
			{
				currentAmountOfLifes--;
			}

			uiLifeIndicator.SetAmountOfLifeIndicatore(currentAmountOfLifes);
			currentPlayerObject.GetComponent<IDamageable>().RemoveDamageableListener(this);
			foreach (AbstractPlayerInput playerInput in playerInputs)
			{
				playerInput.RemovePlayerInputListener(currentPlayerObject);
			}
			if (currentAmountOfLifes > 0)
			{
				Invoke("SpawnPlayer", timeToRestartAfterDeath);
			} else
			{
				Invoke("GameOver", timeToGameOverAfterDeath);
			}
		}

		private void TransitionToMainMenu()
		{
			SceneNavigator.GetInstance().OpenScene_OfflineWithLoadingTransition(SceneNavigator.EGameLevels.MAIN_MENU);
		}

		public void NotifyHeal(int currentLifes)
		{
			if (currentAmountOfLifes < maxAmountOfLifes)
			{
				currentAmountOfLifes++;
			}
		}
		#endregion

		#region Game pause

		private void UpdateIsGamePaused()
		{
			if (Time.timeScale == 0f)
			{
				isGamePaused = true;
			}
			else
			{
				isGamePaused = false;
			}
		}

		private void UpdatePauseObjectVisibility()
		{
			if (isGamePaused) pauseObject.SetActive(true);
			else pauseObject.SetActive(false);
		}

		public static void NotifyPauseRequest()
		{
			if (!isGamePaused) PauseTime();
			else if (isGamePaused) UnpauseTime();
		}

		public static void PauseTime()
		{
			if (!isGamePaused) Time.timeScale = 0.0f;
		}

		public static void UnpauseTime()
		{
			if (isGamePaused) Time.timeScale = 1.0f;
		}

		public static bool IsGamePaused()
		{
			return isGamePaused;
		}

		private void GameOver()
		{
			gameOverObject.SetActive(true);
		}

		#endregion

		#region Game level management

		/// <summary>
		/// Notifies that the end of the level has been reached
		/// </summary>
		public void NotifyEndOfLevelReached()
		{
			levelLoaderManager.EndOfLevelreached();
		}

		#endregion
		public CoinManager GetCoinManager(){
			return coinManager;
		}

		public float GetZAssignationValue()
		{
			return zAssignationValue;
		}
	}


}

