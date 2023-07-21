using com.blackantgames.managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZPositionFixer : MonoBehaviour
{
	private void OnEnable()
	{
		if (GameManager.GetInstance() != null)
		{
			transform.position = new Vector3(transform.position.x, transform.position.y, GameManager.GetInstance().GetZAssignationValue());
		}
	}
}
