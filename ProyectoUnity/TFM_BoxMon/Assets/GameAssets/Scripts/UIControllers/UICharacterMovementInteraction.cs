using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.blackantgames.character.controller;
using com.blackantgames.managers;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace com.blackantgames.ui.controllers
{
    /// <summary>
    /// Class to handle the user interaction to request the player movement
    /// </summary>
    public class UICharacterMovementInteraction : MonoBehaviour
    {
		private TouchScreenPlayerInput touchScreenPlayerInput;
        
		private void Start()
		{
            List<AbstractPlayerInput> playerInputs = GameManager.GetInstance().GetPlayerInput();
            foreach (AbstractPlayerInput aPlayerInput in playerInputs)
            {
                if (aPlayerInput.GetType().Equals(typeof(TouchScreenPlayerInput)))
                {
                    touchScreenPlayerInput = (TouchScreenPlayerInput)aPlayerInput;
                }
            }

#if UNITY_EDITOR
            if (touchScreenPlayerInput == null)
            {
                Debug.LogError("Error - mobile player input not available");
            }
#endif
		}

        /// <summary>
        /// Notification about the press action in a child action button
        /// </summary>
        /// <param name="action">Action attached to the child button</param>
		public void ActionButtonPressed(ECharacterMovementActionType action)
        {
            switch(action)
            {
                case ECharacterMovementActionType.LEFT:
                    touchScreenPlayerInput.ActionLeftMovementStarted();
                    break;
                case ECharacterMovementActionType.RIGHT:
                    touchScreenPlayerInput.ActionRightMovementStarted();
                    break;
                case ECharacterMovementActionType.JUMP:
                    touchScreenPlayerInput.ActionJump();
					break;
                default:
#if UNITY_EDITOR
                    Debug.LogError("ERROR - unhandled situation in UI CharacterMovementInteraction button pressed. Action: " + action.ToString());
#endif
                    break;
            }
        }

        public void ActionButtonReleased(ECharacterMovementActionType action)
        {
			switch (action)
			{
				case ECharacterMovementActionType.LEFT:
                    touchScreenPlayerInput.ActionLeftMovementFinished();
					break;
				case ECharacterMovementActionType.RIGHT:
                    touchScreenPlayerInput.ActionRightMovementFinished();
                    break;
                case ECharacterMovementActionType.JUMP:
					// Nothing to do
					break;
				default:
#if UNITY_EDITOR
					Debug.LogError("ERROR - unhandled situation in UI CharacterMovementInteraction button released. Action: " + action.ToString());
#endif
					break;
			}
		}

        public enum ECharacterMovementActionType
        {
            LEFT,
            RIGHT,
            JUMP
        }
	}

}