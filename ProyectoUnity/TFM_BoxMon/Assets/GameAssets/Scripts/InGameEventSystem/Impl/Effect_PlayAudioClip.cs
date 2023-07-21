using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Effect_PlayAudioClip : AbstracEffect
{
    [SerializeField] private AudioClip clip;
	[SerializeField] private bool onePlayOnly;

	private AudioSource audioSource;
	private bool clipPlayed = false;

	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();
	}

	public override void TriggerEffect()
	{
		if (onePlayOnly && clipPlayed)
		{
			return;
		}
		
		audioSource.PlayOneShot(clip);
		clipPlayed = true;
	}
}
