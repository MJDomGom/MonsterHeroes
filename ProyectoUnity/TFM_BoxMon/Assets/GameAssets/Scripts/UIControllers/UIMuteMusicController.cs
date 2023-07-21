using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.blackantgames.managers;
using com.blackantgames.musiccontroller;

public class UIMuteMusicController : MonoBehaviour
{
	[SerializeField] private GameObject forbiddenIcon;

	private void Start()
	{
		UpdateMutedStateIcon();
	}

	public void UpdateMutedStateIcon()
	{
		forbiddenIcon.SetActive(MusicManager.GetInstance().GetMuteMusicState());
		
	}
}
