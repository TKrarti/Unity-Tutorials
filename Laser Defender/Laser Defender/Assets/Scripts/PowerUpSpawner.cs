using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    [SerializeField] List<PowerUp> powerUps;
    [SerializeField] float padding = 1f;
    [SerializeField] float powerUpSpeed = 2f;
    [SerializeField] float timeBetweenSpawns = 15f;
    [SerializeField] float spawnRandomFactor = 2f;

    float xMin;
    float xMax;
    float yPos;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        SetUpSpawnRange(padding);
        do
        {
            yield return StartCoroutine(SpawnPowerUps());
        }
        while (true);
    }

    private void SetUpSpawnRange(float padding)
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
        yPos = gameCamera.ViewportToWorldPoint(new Vector3(0, 1.2f, 0)).y;
    }

    private IEnumerator SpawnPowerUps()
    {
        float spawnTime = timeBetweenSpawns +
                Random.Range(-spawnRandomFactor, spawnRandomFactor);
        yield return new WaitForSeconds(spawnTime);

        int powerUpIndex = Random.Range(0, powerUps.Count);
        Vector3 spawnPos = new Vector3(Random.Range(xMin, xMax), yPos, 0);
        var newPowerUp = Instantiate(
            powerUps[powerUpIndex],
            spawnPos,
            Quaternion.identity);
        newPowerUp.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -powerUpSpeed);
    }
}
