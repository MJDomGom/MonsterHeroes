using com.blackantgames.managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallForceController : MonoBehaviour
{
	[Range(-1f, 1f)]
	[SerializeField] private float sideDirection;


	private Rigidbody rigidBody;

	private void Start()
	{
		rigidBody = GetComponent<Rigidbody>();
	}


	private void FixedUpdate()
	{
		rigidBody.AddForce(Vector3.right * sideDirection, ForceMode.Force);
		transform.position = new Vector3(transform.position.x, transform.position.y, GameManager.GetInstance().GetZAssignationValue());
	}
}
