using UnityEngine;

/// <summary>
/// Character spawm point controller
/// This class represents an object that is able to spawn the player in the world level
/// </summary>
public class CharacterSpawnPoint : MonoBehaviour
{
	
	/// <summary>
	/// Spawn the player
	/// </summary>
	/// <param name="characterPrefab">Reference to the character prefab to be spawned</param>
	public GameObject SpawnPlayer(GameObject characterPrefab)
	{
		return Instantiate(characterPrefab, transform.position, transform.rotation);
	}

	private void OnDrawGizmos()
	{
		// Draw a sphere in the position where the player is going to be spawned
		Gizmos.color = Color.blue;
		Gizmos.DrawSphere(transform.position, .25f);

		// Draw an arrow indicating the forward direction to understand how is the player going to be spawn
		Gizmos.color = Color.green;
		Vector3 targetDirection = transform.forward * .5f;
		Gizmos.DrawRay(transform.position, targetDirection);

		Vector3 right = Quaternion.LookRotation(targetDirection) * Quaternion.Euler(0, 180 + 10f, 0) * new Vector3(0, 0, 1);
		Vector3 left = Quaternion.LookRotation(targetDirection) * Quaternion.Euler(0, 180 - 10f, 0) * new Vector3(0, 0, 1);
		Gizmos.DrawRay(transform.position + targetDirection, right * .25f);
		Gizmos.DrawRay(transform.position + targetDirection, left * .25f);

	}
}
