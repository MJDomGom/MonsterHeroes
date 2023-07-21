using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class UIBossTextController : MonoBehaviour
{
	private static readonly int AC_TRIGGER_SHOWBOSSTEXT = Animator.StringToHash("showBossText");

    private Animator animator;

	private void Awake()
	{
		animator = GetComponent<Animator>();
	}

	public void TriggerBossTextAnimation()
	{
		animator.SetTrigger(AC_TRIGGER_SHOWBOSSTEXT);
	}
}
