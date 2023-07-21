using com.blackantgames.managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


namespace com.blackantgames.character.controller
{
    /// <summary>
    /// Player input with physical device
    /// </summary>
    public class DevicePlayerInput : AbstractPlayerInput
    {
        [Header("Player input character movement")]
        [SerializeField] private InputAction rightMovementAction;            // TODO In mobile devices it makes sense to have only a binary button, does it make sense in physical devices?
		[SerializeField] private InputAction leftMovementAction;
		[SerializeField] private InputAction jumpAction;
        [SerializeField] private InputAction pauseAction;

        [Header("DEBUGGER MODE INPUT")]
        [SerializeField] private InputAction debuggerModeRequest;

		private void OnEnable()
		{
            rightMovementAction.Enable();
            leftMovementAction.Enable();
            jumpAction.Enable();
			pauseAction.Enable();
			debuggerModeRequest.Enable();
		}

		private void OnDisable()
		{
            rightMovementAction.Disable();
            leftMovementAction.Disable();
            jumpAction.Disable();
			pauseAction.Disable();
			debuggerModeRequest.Disable();  
		}

		private void Update()
		{
            ReadPlayerMovementInputs();
		}

        /// <summary>
        /// Read the player movement inputs
        /// </summary>
        private void ReadPlayerMovementInputs()
        {
			// Player movement
			NotifySideMovementInput((rightMovementAction.IsPressed() ? 1f : -1f) 
                + (leftMovementAction.IsPressed() ? -1f : 1f));

            // Player jump requests
            if (jumpAction.WasPerformedThisFrame())
            {
                NotifyJumpRequest();
            }

            // Pause action
            if (pauseAction.WasPerformedThisFrame())
            {
                GameManager.NotifyPauseRequest();

			}

			// Debugger mode request
			if(debuggerModeRequest.WasPerformedThisFrame() ) {
                ActivateDebuggerMode();
            }

		}
	}
}