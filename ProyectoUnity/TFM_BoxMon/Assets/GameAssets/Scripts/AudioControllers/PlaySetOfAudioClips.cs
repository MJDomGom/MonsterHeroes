using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.blackantgames.musiccontroller
{

	/// <summary>
	/// Auxiliary class to play different not-connected audio clips as one
	/// with a specific delay between different audio clips
	/// </summary>
	[RequireComponent(typeof(AudioSource))]
	public class PlaySetOfAudioClips : MonoBehaviour
	{
		[SerializeField] private AudioClip[] setOfClipsToPlay;
		[SerializeField] private float[] delayBetweenClips;

		private AudioSource audioSource;
		private int currentAudioClip;

		private void Awake()
		{
			currentAudioClip = 0;
			audioSource = GetComponent<AudioSource>();
		}

		public void PlayAudioClips()
		{

			Invoke("PlayNextAudioClip", delayBetweenClips[0]);
		}

		private void PlayNextAudioClip()
		{
			if (currentAudioClip < setOfClipsToPlay.Length)
			{
				audioSource.PlayOneShot(setOfClipsToPlay[currentAudioClip]);
				currentAudioClip++;
				if (currentAudioClip < setOfClipsToPlay.Length)
				{
					Invoke("PlayNextAudioClip", delayBetweenClips[currentAudioClip]);
				}
			}
		}

	}
}