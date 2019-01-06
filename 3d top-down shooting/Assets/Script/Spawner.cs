using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    [System.Serializable]
	public class Wave
    {
        public int enemyCount;
        public float timeBetweenSpawns;
    }
    public Wave[] waves;
    public Enemy enemy;
    int enemiesRemainingToSpawn;
    int enemiesRemainingAlive;
    float nextSpawnTime;
    Wave currentWave;
    int currentWaveNumber;

    void Start()
    {
        NextWave();
    }
    void Update()
    {
        if(enemiesRemainingToSpawn > 0 && Time.time > nextSpawnTime)
        {
            enemiesRemainingToSpawn--; // 적을 하나 스폰하므로 값을 1 줄임
            nextSpawnTime = Time.time + currentWave.timeBetweenSpawns; // nextSpawnTime을 조정

            Enemy spawnedEnemy = Instantiate(enemy, Vector3.zero, Quaternion.identity) as Enemy;
            spawnedEnemy.OnDeath += OnEnemyDeath; // OnDeath에 OnEnemyDeath를 +하여 적이 죽었을때 OnEnemyDeath() 호출
        }
    }
    void OnEnemyDeath()
    {
        enemiesRemainingAlive--;
        if(enemiesRemainingAlive == 0)
        {
            NextWave();
        }
    }
    void NextWave()
    {
        currentWaveNumber++;
        if (currentWaveNumber - 1 < waves.Length)
        {
            currentWave = waves[currentWaveNumber - 1];

            enemiesRemainingToSpawn = currentWave.enemyCount; // 스폰할 횟수를 받아옴
            enemiesRemainingAlive = enemiesRemainingToSpawn; // 남아있는 적의 개수를 받아옴
        }
    }
}
