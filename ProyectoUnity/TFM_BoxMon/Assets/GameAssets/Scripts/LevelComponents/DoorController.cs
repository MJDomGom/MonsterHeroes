using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]
public class DoorController : MonoBehaviour
{
	private static int AC_BOOL_OPEN_STATE = Animator.StringToHash("doorOpen");

	[SerializeField] private AudioClip doorClip;

    private Animator animator;
	private AudioSource audioSource;

	private void Awake()
	{
		animator = GetComponent<Animator>();
		audioSource = GetComponent<AudioSource>();
	}

	public void SetDoorOpenStatus(bool isOpen)
	{
		animator.SetBool(AC_BOOL_OPEN_STATE, isOpen);
		audioSource.PlayOneShot(doorClip);
	}
}
