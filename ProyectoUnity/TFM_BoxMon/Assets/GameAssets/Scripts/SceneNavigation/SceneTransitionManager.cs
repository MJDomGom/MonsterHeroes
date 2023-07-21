using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.blackantgames.scenenavigation
{

	/// <summary>
	/// Auxiliary class to transition from the Loading scene to the target scene
	/// </summary>
	public class SceneTransitionManager : MonoBehaviour
	{
		[Tooltip("This is required to ensure that all the online users are synchronized in the loading scene")]
		[SerializeField] private float offlineDelay = 0.1f;
		
		private void Start()
		{
			Invoke("CompleteLevelTransition", offlineDelay);
		}

		/// <summary>
		/// Request the definitive level transition
		/// </summary>
		public void CompleteLevelTransition()
		{
			// TODO In online mode it would be interesting to indicate some additional messages like "Waiting players to join" or something similar in the loading time
			SceneNavigator.GetInstance().CompleteLevelTransition();
		}
	}
}