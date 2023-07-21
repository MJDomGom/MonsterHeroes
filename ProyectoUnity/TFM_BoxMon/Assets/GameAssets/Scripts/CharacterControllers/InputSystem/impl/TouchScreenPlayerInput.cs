using com.blackantgames.ui.controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace com.blackantgames.character.controller
{

    /// <summary>
    /// Player input with touch screen devices
    /// </summary>
    public class TouchScreenPlayerInput : AbstractPlayerInput
    {
        private bool moveRight;
        private bool moveLeft;
        private UICharacterMovementInteraction uiComponent;

		private void Awake()
		{
			// Enable the canvas where the UI interaction is located
			UICharacterMovementInteraction[] uiComponents = Resources.FindObjectsOfTypeAll<UICharacterMovementInteraction>();
            if (uiComponents.Length > 0)
            {
                if (uiComponents[0] != null)
                {
                    uiComponents[0].gameObject.SetActive(true);
                }
            } else
            {
#if UNITY_EDITOR
                Debug.LogError("Error: " + transform.name + " found an expected amount of UICharacterMovementInteraction instances: " + uiComponents.Length);
#endif
            }
		}

		private void Update()
		{
			NotifySideMovementInput((moveRight ? 1f : -1f) + (moveLeft ? -1f : 1f));
		}

        /// <summary>
        /// Indicates the beginning of the left movement action
        /// </summary>
		public void ActionLeftMovementStarted()
        {
            moveLeft = true;
        }

        /// <summary>
        /// Indicats the end of the left movement action
        /// </summary>
		public void ActionLeftMovementFinished()
		{
            moveLeft = false;
		}

        /// <summary>
        /// Indicates the beginning of the right movement action
        /// </summary>
        public void ActionRightMovementStarted()
        {
            moveRight = true;
        }

        /// <summary>
        /// Indicates the end of the right movement action
        /// </summary>
        public void ActionRightMovementFinished()
        {
            moveRight = false;
        }

        /// <summary>
        /// Notifies that it was requested to perform a Jump action
        /// </summary>
        public void ActionJump()
        {
            NotifyJumpRequest();
        }
    }
}