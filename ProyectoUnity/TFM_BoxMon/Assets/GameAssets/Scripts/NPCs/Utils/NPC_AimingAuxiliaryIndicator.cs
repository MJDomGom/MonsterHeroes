using com.blackantgames.managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class NPC_AimingAuxiliaryIndicator : MonoBehaviour
{
	[SerializeField] private Transform targetTransform;
	[SerializeField] private TwoBoneIKConstraint tbikc;
	[SerializeField] private Transform shootEffectReference;
	[SerializeField] private float shootEffectSpeed = 25f;

	private bool aiming;

	[SerializeField]  private bool shooting;
	private bool recovering;
	private float diffValue;

	Vector3 pos = Vector3.zero;

	private void Update()
	{
		if (aiming && tbikc.weight < 1f)
		{
			tbikc.weight += Time.deltaTime * .5f;
			if (tbikc.weight > 1f)
			{
				tbikc.weight = 1f;
			}
		} else if (!aiming && tbikc.weight > 0.0f)
		{
			tbikc.weight -= Time.deltaTime * .5f;
			if (tbikc.weight < 0f)
			{
				tbikc.weight = 0f;
			}
		}

		if (!shooting)
		{
			if (GameManager.GetInstance().GetCurrentPlayerCharacterController() != null)
			{
				targetTransform.position = GameManager.GetInstance().GetCurrentPlayerCharacterController().GetMassCenter().position;
			}
		} else
		{
			if (pos == Vector3.zero) {
				targetTransform.position = shootEffectReference.position;
				targetTransform.LookAt(GameManager.GetInstance().GetCurrentPlayerCharacterController().GetMassCenter().position);
				targetTransform.position += targetTransform.forward;
				pos = targetTransform.position;
			}

			if (!recovering) {
				diffValue = Mathf.Lerp(diffValue, 1f, Time.deltaTime * shootEffectSpeed);
				if (diffValue > 0.9f)
				{
					recovering = true;
				}
			} else
			{
				diffValue = Mathf.Lerp(diffValue, 0f, Time.deltaTime * shootEffectSpeed);
				if (diffValue < 0.1f)
				{
					recovering = false;
					shooting = false;
					pos = Vector3.zero;
					return;
				}
			}
			targetTransform.position = pos + transform.up * diffValue;
		}
	}

	public void SetAimingState(bool aiming)
    {
		this.aiming = aiming;
    }

	/// <summary>
	/// Trigger the Shoot effect
	/// </summary>
	public void TriggerShootEffect()
	{
		shooting = true;
	}
}
