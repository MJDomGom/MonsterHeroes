using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuMusicController : MonoBehaviour
{
    AudioSource musicSource;
	private void Awake()
	{
		musicSource = GetComponent<AudioSource>();
		if (PlayerPrefs.GetInt("MUTED", 0) == 1)
		{
			musicSource.Stop();
		}
	}
}
