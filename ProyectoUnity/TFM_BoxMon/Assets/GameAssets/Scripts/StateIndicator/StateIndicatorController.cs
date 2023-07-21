using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateIndicatorController : MonoBehaviour
{
    private static readonly int AC_ALERT_TRIGGER = Animator.StringToHash("alert");
    private static readonly int AC_BACK_TO_NORMAL_TRIGGER = Animator.StringToHash("normal");

    private Animator animator;

	private void Awake()
	{
        animator = GetComponent<Animator>();
	}
	
	public void TriggerAlert()
	{
		animator.SetTrigger(AC_ALERT_TRIGGER);
	}

	public void TriggerBackToNormal()
	{
		animator.SetTrigger(AC_BACK_TO_NORMAL_TRIGGER);
	}

}
