using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MarwanZaky;
public class EnemySpawn : MonoBehaviour
{

    [SerializeField]
    private GameObject[] enemies;

    [SerializeField]
    private int spawnCount = 10;

    [SerializeField]
    private float spawnTime;

    [SerializeField]
    private float spawnDelay;

    [SerializeField]
    public Vector3[] spawnPoints;

    private int remainingEnemies = 0;
    int randomSpawnPoint;

    GameObject player;

    void Start()
    {
        StartCoroutine(SpawnObjects());

        if (player == null)
        {
            player = GameObject.Find("Third Person Player");
            
        }
    }

    void Update()
    {
        if (player.GetComponent<HealthManager>().isGameOver == true)
            Destroy(this);
        else
            return;
    }

    private IEnumerator SpawnObjects()
    {
        WaitForSeconds wait = new WaitForSeconds(spawnDelay);

        // Initial wait
        yield return new WaitForSeconds(spawnTime);


        for (int count = spawnCount; count > 0; --count)
        {
            randomSpawnPoint = Random.Range(0, spawnPoints.Length);
            int randomSpawnEnemy = Random.Range(0, enemies.Length);
            GameObject clone = Instantiate(enemies[randomSpawnEnemy], spawnPoints[randomSpawnPoint], Quaternion.identity);
            player.GetComponent<StackManager>().enemy.Add(clone);
            // Detect when an ennemy gets destroyed

            remainingEnemies++;

            // Wait before next spawn
            yield return wait;
        }

        Debug.Log("All the ennemies have been instantiated !");
    }
}
