using com.blackantgames.damageable;
using com.blackantgames.managers;
using com.blackantgames.musiccontroller;
using com.blackantgames.npc.ai;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to control the enemy waves in a specific area
/// </summary>
public class NPCWaveController : MonoBehaviour, IDamageableListener
{
    [Header("Music reference set to configuration the audio in the wave")]
    [SerializeField] private MusicReferenceSet waveMusicSet;

    [Header("Doors to close the area that delimit the wave zone")]
    [SerializeField] private DoorController[] closingDoorsOnStart;
    [SerializeField] private DoorController[] openDoorsOnStart;

    [SerializeField] private DoorController[] doorsToOpenAtTheEndOfWave;
    [SerializeField] private DoorController[] doorsToCloseAtTheEndOfWave;

    [Header("Spawn points for the enemies")]
    [SerializeField] private NPCSpawnPoint[] spawnPoints;

    [Header("Wave time configuration")]
    [SerializeField] private int minTimeToSpawnEnemies;
    [SerializeField] private int maxTimeToSpawnEnemies;

    [Header("Wave enemies")]
    [SerializeField] private int maxAmountOfEnemiesActive;
    [SerializeField] private BasicMonsterController enemyToSpawn;
    [SerializeField] private int amountOfEnemiesToSpawn;

    private int currentAmountOfEnemies = 0;
    private int totalSpawnedEnemies = 0;
    private int defeatedEnemies = 0;
    private bool waveStarted;
    
    private MusicReferenceSet musicSetToRestore;
    private NPCSpawnPoint lastSpawnPoint = null;
    private List<IDamageable> activeDamageables;

    
    void Awake()
    {
#if UNITY_EDITOR

		if (spawnPoints == null || spawnPoints.Length == 0)
		{
			Debug.LogError("Error: No spawn points in the wave " + transform.name);
		}

		if (enemyToSpawn == null)
		{
			Debug.LogError("Error: No enemies to spawn in the wave " + transform.name);
		}

		if (amountOfEnemiesToSpawn < 1)
		{
			Debug.LogError("Error: No enemies to spawn " + transform.name);
		}
#endif

		activeDamageables = new List<IDamageable>();
	}

    /// <summary>
    /// Function to start the wave
    /// </summary>
    public void StartWave()
    {
        // Start the wave opening and closing doors as required
        foreach(DoorController controller in closingDoorsOnStart)
        {
            controller.SetDoorOpenStatus(false);
        }
		foreach (DoorController controller in openDoorsOnStart)
		{
			controller.SetDoorOpenStatus(true);
		}

		// Change the music to the Wave music set
		musicSetToRestore = MusicManager.GetInstance().GetCurrentMusicSet();
		
		MusicManager.GetInstance().PlayMusicFromSet(waveMusicSet, true);
        waveStarted = true;

        if(GameManager.GetInstance().GetUIBossTextController() != null)
        {
            GameManager.GetInstance().GetUIBossTextController().TriggerBossTextAnimation();
		}

        // Start the spawn of enemies
        Invoke("SpawnEnemy", 3f);
    }

    /// <summary>
    /// Indicates if the wave is started
    /// </summary>
    /// <returns>True if the wave is started</returns>
    public bool IsWaveStarted()
    {
        return waveStarted;
    }

    /// <summary>
    /// Provides the Wave Music Reference Set
    /// </summary>
    /// <returns>Wave music reference set</returns>
    public MusicReferenceSet GetWaveMusicReferenceSet()
    {
        return waveMusicSet;
    }

    /// <summary>
    /// Function to finish the wave
    /// </summary>
    private void EndWave()
    {
        MusicManager.GetInstance().PlayMusicFromSet(musicSetToRestore, true);
        waveStarted = false;

		// Start the wave opening and closing doors as required
		foreach (DoorController controller in doorsToCloseAtTheEndOfWave)
		{
			controller.SetDoorOpenStatus(false);
		}
		foreach (DoorController controller in doorsToOpenAtTheEndOfWave)
		{
			controller.SetDoorOpenStatus(true);
		}
	}

    private void SpawnEnemy()
    {
        if (!IsWaveCompleted())
        {
            if (activeDamageables.Count < maxAmountOfEnemiesActive 
                && totalSpawnedEnemies < amountOfEnemiesToSpawn)
            {
                NPCSpawnPoint spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                while (spawnPoint == lastSpawnPoint)
                {
					spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
				}
                lastSpawnPoint = spawnPoint;

                IDamageable spawnedEnemyDamageable = spawnPoint.SpawnEnemy(enemyToSpawn);
                if (spawnedEnemyDamageable != null)
                {
                    spawnedEnemyDamageable.AttachDamageableListener(this);
					if (!activeDamageables.Contains(spawnedEnemyDamageable))
                    {
						activeDamageables.Add(spawnedEnemyDamageable);
						currentAmountOfEnemies++;
					}
                    totalSpawnedEnemies++;
				}
            }
            Invoke("SpawnEnemy", Random.Range(minTimeToSpawnEnemies, maxTimeToSpawnEnemies));
        }
        else
        {
            EndWave();
        }

    }

    /// <summary>
    /// Function that checks if the wave is over or not
    /// </summary>
    /// <returns>True if wave is completed, false otherwise</returns>
    public bool IsWaveCompleted()
    {
        return (defeatedEnemies >=  amountOfEnemiesToSpawn);
    }

	private void OnDisable()
	{
		CancelInvoke();
	}

	public void NotifyDamage()
	{
		List<IDamageable> damageablesToRemove = new List<IDamageable>();
		foreach (var damageable in activeDamageables)
        {
            if (damageable == null || !damageable.IsAlive())
            {
                damageablesToRemove.Add(damageable);
            }
        }

		foreach (var damageableToRemove in damageablesToRemove)
		{
			activeDamageables.Remove(damageableToRemove);
			currentAmountOfEnemies--;
		}

		defeatedEnemies++;
	}

	public void NotifyHeal(int currentLifes)
	{
		// Nothing to do
	}
}
