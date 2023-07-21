using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace com.blackantgames.ui.controllers
{
	/// <summary>
	/// Movement action button listener.
	/// This class will handle the user interaction with the UI to clock and release buttons
	/// that must take an action with the player interaction.
	/// </summary>
	public class UICharacterActionButtonListener : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
	{
		[Tooltip("Expected action to be performed by this button/UI component")]
		[SerializeField] UICharacterMovementInteraction.ECharacterMovementActionType buttonActionType;

		UICharacterMovementInteraction characterMovementInteractor;

		private void Awake()
		{
			// Find the UICharacterMovementInteraction in the parent objects
			Transform parentTransform = transform.parent;
			while (characterMovementInteractor == null && parentTransform != null)
			{
				characterMovementInteractor = parentTransform.GetComponent<UICharacterMovementInteraction>();
				if (characterMovementInteractor == null) {
					parentTransform = parentTransform.parent;
				}
			}

#if UNITY_EDITOR
			if (characterMovementInteractor == null)
			{
				Debug.LogError("Error: Button " + transform.name + " could not find parent UI Character mvement interactor");
			}
#endif
		}

		/// <summary>
		/// Pointer down implementation. Button pressed.
		/// </summary>
		/// <param name="eventData">Pointer event data</param>
		public void OnPointerDown(PointerEventData eventData)
		{
			characterMovementInteractor.ActionButtonPressed(buttonActionType);
		}

		/// <summary>
		/// Pointer up implementation. Button released.
		/// </summary>
		/// <param name="eventData">´pointer event data</param>
		public void OnPointerUp(PointerEventData eventData)
		{
			characterMovementInteractor.ActionButtonReleased(buttonActionType);
		}
	}
}