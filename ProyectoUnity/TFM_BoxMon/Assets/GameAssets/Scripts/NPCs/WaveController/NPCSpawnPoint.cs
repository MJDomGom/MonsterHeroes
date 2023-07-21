using com.blackantgames.damageable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.blackantgames.npc.ai;

public class NPCSpawnPoint : MonoBehaviour
{

	public IDamageable SpawnEnemy(BasicMonsterController enemyToSpawn)
	{
		BasicMonsterController createdEnemy = Instantiate<BasicMonsterController>(enemyToSpawn, transform.position, transform.rotation, null);
		createdEnemy.ForceCombatMode();
		return createdEnemy.GetComponent<IDamageable>();
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(transform.position, .2f);
	}
}
