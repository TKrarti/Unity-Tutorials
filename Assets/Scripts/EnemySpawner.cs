using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Range(0.1f, 120f)]
    [SerializeField] float secondsBetweenSpawns = 10f;
    [SerializeField] Pathfinder enemyPrefab;
    [SerializeField] Waypoint end;
    
    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while(true)
        {
            yield return new WaitForSeconds(secondsBetweenSpawns);
            var newEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            newEnemy.transform.parent = gameObject.transform;
        }
    }

    public Waypoint GetEnd()
    {
        return end;
    }
}
