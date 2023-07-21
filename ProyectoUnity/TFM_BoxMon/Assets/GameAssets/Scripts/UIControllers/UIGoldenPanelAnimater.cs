using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGoldenPanelAnimater : MonoBehaviour
{
	private static readonly int AC_TRIGGER_ANIMATE = Animator.StringToHash("triggerGoldenCoinAnimator");
    private Animator animator;

	public void Awake()
	{
		animator = GetComponent<Animator>();
	}

	public void TriggerGoldenCoinAnimator()
	{
		animator.SetTrigger(AC_TRIGGER_ANIMATE);
	}
}
