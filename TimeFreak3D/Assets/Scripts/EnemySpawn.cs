using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EnemySpawn : MonoBehaviour
{

    [SerializeField]
    private GameObject prefab;

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

    void Start()
    {
        StartCoroutine(SpawnObjects());
        
    }

    private IEnumerator SpawnObjects()
    {
        WaitForSeconds wait = new WaitForSeconds(spawnDelay);

        // Initial wait
        yield return new WaitForSeconds(spawnTime);


        for (int count = spawnCount; count > 0; --count)
        {
            randomSpawnPoint = Random.Range(0, spawnPoints.Length);
            GameObject clone = Instantiate(prefab, spawnPoints[randomSpawnPoint], Quaternion.identity);

            // Detect when an ennemy gets destroyed


            remainingEnemies++;

            // Wait before next spawn
            yield return wait;
        }

        Debug.Log("All the ennemies have been instantiated !");
    }
}
