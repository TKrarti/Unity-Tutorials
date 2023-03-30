using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float padding = 1f;
    [SerializeField] int playerHealth = 1000;
    [SerializeField] int maxPlayerHealth = 1000;

    [Header("Sound Effects")]
    [SerializeField] AudioClip deathSFX;
    [SerializeField] [Range(0, 1)] float deathSoundVolume = 1f;
    [SerializeField] AudioClip shootSound;
    [SerializeField] [Range(0, 1)] float shootSoundVolume = 0.25f;
    [SerializeField] AudioClip hitSound;
    [SerializeField] [Range(0, 1)] float hitSoundVolume = 0.5f;

    [Header("Visual Effects")]
    [SerializeField] GameObject deathVFX;
    [SerializeField] float durationOfExplosion = 1f;
    [SerializeField] GameObject hitVFX;
    [SerializeField] float durationOfHitExplosion = 1f;
    [SerializeField] GameObject lightningVFX;
    [SerializeField] float durationOfLightning = 1f;
    [SerializeField] GameObject healthPowerupVFX;
    [SerializeField] float durationOfHealthPowerupVFX = 1f;
    [SerializeField] GameObject shieldPowerupVFX;
    [SerializeField] float durationOfShieldPowerupVFX = 1f;
    [SerializeField] GameObject lightningPowerupVFX;
    [SerializeField] float durationOfLightningPowerupVFX = 1f;
    [SerializeField] GameObject starPowerupVFX;
    [SerializeField] float durationOfStarPowerupVFX = 1f;


    [Header("Projectile")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] GameObject starProjectilePrefab;
    [SerializeField] float laserSpeed = 10f;
    [SerializeField] float laserFiringPeriod = 0.1f;
    [SerializeField] float starProjectileSpeed = 8f;
    [SerializeField] float starProjectileFiringPeriod = 0.2f;
    [SerializeField] float initialStarProjectileDuration = 5f;

    [Header("Protection")]
    [SerializeField] GameObject shieldPrefab;

    Coroutine firingCoroutine;

    float xMin;
    float xMax;
    float yMin;
    float yMax;
    bool isStarProjectile;
    float starProjectileDuration;

    // Start is called before the first frame update
    void Start()
    {
        isStarProjectile = false;
        starProjectileDuration = initialStarProjectileDuration;
        SetUpMoveBoundaries();
    }


    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
        CountDownStarProjectileDuration();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageDealer damageDealer = collision.gameObject.GetComponent<DamageDealer>();
        PowerUp powerUp = collision.gameObject.GetComponent<PowerUp>();
        HealthDealer healthDealer = collision.gameObject.GetComponent<HealthDealer>();

        if(!damageDealer && !powerUp)
        {
            return;
        }
        
        if(powerUp) 
        {
            Debug.Log(collision.gameObject.name);
            if (collision.gameObject.name.Contains("HealthPowerUp")) 
            {
                playerHealth += healthDealer.GetHealthValue();
                if (playerHealth > maxPlayerHealth)
                {
                    playerHealth = maxPlayerHealth;
                }
                GameObject powerupExplosion = Instantiate(healthPowerupVFX, transform.position, Quaternion.identity);
                Destroy(powerupExplosion, durationOfHealthPowerupVFX);
            }
            else if (collision.gameObject.name.Contains("LightningPowerUp")) 
            {
                Enemy[] currentEnemies = FindObjectsOfType<Enemy>();
                for (int enemyIndex = 0; enemyIndex < currentEnemies.Length; enemyIndex++)
                {
                    currentEnemies[enemyIndex].ProcessHit(damageDealer);
                }
                GameObject powerupExplosion = Instantiate(lightningPowerupVFX, transform.position, Quaternion.identity);
                Destroy(powerupExplosion, durationOfLightningPowerupVFX);
            }
            else if (collision.gameObject.name.Contains("ShieldPowerUp"))
            {
                Shield shield = FindObjectOfType<Shield>();
                if(shield)
                {
                    shield.ResetShieldHealth();
                }
                else
                {
                    Instantiate(shieldPrefab, transform.position, Quaternion.identity);
                }
                GameObject powerupExplosion = Instantiate(shieldPowerupVFX, transform.position, Quaternion.identity);
                Destroy(powerupExplosion, durationOfShieldPowerupVFX);
            }
            else if (collision.gameObject.name.Contains("StarPowerUp"))
            {
                isStarProjectile = true;
                GameObject powerupExplosion = Instantiate(starPowerupVFX, transform.position, Quaternion.identity);
                Destroy(powerupExplosion, durationOfStarPowerupVFX);
            }
            else
            {
                Debug.Log("Unknown Power Up");
                return;
            }
            powerUp.Hit();
        }
        else if(damageDealer)
        {
            ProcessHit(damageDealer);
        }
    }

    private void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            firingCoroutine = StartCoroutine(FireContinuously());
        }
        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
        }
    }

    IEnumerator FireContinuously()
    {
        while (true)
        {
            if(!isStarProjectile)
            {
                yield return StartCoroutine(FireProjectile(laserPrefab, laserSpeed, laserFiringPeriod, transform.position));
            }
            else
            {
                yield return StartCoroutine(FireProjectile(starProjectilePrefab, starProjectileSpeed, starProjectileFiringPeriod, transform.position));
            }
        }
    }

    IEnumerator FireProjectile(GameObject projectilePrefab, float projectileSpeed, float projectileFiringPeriod, Vector3 position)
    {
        GameObject laser = Instantiate(
                projectilePrefab,
                position,
                Quaternion.identity) as GameObject;
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
        AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootSoundVolume);
        yield return new WaitForSeconds(projectileFiringPeriod);
    }

    private void CountDownStarProjectileDuration()
    {
        if (isStarProjectile)
        {
            starProjectileDuration -= Time.deltaTime;
            if (starProjectileDuration <= 0)
            {
                isStarProjectile = false;
                starProjectileDuration = initialStarProjectileDuration;
            }
        }
    }

    private void Move()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);
        transform.position = new Vector2(newXPos, newYPos);
    }

    private void SetUpMoveBoundaries()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        playerHealth -= damageDealer.GetDamage();
        damageDealer.Hit();
        GameObject explosion = Instantiate(hitVFX, transform.position, Quaternion.identity);
        Destroy(explosion, durationOfHitExplosion);
        AudioSource.PlayClipAtPoint(hitSound, Camera.main.transform.position,hitSoundVolume);
        if (playerHealth <= 0)
        {
            playerHealth = 0;
            Die();
        }
    }

    private void Die()
    {
        FindObjectOfType<Level>().LoadGameOver();
        Destroy(gameObject);
        GameObject explosion = Instantiate(deathVFX, transform.position, Quaternion.identity);
        Destroy(explosion, durationOfExplosion);
        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, deathSoundVolume);
    }

    public int GetHealth()
    {
        return playerHealth;
    }
}
