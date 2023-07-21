using com.blackantgames.managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.blackantgames.character.controller
{

    /// <summary>
    /// Abstract base definition of the PlayerInput
    /// </summary>
    public abstract class AbstractPlayerInput : MonoBehaviour
    {

        // --------------------------------------------------------------------------------------------
        // TODO - The following variables are for the debugger mode and should be deleted in production
        private int clicksToDebuggerMode = 9;
		// --------------------------------------------------------------------------------------------

		// List of listeners that will listen for the player inputs
		private List<IPlayerInputListener> playerInputListeners = new List<IPlayerInputListener>();

        /// <summary>
        /// Add a new player input listener to the list of active player
        /// input listeners
        /// </summary>
        /// <param name="listener">Listener to be added to the list of active listeners</param>
        public void AddPlayerInputListener(IPlayerInputListener listener)
        {
            if (!playerInputListeners.Contains(listener))
            {
                playerInputListeners.Add(listener);
            }
        }

        /// <summary>
        /// Remove a player input listener from the list of active player input listeners
        /// </summary>
        /// <param name="listener">Listener to be removed from the list of active listeners</param>
        public void RemovePlayerInputListener(IPlayerInputListener listener)
        {
            playerInputListeners.Remove(listener);
        }

        /// <summary>
        /// Notifies the listeners about the side movement input values
        /// </summary>
        /// <param name="lateralDirection">Player lateral direction movement</param>
        public void NotifySideMovementInput(float lateralDirection)
        {
            foreach(IPlayerInputListener listener in playerInputListeners)
            {
                listener.NotifyMovementValues(lateralDirection);
            }
        }

        /// <summary>
        /// Notifies the player input listeners that it was requested to perform a jump
        /// </summary>
        public void NotifyJumpRequest()
        {
            foreach(IPlayerInputListener listener in playerInputListeners)
            {
                listener.NotifyJumpRequest();
            }
        }

        /// <summary>
        /// Activates the debugger mode
        /// </summary>
        public void ActivateDebuggerMode()
        {
            clicksToDebuggerMode--;
            if (clicksToDebuggerMode == 0)
            {
                GameManager.GetInstance().ActivateDebuggerMode();
            }

		}
	}
}