using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using com.blackantgames.playerstoragesystem;

namespace com.blackantgames.scenenavigation
{

	/// <summary>
	/// Scene navigation controller
	/// This class implements the functionality to navigate through the different scenes
	/// in the game
	/// 
	/// NOTE: This class implements a strict singleton instance and only one instance is
	/// allowed in the scene. Any attempt to create a new instance will result on the
	/// destruction of the instance if the singleton is already assigned
	/// </summary>
	public class SceneNavigator : MonoBehaviour
	{
		public enum EGameLevels
		{
			MAIN_MENU = 0,
			LOADING_LEVEL = 1,
			GAME_LEVEL = 2
		};

		private static SceneNavigator INSTANCE;

		private EGameLevels levelToLoad;

		private void Awake()
		{
			if (INSTANCE != null)
			{
				Destroy(this.gameObject);
				return;
			}

			INSTANCE = this;
			DontDestroyOnLoad(this.gameObject);
		}


		/// <summary>
		/// Access to the singleton instance
		/// </summary>
		/// <returns>Singleton instance</returns>
		public static SceneNavigator GetInstance()
		{
			return INSTANCE;
		}

		/// <summary>
		/// Load a specific game scene using a transition through the Load scene
		/// </summary>
		/// <param name="level">Level to load</param>
		public void OpenScene_OfflineWithLoadingTransition(EGameLevels level)
		{
			levelToLoad = level;
			// Load the loading level scene, which will be responsible for the final transition
			StartCoroutine(OfflineLevelLoad((int) EGameLevels.LOADING_LEVEL));
		}

		/// <summary>
		/// Load a specific level in offline mode
		/// (NOTE: The load will be done asynchronous)
		/// </summary>
		/// <param name="level">Level to open</param>
		public void OpenScene_OfflineMode(EGameLevels level)
		{
			levelToLoad = level;
			// Open in a new thread the final scene to load
			StartCoroutine(OfflineLevelLoad((int)level));
		}

		/// <summary>
		/// Offline level load enumerator
		/// </summary>
		/// <param name="level">Level to be opened</param>
		/// <returns>Null while level not loaded</returns>
		private IEnumerator OfflineLevelLoad(int level)
		{
			// Asynchronous level load
			AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(level);

			// Wait until the asynchronous scene fully loads
			while (!asyncLoad.isDone)
			{
				yield return null;
			}
		}

		/// <summary>
		/// Complete a level transition when the information is already stored about the level to load
		/// </summary>
		public void CompleteLevelTransition()
		{
			OpenScene_OfflineMode(levelToLoad);
		}

		/// <summary>
		/// Close the game
		/// </summary>
		public void CloseGame()
		{
			Application.Quit();
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#endif
			
		}
	}
}
