using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.blackantgames.musiccontroller
{

	public class BasicAudioController : MonoBehaviour
	{
		[SerializeField] private float transitionSpeedMultiplier = .5f;
		[SerializeField] private float transitionStopper = .01f;


		private AudioSource audioSource;
		private float targetAudio;
		private bool transitionStarted;
		private void Awake()
		{
			audioSource = GetComponent<AudioSource>();
			transitionStarted = false;
		}

		private void Update()
		{
			if (transitionStarted)
			{
				audioSource.volume = Mathf.Lerp(audioSource.volume, targetAudio, Time.deltaTime * transitionSpeedMultiplier);
				if (Mathf.Abs(audioSource.volume - targetAudio) < transitionStopper)
				{
					audioSource.volume = targetAudio;
					transitionStarted = false;
				}
			}
		}

		public void SetAudioVolumeTransition(float targetAudio)
		{
			this.targetAudio = targetAudio;
			transitionStarted = true;
		}

		public void PlayAudioClip(AudioClip clip)
		{
			AudioSource.PlayClipAtPoint(clip, transform.position);
			//audioSource.PlayOneShot(clip);
		}
	}
}